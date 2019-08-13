use ACM_Studio_V2
go

if (object_id('dbo.tbl_lku_RegionalDirectorRate') is null)
    begin
        create table dbo.tbl_lku_RegionalDirectorRate
        (
            RegionalDirectorRateIid int primary key not null identity (1,1),
            RegionalDirectorIid int,
            RateTypeIid int,
            CommissionTypeIid int,
            Rate decimal(9,6),
            CreatedOn datetime,
            ModifiedOn datetime,
            ModifiedBy varchar(50)
        );
    end
go