use ACM_Studio_V2
go

if (object_id('dbo.fn_tbl_RegionalDirector_RateInfo') is not null)  
    drop function dbo.fn_tbl_RegionalDirector_RateInfo
 go
 
 create function dbo.fn_tbl_RegionalDirector_RateInfo (@RegionalDirectorKey varchar(20), @RateTypeIid int, @CommissionTypeIid int)
    returns @RegionalDirectorRateInfo table
    (
      RegionalDirectorIID  int,
      RegionalDirectorKey  varchar(20),
      RegionalDirectorName varchar(100),
      Rate                 decimal(9, 6),
      SeasoningMonths      int
    )
    as
      begin
        insert into @RegionalDirectorRateInfo
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
--             charindex(Director.RegionalDirectorKey, @RegionalDirectorKey) > 0
            charindex(@RegionalDirectorKey, Director.RegionalDirectorKey) > 0
            and rdr.RateTypeIid = @RateTypeIid
            and rdr.CommissionTypeIid = @CommissionTypeIid
        return
      end
        