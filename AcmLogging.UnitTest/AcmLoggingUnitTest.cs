using System;
using Dapper;
using DapperDatabaseAccess;
using Xunit;
//Testing GitHub
namespace AcmLogging.UnitTest
{
    public class Customer
    {
        public int     CustomerIid    { get; set; }
        public string  Name           { get; set; }
        public decimal TotalPurchases { get; set; }
        public decimal TotalReturns   { get; set; }
    }

    public class AcmLoggingUnitTest
    {
        public static string ConnectionString { get; } =
            @"Server=PLP0010662\SQLEXPRESS;Database=ACM_Studio_V2;Trusted_Connection=true;Connection Timeout=30;";

        [Fact] public void PerformanceLoggingTest()
        {
            var fd = GetAcmLoggingDetail("Starting Performance Unit Test", null);
            AcmLogger.WriteDiagnostic(fd);

            var tracker = new PerformanceTracker(message: "PerformanceTracker_UnitTest", userId: "NoUserId",
                                                 fd.UserName, fd.Location, fd.Product, fd.Layer);

            try
            {
                var ex = new Exception("Something wierd went down");
                ex.Data.Add("input param", "Nuphin to be lookin at here");
                throw ex;
            }
            catch (Exception e)
            {
                fd = GetAcmLoggingDetail("", e);
                AcmLogger.WriteError(fd);
            }

            fd = GetAcmLoggingDetail("Used unit test", null);
            AcmLogger.WriteUsage(fd);

            fd = GetAcmLoggingDetail("stoping the test", null);
            AcmLogger.WriteDiagnostic(fd);

            tracker.Stop();

            Assert.NotNull(tracker);
        }

        [Fact] public void StoredProcErrorTest()
        {
            using (var db = new DapperDatabaseAccess<Customer>(ConnectionString))
            {
                try
                {
                    var parms = new DynamicParameters();
                    parms.Add("@name", "SuperCalifragilisticexpialidocious");
                    parms.Add("@totalPurchases", 1200.00m);
                    parms.Add("@totalReturns", 200.00m);

                    db.SqlExecute("UnitTestCreateCustomer", parms);
                }
                catch (Exception ex)
                {
                    var efd = GetAcmLoggingDetail("", ex);
                    AcmLogger.WriteError(efd);
                }
            }

            Assert.True(true);
        }

        private AcmLoggingDetail GetAcmLoggingDetail(string message, Exception ex)
        {
            return new AcmLoggingDetail()
                   {
                       Product   = "AcmLoggingUnitTest",
                       Location  = "PerformanceLoggingTest",
                       Layer     = "xUnit Test",
                       UserName  = Environment.UserName,
                       Hostname  = Environment.MachineName,
                       Message   = message,
                       Exception = ex
                   };
        }
    }
}