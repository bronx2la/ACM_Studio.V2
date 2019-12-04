using System;

namespace Core.DataModels
{
    public class GenevaSmaOngoingDetailDataModel
    {
        public string RegionalDirector { get; set; }
        public string Portfolio { get; set; }
        public string PortShortName { get; set; }
        public DateTime Inception { get; set; }
        public string Goal { get; set; }
        public string ConsultantFirm { get; set; }
        public string ConsultantName { get; set; }
        public string IntMktPerson { get; set; }
        public DateTime ReportEndDate { get; set; }
        public decimal Total { get; set; }
        public decimal InFlows { get; set; }
        public decimal SeasonedValue { get; set; }
        public decimal Commission { get; set; }
        public decimal Rate { get; set; }
        public decimal AnnualRate { get; set; }
    }
}