using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Core.DataModels;
using Core.DataModels.Broadridge;
using Core.DateTimeHandlers;
using Core.DbHandlers;
using Core.ListHandlers;
using CsvHelper;
using Dapper;
using Microsoft.Extensions.Configuration;
using DapperDatabaseAccess;

namespace ETL
{
    public class Etl_Broadridge
    {
        private static string                _connectionString;
        private static IConfigurationRoot    _configuration;
        private static TaskMetaDataDataModel _metaData;
        private static string _path;
        private static string _reportingDate;

        public Etl_Broadridge(TaskMetaDataDataModel metaData, IConfigurationRoot configuration, string connectionString, string reportingDate)
        {
            _metaData = metaData;
            _configuration = configuration;
            _connectionString = connectionString;
            _reportingDate = reportingDate;

            string fyle = string.Format(_metaData.inboundFile, _reportingDate);
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
                        var dal = new DapperDatabaseAccess<BroadridgeSalesInbound>(_connectionString);
                        dal.SqlExecute("dbo.prc_BroadridgeSales_Staging_Truncate", null);
                        
                        //Ingest sales into staging table
                        List<BroadridgeSalesInbound> inboundSales = csv.GetRecords<BroadridgeSalesInbound>().ToList();
                        DbHandlers.BulkInsertToDatabase(ListDataTable.ListToDataTable(inboundSales), _connectionString, 1000, _metaData.stagingTable);

                        //Transform from staging to production
                        var parms = new DynamicParameters();
                        parms.Add("@ReportingDate", DateTimeHandlers.GetQuarterEndDateFromYearQuarter(_reportingDate).ToString("yyyyMMdd"));
//                        parms.Add("@ReportingDate", _reportingDate);
                        dal.SqlExecute("dbo.prc_BroadridgeSales_Transform", parms);
                    }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void ProcessInboundAssetsFile()
        {
            try
            {
                using (var reader = new StreamReader(_path))
                    using (var csv = new CsvReader(reader))
                    {
                        //Truncate inbound sales staging file
                        var dal = new DapperDatabaseAccess<BroadridgeAssetsInbound>(_connectionString);
                        dal.SqlExecute("dbo.prc_BroadridgeAssets_Staging_Truncate", null);
                        
                        //Ingest sales into staging table
                        List<BroadridgeAssetsInbound> inboundSales = csv.GetRecords<BroadridgeAssetsInbound>().ToList();
                        DbHandlers.BulkInsertToDatabase(ListDataTable.ListToDataTable(inboundSales), _connectionString, 1000, _metaData.stagingTable);

                        //Transform from staging to production
                        var parms = new DynamicParameters();
                        parms.Add("@ReportingDate", DateTimeHandlers.GetQuarterEndDateFromYearQuarter(_reportingDate).ToString("yyyyMMdd"));
                        dal.SqlExecute("dbo.prc_BroadridgeAssets_Transform", parms);
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