use ACM_Studio_V2
go

if (object_id('dbo.tbl_GenevaSma_Staging') is null)
begin
    create table dbo.tbl_GenevaSma_Staging
    (
       Textbox1 varchar(500),
       Textbox3 varchar(500),
       Textbox5 varchar(500),
       Textbox59 varchar(500),
       Textbox60 varchar(500),
       Textbox41 varchar(500),
       Textbox32 varchar(500),
       PortfolioCode varchar(500),
       ReportEndDate3 varchar(500),
       Portfolio varchar(500),
       PortShortName varchar(500),
       PortStartDate varchar(500),
       TradeDate varchar(500),
       TranType varchar(500),
       TranDesc varchar(500),
       InvCode varchar(500),
       Security varchar(500),
       Quantity varchar(500),
       Price varchar(500),
       Amount varchar(500),
       InvestmentType varchar(500),
       Strategy varchar(500),
       ConsultantFirm varchar(500),
       ConsultantName varchar(500),
       PM varchar(500),
       InternalMarketingPerson varchar(500),
       SalesTeam varchar(500),
       RM varchar(500),
       RM2 varchar(500),
       RM3 varchar(500),
       Sponsor varchar(500)
    );
end
go