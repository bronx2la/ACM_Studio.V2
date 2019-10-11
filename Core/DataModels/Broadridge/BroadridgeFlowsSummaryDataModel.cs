namespace Core.DataModels.Broadridge
{
    public class BroadridgeFlowsSummaryDataModel
    {
        public string Territory { get; set; }
        public string TheSystem { get; set; }
        public string SystemOfficeAddress { get; set; }
        public string OfficeState { get; set; }
        public string ProductName { get; set; }
        public string SystemFAName { get; set; }
        public decimal Flows { get; set; }
        public decimal Commission { get; set; }
        public decimal Rate { get; set; }
    }
}