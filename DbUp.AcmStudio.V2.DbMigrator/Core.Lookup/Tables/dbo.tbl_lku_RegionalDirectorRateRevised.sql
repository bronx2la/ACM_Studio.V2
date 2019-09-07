use ACM_Studio_V2
go

if (object_id('dbo.tbl_lku_RegionalDirectorRateRevised') is null)
    begin
        create table dbo.tbl_lku_RegionalDirectorRateRevised
        (
            RegionalDirectorRateRevisedIid int primary key not null identity (1,1),
            IsGeneva bit,
            RegionalDirectorIid int,
            Strategy varchar(200),
            NewAssetRate decimal(9,6),
            OngoingRate decimal(9,6)
        );
    end
go

