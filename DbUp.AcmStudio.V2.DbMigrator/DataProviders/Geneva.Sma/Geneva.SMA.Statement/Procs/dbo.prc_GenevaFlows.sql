use ACM_Studio_V2
go

if (object_id('dbo.prc_GenevaFlows') is not null)
    drop procedure dbo.prc_GenevaFlows
go

create procedure dbo.prc_GenevaFlows
    @ReportEndDate date
as
    select distinct
        Portfolio,
        PortStartDate,
        Sum(Amount) as FlowAmount
    from dbo.tbl_GenevaSma
    where ReportEndDate3 = @ReportEndDate
    group by Portfolio, PortStartDate
    order by Portfolio

go
    