use ACM_Studio_V2
go

set identity_insert dbo.tbl_lku_CommissionType on

merge into dbo.tbl_lku_CommissionType target
using
  (
    values
      (1, 'SMA'),
      (2, 'Mutual Funds'),
      (3, 'UMA')
  ) as source
  (CommissionTypeIid, CommissionType)
on (target.CommissionTypeIid = source.CommissionTypeIid)
when matched and target.CommissionType <> source.CommissionType
then update set
  target.CommissionType = source.CommissionType
  when not matched then
insert
  (CommissionTypeIid, CommissionType)
values
  (source.CommissionTypeIid, source.CommissionType)

when not matched by source then
delete;

set identity_insert dbo.tbl_lku_CommissionType off