namespace Core.DataModels.Broadridge
{
    public class BroadridgeNewAssetsSummaryDataModel
    {
        public string Territory { get; set; }
        public string TheSystem { get; set; }
        public string OfficeAddress { get; set; }
        public string OfficeState { get; set; }
        public string ProductName { get; set; }

        public string FAName { get; set; }
        public string PersonLastName { get; set; }
        public decimal NewAssetValue { get; set; }
        public decimal Commission { get; set; }
        public decimal Rate { get; set; }
    }
}