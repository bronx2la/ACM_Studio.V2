use ACM_Studio_V2
go

if (object_id('dbo.prc_TaskMetaData_Fetch_By_ObjectCode') is not null)
  drop procedure dbo.prc_TaskMetaData_Fetch_By_ObjectCode
go

create procedure dbo.prc_TaskMetaData_Fetch_By_ObjectCode
    @ObjectCode varchar(40)
as
  begin
    select
      TaskMetaDataIID,
      ObjectCode,
      EnvironmentIID,
      DbServer,
      DatabaseName,
      StagingTable,
      ProdTable,
      FileTypeIID,
      DelimiterChar,
      InboundFolder,
      InboundFile,
      OutboundFolder,
      OutboundFile,
      IsTruncatable,
      CreatedOn,
      ModifiedOn,
      ModifiedBy,
      RunStatus
    from dbo.tbl_TaskMetaData
    where
      ObjectCode = @ObjectCode
  end
    
    