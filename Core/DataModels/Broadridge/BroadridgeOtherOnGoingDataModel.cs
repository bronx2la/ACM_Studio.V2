using System;

namespace Core.DataModels.Broadridge
{
    public class BroadridgeOtherOnGoingDataModel
    {
        public string System { get; set; }
        public string FirmName { get; set; }
        public string FirmId { get; set; }
        public string FirmCrdNumber { get; set; }
        public string HoldingId { get; set; }
        public string HoldingExternalAccountNumber { get; set; }
        public string HoldingName { get; set; }
        public DateTime HoldingStartDate { get; set; }
        public DateTime HoldingCreateDate { get; set; }
        public decimal MostRecentMonthAssetBalance { get; set; }
        public decimal Flows { get; set; }
        public decimal PayableAmount { get; set; }
        public decimal Commission { get; set; }
        public decimal QtlyRate { get; set; }
        public decimal Rate { get; set; }
        public decimal Month1AgoAssetBalance { get; set; }
        public decimal Month2AgoAssetBalance { get; set; }
        public decimal Month3AgoAssetBalance { get; set; }
        public string ProductType { get; set; }
        public string Channel { get; set; }
        public string Region { get; set; }
        public string Territory { get; set; }
        public string PersonCrdNumber { get; set; }
        public string PersonFirstName { get; set; }
        public string PersonLastName { get; set; }
        public string PersonId { get; set; }
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
        public string HoldingAddressLine1 { get; set; }
        public string AccountTaNumber { get; set; }
        public string ExternalAccountNumber { get; set; }
        public string AccountId { get; set; }
    }
}