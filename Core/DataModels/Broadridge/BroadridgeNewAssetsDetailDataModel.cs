using System;

namespace Core.DataModels.Broadridge
{
    public class BroadridgeNewAssetsDetailDataModel
    {
        public string  TheSystem { get; set; }
        public string FirmName { get; set; }
        public string FirmId { get; set; }
        public string FirmCrdNumber { get; set; }
        public string HoldingId { get; set; }
        public string HoldingExterAccountNumber { get; set; }
        public string HoldingName { get; set; }
        public DateTime HoldingStartDate { get; set; }
        public DateTime HoldingCreateDate { get; set; }
        public decimal MostRecentMonthAssetBalance { get; set; }
        public decimal Month1AgoAssetBalance { get; set; }
        public decimal Month2AgoAssetBalance { get; set; }
        public decimal Month3AgoAssetBalance { get; set; }
        public decimal NewAssetValue { get; set; }
        public decimal Commission { get; set; }
        public decimal Rate { get; set; }
        public string Territory { get; set; }
        public string SalesRunTerritory { get; set; }
        public bool IsTerritorySame { get; set; }
        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public string Channel { get; set; }
        public string Region { get; set; }
        public string PersonCRDNumber { get; set; }
        public string PersonFirstName { get; set; }
        public string PersonLastName { get; set; }
        public string OfficeAddressLine1 { get; set; }
        public string OfficeAddressLine2 { get; set; }
        public string OfficeCity { get; set; }
        public string OfficeRegionRefCode { get; set; }
        public string OfficePostalCode { get; set; }
        public string PersonBrokerTeamFlag { get; set; }
        public decimal Month4AgoAssetBalance { get; set; }
        public decimal Month5AgoAssetBalance { get; set; }
        public decimal Month6AgoAssetBalance { get; set; }
        public decimal Month7AgoAssetBalance { get; set; }
        public decimal Month8AgoAssetBalance { get; set; }
        public decimal Month9AgoAssetBalance { get; set; }
        public decimal Month10AgoAssetBalance { get; set; }
        public decimal Month11AgoAssetBalance { get; set; }
        public decimal Month12AgoAssetBalance { get; set; }
        public string HoldingAddressLine1 { get; set; }
        public string SystemFAName { get; set; }
        public string SystemOfficeAddress { get; set; }
        public string SystemOfficeState { get; set; }
        public decimal SystemQEAssets { get; set; }
        public decimal AssetCheck { get; set; }
        public string SystemRDCredit { get; set; }
        public string RDCheck { get; set; }
        public string AccountTANumber { get; set; }
        public string ExternalAccountNumber { get; set; }
        public string AccountId { get; set; }
    }
}