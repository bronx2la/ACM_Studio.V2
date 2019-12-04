namespace Core.DataModels
{
    public class GenevaSmaOngoingSummaryDataModel
    {
        public string RowLabels { get; set; }
        public string ConsultantFirm { get; set; }
        public string ConsultantName { get; set; }
        public string Goal { get; set; }
        public decimal Total { get; set; }
        public decimal Inflows { get; set; }
        public decimal SeasonedValue { get; set; }
        public decimal Commission { get; set; }
        public decimal Rate { get; set; }
        public decimal AnnualRate { get; set; }
    }
}