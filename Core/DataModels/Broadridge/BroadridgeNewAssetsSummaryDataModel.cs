namespace Core.DataModels.Broadridge
{
    public class BroadridgeNewAssetsSummaryDataModel
    {
        public string TheSystem { get; set; }
        public string Territory { get; set; }
        public string FirmName { get; set; }
        public string OfficeRegionRefCode { get; set; }
        public string OfficeCity { get; set; }
        public string OfficeAddress { get; set; }
        public string FAName { get; set; }
        public string PersonLastName { get; set; }
        public decimal MarketValue { get; set; }
        public decimal Rate { get; set; }
        public decimal Commission { get; set; }
    }
}