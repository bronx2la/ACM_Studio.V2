use ACM_Studio_V2
go

set identity_insert dbo.tbl_lku_RegionalDirectorRate on

merge into dbo.tbl_lku_RegionalDirectorRate target
using
  (
    values
      (1, 1, 1, 1, 0.00085, getdate(), null, user_name()), --Carroll New Assets SMA
      (2, 1, 1, 2, 0.00130, getdate(), null, user_name()), --Carroll New Assets Mutual Funds
      (3, 1, 1, 3, 0.00085, getdate(), null, user_name()), --Carroll New Assets UMA

      (4, 1, 2, 1, 0.00020, getdate(), null, user_name()), --Carroll On going SMA
      (5, 1, 2, 2, 0.00020, getdate(), null, user_name()), --Carroll On going Mutual Funds
      (6, 1, 2, 3, 0.00020, getdate(), null, user_name()), --Carroll On going UMA

      (7, 2, 1, 1, 0.0012, getdate(), null, user_name()), --Sindici New Assets SMA
      (8, 2, 1, 2, 0.0018, getdate(), null, user_name()), --Sindici New Assets Mutual Funds
      (9, 2, 1, 3, 0.0007, getdate(), null, user_name()), --Sindici New Assets UMA

      (10, 2, 2, 1, 0.0006, getdate(), null, user_name()), --Sindici On going SMA
      (11, 2, 2, 2, 0.0006, getdate(), null, user_name()), --Sindici On going Mutual Funds
      (12, 2, 2, 3, 0.0004, getdate(), null, user_name()), --Sindici On going UMA

      (13, 2, 3, 1, 0.0002, getdate(), null, user_name()), --Sindici Legacy SMA

      (20, 4, 1, 1, 0.00085, getdate(), null, user_name()),--Helton New Assets SMA
      (21, 4, 1, 2, 0.00250, getdate(), null, user_name()),--Helton New Assets Mutual Funds
      (22, 4, 1, 3, 0.00085, getdate(), null, user_name()),--Helton New Assets UMA

      (23, 4, 2, 1, 0.00020, getdate(), null, user_name()),--Helton On going SMA
      (24, 4, 2, 2, 0.00060, getdate(), null, user_name()),--Helton On going Mutual Funds
      (25, 4, 2, 3, 0.00020, getdate(), null, user_name()),--Helton On going UMA

      (26, 6, 1, 1, 0.00085, getdate(), null, user_name()),--Maxwell New Assets SMA
      (27, 6, 1, 2, 0.00250, getdate(), null, user_name()),--Maxwell New Assets Mutual Funds
      (28, 6, 1, 3, 0.00085, getdate(), null, user_name()),--Maxwell New Assets UMA

      (29, 6, 2, 1, 0.00020, getdate(), null, user_name()),--Maxwell On going SMA
      (30, 6, 2, 2, 0.00060, getdate(), null, user_name()),--Maxwell On going Mutual Funds
      (31, 6, 2, 3, 0.00020, getdate(), null, user_name()),--Maxwell On going UMA

      (44, 7, 1, 1, 0.00070, getdate(), null, user_name()),--Sykes On new assets SMA
      (45, 7, 1, 2, 0.00070, getdate(), null, user_name()),--Sykes On new assets Mutual Funds
      (46, 7, 1, 3, 0.00070, getdate(), null, user_name()), --Sykes On new assets UMA

      (47, 7, 2, 1, 0.00060, getdate(), null, user_name()),--Sykes On going SMA
      (48, 7, 2, 2, 0.00021, getdate(), null, user_name()),--Sykes On going Mutual Funds
      (49, 7, 2, 3, 0.00060, getdate(), null, user_name()), --Sykes On going UMA
    
      (38, 8, 1, 1, 0.00085, getdate(), null, user_name()),--Huennekens New Assets SMA
      (39, 8, 1, 2, 0.00250, getdate(), null, user_name()),--Huennekens New Assets Mutual Funds
      (40, 8, 1, 3, 0.00085, getdate(), null, user_name()),--Huennekens New Assets UMA

      (41, 8, 2, 1, 0.00020, getdate(), null, user_name()),--Huennekens On going SMA
      (42, 8, 2, 2, 0.00060, getdate(), null, user_name()),--Huennekens On going Mutual Funds
      (43, 8, 2, 3, 0.00020, getdate(), null, user_name()) --Huennekens On going UMA


  ) as source
  (RegionalDirectorRateIID, RegionalDirectorIID, RateTypeIID, CommissionTypeIID, Rate, CreatedOn, ModifiedOn, ModifiedBy)
on (source.RegionalDirectorRateIID = target.RegionalDirectorRateIID)

when matched
  and target.RegionalDirectorIID <> source.RegionalDirectorIID
      or target.RateTypeIID <> source.RateTypeIID
      or target.CommissionTypeIID <> source.CommissionTypeIID
      or target.Rate <> source.Rate
then
  update
  set
    target.RegionalDirectorIID = source.RegionalDirectorIID,
    target.RateTypeIID         = source.RateTypeIID,
    target.CommissionTypeIID   = source.CommissionTypeIID,
    target.Rate                = source.Rate,
    ModifiedOn                 = getdate(),
    ModifiedBy                 = user_name()

when not matched then
  insert
  (
    RegionalDirectorRateIID,
    RegionalDirectorIID,
    RateTypeIID,
    CommissionTypeIID,
    Rate,
    CreatedOn,
    ModifiedOn,
    ModifiedBy
  )
  values
    (
      source.RegionalDirectorRateIID,
      source.RegionalDirectorIID,
      source.RateTypeIID,
      source.CommissionTypeIID,
      source.Rate,
      source.CreatedOn,
      source.ModifiedOn,
      source.ModifiedBy
    )


when not matched by source then
  delete;

set identity_insert dbo.tbl_lku_RegionalDirectorRate off