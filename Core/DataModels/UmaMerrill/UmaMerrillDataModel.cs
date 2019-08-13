using System;

namespace Core.DataModels.UmaMerrill
{
    public class UmaMerrillDataModel
    {
        public int Id { get; set; }
        public DateTime AsOfDate { get; set; }
        public DateTime MonthEnd { get; set; }
        public string SleeveManagerID { get; set; }
        public string SleeveManagerName { get; set; }
        public string UDPModel_UDPSelects_Standalone { get; set; }
        public string ModelID { get; set; }
        public string ModelName_RiskProfile { get; set; }
        public string UniqueID { get; set; }
        public DateTime StrategyEnrollmentDate { get; set; }
        public DateTime TerminationDate { get; set; }
        public string Division { get; set; }
        public string Region { get; set; }
        public string ComplexNumber { get; set; }
        public string Complex { get; set; }
        public string OfficeNumber { get; set; }
        public string Office { get; set; }
        public string OfficeLine1Address { get; set; }
        public string OfficeLine2Address { get; set; }
        public string OfficeLine3Address { get; set; }
        public string OfficeStateCode { get; set; }
        public string ZipCode { get; set; }
        public string FANum { get; set; }
        public string FAName { get; set; }
        public decimal PercentOfAllocation { get; set; }
        public decimal TotalAssets { get; set; }
        public string RegionalDirector { get; set; }
        public int RegionalDirectorIID { get; set; }
    }
}