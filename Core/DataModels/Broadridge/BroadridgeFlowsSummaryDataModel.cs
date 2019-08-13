namespace Core.DataModels.Broadridge
{
    public class BroadridgeFlowsSummaryDataModel
    {
        public string FirmName { get; set; }
        public string Territory { get; set; }
        public decimal SumFlowAmount { get; set; }
        public decimal Rate { get; set; }
        public decimal SumCommission { get; set; }
    }
}