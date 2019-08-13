use ACM_Studio_V2
go

if (object_id('dbo.fn_Get_EndOfMonth_Date') is not null)
    drop function dbo.fn_Get_EndOfMonth_Date
go

create function dbo.fn_Get_EndOfMonth_Date (@InDate date)
  returns date
as 
  begin 
    return EOMONTH(@InDate)
  end