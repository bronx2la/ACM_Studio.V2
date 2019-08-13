using System;

namespace Core
{
    public static class FileNameHelpers
    {
        public static string FormatOutboundFileName(string b)
        {
            return string.Format(b, DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.ToString("HHmmss"));
        }
    }
}