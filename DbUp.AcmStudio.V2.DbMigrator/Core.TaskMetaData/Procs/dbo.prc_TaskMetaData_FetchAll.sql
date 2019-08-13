use ACM_Studio_V2
go

if (object_id('dbo.prc_TaskMetaData_FetchAll') is not null)
  drop procedure dbo.prc_TaskMetaData_FetchAll
go


create procedure dbo.prc_TaskMetaData_FetchAll
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
      ModifiedOn
        ModifiedBy,
      RunStatus
    from dbo.tbl_TaskMetaData
    order by TaskMetaDataIID
  end
    
    