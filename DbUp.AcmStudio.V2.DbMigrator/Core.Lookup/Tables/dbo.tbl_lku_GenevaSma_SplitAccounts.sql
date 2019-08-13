use ACM_Studio_V2
go

if (object_id('dbo.tbl_lku_GenevaSma_SplitAccounts') is null)
begin
    create table dbo.tbl_lku_GenevaSma_SplitAccounts
    (
      SplitAccountIid int primary key not null identity (1,1),
      PortfolioCode varchar(30),
      ReportingPeriod varchar(6)
    );
end
go