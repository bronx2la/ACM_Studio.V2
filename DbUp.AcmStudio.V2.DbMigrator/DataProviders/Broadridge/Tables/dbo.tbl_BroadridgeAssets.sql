use ACM_Studio_V2
go

--drop table dbo.tbl_BroadridgeAssets

if (object_id('dbo.tbl_BroadridgeAssets') is null)
begin
    create table dbo.tbl_BroadridgeAssets
    (
      TheSystem                    varchar(500),
      FirmName                     varchar(500),
      FirmId                       varchar(500),
      FirmCRDNumber                varchar(500),
      HoldingId                    varchar(500),
      HoldingExternalAccountNumber varchar(500),
      HoldingName                  varchar(500),
      HoldingStartDate             datetime,
      HoldingCreateDate            datetime,
      MostRecentMonthAssetBalance  money,
      Month1AgoAssetBalance        money,
      Month2AgoAssetBalance        money,
      Month3AgoAssetBalance        money,
      ProductName                  varchar(500),
      ProductType                  varchar(500),
      Channel                      varchar(500),
      Region                       varchar(500),
      Territory                    varchar(500),
      PersonCRDNumber              varchar(500),
      PersonFirstName              varchar(500),
      PersonLastName               varchar(500),
      PersonId                     varchar(500),
      OfficeAddressLine1           varchar(500),
      OfficeAddressLine2           varchar(500),
      OfficeCity                   varchar(500),
      OfficeRegionRefCode          varchar(500),
      OfficePostalCode             varchar(500),
      PersonBrokerTeamFlag         varchar(500),
      Month4AgoAssetBalance        money,
      Month5AgoAssetBalance        money,
      Month6AgoAssetBalance        money,
      Month7AgoAssetBalance        money,
      Month8AgoAssetBalance        money,
      Month9AgoAssetBalance        money,
      Month10AgoAssetBalance       money,
      Month11AgoAssetBalance       money,
      Month12AgoAssetBalance       money,
      HoldingAddressLine1          varchar(500),
      AccountTANumber              varchar(500),
      ExternalAccountNumber        varchar(500),
      AccountId                    varchar(500),
      ReportingDate                datetime,
      IsNewAsset                   bit
    );
end
go

alter table dbo.tbl_BroadridgeAssets
 Add IsNewAsset bit
go