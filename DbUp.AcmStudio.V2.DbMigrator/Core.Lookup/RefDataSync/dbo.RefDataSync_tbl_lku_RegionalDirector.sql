use ACM_Studio_V2
go


truncate table dbo.tbl_lku_RegionalDirector 
  
set identity_insert dbo.tbl_lku_RegionalDirector on

merge into dbo.tbl_lku_RegionalDirector target
using
  (
    values
      (1, 'mcarroll', 'Michael', 'Carroll', 3, getdate(), null, user_name()),
      (2, 'msindici', 'Michael', 'Sindici', 12, getdate(), null, user_name()),
      (3, 'unk', 'Unknown', 'Unknown', -1, getdate(), null, user_name()),
      (4, 'khelton', 'Kyle', 'Helton', 3, getdate(), null, user_name()),
      (5, 'house', 'House', 'House', 3, getdate(), null, user_name()),
      (6, 'cmaxwell', 'Craig', 'Maxwell', 3, getdate(), null, user_name()),
      (7,'psykes', 'Peter', 'Sykes', 3, getdate(), null, user_name()),
      (8,'jhuennekens', 'Jim', 'Huennekens', 3, getdate(), null, user_name())
  ) as source
  (
  RegionalDirectorIID,
  RegionalDirectorKey,
  FirstName,
  LastName,
  SeasoningMonths,
  CreatedOn,
  ModifiedOn,
  ModifiedBy
  )
on (source.RegionalDirectorIID = target.RegionalDirectorIID)

when matched
  and source.RegionalDirectorKey <> target.RegionalDirectorKey
      or source.FirstName <> target.FirstName
      or source.LastName <> target.LastName
      or source.SeasoningMonths <> target.SeasoningMonths
then update set
  target.RegionalDirectorKey = source.RegionalDirectorKey,
  target.FirstName           = source.FirstName,
  target.LastName            = source.LastName,
  target.SeasoningMonths     = source.SeasoningMonths,
  target.ModifiedOn          = getdate(),
  target.ModifiedBy          = user_name()

when not matched then
  insert
  (
    RegionalDirectorIID,
    RegionalDirectorKey,
    FirstName,
    LastName,
    SeasoningMonths,
    CreatedOn,
    ModifiedOn,
    ModifiedBy
  )
  values
    (
      source.RegionalDirectorIID,
      source.RegionalDirectorKey,
      source.FirstName,
      source.LastName,
      source.SeasoningMonths,
      source.CreatedOn,
      source.ModifiedOn,
      source.ModifiedBy
    )
when not matched by source then
  delete;

set identity_insert dbo.tbl_lku_RegionalDirector off