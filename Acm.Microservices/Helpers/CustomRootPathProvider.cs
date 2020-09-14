using System.IO;
using Nancy;

namespace Acm.MicroServices.Helpers
{
    public class CustomRootPathProvider : IRootPathProvider
    {
        public string GetRootPath()
        {
            return Directory.GetCurrentDirectory();
        }
    }
}