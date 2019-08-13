use ACM_Studio_V2
go

if (object_id('dbo.fn_tbl_Geneva_NewAssets_Summary_Detail') is not null)
    drop function dbo.fn_tbl_Geneva_NewAssets_Summary_Detail
 go
 
 create function dbo.fn_tbl_Geneva_NewAssets_Summary_Detail (@StartDate date, @EndDate date )
   returns @NewAssetsSummary table
   (
     RegionalDirectorKey    varchar(500),
     ConsultantFirm         varchar(500),
     ConsultantName         varchar(500),
     Assets                 money,
     Flows                  money,
     PayableAmount          money,
     Commission             money,
     Rate                   decimal(20,6),
     AnnualRate             decimal(20,6)
   )
    
    as
      begin

          declare @TempSummary table
          (
            RegionalDirectorKey    varchar(500),
            ConsultantFirm         varchar(500),
            ConsultantName         varchar(500),
            Assets                 money,
            Flows                  money,
            PayableAmount          money,
            Commission             money,
            Rate                   decimal(20,6),
            AnnualRate             decimal(20,6)
          )

          declare @SummSummary table
          (
            RegionalDirectorKey    varchar(500),
            ConsultantFirm         varchar(500),
            ConsultantName         varchar(500),
            Assets                 money,
            Flows                  money,
            PayableAmount          money,
            Commission             money,
            Rate                   decimal(20,6),
            AnnualRate             decimal(20,6)
          )
        
        declare @SmaNewAssets table
        (
          Id                     int primary key not null identity (1,1),
          InternalMarketingPerson varchar(500),
          SecurityID             int,
          Portfolio          varchar(500),
          PortStartDate       date,
          TradeDate           date,
          TranType            varchar(100),
          Strategy            varchar(500),
          ConsultantFirm      varchar(500),
          ConsultantName      varchar(500),
          Ticker              varchar(500),
          Security            varchar(500),
          Quantity            decimal(20,6),
          Price               decimal(20,6),
          Amount              money,
          CommissionRate      decimal(20,6),
          CommissionAmount    money,
          IsStartBeforePeriod bit,
          SumTradeAmountByDate money,
          SumTradeAmountByPortfolioCode money,
          IsValid bit
        )
        
        declare @cRegionalDirectorIID int
        declare @cRegionDirectorKey varchar(500)
        declare @cFirstName varchar(500)
        declare @cLastName varchar(500)
        declare @cSeasoningMonths int
        declare @cur cursor

        delete from @SmaNewAssets where 1 = 1
        insert into @SmaNewAssets
          select
            InternalMarketingPerson,
            ID as SecurityID,
            Portfolio,
            PortStartDate,
            TradeDate,
            TranType,
            Strategy,
            ConsultantFirm,
            ConsultantName,
            Ticker,
            Security,
            Quantity,
            Price,
            Amount,
            CommissionRate,
            CommissionAmount,
            IsStartBeforePeriod,
            SumTradeAmountByDate,
            SumTradeAmountByPortfolioCode,
            IsValid
          from dbo.fn_tbl_GenevaSma_NewAssets_Detail(@StartDate, @EndDate)
          where IsStartBeforePeriod = 'False'

          insert into @TempSummary
          select * from
            (
              select
                InternalMarketingPerson as RegionalDirectorKey,
                ConsultantFirm,
                ConsultantName,
                sum(Amount) as Assets,
                null as Flows,
                null as PayableAmount,
                sum(CommissionAmount) as Commission,
                min(CommissionRate) as Rate,
                min(CommissionRate) as AnnualRate
              from @SmaNewAssets
              where IsValid = 'TRUE'
              group by InternalMarketingPerson, ConsultantFirm, ConsultantName
            ) as dummy

          insert into @SummSummary
          select * from
            (
              select
                InternalMarketingPerson as RegionalDirectorKey,
                'zz' as ConsultantFirm,
                'zz' as ConsultantName,
                sum(Amount) as Assets,
                null as Flows,
                null as PayableAmount,
                sum(CommissionAmount) as Commission,
                min(CommissionRate) as Rate,
                min(CommissionRate) as AnnualRate
              from @SmaNewAssets
              where IsValid = 'TRUE'
              group by InternalMarketingPerson
            ) as dummy

          insert into @NewAssetsSummary
            select * from @TempSummary

          insert into @NewAssetsSummary
            select * from @SummSummary  
--         insert into @NewAssetsSummary
--           select * from
--           (
--             select
--               InternalMarketingPerson as RegionalDirectorKey,
--               ConsultantFirm,
--               ConsultantName,         
--               sum(Amount) as Assets,
--               null as Flows,
--               null as PayableAmount,
--               sum(CommissionAmount) as Commission,         
--               min(CommissionRate) as Rate,       
--               min(CommissionRate) as AnnualRate     
--             from @SmaNewAssets
--             where IsValid = 'TRUE'
--             group by InternalMarketingPerson, ConsultantFirm, ConsultantName
--           ) as dummy

        return 
      end
        