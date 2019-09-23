using System;
using System.Linq;
using Core.DataModels;
using Dapper;
using DapperDatabaseAccess;
using CommissionsStatementGenerator;
using Microsoft.Extensions.Configuration;

namespace AcmCommissionsStatements
{
    internal static class Program
    {
        private static string                                      _connectionString;
        private static IConfigurationRoot                          _configuration;
        private static TaskMetaDataDataModel                       _metaData;
        private static DapperDatabaseAccess<TaskMetaDataDataModel> _dal;
        
        private static int Main(string[] args)
        {

            //Initialize system from appsettings.json
            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            _configuration    = builder.Build();
            _connectionString = _configuration["Database:ConnectionString"];

            //Load task metadata
            _dal = new DapperDatabaseAccess<TaskMetaDataDataModel>(_connectionString);
            var parms = new DynamicParameters();
            parms.Add("@ObjectCode", args[0]);
            _metaData = _dal.SqlServerFetch("dbo.prc_TaskMetaData_Fetch_By_ObjectCode", parms).FirstOrDefault();

            switch (_metaData.objectCode)
            {
                case "COMM.GENEVA.SMA":
                    var smaReport = new GenevaSmaStatement(args[1], args[2], _connectionString, _metaData);
                    smaReport.ProduceReport();
                    break;
                case "COMM.BROADRIDGE.UMA":
                    var broadRidgeStatement = new BroadridgeStatement(args[1], args[2], _connectionString, _metaData, true);
                    broadRidgeStatement.ProduceNonMerrillMorganReport();
                    break;
                case "COMM.BROADRIDGE.MERRILL_MORGAN.UMA":
                    var broadRidgeMerrillMorganStatement = new BroadridgeMerrillMorganStatement(args[1], _connectionString, _metaData);
                    broadRidgeMerrillMorganStatement.ProduceReport();
                    break;
                case "COMM.BROADRIDGE.MUTUALFUNDS":
                    var broadRidgeMfStatement = new BroadridgeStatement(args[1], args[2], _connectionString, _metaData, false);
                    broadRidgeMfStatement.ProduceReport();
                    break;
            }

            return 0;
        }
    }
}