using System;
using System.Security.Principal;

namespace Core.DataModels.Broadridge
{
    public class BroadridgeSalesInbound
    
    {
        public string TradeID { get; set; }
        public string TransactionCodeOverrideDescription { get; set; }
        public string TradeDate { get; set; }
        public string SettledDate { get; set; }
        public string SuperSheetDate { get; set; }
        public string TradeAmount { get; set; }
        public string System { get; set; }
        public string DealerNum { get; set; }
        public string DealerBranchBranchCode { get; set; }
        public string RepCode { get; set; }
        public string FirmId { get; set; }
        public string FirmName { get; set; }
        public string OfficeAddressLine1 { get; set; }
        public string OfficeCity { get; set; }
        public string OfficeRegionRefCode { get; set; }
        public string OfficePostalCode { get; set; }
        public string PersonFirstName { get; set; }
        public string PersonLastName { get; set; }
        public string LineOfBusiness { get; set; }
        public string Channel { get; set; }
        public string Region { get; set; }
        public string Territory { get; set; }
        public string ProductNasdaqSymbol { get; set; }
        public string ProductName { get; set; }
    }

    public class BroadridgeSales
    {
        public string TradeId                            { get; set; }
        public string TransactionCodeOverrideDescription { get; set; }
        public DateTime TradeDate                        { get; set; }
        public DateTime SettledDate                      { get; set; }
        public DateTime SuperSheetDate                   { get; set; }
        public decimal TradeAmount                       { get; set; }
        public string System                             { get; set; }
        public string DealerNum                          { get; set; }
        public string DealerBranchBranchCode             { get; set; }
        public string RepCode                            { get; set; }
        public string FirmId                             { get; set; }
        public string FirmName                           { get; set; }
        public string OfficeAddressLine1                 { get; set; }
        public string OfficeCity                         { get; set; }
        public string OfficeRegionRefCode                { get; set; }
        public string OfficePostalCode                   { get; set; }
        public string PersonFirstName                    { get; set; }
        public string PersonLastName                     { get; set; }
        public string LineOfBusiness                     { get; set; }
        public string Channel                            { get; set; }
        public string Region                             { get; set; }
        public string Territory                          { get; set; }
        public string ProductNasdaqSymbol                { get; set; }
        public string ProductName                        { get; set; }
        public DateTime ReportingDate                    { get; set; }
    }

    public class BroadridgeAssetsInbound
    {
        public string System { get; set; }
        public string FirmName { get; set; }
        public string FirmId { get; set; }
        public string FirmCRDNumber { get; set; }
        public string HoldingId { get; set; }
        public string HoldingExternalAccountNumber { get; set; }
        public string HoldingName { get; set; }
        public string HoldingStartDate { get; set; }
        public string HoldingCreateDate { get; set; }
        public string MostRecentMonthAssetBalance { get; set; }
        public string Month1AgoAssetBalance { get; set; }
        public string Month2AgoAssetBalance { get; set; }
        public string Month3AgoAssetBalance { get; set; }
        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public string Channel { get; set; }
        public string Region { get; set; }
        public string Territory { get; set; }
        public string PersonCRDNumber { get; set; }
        public string PersonFirstName { get; set; }
        public string PersonLastName { get; set; }
        public string PersonId { get; set; }
        public string OfficeAddressLine1 { get; set; }
        public string OfficeAddressLine2 { get; set; }
        public string OfficeCity { get; set; }
        public string OfficeRegionRefCode { get; set; }
        public string OfficePostalCode { get; set; }
        public string PersonBrokerTeamFlag { get; set; }
        public string Month4AgoAssetBalance { get; set; }
        public string Month5AgoAssetBalance { get; set; }
        public string Month6AgoAssetBalance { get; set; }
        public string Month7AgoAssetBalance { get; set; }
        public string Month8AgoAssetBalance { get; set; }
        public string Month9AgoAssetBalance { get; set; }
        public string Month10AgoAssetBalance { get; set; }
        public string Month11AgoAssetBalance { get; set; }
        public string Month12AgoAssetBalance { get; set; }
        public string HoldingAddressLine1 { get; set; }
    }

    public class BroadridgeAssets
    {
        public string System { get; set; }
        public string FirmName { get; set; }
        public string Firm { get; set; }
        public string FirmId { get; set; }
        public string FirmCRDNumber { get; set; }
        public string HoldingId { get; set; }
        public string HoldingExternalAccountNumber { get; set; }
        public string HoldingName { get; set; }
        public DateTime HoldingStartdate { get; set; }
        public DateTime HoldingCreatedate { get; set; }
        public decimal MostRecentMonthAssetBalance { get; set; }
        public decimal Month1AgoAssetBalance { get; set; }
        public decimal Month2AgoAssetBalance { get; set; }
        public decimal Month3AgoAssetBalance { get; set; }
        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public string Channel { get; set; }
        public string Region { get; set; }
        public string Territory { get; set; }
        public string PersonCRDNumber { get; set; }
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
        public decimal Month12AgoAssetBalance { get; set; }
        public string HoldingAddressLine1 { get; set; }
        public DateTime ReportingDate { get; set; }
        public bool IsNewAsset { get; set; }
    }

    public class BroadridgeUmaAssets
    {
        public string System { get; set; }
        public string FirmName { get; set; }
        public string FirmId { get; set; }
        public string FirmCRDNumber { get; set; }
        public string HoldingId { get; set; }
        public string HoldingExternalAccountNumber { get; set; }
        public string HoldingName { get; set; }
        public DateTime HoldingStartDate { get; set; }
        public DateTime HoldingCreateDate { get; set; }
        public decimal MostRecentMonthAssetBalance { get; set; }
        public decimal Month1AgoAssetBalance { get; set; }
        public decimal Month2AgoAssetBalance { get; set; }
        public decimal Month3AgoAssetBalance { get; set; }
        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public string Channel { get; set; }
        public string Region { get; set; }
        public string Territory { get; set; }
        public string PersonCRDNmber { get; set; }
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
        public decimal Month12AgoAssetBalance { get; set; }
        public string HoldingAddressLine1 { get; set; }
        public DateTime ReportingDate { get; set; }
        
        
    }

//    public class BroadridgeSeasonedValues
//    {
//        public string System { get; set; }
//        public string HoldingId { get; set; }
//        public string HoldingExternalAccountNumber { get; set; }
//        public string FirmName { get; set; }
//        public string Firm { get; set; }
//        public string ProductName { get; set; }
//        public string ProductType { get; set; }
//        public string Region { get; set; }
//        public string Territory { get; set; }
//        public decimal MostRecentMonthAssetBalance { get; set; }
//        public decimal Flows { get; set; }
//        public decimal SeasonedValue { get {return MostRecentMonthAssetBalance - Flows; }
//    }
}