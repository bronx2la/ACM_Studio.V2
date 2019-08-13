namespace Core.DataModels.Broadridge
{
    public class BroadridgeOgSummaryDataModel
    {
        public string RM { get; set; }
        public string ConsultantName { get; set; }
        public decimal AUM { get; set; }
        public decimal InFlows { get; set; }
        public decimal SeasonedValue { get; set; }
        public decimal Rate { get; set; }
        public decimal Commission { get; set; }
    }
}