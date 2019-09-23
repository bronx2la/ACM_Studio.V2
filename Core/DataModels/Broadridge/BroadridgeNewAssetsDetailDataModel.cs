using System;

namespace Core.DataModels.Broadridge
{
    public class BroadridgeNewAssetsDetailDataModel
    {
        public string HoldingExterAccountNumber { get; set; }
        public DateTime HoldingCreateDate { get; set; }
        public string HoldingName { get; set; }
        public string FirmName { get; set; }
        public string FirmId { get; set; }
        public string FirmCrdNumber { get; set; }
        public string ProductName { get; set; }
        public string PersonFirstName { get; set; }
        public string PersonLastName { get; set; }
        public string Territory { get; set; }
        public string OfficeAddressLine1 { get; set; }
        public string OfficeCity { get; set; }
        public string OfficeRegionRefCode { get; set; }
        public decimal MarketValue { get; set; }
        public decimal Rate { get; set; }
        public decimal Commission { get; set; }
        public string  TheSystem { get; set; }
        public bool  IsNewAsset { get; set; }
        public string  IsTransfer { get; set; }
        public string  IsGrayStone { get; set; }
    }
}