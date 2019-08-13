use ACM_Studio_V2
go

if (object_id('dbo.tbl_lku_ColumnStyles') is null)
begin
    create table dbo.tbl_lku_ColumnStyles
    (
      ReportName varchar(100),
      ColumnNumber int,
      FormatString varchar(20),
      ColumnWidth int
    );
end
go