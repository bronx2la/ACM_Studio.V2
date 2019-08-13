use ACM_Studio_V2
go

if (object_id('dbo.prc_Uma_Merrill_Data_Fetch') is not null)
    drop procedure dbo.prc_Uma_Merrill_Data_Fetch
go

create procedure dbo.prc_Uma_Merrill_Data_Fetch
    @StartDate date,
    @EndDate date

as

    select
       [Id]
      ,[AsOfDate]
      ,[MonthEnd]
      ,[SleeveManagerID]
      ,[SleeveManagerName]
      ,[UDPModel_UDPSelects_Standalone]
      ,[ModelID]
      ,[ModelName_RiskProfile]
      ,[UniqueID]
      ,[StrategyEnrollmentDate]
      ,[TerminationDate]
      ,[Division]
      ,[Region]
      ,[ComplexNumber]
      ,[Complex]
      ,[OfficeNumber]
      ,[Office]
      ,[OfficeLine1Address]
      ,[OfficeLine2Address]
      ,[OfficeLine3Address]
      ,[OfficeStateCode]
      ,[ZipCode]
      ,[FANum]
      ,[FAName]
      ,[PercentOfAllocation]
      ,[TotalAssets]
      ,[RegionalDirector]
      ,[RegionalDirectorIID]
    from dbo.tbl_Uma_Merrill
    where
        AsOfDate between DateAdd(year, -1, @StartDate) and @EndDate  
        and RegionalDirector is not null
    order by 
        RegionalDirector, Office, OfficeStateCode, FANum
go
    