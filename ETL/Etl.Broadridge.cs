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

                        foreach (var item in inboundSales)
                        {
                            var p = new DynamicParameters();
                            p.Add("@TradeID", item.TradeID);
                            p.Add("@TransactionCodeOverrideDescription", item.TransactionCodeOverrideDescription);
                            p.Add("@TradeDate", item.TradeDate);
                            p.Add("@SettledDate", item.SettledDate);
                            p.Add("@SuperSheetDate", item.SuperSheetDate);
                            p.Add("@TradeAmount", item.TradeAmount);
                            p.Add("@System", item.System);
                            p.Add("@DealerNum", item.DealerNum);
                            p.Add("@DealerBranchBranchCode", item.DealerBranchBranchCode);
                            p.Add("@RepCode", item.RepCode);
                            p.Add("@FirmId", item.FirmId);
                            p.Add("@FirmName", item.FirmName);
                            p.Add("@OfficeAddressLine1", item.OfficeAddressLine1);
                            p.Add("@OfficeCity", item.OfficeCity);
                            p.Add("@OfficeRegionRefCode", item.OfficeRegionRefCode);
                            p.Add("@OfficePostalCode", item.OfficePostalCode);
                            p.Add("@PersonFirstName", item.PersonFirstName);
                            p.Add("@PersonLastName", item.PersonLastName);
                            p.Add("@LineOfBusiness", item.LineOfBusiness);
                            p.Add("@Channel", item.Channel);
                            p.Add("@Region", item.Region);
                            p.Add("@Territory", item.Territory);
                            p.Add("@ProductNasdaqSymbol", item.ProductNasdaqSymbol);
                            p.Add("@ProductName", item.ProductName);
                            p.Add("@AccountTANumber", item.AccountTANumber);
                            p.Add("@AccountId", item.AccountId);
                            p.Add("@ExternalAccountNumber", item.ExternalAccountNumber);
                            p.Add("@HoldingId", item.HoldingId);
                            p.Add("@HoldingExternalAccountNumber", item.HoldingExternalAccountNumber);
                            p.Add("@HoldingName", item.HoldingName);
                            dal.SqlExecute("dbo.prc_BroadridgeSales_Staging_Save", p);
                        }

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
                        List<BroadridgeAssetsInbound> inboundAssets = csv.GetRecords<BroadridgeAssetsInbound>().ToList();
//                        DbHandlers.BulkInsertToDatabase(ListDataTable.ListToDataTable(inboundAssets), _connectionString, 1000, _metaData.stagingTable);

                        foreach (var item in inboundAssets)
                        {
                            var p = new DynamicParameters();
                            p.Add("@System", item.System);
                            p.Add("@FirmName", item.FirmName);
                            p.Add("@FirmId", item.FirmId);
                            p.Add("@FirmCRDNumber", item.FirmCRDNumber);
                            p.Add("@HoldingId", item.HoldingId);
                            p.Add("@HoldingExternalAccountNumber", item.HoldingExternalAccountNumber);
                            p.Add("@HoldingName", item.HoldingName);
                            p.Add("@HoldingStartDate", item.HoldingStartDate);
                            p.Add("@HoldingCreateDate", item.HoldingCreateDate);
                            p.Add("@MostRecentMonthAssetBalance", item.MostRecentMonthAssetBalance);
                            p.Add("@Month1AgoAssetBalance", item.Month1AgoAssetBalance);
                            p.Add("@Month2AgoAssetBalance", item.Month2AgoAssetBalance);
                            p.Add("@Month3AgoAssetBalance", item.Month3AgoAssetBalance);
                            p.Add("@ProductName", item.ProductName);
                            p.Add("@ProductType", item.ProductType);
                            p.Add("@Channel", item.Channel);
                            p.Add("@Region", item.Region);
                            p.Add("@Territory", item.Territory);
                            p.Add("@PersonCRDNumber", item.PersonCRDNumber);
                            p.Add("@PersonFirstName", item.PersonFirstName);
                            p.Add("@PersonLastName", item.PersonLastName);
                            p.Add("@PersonId", item.PersonId);
                            p.Add("@OfficeAddressLine1", item.OfficeAddressLine1);
                            p.Add("@OfficeAddressLine2", item.OfficeAddressLine2);
                            p.Add("@OfficeCity", item.OfficeCity);
                            p.Add("@OfficeRegionRefCode", item.OfficeRegionRefCode);
                            p.Add("@OfficePostalCode", item.OfficePostalCode);
                            p.Add("@PersonBrokerTeamFlag", item.PersonBrokerTeamFlag);
                            p.Add("@Month4AgoAssetBalance", item.Month4AgoAssetBalance);
                            p.Add("@Month5AgoAssetBalance", item.Month5AgoAssetBalance);
                            p.Add("@Month6AgoAssetBalance", item.Month6AgoAssetBalance);
                            p.Add("@Month7AgoAssetBalance", item.Month7AgoAssetBalance);
                            p.Add("@Month8AgoAssetBalance", item.Month8AgoAssetBalance);
                            p.Add("@Month9AgoAssetBalance", item.Month9AgoAssetBalance);
                            p.Add("@Month10AgoAssetBalance", item.Month10AgoAssetBalance);
                            p.Add("@Month11AgoAssetBalance", item.Month11AgoAssetBalance);
                            p.Add("@Month12AgoAssetBalance", item.Month12AgoAssetBalance);
                            p.Add("@HoldingAddressLine1", item.HoldingAddressLine1);
                            p.Add("@AccountTANumber", item.AccountTANumber);
                            p.Add("@ExternalAccountNumber", item.ExternalAccountNumber);
                            p.Add("@AccountId", item.AccountId);
                            dal.SqlExecute("dbo.prc_BroadridgeAssets_Staging_Save", p);
                        }
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