use ACM_Studio_V2
go

if (object_id('dbo.prc_BroadridgeAssets_Transform') is not null)
    drop procedure dbo.prc_BroadridgeAssets_Transform
go

create procedure dbo.prc_BroadridgeAssets_Transform
@ReportingDate date
as

delete from dbo.tbl_BroadridgeAssets
where ReportingDate = @ReportingDate

insert into dbo.tbl_BroadridgeAssets
select
    [System]                     ,
    FirmName                     ,
    FirmId                       ,
    FirmCRDNumber                ,
    HoldingId                    ,
    HoldingExternalAccountNumber ,
    HoldingName                  ,
    cast(HoldingStartDate  as date),
    cast(HoldingCreateDate as date),
    cast(MostRecentMonthAssetBalance as money),
    cast(Month1AgoAssetBalance       as money),
    cast(Month2AgoAssetBalance       as money),
    cast(Month3AgoAssetBalance       as money),
    ProductName                  ,
    ProductType                  ,
    Channel                      ,
    Region                       ,
    Territory                    ,
    PersonCRDNumber              ,
    PersonFirstName              ,
    PersonLastName               ,
    PersonId                     ,
    OfficeAddressLine1           ,
    OfficeAddressLine2           ,
    OfficeCity                   ,
    OfficeRegionRefCode          ,
    OfficePostalCode             ,
    PersonBrokerTeamFlag         ,
    cast(Month4AgoAssetBalance  as money),
    cast(Month5AgoAssetBalance  as money),
    cast(Month6AgoAssetBalance  as money),
    cast(Month7AgoAssetBalance  as money),
    cast(Month8AgoAssetBalance  as money),
    cast(Month9AgoAssetBalance  as money),
    cast(Month10AgoAssetBalance as money),
    cast(Month11AgoAssetBalance as money),
    cast(Month12AgoAssetBalance as money),
    HoldingAddressLine1          ,
    @ReportingDate,
    case
        when cast(Month3AgoAssetBalance as money) = 0 then 1
        else 0
        end as IsNewAsset
from dbo.tbl_BroadridgeAssets_Staging
where FirmId <> 'FirmId'

go
    