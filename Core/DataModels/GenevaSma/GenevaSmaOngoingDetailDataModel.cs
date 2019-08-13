using System;

namespace Core.DataModels
{
    public class GenevaSmaOngoingDetailDataModel
    {
        public string RM { get; set; }
        public string Portfolio { get; set; }
        public string PortShortName { get; set; }
        public DateTime PortStartDate { get; set; }
        public string Strategy { get; set; }
        public string ConsultantFirm { get; set; }
        public string ConsultantName { get; set; }
        public DateTime ReportEndDate { get; set; }
        public decimal AUM { get; set; }
        public decimal InFlows { get; set; }
        public decimal SeasonedValue { get; set; }
        public decimal Commission { get; set; }
        public decimal Rate { get; set; }
        public decimal AnnualRate { get; set; }
        public int SmaOngoing { get; set; }
    }
}