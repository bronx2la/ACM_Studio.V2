namespace Core.DataModels.Broadridge
{
    public class FinalSummaryDataModel
    {
        public string RegionalDirector { get; set; }
        public string CommissionType { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Commission { get; set; }
        public decimal? AverageRate { get; set; }
    }
}