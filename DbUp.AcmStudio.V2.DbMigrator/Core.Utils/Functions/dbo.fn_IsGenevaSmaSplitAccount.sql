use ACM_Studio_V2
go

if (object_id('dbo.fn_IsGenevaSmaSplitAccount') is not null)
    drop function dbo.fn_IsGenevaSmaSplitAccount
 go
 
 create function dbo.fn_IsGenevaSmaSplitAccount (@PortfolioCode varchar(10), @ReportingPeriod varchar(6))
    returns bit
    as
      begin
        declare @result bit = 0
        declare @id int = 
          (
            select SplitAccountIid 
            from 
              dbo.tbl_lku_GenevaSMA_SplitAccounts 
            where 
              PortfolioCode = @PortfolioCode and ReportingPeriod = @ReportingPeriod)

        if @id is not null
          set @result = 1

        return @result
      end
        