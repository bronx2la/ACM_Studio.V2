using System;

namespace Core.DataModels
{
    public class TaskMetaDataDataModel
    {
        public int      taskMetaDataIID { get; set; }
        public string   objectCode      { get; set; }
        public int      environmentIID  { get; set; }
        public string   dbServer        { get; set; }
        public string   databaseName    { get; set; }
        public string   stagingTable    { get; set; }
        public string   prodTable       { get; set; }
        public int      fileTypeIID     { get; set; }
        public string   delimiterChar   { get; set; }
        public string   inboundFolder   { get; set; }
        public string   inboundFile     { get; set; }
        public string   outboundFolder  { get; set; }
        public string   outboundFile    { get; set; }
        public bool     isTruncatable   { get; set; }
        public DateTime createdOn       { get; set; }
        public DateTime modifiedOn      { get; set; }
        public string   modifiedBy      { get; set; }
        public int      runStatus       { get; set; }
    }
}