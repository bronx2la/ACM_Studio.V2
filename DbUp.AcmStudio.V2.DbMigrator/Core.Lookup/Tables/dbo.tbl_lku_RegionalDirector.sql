use ACM_Studio_V2
go

if (object_id('dbo.tbl_lku_RegionalDirector') is null)
begin
    create table dbo.tbl_lku_RegionalDirector
    (
      RegionalDirectorIid int primary key not null identity (1,1),
      RegionalDirectorKey varchar(20),
      FirstName varchar(40),
      LastName varchar(60),
      SeasoningMonths int,
      CreatedOn datetime,
      ModifiedOn datetime,
      ModifiedBy varchar(50)
    );
end
go