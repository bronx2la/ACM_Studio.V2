use ACM_Studio_V2
go

if (object_id('dbo.fn_GetFirstNonZeroNewAssetValue') is not null)
    drop function dbo.fn_GetFirstNonZeroNewAssetValue
go

create function dbo.fn_GetFirstNonZeroNewAssetValue(@value1 money, @value2 money, @value3 money)
  returns money
as 
  begin 
    declare @retval money = cast(0.0 as money)
    
    set @retval = 
    (
      select
        case
            when @value1 <> 0.0 then @value1
            when @value2 <> 0.0 then @value2
            when @value3 <> 0.0 then @value3
            else 0.0
        end as retval
    )
    return @retval
  end