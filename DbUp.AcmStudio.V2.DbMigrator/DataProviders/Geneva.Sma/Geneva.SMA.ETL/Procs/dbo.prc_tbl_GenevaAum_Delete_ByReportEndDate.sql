use ACM_Studio_V2
go

if (object_id('dbo.prc_tbl_GenevaAum_Delete_ByReportEndDate') is not null)
    drop procedure dbo.prc_tbl_GenevaAum_Delete_ByReportEndDate
go

create procedure dbo.prc_tbl_GenevaAum_Delete_ByReportEndDate
  @ReportEndDate date
  as 
  
  delete from dbo.tbl_GenevaAum where ReportEndDate3 = @ReportEndDate
  
  