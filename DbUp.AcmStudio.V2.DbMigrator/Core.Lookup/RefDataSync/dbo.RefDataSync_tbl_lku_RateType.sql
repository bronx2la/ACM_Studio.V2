use ACM_Studio_V2
go

set identity_insert dbo.tbl_lku_RateType on

merge into dbo.tbl_lku_RateType target
using
  (
    values
      (1, 'New Assets', getdate(), null, user_name()),
      (2, 'Ongoing', getdate(), null, user_name()),
      (3, 'Legacy', getdate(), null, user_name()),
      (4, 'Umbrella New Assets', getdate(), null, user_name()),
      (5, 'Umbrella Ongoing', getdate(), null, user_name())
  ) as source
  (RateTypeIid, RateType, CreatedOn, ModifiedOn, ModifiedBy)
on (target.RateTypeIid = source.RateTypeIid)

when matched and target.RateType <> source.RateType then
  update 
    set 
      target.RateType = source.RateType,
      target.ModifiedOn = getdate(),
      target.ModifiedBy = user_name()
      
when not matched then
insert
  (RateTypeIid, RateType, CreatedOn, ModifiedOn, ModifiedBy)
values
  (source.RateTypeIid, source.RateType, source.CreatedOn, source.ModifiedOn, source.ModifiedBy)

when not matched by source then
delete;

set identity_insert dbo.tbl_lku_RateType  off