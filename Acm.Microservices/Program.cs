using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;


namespace Acm.MicroServices
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            IWebHost host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .UseUrls("http://localhost:5009")
                .Build();
            host.Run();
        }
    }
}