using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.DataModels;
using Core.DbHandlers;
using Core.ListHandlers;
using CsvHelper;
using Dapper;
using Microsoft.Extensions.Configuration;
using DapperDatabaseAccess;

namespace ETL
{
    public class Etl_Geneva
    {
        private static string                _connectionString;
        private static IConfigurationRoot    _configuration;
        private static TaskMetaDataDataModel _metaData;
        private static string                _path;
        private static string                _reportingDate;

        public Etl_Geneva(TaskMetaDataDataModel metaData, IConfigurationRoot configuration, string connectionString,
                          string reportingDate)
        {
            _metaData         = metaData;
            _configuration    = configuration;
            _connectionString = connectionString;
            _reportingDate    = reportingDate;

            var fyle = string.Format(_metaData.inboundFile, _reportingDate);
            _path = $@"{_metaData.inboundFolder}\{fyle}";
        }

        public void ProcessInboundSalesFile()
        {
            try
            {
                using (var reader = new StreamReader(_path))
                    using (var csv = new CsvReader(reader))
                    {
                        //Truncate inbound sales staging file
                        var dal = new DapperDatabaseAccess<GenevaSalesInbound>(_connectionString);
                        dal.SqlExecute("dbo.prc_tbl_GenevaSmaAssets_Staging_Truncate", null);
                        
                        //Ingest sales into staging table
                        List<GenevaSalesInbound> inboundSales = csv.GetRecords<GenevaSalesInbound>().ToList();
                        DbHandlers.BulkInsertToDatabase(ListDataTable.ListToDataTable(inboundSales), _connectionString, 1000, _metaData.stagingTable);

                        //Transform from staging to production
                        var parms = new DynamicParameters();
                        parms.Add("@ReportEndDate", _reportingDate);
                        dal.SqlExecute("dbo.prc_GenevaSma_Transform", parms);
                    }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public void ProcessInboundAumFile()
        {
            try
            {
                using (var reader = new StreamReader(_path))
                    using (var csv = new CsvReader(reader))
                    {
                        //Truncate inbound sales staging file
                        var dal = new DapperDatabaseAccess<GenevaAumInbound>(_connectionString);
                        dal.SqlExecute("dbo.prc_tbl_GenevaAum_Staging_Truncate", null);
                        
                        //Ingest sales into staging table
                        List<GenevaAumInbound> inboundSales = csv.GetRecords<GenevaAumInbound>().ToList();
                        DbHandlers.BulkInsertToDatabase(ListDataTable.ListToDataTable(inboundSales), _connectionString, 1000, _metaData.stagingTable);

                        //Transform from staging to production
                        var parms = new DynamicParameters();
                        parms.Add("@ReportEndDate", _reportingDate);
                        dal.SqlExecute("dbo.prc_GenevaAum_Transform", parms);
                    }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}