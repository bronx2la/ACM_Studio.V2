use ACM_Studio_V2
go

if (object_id('dbo.prc_RegionalDirector_RateInfo_Fetch') is not null)
    drop procedure dbo.prc_RegionalDirector_RateInfo_Fetch
go

create procedure dbo.prc_RegionalDirector_RateInfo_Fetch
    @RegionalDirectorKey varchar(20), 
    @RateTypeIid int, 
    @CommissionTypeIid int
    
as
    
    select
        Director.RegionalDirectorIid,
        Director.RegionalDirectorKey,
        rtrim(Director.FirstName) + ' ' + rtrim(Director.LastName) as RegionalDirectorName,
        rdr.Rate,
        Director.SeasoningMonths
     from
        tbl_lku_RegionalDirector Director
     join tbl_lku_RegionalDirectorRate rdr on Director.RegionalDirectorIID = rdr.RegionalDirectorIID
     where
        charindex(@RegionalDirectorKey, Director.RegionalDirectorKey) > 0
        and rdr.RateTypeIid = @RateTypeIid
        and rdr.CommissionTypeIid = @CommissionTypeIid

go
    