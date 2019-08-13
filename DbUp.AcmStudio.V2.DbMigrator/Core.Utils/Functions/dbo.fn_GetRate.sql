use ACM_Studio_V2
go

if (object_id('dbo.fn_GetRate') is not null)
    drop function dbo.fn_GetRate
 go
 
 create function dbo.fn_GetRate (@regionalDirector varchar(80), @rate decimal(12,6) )
    returns decimal(12,6)
    as
        begin
          if charindex(',', @regionalDirector) > 0
            set @rate = @rate / 2
          return @rate
        end
        