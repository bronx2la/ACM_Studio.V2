use ACM_Studio_V2
go

if (object_id('dbo.tbl_lku_RateType') is null)
begin
    create table dbo.tbl_lku_RateType
    (
      RateTypeIid int primary key not null identity (1,1),
      RateType  varchar(20),
      CreatedOn datetime,
      ModifiedOn datetime,
      ModifiedBy varchar(50)
    );
end
go
