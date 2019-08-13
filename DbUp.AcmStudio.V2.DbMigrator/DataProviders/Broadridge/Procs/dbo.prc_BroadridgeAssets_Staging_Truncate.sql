use ACM_Studio_V2
go

if (object_id('dbo.prc_BroadridgeAssets_Staging_Truncate') is not null)
    drop procedure dbo.prc_BroadridgeAssets_Staging_Truncate
go

create procedure dbo.prc_BroadridgeAssets_Staging_Truncate

as
    truncate table dbo.tbl_BroadridgeAssets_Staging
go
    