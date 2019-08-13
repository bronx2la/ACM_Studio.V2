use ACM_Studio_V2
go

if (object_id('dbo.prc_tbl_GenevaSmaAssets_Transform') is not null)
    drop procedure dbo.prc_tbl_GenevaSmaAssets_Transform
go

create procedure dbo.prc_tbl_GenevaSmaAssets_Transform
  @ReportEndDate date
  as 
  
  delete from tbl_GenevaSmaAssets where ReportEndDate3 = @ReportEndDate
  
  insert into dbo.tbl_GenevaSmaAssets
      select
        Textbox1,
        Textbox3,
        Textbox5,
        Textbox59,
        Textbox60,
        Textbox41,
        Textbox32,
        PortfolioCode,
        @ReportEndDate as ReportEndDate3,
        Portfolio,
        PortShortName,
        cast(PortStartDate as date) as PortStartDate,
        cast(TradeDate as date) as TradeDate,
        TranType,
        TranDesc,
        InvCode,
        Security,
        dbo.fn_ConvertStringToDecimal(Quantity) as Quantity,
        dbo.fn_ConvertStringToDecimal(Price) as Price,
        dbo.fn_ConvertCurrencyToMoney(Amount) as Amount,
        InvestmentType,
        Strategy,
        ConsultantFirm,
        PM,
        InternalMarketingPerson,
        SalesTeam,
        RM,
        RM2,
        RM3,
        Sponsor
      from dbo.tbl_GenevaSmaAssets_Staging
        where Textbox5 <> 'Textbox5'
          and Price <> ''
          and Portfolio <> ''
          and PortStartDate <> ''