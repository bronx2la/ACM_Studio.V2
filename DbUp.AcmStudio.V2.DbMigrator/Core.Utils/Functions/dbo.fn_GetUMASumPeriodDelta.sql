use ACM_Studio_V2
go

if (object_id('dbo.fn_GetUMASumPeriodDelta') is not null)
  drop function dbo.fn_GetUMASumPeriodDelta
go

create function dbo.fn_GetUMASumPeriodDelta
  (
    @Delta1 money, @Threshold1 money,
    @Delta2 money, @Threshold2 money,
    @Delta3 money, @Threshold3 money
  )
  returns money
as

  begin
    declare @value as money = 0.0

    if (isnull(@Delta1, 0.0) > 0) and (@Delta1 > @Threshold1)
      set @value = @value + @Delta1

    if (isnull(@Delta2, 0.0) > 0) and (@Delta2 > @Threshold2)
      set @value = @value + @Delta2

    if (isnull(@Delta3, 0.0) > 0) and (@Delta3 > @Threshold3)
      set @value = @value + @Delta3

    return @value
  end