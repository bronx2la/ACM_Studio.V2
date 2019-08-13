use ACM_Studio_V2
go

truncate table dbo.tbl_TaskMetaData

merge into dbo.tbl_TaskMetaData target
using
(
  values
         (100, 'ETL.BROADRIDGE.SALES', 1, 'LocalHost\SQLExpress', 'ACM_Studio_V2', 'dbo.tbl_BroadridgeSales_Staging', 'dbo.tbl_BroadridgeSales', 1, ',', 'c:\Development.Acm\Inbound\Broadridge', 'BroadridgeSales - {0}.csv', 'N/A', 'N/A', 1, getdate(), getdate(), user_name(), 0),
         (101, 'ETL.BROADRIDGE.ASSETS', 1, 'LocalHost\SQLExpress', 'ACM_Studio_V2', 'dbo.tbl_BroadridgeAssets_Staging', 'dbo.tbl_BroadridgeAssets', 1, ',', 'c:\Development.Acm\Inbound\Broadridge', 'BroadridgeAssets - {0}.csv', 'N/A', 'N/A', 1, getdate(), getdate(), user_name(), 0),
         (103, 'ETL.MF.SALES', 2, 'LocalHost\SQLExpress', 'ACM_Studio_V2', 'tbl_MutualFunds_Sales_Staging', 'tbl_MF_Sales', 1, ',', 'C:\Inbound\MarsData\SalesData', 'mf_sales_data_{0}.csv', null, null, null, getdate(), getdate(), user_name(), 0),
         (104, 'ETL.MF.QE.ASSETS', 2, 'LocalHost\SQLExpress', 'ACM_Studio_V2', 'tbl_MutualFunds_QuarterEnd_Assets_Staging', 'tbl_MF_QuarterEnd_Assets', 1, ',', 'C:\Inbound\MarsData\QE_Assets', 'Monthly{0}.csv', null, null, null, getdate(), getdate(), user_name(), 0),
         (105, 'ETL.UMA.MERRILL', 2, 'LocalHost\SQLExpress', 'ACM_Studio_V2', 'tbl_UMA_Merrill_Staging', 'tbl_UMA_Merrill', 1, ',', 'C:\Inbound\UMA\Merrill', '{0} Merrill Assets.xlsx', 'N/A', 'N/A', 1, getdate(), getdate(), user_name(), 0),
         (106, 'ETL.UMA.MORGAN', 2, 'LocalHost\SQLExpress', 'ACM_Studio_V2', 'tbl_UMA_Morgan_Staging', 'tbl_UMA_Morgan', 1, ',', 'C:\Inbound\UMA\Morgan', 'Morgan Stanley UMA Account List {0}.csv', 'N/A', 'N/A', 1, getdate(), getdate(), user_name(), 0),
         (107, 'ETL.GENEVA.SMA', 2, 'LocalHost\SQLExpress', 'ACM_Studio_V2', 'tbl_GenevaSma_Staging', 'tbl_GenevaSma', 1, ',', 'C:\Development.Acm\Inbound\Geneva\', 'SMA Sales Data.csv', 'N/A', 'N/A', 1, getdate(), getdate(), user_name(), 0),
         (108, 'ETL.GENEVA.AUM', 2, 'LocalHost\SQLExpress', 'ACM_Studio_V2', 'tbl_GenevaAum_Staging', 'tbl_GenevaAum', 1, ',', 'C:\Development.Acm\Inbound\Geneva\', 'SMA AUM Data.csv', 'N/A', 'N/A', 1, getdate(), getdate(), user_name(), 0),
         (109, 'ETL.BROADRIDGE', 2, 'LocalHost\SQLExpress', 'ACM_Studio_V2', 'tbl_Broadridge_Staging', 'tbl_Broadridge', 1, ',', 'C:\Inbound\Broadridge\', 'tabularshell.csv', 'N/A', 'N/A', 1, getdate(), getdate(), user_name(), 0),
--         (108, 'ETL.BROADRIDGE.MUTUALFUNDS', 2, 'LocalHost\SQLExpress', 'ACM_Studio_V2', 'tbl_Broadridge_MutualFunds_Staging', 'tbl_Broadridge_MutualFunds', 1, ',', 'C:\Inbound\Broadridge\', 'MutualFunds.csv', 'N/A', 'N/A', 1, getdate(), getdate(), user_name(), 0),

         (200, 'COMM.UMA.MORGAN', 2, 'LocalHost\SQLExpress', 'ACM_Studio_V2', 'N/A', 'N/A', null, null, 'N/A', 'N/A', 'C:\Development.Acm\Outbound\UMA\Morgan', 'UMA_Morgan_{0}_{1}.xlsx', null, getdate(), getdate(), user_name(), 0),
         (201, 'COMM.UMA.MERRILL', 2, 'LocalHost\SQLExpress', 'ACM_Studio_V2', 'N/A', 'N/A', null, null, 'N/A', 'N/A', 'C:\Development.Acm\Outbound\UMA\Merrill', 'UMA_Merrill_{0}_{1}.xlsx', null, getdate(), getdate(), user_name(), 0),
         (202, 'COMM.MUTUALFUNDS', 2, 'LocalHost\SQLExpress', 'ACM_Studio_V2', 'N/A', 'N/A', null, null, 'N/A', 'N/A', 'C:\Development.Acm\Outbound\MutualFunds', 'MutualFunds_{0}_{1}.xlsx', null, getdate(), getdate(), user_name(), 0),
         (203, 'COMM.GENEVA.SMA', 2, 'LocalHost\SQLExpress', 'ACM_Studio_V2', 'N/A', 'N/A', null, null, 'N/A', 'N/A', 'C:\Development.Acm\Outbound\Geneva.Sma', 'GenevaSma_{0}_{1}.xlsx', null, getdate(), getdate(), user_name(), 0),
         (204, 'COMM.BROADRIDGE.UMA', 2, 'LocalHost\SQLExpress', 'ACM_Studio_V2', 'N/A', 'N/A', null, null, 'N/A', 'N/A', 'C:\Development.Acm\Outbound\Broadridge.Uma', 'BroadridgeUma_{0}_{1}.xlsx', null, getdate(), getdate(), user_name(), 0),
         (205, 'COMM.BROADRIDGE.MERRILL_MORGAN.UMA', 2, 'LocalHost\SQLExpress', 'ACM_Studio_V2', 'N/A', 'N/A', null, null, 'N/A', 'N/A', 'C:\Development.Acm\Outbound\Broadridge.Uma', 'BroadridgeUma_MerrillMorgan{0}_{1}.xlsx', null, getdate(), getdate(), user_name(), 0),
         (206, 'COMM.BROADRIDGE.MUTUALFUNDS', 2, 'LocalHost\SQLExpress', 'ACM_Studio_V2', 'N/A', 'N/A', null, null, 'N/A', 'N/A', 'C:\Development.Acm\Outbound\Broadridge.MutualFunds', 'BroadridgeMutualFunds_{0}_{1}.xlsx', null, getdate(), getdate(), user_name(), 0)


  --       (404, 'ETL.UMA.FISERV', 2, 'ACMLAWS024', 'ACM_Studio_V2', 'tbl_UMA_Fiserv_Staging', 'tbl_UMA_Fiserv', 1, ',', 'H:\Inbound\UMA\RBC_Fiserv', 'AUM Aristotle - RBCWM Overlay - {0} - {1}.csv', 'N/A', 'N/A', 1, getdate(), getdate(), user_name(), 0),
  --       (405, 'ETL.UMA.FISERV.UPD1', 2, 'ACMLAWS024', 'ACM_Studio_V2', 'tbl_UMA_Fiserv_Upd1_Staging', 'tbl_UMA_Fiserv_Upd1', 1, ',', 'H:\Inbound\UMA\RBC_Fiserv', '{0} VAC.xlsx', 'N/A', 'N/A', 1, getdate(), getdate(), user_name(), 0),
  --       (406, 'ETL.UMA.PLACEMARK', 2, 'ACMLAWS024', 'ACM_Studio_V2', 'tbl_UMA_Placemark_Staging', 'tbl_UMA_Placemark', 1, ',', 'H:\Inbound\UMA\RBC_Placemark', 'uma_rbc_placemark_{0}.csv', 'N/A', 'N/A', 1, getdate(), getdate(), user_name(), 0),
  --       (500, 'COMM.MUTUALFUNDS', 2, 'ACMLAWS024', 'ACM_Studio_V2', 'N/A', 'N/A', null, null, 'N/A', 'N/A', 'C:\ITBackoffice\Commissions\{0}\MutualFunds', 'MutualFunds_{0}_{1}.xlsx', null, getdate(), getdate(), user_name(), 0),
  --       (501, 'COMM.SMA', 2, 'ACMLAWS024', 'ACM_Studio_V2', 'N/A', 'N/A', null, null, 'N/A', 'N/A', 'C:\ITBackoffice\Commissions\{0}\SMA', 'SMA_{0}_{1}.xlsx', null, getdate(), getdate(), user_name(), 0),
  --       (502, 'COMM.UMA.MORGAN', 2, 'ACMLAWS024', 'ACM_Studio_V2', 'N/A', 'N/A', null, null, 'N/A', 'N/A', 'C:\ITBackoffice\Commissions\{0}\UMA\Morgan', 'UMA_Morgan_{0}_{1}.xlsx', null, getdate(), getdate(), user_name(), 0),
  --       (503, 'COMM.UMA.MERRILL', 2, 'ACMLAWS024', 'ACM_Studio_V2', 'N/A', 'N/A', null, null, 'N/A', 'N/A', 'C:\ITBackoffice\Commissions\{0}\UMA\Merrill', 'UMA_Merrill_{0}_{1}.xlsx', null, getdate(), getdate(), user_name(), 0),
  --       (504, 'COMM.UMA.RBCPLACEMARK', 2, 'ACMLAWS024', 'ACM_Enterprise', null, null, null, null, null, null, 'C:\ITBackoffice\Commissions\{0}\UMA\RbcPlacemark', 'UMA_RbcPlacemark_{0}_{1}.xlsx', null, getdate(), getdate(), user_name(), 0),
  --       (505, 'COMM.UMA.RBCFISERV', 2, 'ACMLAWS024', 'ACM_Enterprise', null, null, null, null, null, null, 'C:\ITBackoffice\Commissions\{0}\UMA\RbcFiserv', 'UMA_RbcFiserv_{0}_{1}.xlsx', null, getdate(), getdate(), user_name(), 0),
  -- 
  --       (700, 'ETL.UMA.MORGAN', 3, 'ACMLAWS024', 'ACM_Studio_V2', 'tbl_UMA_Morgan_Staging', 'tbl_UMA_Morgan', 1, ',', 'H:\Inbound\UMA\Morgan', 'Morgan Stanley Account Name and Address List.csv', 'N/A', 'N/A', 1, getdate(), getdate(), user_name(), 0),
  --       (701, 'ETL.MF.SALES', 3, 'ACMLAWS024', 'ACM_Studio_V2', 'tbl_MF_Sales_Staging', 'tbl_MF_Sales', 1, ',', 'H:\Inbound\MarsData\SalesData', 'mf_sales_data_{0}.csv', null, null, null, getdate(), getdate(), user_name(), 0),
  --       (702, 'ETL.MF.QE.ASSETS', 3, 'ACMLAWS024', 'ACM_Studio_V2', 'tbl_MF_QuarterEnd_Assets_Staging', 'tbl_MF_QuarterEnd_Assets', 1, ',', 'H:\Inbound\MarsData\QE_Assets', 'Monthly{0}.csv', null, null, null, getdate(), getdate(), user_name(), 0),
  --       (703, 'ETL.UMA.MERRILL', 3, 'ACMLAWS024', 'ACM_Studio_V2', 'tbl_UMA_Merrill_Staging', 'tbl_UMA_Merrill', 1, ',', 'H:\Inbound\UMA\Merrill', '{0}_ML_EnhCal_Aristotle_Monthly_ML_Report.csv', 'N/A', 'N/A', 1, getdate(), getdate(), user_name(), 0),
  --       (704, 'ETL.UMA.FISERV', 3, 'ACMLAWS024', 'ACM_Studio_V2', 'tbl_UMA_Fiserv_Staging', 'tbl_UMA_Fiserv', 1, ',', 'H:\Inbound\UMA\RBC_Fiserv', 'AUM Aristotle - RBCWM Overlay - {0} - {1}.csv', 'N/A', 'N/A', 1, getdate(), getdate(), user_name(), 0),
  --       (705, 'ETL.UMA.FISERV.UPD1', 3, 'ACMLAWS024', 'ACM_Studio_V2', 'tbl_UMA_Fiserv_Staging_Upd1', 'tbl_UMA_Fiserv_Upd1', 1, ',', 'H:\Inbound\UMA\RBC_Fiserv', '{0} VAC.xlsx', 'N/A', 'N/A', 1, getdate(), getdate(), user_name(), 0),
  --       (706, 'ETL.UMA.PLACEMARK', 3, 'ACMLAWS024', 'ACM_Studio_V2', 'tbl_UMA_Placemark_Staging', 'tbl_UMA_Placemark', 1, ',', 'H:\Inbound\UMA\RBC_Placemark', 'uma_rbc_placemark_{0}.csv', 'N/A', 'N/A', 1, getdate(), getdate(), user_name(), 0),
  --       (800, 'COMM.MUTUALFUNDS', 3, 'ACMLAWS024', 'ACM_Studio_V2', 'N/A', 'N/A', null, null, 'N/A', 'N/A', 'P:\ITBackoffice\Commissions\{0}\MutualFunds', 'MutualFunds_{0}_{1}.xlsx', null, getdate(), getdate(), user_name(), 0),
  --       (801, 'COMM.SMA', 3, 'ACMLAWS024', 'ACM_Studio_V2', 'N/A', 'N/A', null, null, 'N/A', 'N/A', 'P:\ITBackoffice\Commissions\{0}\SMA', 'SMA_{0}_{1}.xlsx', null, getdate(), getdate(), user_name(), 0),
  --       (802, 'COMM.UMA.MORGAN', 3, 'ACMLAWS024', 'ACM_Studio_V2', 'N/A', 'N/A', null, null, 'N/A', 'N/A', 'P:\ITBackoffice\Commissions\{0}\UMA\Morgan', 'UMA_Morgan_{0}_{1}.xlsx', null, getdate(), getdate(), user_name(), 0),
  --       (803, 'COMM.UMA.MERRILL', 3, 'ACMLAWS024', 'ACM_Studio_V2', 'N/A', 'N/A', null, null, 'N/A', 'N/A', 'P:\ITBackoffice\Commissions\{0}\UMA\Merrill', 'UMA_Merrill_{0}_{1}.xlsx', null, getdate(), getdate(), user_name(), 0),
  --       (804, 'COMM.UMA.RBCPLACEMARK', 3, 'ACMLAWS024', 'ACM_Enterprise', null, null, null, null, null, null, 'P:\ITBackoffice\Commissions\{0}\UMA\RbcPlacemark', 'UMA_RbcPlacemark_{0}_{1}.xlsx', null, getdate(), getdate(), user_name(), 0),
  --       (805, 'COMM.UMA.RBCFISERV', 3, 'ACMLAWS024', 'ACM_Enterprise', null, null, null, null, null, null, 'P:\ITBackoffice\Commissions\{0}\UMA\RbcFiserv', 'UMA_RbcFiserv_{0}_{1}.xlsx', null, getdate(), getdate(), user_name(), 0)
  )
    as source
  (
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
  )
on (source.TaskMetaDataIID = target.TaskMetaDataIID) and (source.EnvironmentIID = target.EnvironmentIID)
when matched
  and source.ObjectCode <> target.ObjectCode
      or source.EnvironmentIID <> target.EnvironmentIID
      or source.DbServer <> target.DbServer
      or source.DatabaseName <> target.DatabaseName
      or source.StagingTable <> target.StagingTable
      or source.ProdTable <> target.ProdTable
      or source.FileTypeIID <> target.FileTypeIID
      or source.DelimiterChar <> target.DelimiterChar
      or source.InboundFolder <> target.InboundFolder
      or source.InboundFile <> target.InboundFile
      or source.OutboundFolder <> target.OutboundFolder
      or source.OutboundFile <> target.OutboundFile
      or source.IsTruncatable <> target.IsTruncatable
      or source.CreatedOn <> target.CreatedOn
      or source.ModifiedOn <> target.ModifiedOn
      or source.ModifiedBy <> target.ModifiedBy
      or source.RunStatus <> target.RunStatus
then update set
  target.ObjectCode     = source.ObjectCode,
  target.EnvironmentIID = source.EnvironmentIID,
  target.DbServer       = source.DbServer,
  target.DatabaseName   = source.DatabaseName,
  target.StagingTable   = source.StagingTable,
  target.ProdTable      = source.ProdTable,
  target.FileTypeIID    = source.FileTypeIID,
  target.DelimiterChar  = source.DelimiterChar,
  target.InboundFolder  = source.InboundFolder,
  target.InboundFile    = source.InboundFile,
  target.OutboundFolder = source.OutboundFolder,
  target.OutboundFile   = source.OutboundFile,
  target.IsTruncatable  = source.IsTruncatable,
  target.CreatedOn      = source.CreatedOn,
  target.ModifiedOn     = source.ModifiedOn,
  target.ModifiedBy     = source.ModifiedBy,
  target.RunStatus      = source.RunStatus
when not matched then
  insert
  (
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
  )
  values
  (
    source.TaskMetaDataIID,
    source.ObjectCode,
    source.EnvironmentIID,
    source.DbServer,
    source.DatabaseName,
    source.StagingTable,
    source.ProdTable,
    source.FileTypeIID,
    source.DelimiterChar,
    source.InboundFolder,
    source.InboundFile,
    source.OutboundFolder,
    source.OutboundFile,
    source.IsTruncatable,
    source.CreatedOn,
    source.ModifiedOn,
    source.ModifiedBy,
    RunStatus
  )

when not matched by source then
  delete;


