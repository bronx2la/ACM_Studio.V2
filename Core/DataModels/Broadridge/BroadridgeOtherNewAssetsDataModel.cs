using System;

namespace Core.DataModels.Broadridge
{
    public class BroadridgeOtherNewAssetsDataModel
    {
        public string TradeId { get; set; }
        public string TransactionCodeOverrideDescription { get; set; }
        public DateTime TradeDate { get; set; }
        public DateTime SettledDate { get; set; }
        public DateTime SuperSheetDate { get; set; }
        public decimal TradeAmount { get; set; }
        public decimal Commission { get; set; }
        public decimal Rate { get; set; }
        public string System { get; set; }
        public string DealerNum { get; set; }
        public string DealerBranchCode { get; set; }
        public string RepCode { get; set; }
        public string FirmId { get; set; }
        public string FirmName { get; set; }
        public string OfficeAddressLine1 { get; set; }
        public string OfficeCity { get; set; }
        public string OfficeRegionRefCode { get; set; }
        public string officePostalCode { get; set; }
        public string PersonFirstName { get; set; }
        public string PersonLastName { get; set; }
        public string LineOfBusiness { get; set; }
        public string Channel { get; set; }
        public string Region { get; set; }
        public string Territory { get; set; }
        public string ProductNasdaqSymbol { get; set; }
        public string ProductName { get; set; }
        public string AccountTANumber { get; set; }
        public string AccountId { get; set; }
        public string ExternalAccountNumber { get; set; }
        public string HoldingId { get; set; }
        public string HoldingExternalAccountNumber { get; set; }
        public string HoldingName { get; set; }
    }
}