use ACM_Studio_V2
go

if (object_id('dbo.tbl_TaskMetaData') is null)
begin
    create table dbo.tbl_TaskMetaData
    (
      TaskMetaDataIID int primary key not null,
      ObjectCode varchar(50),
      EnvironmentIID int,
      DbServer varchar(60),
      DatabaseName varchar(60),
      StagingTable varchar(200),
      ProdTable varchar(200),
      FileTypeIID int,
      DelimiterChar char,
      InboundFolder varchar(200),
      InboundFile varchar(200),
      OutboundFolder varchar(200),
      OutboundFile varchar(200),
      IsTruncatable bit,
      CreatedOn datetime,
      ModifiedOn datetime,
      ModifiedBy varchar(60),
      RunStatus tinyint
    );
end
go

