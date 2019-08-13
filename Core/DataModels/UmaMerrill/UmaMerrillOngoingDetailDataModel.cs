using System;

namespace Core.DataModels.UmaMerrill
{
    public class UmaMerrillOngoingDetailDataModel
    {
        public string RegionalDirector { get; set; }
        public string UniqueID { get; set; }
        public string OfficeNumber { get; set; }
        public string Office { get; set; }
        public string OfficeStateCode { get; set; }
        public string FAnum { get; set; }
        public string FAName { get; set; }
        public decimal MonthEndValue { get; set; }
        public decimal Flows { get; set; }
        public decimal AnnualRate { get; set; }
        public decimal Rate { get; set; }
        public decimal PayableAmount { get; set; }
        public decimal Commission { get; set; }
        public DateTime MonthEnd { get; set; }
        public DateTime AristotleStartDate { get; set; }
    }
}