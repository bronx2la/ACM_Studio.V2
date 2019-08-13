use ACM_Studio_V2
go

if (object_id('dbo.prc_GenevaSma_Ongoing_Flows') is not null)
    drop procedure dbo.prc_GenevaSma_Ongoing_Flows
go

create procedure dbo.prc_GenevaSma_Ongoing_Flows
    @StartDate date, @EndDate date
as
select distinct
    gs.Portfolio,
    gs.PortShortName,
    gs.RM,
    gs.ConsultantFirm,
    gs.Strategy,
    gs.PortStartDate,
    (select sum(s.Amount) from tbl_GenevaSma s where s.Portfolio = gs.Portfolio) as AUM,
    case
      when gs.RM like '%msindici%' then
        (
          select
            sum(s.Amount)

          from tbl_GenevaSma s
          where
              s.Portfolio = gs.Portfolio
            and s.PortStartDate < dateadd(month, -3, @EndDate)
            and s.TradeDate between @StartDate and @EndDate
              --                                and s.TranDesc in ('NewCash', 'NewAsset')
            and s.RM like '%msindici%'
        )
      else
        (
          select
            sum(s.Amount)
          from tbl_GenevaSma s
          where
              s.Portfolio = gs.Portfolio
            and s.PortStartDate < dateadd(month, -3, @EndDate)
            and s.TradeDate between @StartDate and @EndDate
              --                                and s.TranDesc in ('NewCash', 'NewAsset')
            and s.RM not like '%msindici%'
        )
      end as InFlows
  from tbl_GenevaSma gs
  where gs.TradeDate between @StartDate and @EndDate


go
    