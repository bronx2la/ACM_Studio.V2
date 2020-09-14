using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibOwin;
using Microsoft.AspNetCore.Http;

namespace Acm.MicroServices.Infrastructure
{
    using AppFunc = Func<IDictionary<string, object>, Task>;
    
    public class MonitoringMiddleware
    {
        private AppFunc          next;
        private Func<Task<bool>> healthCheck;
        
        private static readonly PathString monitorPath        = new PathString("/_monitor");
        private static readonly PathString monitorShallowPath = new PathString("/_monitor/shallow");
        private static readonly PathString monitorDeepPath    = new PathString("/_monitor/deep");

        public MonitoringMiddleware(AppFunc next, Func<Task<bool>> healthCheck)
        {
            this.next        = next;
            this.healthCheck = healthCheck;
        }

        public Task Invoke(IDictionary<string, object> env)
        {
            var context = new OwinContext(env);
            return context.Request.Path.Equals("/monitor/shallow") ? ShallowEndpoint(context) : next(env);
        }
        
        private Task HandleMonitorEndpoint(OwinContext context)
        {
            if (context.Request.Path.StartsWithSegments(monitorShallowPath))
                return ShallowEndpoint(context);
            return context.Request.Path.StartsWithSegments(monitorDeepPath) ? DeepEndpoint(context) : Task.FromResult(0);
        }
        
        private async Task DeepEndpoint(IOwinContext context)
        {
            if (await healthCheck())
                context.Response.StatusCode = 204;
            else
                context.Response.StatusCode = 503;
        }

        private static Task ShallowEndpoint(IOwinContext context)
        {
            context.Response.StatusCode = 204;
            return Task.FromResult(0);
        }
    }
}