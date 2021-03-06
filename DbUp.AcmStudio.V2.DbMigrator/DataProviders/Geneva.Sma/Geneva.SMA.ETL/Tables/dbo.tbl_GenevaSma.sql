use ACM_Studio_V2
go

if (object_id('dbo.tbl_GenevaSma') is null)
begin
    create table dbo.tbl_GenevaSma
    (
      Id int primary key not null identity (1,1),
      Textbox1 varchar(500),
      Textbox3 varchar(500),
      Textbox5 varchar(500),
      Textbox59 varchar(500),
      Textbox60 varchar(500),
      Textbox41 varchar(500),
      Textbox32 varchar(500),
      PortfolioCode varchar(500),
      ReportEndDate3 date,
      Portfolio varchar(500),
      PortShortName varchar(500),
      PortStartDate date,
      TradeDate date,
      TranType varchar(500),
      TranDesc varchar(500),
      InvCode varchar(500),
      Security varchar(500),
      Quantity decimal(20,6),
      Price decimal(20,6),
      Amount money,
      InvestmentType varchar(500),
      Strategy varchar(500),
      ConsultantFirm varchar(500),
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