use R2D2
go

if(object_id('dbo.prc_GenevaAum') is not null)
    drop procedure dbo.prc_GenevaAum
go

create procedure dbo.prc_GenevaAum
    @ReportEndDate date
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
    ReportEndDate3 = @ReportEndDate
order by
    PortfolioCode