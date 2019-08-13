using System;

namespace Core.DataModels
{
    public class GenevaSmaAumDataModel
    {
        public int Id { get; set; }
        public string GroupPortfolioCode { get; set; }
        public DateTime ReportEndDate3 { get; set; }
        public string PortfolioCode { get; set; }
        public string PortfolioName { get; set; }
        public DateTime Inception { get; set; }
        public decimal Equity { get; set; }
        public decimal FixedIncome { get; set; }
        public decimal CashandEquiv { get; set; }
        public decimal Total { get; set; }
        public string Goal { get; set; }
        public string ClientType { get; set; }
        public string TaxStatus { get; set; }
        public string State { get; set; }
        public string ERISA { get; set; }
        public string Class3 { get; set; }
        public string IsIntlADROnly { get; set; }
        public string PM { get; set; }
        public string RM { get; set; }
        public string Sponsor { get; set; }
        public string AcctClass { get; set; }
        public string PortCustodian { get; set; }
        public string CustAcct { get; set; }
        public string ConsultantFirm { get; set; }
        public string ConsultantName { get; set; }
        public string IntMktPerson { get; set; }
        public string SalesTeam { get; set; }
    }
}

