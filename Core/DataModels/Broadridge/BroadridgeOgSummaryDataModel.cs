namespace Core.DataModels.Broadridge
{
    public class BroadridgeOgSummaryDataModel
    {
        public string RM { get; set; }
        public string SystemOfficeAddress { get; set; }
        public string TheSystem { get; set; }
        public string State { get; set; }
        public string SystemFAName { get; set; }
        public decimal SystemQuarterEndAssets { get; set; }
        public decimal Flows { get; set; }
        public decimal PayableAmount { get; set; }
        public decimal Commission { get; set; }
        public decimal QuarterlyRate { get; set; }
        public decimal AnnualRate { get; set; }
    }
}