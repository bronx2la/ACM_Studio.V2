use ACM_Studio_V2
go

if (object_id('dbo.prc_BroadridgeSales_Transform') is not null)
    drop procedure dbo.prc_BroadridgeSales_Transform
go

create procedure dbo.prc_BroadridgeSales_Transform
    @ReportingDate date

as
    delete from dbo.tbl_BroadridgeSales
        where ReportingDate = @ReportingDate
        
    insert into dbo.tbl_BroadridgeSales
        select
            TradeID                            ,
            TransactionCodeOverrideDescription ,
            cast(TradeDate as date)            ,
            cast(SettledDate as date)           ,
            cast(SuperSheetDate as date)       ,
            cast(TradeAmount as money)         ,
            [System]                           ,
            DealerNum                          ,
            DealerBranchBranchCode             ,
            RepCode                            ,
            FirmId                             ,
            FirmName                           ,
            OfficeAddressLine1                 ,
            OfficeCity                         ,
            OfficeRegionRefCode                ,
            OfficePostalCode                   ,
            PersonFirstName                    ,
            PersonLastName                     ,
            LineOfBusiness                     ,
            Channel                            ,
            Region                             ,
            Territory                          ,
            ProductNasdaqSymbol                ,
            ProductName                        ,
            @ReportingDate                        
        from dbo.tbl_BroadridgeSales_Staging
        where TradeID <> 'TradeID'
go
    