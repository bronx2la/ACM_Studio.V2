use ACM_Studio_V2
go

if (object_id('dbo.prc_RegionalDirector_RateInfo_Fetch_All') is not null)
    drop procedure dbo.prc_RegionalDirector_RateInfo_Fetch_All
go

create procedure dbo.prc_RegionalDirector_RateInfo_Fetch_All
as

select
    tlRD.FirstName,
    tlRD.LastName,
    tlRD.SeasoningMonths,
    tlRT.RateType,
    tlCT.CommissionType,
    tlRDR.Rate
    from dbo.tbl_lku_RegionalDirectorRate tlRDR
             join dbo.tbl_lku_RegionalDirector tlRD on tlRDR.RegionalDirectorIid = tlRD.RegionalDirectorIid
             join dbo.tbl_lku_RateType tlRT on tlRDR.RateTypeIid = tlRT.RateTypeIid
             join dbo.tbl_lku_CommissionType tlCT on tlRDR.CommissionTypeIid = tlCT.CommissionTypeIid
    order by tlRD.LastName, tlRD.FirstName, tlRT.RateType, tlCT.CommissionType


go
