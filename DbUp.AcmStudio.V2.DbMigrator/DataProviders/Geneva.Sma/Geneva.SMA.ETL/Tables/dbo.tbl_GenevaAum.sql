use ACM_Studio_V2
go

if (object_id('dbo.tbl_GenevaAum') is null)
begin
    create table dbo.tbl_GenevaAum
    (
      Id int primary key not null identity (1,1),
      GroupPortfolioCode varchar(500),
      ReportEndDate3 date,
      PortfolioCode varchar(500),
      PortfolioName varchar(500),
      Inception date,
      Equity money,
      FixedIncome money,
      CashandEquiv money,
      Total money,
      Goal varchar(500),
      ClientType varchar(500),
      TaxStatus varchar(500),
      State varchar(500),
      ERISA varchar(500),
      Class3 varchar(500),
      IsIntlADROnly varchar(500),
      PM varchar(500),
      RM varchar(500),
      Sponsor varchar(500),
      AcctClass varchar(500),
      PortCustodian varchar(500),
      CustAcct varchar(500),
      ConsultantFirm varchar(500),
      ConsultantName varchar(500),
      IntMktPerson varchar(500),
      SalesTeam varchar(500)
    );
end
go