use ACM_Studio_V2
go

if (object_id('dbo.fn_GetReportingPeriodString') is not null)
    drop function dbo.fn_GetReportingPeriodString
 go
 
 create function dbo.fn_GetReportingPeriodString (@DateValue date)
    returns varchar(6) 
    as
      begin
        declare @quarter varchar(2)
        declare @yearvalue varchar(4)
        declare @result varchar(6)
      
        set @yearvalue = cast(datepart(year, @DateValue) as varchar(4))
        
        if datepart(month, @DateValue) between 1 and 3
          set @quarter = 'Q1'

        if datepart(month, @DateValue) between 4 and 6
          set @quarter = 'Q2'

        if datepart(month, @DateValue) between 7 and 9
          set @quarter = 'Q3'

        if datepart(month, @DateValue) between 10 and 12
          set @quarter = 'Q4'
        
        set @result = @yearvalue + @quarter
        return @result
      end
        