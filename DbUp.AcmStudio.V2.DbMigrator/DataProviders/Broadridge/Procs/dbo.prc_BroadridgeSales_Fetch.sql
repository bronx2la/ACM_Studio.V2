use ACM_Studio_V2
go

if (object_id('dbo.prc_BroadridgeSales_Fetch') is not null)
    drop procedure dbo.prc_BroadridgeSales_Fetch
go

create procedure dbo.prc_BroadridgeSales_Fetch
    @ReportingDate date,
    @IsUma bit
    
as
    select
      TradeID                            ,
      TransactionCodeOverrideDescription ,
      TradeDate                          ,
      SettleDate                         ,
      SuperSheetDate                     ,
      TradeAmount                        ,
      System                             ,
      DealerNum                          ,
      DealerBranchBranchCode             ,
      RepCode                            ,
      FirmId                             ,
      FirmName                           ,
      OfficeAddressLine1                 ,
      OfficeCity                         ,
      OfficeRegionRefCode                ,
      OfficePostalCode                   ,
      PersonFirstName                    ,
      PersonLastName                     ,
      LineOfBusiness                     ,
      Channel                            ,
      Region                             ,
      Territory                          ,
      ProductNasdaqSymbol                ,
      ProductName                        ,
      AccountTANumber                    ,
      AccountId                          ,
      ExternalAccountNumber              ,
      HoldingId                          ,
      HoldingExteralAccountNumber        ,
      HoldingName                        ,
      ReportingDate                      
    from 
        dbo.tbl_BroadridgeSales
    where 
        ReportingDate = @ReportingDate
--         and TransactionCodeOverrideDescription in ('Purchase', 'Transfer In')
go
    