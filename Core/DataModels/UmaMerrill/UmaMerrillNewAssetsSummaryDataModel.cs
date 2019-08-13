using System;

namespace Core.DataModels.UmaMerrill
{
    public class UmaMerrillNewAssetsSummaryDataModel
    {
        public string RegionalDirectorKey { get; set; }
        public string OfficeStateCode { get; set; }
        public string Office { get; set; }
        public string FAName { get; set; }
        public decimal? PayableValue { get; set; }
        public decimal Rate { get; set; }
        public decimal? Commission { get; set; }
        public DateTime AristotleStartDate { get; set; }
    }
}