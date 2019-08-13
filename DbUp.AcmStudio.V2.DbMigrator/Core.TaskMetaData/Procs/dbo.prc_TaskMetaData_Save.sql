use ACM_Studio_V2
go

if (object_id('dbo.prc_TaskMetaData_Save') is not null)
  drop procedure dbo.prc_TaskMetaData_Save
go

create procedure dbo.prc_TaskMetaData_Save
    @TaskMetaDataIID int,
    @ObjectCode varchar(50),
    @EnvironmentIID int,
    @DbServer varchar(60),
    @DatabaseName varchar(60),
    @StagingTable varchar(200),
    @ProdTable varchar(200),
    @FileTypeIID int,
    @DelimiterChar char,
    @InboundFolder varchar(200),
    @InboundFile varchar(200),
    @OutboundFolder varchar(200),
    @OutboundFile varchar(200),
    @IsTruncatable bit,
    @CreatedOn datetime,
    @ModifiedOn datetime,
    @ModifiedBy varchar(60),
    @RunStatus tinyint
as
  begin
    update dbo.tbl_TaskMetaData
    set

      ObjectCode = isnull(@ObjectCode, ObjectCode) ,
      EnvironmentIID = isnull(@EnvironmentIID, EnvironmentIID),
      DbServer = isnull(@DbServer, DbServer) ,
      DatabaseName = isnull(@DatabaseName, DatabaseName) ,
      StagingTable = isnull(@StagingTable, StagingTable) ,
      ProdTable = isnull(@ProdTable, ProdTable) ,
      FileTypeIID = isnull(@FileTypeIID, FileTypeIID) ,
      DelimiterChar = isnull(@DelimiterChar, DelimiterChar) ,
      InboundFolder = isnull(@InboundFolder, InboundFolder) ,
      InboundFile = isnull(@InboundFile, InboundFile),
      OutboundFolder = isnull(@OutboundFolder, OutboundFolder),
      OutboundFile = isnull(@OutboundFile, OutboundFile),
      IsTruncatable = isnull(@IsTruncatable, IsTruncatable),
      CreatedOn = isnull(@CreatedOn, CreatedOn),
      ModifiedOn = isnull(@ModifiedOn, ModifiedOn),
      ModifiedBy = isnull(@ModifiedBy, ModifiedBy),
      RunStatus = isnull(@RunStatus, RunStatus)
    where
      TaskMetaDataIID = @TaskMetaDataIID

    if @@rowcount > 0
      return

    insert into dbo.tbl_TaskMetaData
    (
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
    )
    values
      (
        @ObjectCode,
        @EnvironmentIID,
        @DbServer,
        @DatabaseName,
        @StagingTable,
        @ProdTable,
        @FileTypeIID,
        @DelimiterChar,
        @InboundFolder,
        @InboundFile,
        @OutboundFolder,
        @OutboundFile,
        @IsTruncatable,
        @CreatedOn,
        @ModifiedOn,
        @ModifiedBy,
        @RunStatus
      )

  end 