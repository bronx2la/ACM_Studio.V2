use ACM_Studio_V2
go

if (object_id('dbo.prc_RegionalDirector_Fetch_All') is not null)
    drop procedure dbo.prc_RegionalDirector_Fetch_All
go

create procedure dbo.prc_RegionalDirector_Fetch_All
as

select
    RegionalDirectorIid,
    RegionalDirectorKey,
    FirstName,
    LastName,
    SeasoningMonths,
    CreatedOn,
    ModifiedOn,
    ModifiedBy
from
    tbl_lku_RegionalDirector 

go
    