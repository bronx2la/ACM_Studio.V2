using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace DapperDatabaseAccess
{
    public class DapperDatabaseAccess<T> : IDisposable
    {
        private string ConnectionString { get; set; }


        #region SQL Server methods
        
        /// <summary>
        /// Rapid_Dapper_Dal Constructor
        /// </summary>
        /// <param name="connectionString"></param>
        public DapperDatabaseAccess(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// Executes a SQL query
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IEnumerable<T> SqlQuery(string sql)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    IEnumerable<T> itemList = connection.Query<T>(sql, null, commandType: CommandType.Text, commandTimeout:1000);
                    return itemList;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        /// <summary>
        /// Executes a SQL Server stored procedure data fetch
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public IEnumerable<T> SqlServerFetch(string procName, DynamicParameters parms)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    IEnumerable<T> itemList = connection.Query<T>(procName, parms, commandType: CommandType.StoredProcedure, commandTimeout:1000);
                    return itemList;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Executes a SQL Server stored procedure
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public int SqlExecute(string procName, DynamicParameters parms)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    int affectedRows = connection.Execute(procName, parms, commandType: CommandType.StoredProcedure, commandTimeout:1000);
                    return affectedRows;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        #endregion
        
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}