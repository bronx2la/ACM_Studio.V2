use ACM_Studio_V2
go

if (object_id('dbo.prc_GenevaAum_Transform') is not null)
    drop procedure dbo.prc_GenevaAum_Transform
go

create procedure dbo.prc_GenevaAum_Transform
  @ReportEndDate date
  as 
  
  delete from tbl_GenevaAum where ReportEndDate3 = @ReportEndDate
  
  insert into dbo.tbl_GenevaAum
      select
        GroupPortfolioCode,
        cast(ReportEndDate3 as date),
        PortfolioCode,
        PortfolioName,
        cast(Inception as date),
        dbo.fn_ConvertCurrencyToMoney(Equity),
        dbo.fn_ConvertCurrencyToMoney(FixedIncome),
        dbo.fn_ConvertCurrencyToMoney(CashandEquiv),
        dbo.fn_ConvertCurrencyToMoney(Total),
        Goal,
        ClientType ,
        TaxStatus ,
        State,
        ERISA,
        Class3,
        IsIntlADROnly,
        PM,
        RM,
        Sponsor,
        AcctClass,
        PortCustodian,
        CustAcct,
        ConsultantFirm,
        ConsultantName,
        IntMktPerson,
        SalesTeam
      from dbo.tbl_GenevaAum_Staging
