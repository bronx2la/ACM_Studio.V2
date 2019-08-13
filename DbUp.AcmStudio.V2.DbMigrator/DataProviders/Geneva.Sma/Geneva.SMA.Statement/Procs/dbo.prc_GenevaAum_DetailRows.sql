use ACM_Studio_V2
go

if (object_id('dbo.prc_GenevaAum_DetailRows') is not null)
  drop procedure dbo.prc_GenevaAum_DetailRows
go

create procedure dbo.prc_GenevaAum_DetailRows
--     @StartDate date,
    @EndDate date
as

    select distinct
      Id,
      GroupPortfolioCode,
      ReportEndDate3 ,
      PortfolioCode,
      PortfolioName,
      Inception ,
      Equity ,
      FixedIncome ,
      CashandEquiv ,
      Total ,
      Goal ,
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
      ConsultantName ,
      IntMktPerson ,
      SalesTeam
    from
        dbo.tbl_GenevaAum
    where
--          Inception <= @StartDate
         ReportEndDate3 = @EndDate
--          and IntMktPerson = 'msindici'
    order by
        Inception
--         RM, ConsultantFirm


