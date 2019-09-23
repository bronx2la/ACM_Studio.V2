using System;
using System.Data;
using System.Data.SqlClient;

namespace Core.DbHandlers
{
    public static class DbHandlers
    {
        /// <summary>
        /// Bulk inserts contest of data table
        /// </summary>
        public static void BulkInsertToDatabase(DataTable dataTable, string connectionString, int bulkInsertBatchSize, string stagingTable, string exceptionTable = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                using (var sqlBulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                {
                    sqlBulkCopy.BatchSize            = bulkInsertBatchSize;
                    sqlBulkCopy.DestinationTableName = stagingTable;
                    sqlBulkCopy.BulkCopyTimeout      = 1500;

                    try
                    {
                        sqlBulkCopy.ColumnMappings.Clear();
                        foreach(object column in dataTable.Columns)
                            sqlBulkCopy.ColumnMappings.Add(column.ToString(),column.ToString());

                        sqlBulkCopy.WriteToServer(dataTable);
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        connection.Close();
                        throw;
                    }
                }
            }
        }
    }
}