using System;
using System.Linq;
using System.Reflection;
using DbUp.Engine;
using DbUp.Helpers;

namespace DbUp.AcmStudio.V2.DbMigrator
{
    internal static class Program
    {
        private static int Main(string[] args)
        {
            string connectionString = string.Empty;

            string firstOrDefault = args.FirstOrDefault();
            if (firstOrDefault == null) return -1;

            switch (firstOrDefault.ToUpper())
            {
                case "PLANO":
                    connectionString =
                        @"Server=PDP0010469\SQLEXPRESS;Database=ACM_Studio_V2;Trusted_Connection=True;Connection Timeout=300;";
                    break;
                case "FRISCO":
                    connectionString =
                        @"Server=PLP0010662\SQLEXPRESS;Database=ACM_Studio_V2;Trusted_Connection=true;Connection Timeout=30;";
                    break;
            }

            EnsureDatabase.For.SqlDatabase(connectionString);

            UpgradeEngine upgrader = DeployChanges.To
                                        .SqlDatabase(connectionString)
                                        .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), f => IsTable(f))
                                        .JournalToSqlTable("dbo", "SchemaVersions")
//                                        .JournalTo(new NullJournal())
                                        .WithTransaction()
                                        .LogToConsole()
                                        .Build();
            
//            UpgradeEngine upgrader = DeployChanges.To
//                .SqlDatabase(connectionString)
//                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
//                .LogToConsole()
//                .JournalTo(new NullJournal())
//                .WithTransaction()
//                .Build();

            UpgradeEngine functionUpgrader = DeployChanges.To
                                                          .SqlDatabase(connectionString)
                                                          .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), f => IsFunction(f))
                                                          .LogToConsole()
                                                          .JournalTo(new NullJournal())
                                                          .WithTransaction()
                                                          .Build();

            UpgradeEngine storedProcedureUpgrader = DeployChanges.To
                                                                 .SqlDatabase(connectionString)
                                                                 .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), s => IsStoredProc(s))
                                                                 .LogToConsole()
                                                                 .JournalTo(new NullJournal())
                                                                 .WithTransaction()
                                                                 .Build();

            UpgradeEngine refDataSyncUpgrader = DeployChanges.To
                                                             .SqlDatabase(connectionString)
                                                             .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), s => IsRefDataSync(s))
                                                             .LogToConsole()
                                                             .JournalTo(new NullJournal())
                                                             .WithTransaction()
                                                             .Build();

            if (!UpgradeAndLog(upgrader))
                return 1;

            if (!UpgradeAndLog(functionUpgrader))
                return 1;

            if (!UpgradeAndLog(storedProcedureUpgrader))
                return 1;

            if (!UpgradeAndLog(refDataSyncUpgrader))
                return 1;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
            return 0;
        }

        private static bool UpgradeAndLog(UpgradeEngine upgrader)
        {
            var returnvalue = true;
            try
            {
                DatabaseUpgradeResult result = upgrader.PerformUpgrade();
                if (!result.Successful)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(result.Error);
                    Console.ResetColor();
                }
            }
            catch (Exception e)
            {
                returnvalue = false;
                Console.WriteLine(e);
                throw;
            }
            return returnvalue;
        }

        private static bool IsTable(string scriptName)
        {
            return scriptName.ToUpper().Contains(".TBL_");
        }
        
        private static bool IsFunction(string scriptName)
        {
            return scriptName.ToUpper().Contains(".FN_");
        }

        private static bool IsStoredProc(string scriptName)
        {
            return scriptName.ToUpper().Contains(".PRC_");
        }

        private static bool IsRefDataSync(string scriptName)
        {
            return scriptName.ToUpper().Contains(".REFDATASYNC_");
        }
    }
}