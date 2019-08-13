use ACM_Studio_V2
go

if (object_id('dbo.fn_tbl_GenevaSmaAssets_NewAssets') is not null)
  drop function dbo.fn_tbl_GenevaSmaAssets_NewAssets
go

create function dbo.fn_tbl_GenevaSmaAssets_NewAssets (@StartDate date, @EndDate date)
  returns @NewAssets table
  (
    ID int not null primary key identity (1,1),
    InternalMarketingPerson varchar(500),
    Portfolio varchar(500),
    PortShortName varchar(500),
    PortStartDate date,
    TradeDate date,
    TranType varchar(100),
    Strategy varchar(500),
    ConsultantFirm varchar(500),
    ConsultantName varchar(500),
    Ticker varchar(500),
    Security varchar(500),
    Quantity decimal(20,6),
    Price decimal(20,6),
    Amount money,
    CommissionRate decimal(20,6),
    CommissionAmount money,
    IsStartBeforePeriod varchar(6),
    SumTradeAmountByDate   money,
    SumTradeAmountByPortfolioCode money,
    IsValid varchar(10)
  )

as
  begin
    declare @RateTypeIID int = (select RateTypeIID from tbl_lku_RateType where RateType = 'New Assets')
    declare @CommissionTypeIID int = (select CommissionTypeIID from tbl_lku_CommissionType where CommissionType = 'SMA')

    --Base new assets data
    insert into @NewAssets
      select
        gs.InternalMarketingPerson,
        gs.Portfolio,
        gs.PortShortName,
        gs.PortStartDate,
        gs.TradeDate,
        gs.TranType,
        gs.Strategy,
        gs.ConsultantFirm,
        ga.ConsultantName,
        gs.InvCode as Ticker,
        gs.Security,
        gs.Quantity,
        gs.Price,
        gs.Amount,
        (select Rate from dbo.fn_tbl_RegionalDirector_RateInfo(gs.RM, @RateTypeIID, @CommissionTypeIID)) as Rate,
        case
          when (gs.Amount > 0) and (TranDesc  in ('NewCash', 'NewAsset')) then gs.Amount * (select Rate from dbo.fn_tbl_RegionalDirector_RateInfo(gs.RM, @RateTypeIID, @CommissionTypeIID))
          else 0.0
        end as Commission,
        case
          when (gs.PortStartDate < gs.TradeDate) and gs.PortStartDate > '19000101' then 'True'
          else 'False'
        end as IsStartBeforePeriod,
        null as SumTradeAmountByDate ,
        null as SumTradeAmountByPortfolioCode,
        'False' as IsValid
      from
        tbl_GenevaSmaAssets gs
      left outer join tbl_GenevaAUM ga on gs.Portfolio = ga.PortfolioCode
      where
        gs.TradeDate between @StartDate and @EndDate
        and gs.Amount > 0
      order by  
        gs.RM, gs.Portfolio

    update @NewAssets
      set IsValid = 'True'
      where CommissionAmount > 0
      
    update @NewAssets
      set CommissionRate = CommissionRate / 2
      where charindex(',', InternalMarketingPerson) > 0
    
    return
  end
       