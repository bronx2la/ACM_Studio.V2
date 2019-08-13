use ACM_Studio_V2
go

if (object_id('dbo.fn_Get_StartOfMonth_Date') is not null)
  drop function dbo.fn_Get_StartOfMonth_Date
go

create function dbo.fn_Get_StartOfMonth_Date (@InDate date)
  returns date
as
  begin
    return DATEADD(month, DATEDIFF(month, 0, @InDate), 0)
  end