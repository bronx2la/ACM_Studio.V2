use ACM_Studio_V2
go

if (object_id('dbo.prc_ColumnStyles_Fetch') is not null)
    drop procedure dbo.prc_ColumnStyles_Fetch
go    

create procedure dbo.prc_ColumnStyles_Fetch
    @ReportName varchar(100)
as

    select
      ReportName,
      ColumnNumber,
      FormatString as Format,
      ColumnWidth 
    from dbo.tbl_lku_ColumnStyles
    where ReportName = @ReportName

go
    