use ACM_Studio_V2
go

if (object_id('dbo.fn_ConvertCurrencyToMoney') is not null)
    drop function dbo.fn_ConvertCurrencyToMoney
go

create function dbo.fn_ConvertCurrencyToMoney (@string varchar(50))
  returns money
as 
  begin 
    set @string = replace(@string, '$', '')
    set @string = replace(@string, ',', '')
    set @string = replace(@string, '(', '')
    set @string = replace(@string, ')', '')
    set @string = replace(@string, ' ', '')
    
    return cast(@string as money)
  end