use ACM_Studio_V2
go

if (object_id('dbo.prc_tbl_GenevaAum_Transform') is not null)
    drop procedure dbo.prc_tbl_GenevaAum_Transform
go

create procedure dbo.prc_tbl_GenevaAum_Transform
  @ReportEndDate date
  as 
  
  exec dbo.prc_tbl_GenevaAum_Delete_ByReportEndDate3 @ReportEndDate
  
  insert into dbo.tbl_GenevaAum
      select
        GroupPortfolioCode,
        cast(ReportEndDate3 as date) as ReportEndDate3,
        PortfolioCode,
        PortfolioName,
        cast(Inception as date) as Inception,
        dbo.fn_ConvertCurrencyToMoney(Equity) as Equity,
        dbo.fn_ConvertCurrencyToMoney(FixedIncome) as FixedIncome,
        dbo.fn_ConvertCurrencyToMoney(CashandEquiv) as CashandEquiv,
        dbo.fn_ConvertCurrencyToMoney(Total) as Total,
        Goal,
        ClientType ,
        TaxStatus ,
        State ,
        ERISA ,
        Class3 ,
        IsIntlADROnly ,
        PM ,
        RM ,
        Sponsor ,
        AcctClass ,
        PortCustodian ,
        CustAcct ,
        ConsultantFirm ,
        ConsultantName,
        IntMktPerson ,
        SalesTeam 
      from dbo.tbl_GenevaAum_Staging
      where PortfolioCode <> 'PortfolioCode'
