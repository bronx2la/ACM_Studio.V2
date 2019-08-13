using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace AcmLogging
{
    public class PerformanceTracker
    {
        private readonly Stopwatch _stopwatch;
        private readonly AcmLoggingDetail _infoToLog;

        public PerformanceTracker(string message, string userId, string userName, string location, string product, string layer)
        {
            _stopwatch = Stopwatch.StartNew();
            _infoToLog = new AcmLoggingDetail()
            {
                Message = message,
                UserId = userId,
                UserName = userName,
                Product = product,
                Layer = layer,
                Location = location,
                Hostname = Environment.MachineName
            };

            var beginTime = DateTime.Now;
            _infoToLog.AdditionalInfo = new Dictionary<string, object>()
            {
                {"Started", beginTime.ToString(CultureInfo.InvariantCulture)}
            };
        }

        public PerformanceTracker(string message, string userId, string userName, 
                                  string location, string product, string layer,
                                 Dictionary<string, object> performanceParams) : this(message, userId, userName, location, product, layer)
        {
            foreach (var item in performanceParams)
                _infoToLog.AdditionalInfo.Add("input-" + item.Key, item.Value);
        }

        public void Stop()
        {
            _stopwatch.Stop();
            _infoToLog.ElapsedMilliseconds = _stopwatch.ElapsedMilliseconds;
            AcmLogger.WritePerformance(_infoToLog);
        }
    }
}