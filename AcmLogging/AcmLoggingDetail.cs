using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AcmLogging
{
    public class AcmLoggingDetail
    {
        public DateTime Timestamp { get; private set; }
        public string Message { get; set; }
        
        //Log from where properties
        public string Product { get; set; }
        public string Layer { get; set; }
        public string Location { get; set; }
        public string Hostname { get; set; }
        
        //Log from who properties
        public string UserId { get; set; }
        public string UserName { get; set; }
        
        //Everything else
        public long? ElapsedMilliseconds { get; set; }
        public Exception Exception { get; set; }
        public string CorrelationId { get; set; }
        public Dictionary<string, object> AdditionalInfo { get; set; }
        
        public AcmLoggingDetail()
        {
            Timestamp = DateTime.Now;
        }
    }
}