using System;

namespace Core.DataModels.Broadridge
{
    public class BroadridgeFlows
    {
        public string FirmName { get; set; }
        public string FirmId { get; set; }
        public string FirmCRDNumber { get; set; }
        public string   Portfolio     { get; set; }
        public string HoldingExternalAccountNumber { get; set; }
        public string HoldingName { get; set; }
        public string ProductName { get; set; }
        public string Territory { get; set; }
        public string PersonFirstName { get; set; }
        public string PersonLastName { get; set; }
        public string OfficeCity { get; set; }
        public DateTime PortStartDate { get; set; }
        public decimal MostRecentMonthAssetBalance { get; set; }
        public decimal Month1AgoAssetBalance { get; set; }
        public decimal Month2AgoAssetBalance { get; set; }
        public decimal Month3AgoAssetBalance { get; set; }

        public decimal Diff1 { get; set; }
        public decimal Diff2 { get; set; }
        public decimal Diff3 { get; set; }
        public decimal  FlowAmount    { get; set; }
        public decimal Rate { get; set; }
        public decimal Commission { get; set; }
    }
}