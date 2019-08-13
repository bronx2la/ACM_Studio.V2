use ACM_Studio_V2
go

if (object_id('dbo.tbl_BroadridgeAssets_Staging') is null)
begin
    create table dbo.tbl_BroadridgeAssets_Staging
    (
      System                       varchar(500),
      FirmName                     varchar(500),
      FirmId                       varchar(500),
      FirmCRDNumber                varchar(500),
      HoldingId                    varchar(500),
      HoldingExternalAccountNumber varchar(500),
      HoldingName                  varchar(500),
      HoldingStartDate             varchar(500),
      HoldingCreateDate            varchar(500),
      MostRecentMonthAssetBalance  varchar(500),
      Month1AgoAssetBalance        varchar(500),
      Month2AgoAssetBalance        varchar(500),
      Month3AgoAssetBalance        varchar(500),
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
      Month4AgoAssetBalance        varchar(500),
      Month5AgoAssetBalance        varchar(500),
      Month6AgoAssetBalance        varchar(500),
      Month7AgoAssetBalance        varchar(500),
      Month8AgoAssetBalance        varchar(500),
      Month9AgoAssetBalance        varchar(500),
      Month10AgoAssetBalance       varchar(500),
      Month11AgoAssetBalance       varchar(500),
      Month12AgoAssetBalance       varchar(500),
      HoldingAddressLine1          varchar(500),
      ReportingDate                varchar(500)
    );
end
go