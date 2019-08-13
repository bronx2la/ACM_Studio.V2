use ACM_Studio_V2
go

if (object_id('dbo.prc_MutualFundsFlows') is not null)
  drop procedure dbo.prc_MutualFundsFlows
go

create procedure dbo.prc_MutualFundsFlows
  @ReportingDate date
as
  select *
--       DealerNum,
--       TradeDate,
--     Sum(TradeAmount) as FlowAmount
  from
       dbo.tbl_BroadridgeSales
--   where
--         TradeDate = @ReportingDate
--         and TransactionCodeOverrideDescription in ('Purchase', 'Transfer In')
--   group by HoldingExternalAccountNumber, TradeDate
--   order by HoldingExternalAccountNumber
  

go
    