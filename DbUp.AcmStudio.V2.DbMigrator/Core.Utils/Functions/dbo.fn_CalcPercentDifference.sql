use ACM_Studio_V2
go

if(object_id('dbo.fn_CalcPercentDifference') is not null)
    drop function dbo.fn_CalcPercentDifference
go

create function dbo.fn_CalcPercentDifference(@oldValue money, @newValue money)
  returns decimal(12,6)

as
  begin
    declare @result decimal(12,6)

    set @oldValue = isnull(@oldValue, 0.0)
    set @newValue = isnull(@newValue, 0.0)

    if @oldValue = 0.0
      set @result = 0.0
    else
      begin
        if (@newValue - @oldValue) < 0
          set @result = 0.0
        else
          set @result = (@newValue - @oldValue) / @oldValue
      end

    return @result
  end