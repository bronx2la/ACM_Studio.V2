namespace Core.DataModels
{
    public class GenevaSmaOngoingSummaryDataModel
    {
        public string RegionalDirector { get; set; }
        public string ConsultantFirm { get; set; }
        public string ConsultantName { get; set; }
        public decimal AUM { get; set; }
        public decimal Inflows { get; set; }
        public decimal SeasonedValue { get; set; }
        public decimal Commission { get; set; }
        public decimal Rate { get; set; }
        public decimal AnnualRate { get; set; }
    }
}