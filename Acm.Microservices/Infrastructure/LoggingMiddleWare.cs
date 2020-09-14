using System;
using System.Diagnostics;
using LibOwin;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Context;

namespace Acm.MicroServices.Infrastructure
{
    using AppFunc = Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;
    
    public class RequestLogging
    {
        private IConfigurationRoot _configuration; 

        public static AppFunc Middleware(AppFunc next, ILogger log)
        {
            return async env => 
            {
                var owinContext = new OwinContext(env);

                log.Information(
                    "Incoming request: {@RapidLogUid}, {@LogLevel}, {@Message}, {@Exception}, {@LogEvent}, {@LoggedOn}, {@LoggedBy} ",
                    env["correlationToken"],
                    "Information",
                    owinContext.Request.Headers,
                    string.Empty,
                    string.Empty,
                    DateTime.Now,
                    "Rapid2.0");
                await next(env);
                
                log.Information(
                    "Outgoing response: {@RapidLogUid}, {@LogLevel}, {@Message}, {@Exception}, {@LogEvent}, {@LoggedOn}, {@LoggedBy} ",
                    env["correlationToken"],
                    "Information",
                    owinContext.Response.StatusCode,
                    owinContext.Response.Headers,
                    string.Empty,
                    string.Empty,
                    DateTime.Now,
                    "Rapid2.0");
            };
        }
    }
    
    public class PerformanceLogging
    {
        public static AppFunc Middleware(AppFunc next, ILogger log)
        {
            return async env => 
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                await next(env);
                stopWatch.Stop();
                var owinContext = new OwinContext(env);
                log.Information("Request: {@RapidLogUid}, {@Method} {@Path} executed in {RequestTime:000} ms",
                                env["correlationToken"],
                                owinContext.Request.Method, owinContext.Request.Path, 
                                stopWatch.ElapsedMilliseconds);
            };
        }
    }
    
    public class CorrelationToken
    {
        public static AppFunc Middleware(AppFunc next)
        {
            return async env => 
            {
                Guid correlationToken;
                var  owinContext = new OwinContext(env);
                if (!(owinContext.Request.Headers["Correlation-Token"] != null
                      && Guid.TryParse(owinContext.Request.Headers["Correlation-Token"], out correlationToken)))
                    correlationToken = Guid.NewGuid();

                owinContext.Set("correlationToken", correlationToken.ToString());  
                using (LogContext.PushProperty("CorrelationToken", correlationToken))
                    await next(env);
            };
        }    
    }
    
    public class GlobalErrorLogging
    {
        public static AppFunc Middleware(AppFunc next, ILogger log)
        {
            return async env =>
            {
                try
                {
                    await next(env);  
                }
                catch (Exception ex)
                {
                    log.Error(ex, "Unhandled exception");
                }
            };
        }
    }
}