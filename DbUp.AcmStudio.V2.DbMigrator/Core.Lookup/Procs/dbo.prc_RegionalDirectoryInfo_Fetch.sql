use ACM_Studio_V2
go

if (object_id('dbo.prc_RegionalDirectoryInfo_Fetch') is not null)
    drop procedure dbo.prc_RegionalDirectoryInfo_Fetch
go

create procedure dbo.prc_RegionalDirectoryInfo_Fetch
as

select
    rd.RegionalDirectorIid,
    rd.RegionalDirectorKey,
    rd.FirstName,
    rd.LastName,
    rd.SeasoningMonths,
    rd.CreatedOn,
    rd.ModifiedOn,
    rd.ModifiedBy,
    rates.RegionalDirectorRateRevisedIid,
    rates.IsGeneva,
    rates.Strategy,
    rates.NewAssetRate,
    rates.OngoingRate
from dbo.tbl_lku_RegionalDirector rd
join dbo.tbl_lku_RegionalDirectorRateRevised rates on rd.RegionalDirectorIid = rates.RegionalDirectorIid
order by rd.RegionalDirectorIid, rates.IsGeneva, rates.Strategy

