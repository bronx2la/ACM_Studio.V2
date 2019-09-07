use ACM_Studio_V2
go

if (object_id('dbo.prc_GenevaSmaAssets_DetailRows') is not null)
  drop procedure dbo.prc_GenevaSmaAssets_DetailRows
go

create procedure dbo.prc_GenevaSmaAssets_DetailRows
    @StartDate date,
    @EndDate date
as

select distinct
    InternalMarketingPerson as RegionalDirector,
    Portfolio,
    PortShortName ,
    ConsultantFirm ,
    Strategy ,
    InternalMarketingPerson,
    PortStartDate ,
    TradeDate ,
    TranType ,
    TranDesc ,
    InvCode ,
    Quantity ,
    Price ,
    Amount ,

    SalesTeam ,

    Textbox1 ,
    Textbox3 ,
    Textbox5 ,
    Textbox59 ,
    Textbox60 ,
    Textbox41 ,
    Textbox32 ,
    PortfolioCode ,
    ReportEndDate3,
    Security ,
    InvestmentType ,
    PM ,
    RM ,
    RM2 ,
    RM3 ,
    Sponsor
    from
        dbo.tbl_GenevaSma
    where
      ReportEndDate3 = @EndDate
    order by
        RM, ConsultantFirm
