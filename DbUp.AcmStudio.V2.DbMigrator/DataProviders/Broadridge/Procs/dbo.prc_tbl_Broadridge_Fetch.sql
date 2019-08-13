use ACM_Studio_V2
go

if (object_id('dbo.prc_tbl_Broadridge_Fetch') is not null)
    drop procedure dbo.prc_tbl_Broadridge_Fetch
go

create procedure dbo.prc_tbl_Broadridge_Fetch
    @ReportingDate date

as
    select
        System ,
        FirmName ,
        FirmId ,
        FirmCRDNumber ,
        HoldingId ,
        HoldingExternalAccountNumber ,
        HoldingName ,
        HoldingStartDate ,
        HoldingCreateDate ,
        MostRecentMonthAssetBalance ,
        Month1AgoAssetBalance ,
        Month2AgoAssetBalance ,
        Month3AgoAssetBalance ,
        ProductName ,
        ProductType ,
        Channel ,
        Region ,
        Territory ,
        PersonCRDNumber ,
        PersonFirstName ,
        PersonLastName ,
        PersonId ,
        OfficeAddressLine1 ,
        OfficeAddressLine2 ,
        OfficeCity ,
        OfficeRegionRefCode ,
        OfficePostalCode ,
        PersonBrokerTeamFlag ,
        Month4AgoAssetBalance ,
        Month5AgoAssetBalance ,
        Month6AgoAssetBalance ,
        Month7AgoAssetBalance ,
        Month8AgoAssetBalance ,
        Month9AgoAssetBalance ,
        Month10AgoAssetBalance ,
        Month11AgoAssetBalance ,
        Month12AgoAssetBalance ,
        HoldingAddressLine1 ,
        ReportingDate 
    from dbo.tbl_Broadridge
    where ReportingDate = @ReportingDate
go
    