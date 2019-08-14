using System;

namespace Core.DataModels
{
    public class GenevaSmaNewAssetsDataModel
    {
        public string RegionalDirector { get; set; }
        public string Portfolio { get; set; }
        public string PortShortName { get; set; }
        public string ConsultantFirm { get; set; }
        public string ConsultantName { get; set; }
        public string Strategy { get; set; }
        public string InteralMarketingPerson { get; set; }
        public DateTime PortStartDate { get; set; }
        public DateTime TradeDate { get; set; }
        public string TranType { get; set; }
        public string Trandesc { get; set; }
        public string InvCode { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public decimal CommissionAmount { get; set; }
        public decimal CommissionRate { get; set; }
        public string IsStartBeforePeriod { get; set; }
        public string SalesTeam { get; set; }
        public string Transfer { get; set; }
    }
}