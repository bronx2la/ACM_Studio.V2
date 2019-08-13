use ACM_Studio_V2
go

if (object_id('dbo.tbl_lku_CommissionType') is null)
begin
    create table dbo.tbl_lku_CommissionType
    (
      CommissionTypeIid int primary key not null identity (1,1),
      CommissionType varchar(30),
      IsActive bit
    );
end
go