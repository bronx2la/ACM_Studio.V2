using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Acm.MicroServices.Helpers;
using Acm.MicroServices.Infrastructure;
using Microsoft.AspNetCore.Builder;
// using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nancy.Owin;
using AcmDataModels;
using Microsoft.AspNetCore.Hosting;
// using T3.Microservices.Csa.Helpers;
// using T3.Microservices.Csa.Infrastructure;
// using T3.TFS.Toyota.com.Db;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Acm.MicroServices
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddCors(options => options.AddPolicy("Cors", builder =>
            {
                builder
                   .AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
            }));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("Cors");

            Logger log = ConfigurationLogger();
            
            app.UseOwin(buildFunc => 
            {
                buildFunc(next => GlobalErrorLogging.Middleware(next, log));
                buildFunc(next => CorrelationToken.Middleware(next));
                buildFunc(next => RequestLogging.Middleware(next, log));
                buildFunc(next => PerformanceLogging.Middleware(next, log));
                buildFunc(next => new MonitoringMiddleware(next, HealthCheck).Invoke);
                buildFunc.UseNancy(opt => opt.Bootstrapper = new CustomBootstrapper());
            });
            
        }

        private async Task<bool> HealthCheck()
        {
            var env = CustomBootstrapper.Configuration["RuntimeEnvironment:Environment"];
            var dal = new DapperDatabaseAccess<R2D2HealthcheckPortfolioSwapCounterpartyActivity>(CustomBootstrapper.Configuration[$"{env}:R2D2:ConnectionString"]);
            var rowcount = dal.SqlQuery("select count(CounterpartyActivityIid) from dbo.PortfolioSwapCounterpartyActivity").FirstOrDefault();
            return rowcount != null && rowcount.ActivityCount > 0;
        }

        private static Logger ConfigurationLogger()
        {
            IConfiguration config = new ConfigurationBuilder()
                                   .AddJsonFile("appsettings.json", true, true)
                                   .Build();

            string rollingFileName = config.GetSection("RapidLogging")["AppSettingsRollingFile"];
            
            Logger logger =  new LoggerConfiguration()
                   .Enrich.FromLogContext()
                   .MinimumLevel.Debug()
                   .WriteTo.RollingFile(rollingFileName, retainedFileCountLimit:5, shared:true)  
                   .WriteTo.ColoredConsole(
                        LogEventLevel.Verbose,
                        "{NewLine}{Timestamp:HH:mm:ss} [{Level}] ({CorrelationToken}) {Message}{NewLine}{Exception}")
                   .CreateLogger();
            
            Serilog.Debugging.SelfLog.Enable(msg =>
            {
                Debug.Print(msg);
                //Debugger.Break();
            });

            return logger;
        }
    }
}