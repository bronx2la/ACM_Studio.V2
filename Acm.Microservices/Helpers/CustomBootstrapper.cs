using Microsoft.Extensions.Configuration;
using Nancy;
using Nancy.TinyIoc;

namespace Acm.MicroServices.Helpers
{
    public class CustomBootstrapper : DefaultNancyBootstrapper
    {
        public static IConfigurationRoot Configuration;

        public CustomBootstrapper()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(RootPathProvider.GetRootPath())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }
        
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            container.Register<IConfiguration>(Configuration);
        }
    }
}