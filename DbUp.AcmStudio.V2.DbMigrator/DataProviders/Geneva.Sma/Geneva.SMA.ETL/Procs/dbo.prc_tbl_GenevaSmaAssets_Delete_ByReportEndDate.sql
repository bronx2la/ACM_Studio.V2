use ACM_Studio_V2
go

if (object_id('dbo.prc_tbl_GenevaSmaAssets_Delete_ByReportEndDate') is not null)
    drop procedure dbo.prc_tbl_GenevaSmaAssets_Delete_ByReportEndDate
go

create procedure dbo.prc_tbl_GenevaSmaAssets_Delete_ByReportEndDate
  @ReportEndDate3 date
  as
  
  delete from dbo.tbl_GenevaSmaAssets where @ReportEndDate3 = @ReportEndDate3