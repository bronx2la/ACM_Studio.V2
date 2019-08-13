use ACM_Studio_V2
go

if(object_id('dbo.fn_tbl_RegionalDirector_RateInfo_Table') is not null)
    drop function dbo.fn_tbl_RegionalDirector_RateInfo_Table
go

create function dbo.fn_tbl_RegionalDirector_RateInfo_Table(@RegionalDirectorKey varchar(20), @RateTypeIID int, @CommissionTypeIID int)
  returns @RegionalDirectorRateInfoTable table
  (
    RegionalDirectorIID  int,
    RegionalDirectorKey  varchar(20),
    RegionalDirectorName varchar(100),
    Rate                 decimal(9, 6),
    SeasoningMonths      int
  )
as
  begin
    insert into @RegionalDirectorRateInfoTable
    select
           Director.RegionalDirectorIID,
           Director.RegionalDirectorKey,
           rtrim(Director.FirstName) + ' ' + rtrim(Director.LastName) as RegionalDirectorName,
           rdr.Rate,
           Director.SeasoningMonths
    from
         tbl_lku_RegionalDirector Director
    join tbl_lku_RegionalDirectorRate rdr on Director.RegionalDirectorIID = rdr.RegionalDirectorIID
    where
            charindex(Director.RegionalDirectorKey, @RegionalDirectorKey) > 0
        and rdr.RateTypeIID = @RateTypeIID
        and rdr.CommissionTypeIID = @CommissionTypeIID

    return
  end