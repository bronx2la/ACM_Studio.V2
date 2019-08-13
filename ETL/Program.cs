using System;
using System.IO;
using System.Linq;
using Core.DataModels;
using Dapper;
using DapperDatabaseAccess;
using Microsoft.Extensions.Configuration;

namespace ETL
{
    internal static class Program
    {
        private enum ExeRunStatus { Success = 0, Failure = 1 }
        private static int      EnvironmentIid { get; set; }
        private static string[] ObjectCode     { get; set; }
        private static DateTime StartDate      { get; set; }
        private static DateTime EndDate        { get; set; }
        
        private static string                                      _connectionString;
        private static IConfigurationRoot                          _configuration;
        private static TaskMetaDataDataModel                       _metaData;
        private static DapperDatabaseAccess<TaskMetaDataDataModel> _dal;
        
        private static int Main(string[] args)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()           
                         .SetBasePath(Directory.GetCurrentDirectory())
                         .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            _configuration    = builder.Build();
            _connectionString = _configuration["Database:ConnectionString"];

            try
            {
                //Load task metadata
                _dal = new DapperDatabaseAccess<TaskMetaDataDataModel>(_connectionString);
                var parms = new DynamicParameters();
                parms.Add("@ObjectCode", args[0]);
                _metaData = _dal.SqlServerFetch("dbo.prc_TaskMetaData_Fetch_By_ObjectCode", parms).FirstOrDefault();

                switch (_metaData.objectCode)
                {
                    case "ETL.BROADRIDGE.SALES":
                        var broadridgeSales = new Etl_Broadridge(_metaData, _configuration, _connectionString, args[1]);
                        broadridgeSales.ProcessInboundSalesFile();
                        break;
                    case "ETL.BROADRIDGE.ASSETS":
                        var broadridgeAssets = new Etl_Broadridge(_metaData, _configuration, _connectionString, args[1]);
                        broadridgeAssets.ProcessInboundAssetsFile();
                        break;
                    case "ETL.GENEVA.SMA":
                        var genevaSales = new Etl_Geneva(_metaData, _configuration, _connectionString, args[1]);
                        genevaSales.ProcessInboundSalesFile();
                        break;
                    case "ETL.GENEVA.AUM":
                        var genevaAum = new Etl_Geneva(_metaData, _configuration, _connectionString, args[1]);
                        genevaAum.ProcessInboundAumFile();
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           
            Console.WriteLine("Closing the application");
            Console.WriteLine("Hit enter...");
            Console.ReadLine();
            return (int) ExeRunStatus.Success; 
        }
    }
}