using System;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Serilog;
using Serilog.Events;

namespace AcmLogging
{
    public static class AcmLogger
    {
        private static readonly ILogger _performanceLogger;
        private static readonly ILogger _usageLogger;
        private static readonly ILogger _errorLogger;
        private static readonly ILogger _diagnosticLogger;

        static AcmLogger()
        {
            _performanceLogger = new LoggerConfiguration()
                .WriteTo.File(@"c:\Development.Acm\Logging\performance.txt")
                .CreateLogger();
            
            _usageLogger = new LoggerConfiguration()
                .WriteTo.File(@"c:\Development.Acm\Logging\usage.txt")
                .CreateLogger();
            
            _errorLogger = new LoggerConfiguration()
                .WriteTo.File(@"c:\Development.Acm\Logging\error.txt")
                .CreateLogger();
            
            _diagnosticLogger = new LoggerConfiguration()
                .WriteTo.File(@"c:\Development.Acm\Logging\diagnostic.txt")
                .CreateLogger();
        }

        public static void WritePerformance(AcmLoggingDetail infoToLog)
        {
            _performanceLogger.Write(LogEventLevel.Information, "{@AcmLoggingDetail}", infoToLog);
        }
        
        public static void WriteUsage(AcmLoggingDetail infoToLog)
        {
            _usageLogger.Write(LogEventLevel.Information, "{@AcmLoggingDetail}", infoToLog);
        }
        
        public static void WriteError(AcmLoggingDetail infoToLog)
        {
            if (infoToLog.Exception != null)
            {
                var procName = FindProcName(infoToLog.Exception);
                infoToLog.Location = string.IsNullOrEmpty(procName) ? infoToLog.Location : procName;
                infoToLog.Message = GetMessageFromException(infoToLog.Exception);
            }
            
            _errorLogger.Write(LogEventLevel.Information, "{@AcmLoggingDetail}", infoToLog);
        }
        
        public static void WriteDiagnostic(AcmLoggingDetail infoToLog)
        {
            var writeDiagnostics = true;
            if (!writeDiagnostics)
                return;
            
            _diagnosticLogger.Write(LogEventLevel.Information, "{@AcmLoggingDetail}", infoToLog);
        }

        private static string GetMessageFromException(Exception ex)
        {
            return ex.InnerException != null ? GetMessageFromException(ex.InnerException) : ex.Message;
        }

        private static string FindProcName(Exception ex)
        {
            var sqlEx = ex as SqlException;
            if (sqlEx != null)
            {
                var procName = sqlEx.Procedure;
                if (!string.IsNullOrEmpty((string) ex.Data["Procedure"]))
                    return (string) ex.Data["Procedure"];
            }

            if (ex.InnerException != null)
                    return FindProcName(ex.InnerException);

            return null;
        }
    }
}