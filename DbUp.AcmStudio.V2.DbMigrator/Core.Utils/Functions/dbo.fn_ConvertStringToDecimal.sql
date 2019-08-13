use ACM_Studio_V2
go

if (object_id('dbo.fn_ConvertStringToDecimal') is not null)
  drop function dbo.fn_ConvertStringToDecimal
go

create function dbo.fn_ConvertStringToDecimal (@string varchar(50))
  returns decimal(20, 6)
as
  begin
    set @string = replace(@string, '$', '')
    set @string = replace(@string, ',', '')
    set @string = replace(@string, '(', '')
    set @string = replace(@string, ')', '')

    return cast(@string as decimal(20,6))
  end