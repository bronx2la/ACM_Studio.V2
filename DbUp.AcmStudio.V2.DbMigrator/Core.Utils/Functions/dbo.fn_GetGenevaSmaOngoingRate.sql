use ACM_Studio_V2
go

if (object_id('dbo.fn_GetGenevaSmaOngoingRate') is not null)
    drop function dbo.fn_GetGenevaSmaOngoingRate
 go
 
 create function dbo.fn_GetGenevaSmaOngoingRate (@ACMmarketing varchar(100), @PortfolioCode varchar(100), @QuarterlyRate decimal(12,6), @EndDate date)
    returns decimal(12, 6)
    as
      begin
        declare @result decimal(12,6) = dbo.fn_GetRate(@ACMmarketing, @QuarterlyRate)

        if charindex('-', @ACMmarketing) > 0
          set @result = @result / 2

        if dbo.fn_IsGenevaSmaSplitAccount(@PortfolioCode, dbo.fn_GetReportingPeriodString(@EndDate)) = 1
          set @result = @result / 2

        return @result
      end
        