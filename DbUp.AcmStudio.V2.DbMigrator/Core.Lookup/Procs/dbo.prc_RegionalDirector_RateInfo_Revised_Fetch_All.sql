use ACM_Studio_V2
go

if (object_id('dbo.prc_RegionalDirector_RateInfo_Revised_Fetch_All') is not null)
    drop procedure dbo.prc_RegionalDirector_RateInfo_Revised_Fetch_All
go

create procedure dbo.prc_RegionalDirector_RateInfo_Revised_Fetch_All
as

select
    rates.RegionalDirectorIid,
    rd.RegionalDirectorKey,
    rates.IsGeneva,   
    rd.FirstName,
    rd.LastName,
    rd.SeasoningMonths,
    rates.Strategy,
    rates.NewAssetRate,
    rates.OngoingRate
from dbo.tbl_lku_RegionalDirectorRateRevised rates
join dbo.tbl_lku_RegionalDirector rd on rates.RegionalDirectorIid = rd.RegionalDirectorIid


go


