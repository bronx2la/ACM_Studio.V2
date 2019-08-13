use ACM_Studio_V2
go

if (object_id('dbo.prc_tbl_GenevaSmaAssets_Staging_Truncate') is not null)
  drop procedure dbo.prc_tbl_GenevaSmaAssets_Staging_Truncate
go

create procedure dbo.prc_tbl_GenevaSmaAssets_Staging_Truncate
as
  set nocount on
  truncate table dbo.tbl_GenevaSmaAssets_Staging