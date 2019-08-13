use ACM_Studio_V2
go

if (object_id('dbo.tbl_Broadridge_MutualFunds_Staging') is null)
begin
    create table dbo.tbl_Broadridge_MutualFunds_Staging
    (
      TradeID                            varchar(500),
      TransactionCodeOverrideDescription varchar(500),
      TradeDate                          varchar(500),
      SettleDate                         varchar(500),
      SuperSheetDate                     varchar(500),
      TradeAmount                        varchar(500),
      System                             varchar(500),
      DealerNum                          varchar(500),
      DealerBranchBranchCode             varchar(500),
      RepCode                            varchar(500),
      FirmId                             varchar(500),
      FirmName                           varchar(500),
      OfficeAddressLine1                 varchar(500),
      OfficeCity                         varchar(500),
      OfficeRegionRefCode                varchar(500),
      OfficePostalCode                   varchar(500),
      PersonFirstName                    varchar(500),
      PersonLastName                     varchar(500),
      LineOfBusiness                     varchar(500),
      Channel                            varchar(500),
      Region                             varchar(500),
      Territory                          varchar(500),
      ProductNasdaqSymbol                varchar(500),
      ProductName                        varchar(500)
    );
end
go