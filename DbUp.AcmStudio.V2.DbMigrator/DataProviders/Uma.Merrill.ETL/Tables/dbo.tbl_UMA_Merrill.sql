use ACM_Studio_V2
go

if (object_id('dbo.tbl_UMA_Merrill') is null)
begin
    create table dbo.tbl_UMA_Merrill(
        Id int not null,
        AsOfDate date null,
        MonthEnd date null,
        SleeveManagerID varchar(500) null,
        SleeveManagerName varchar(500) null,
        UDPModel_UDPSelects_Standalone varchar(500) null,
        ModelID varchar(500) null,
        ModelName_RiskProfile varchar(500) null,
        UniqueID varchar(500) null,
        StrategyEnrollmentDate date null,
        TerminationDate date null,
        Division varchar(500) null,
        Region varchar(500) null,
        ComplexNumber varchar(500) null,
        Complex varchar(500) null,
        OfficeNumber varchar(500) null,
        Office varchar(500) null,
        OfficeLine1Address varchar(500) null,
        OfficeLine2Address varchar(500) null,
        OfficeLine3Address varchar(500) null,
        OfficeStateCode varchar(500) null,
        ZipCode varchar(500) null,
        FANum varchar(500) null,
        FAName varchar(500) null,
        PercentOfAllocation decimal(8, 4) null,
        TotalAssets money null,
        RegionalDirector varchar(20) null,
        RegionalDirectorIID int null
    ) 
end
go




