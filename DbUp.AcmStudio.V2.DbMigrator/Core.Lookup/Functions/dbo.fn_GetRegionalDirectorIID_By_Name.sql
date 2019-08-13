use ACM_Studio_V2
go

if (object_id('dbo.fn_GetRegionalDirectorIID_By_Name') is not null)
    drop function dbo.fn_GetRegionalDirectorIID_By_Name
 go
 
 create function dbo.fn_GetRegionalDirectorIID_By_Name(@RegionalDirector varchar(100))
    returns  int
    
    as
        begin
            declare @retval int 
            set @retval = (select RegionalDirectorIID from tbl_lku_RegionalDirector where RegionalDirectorKey = @RegionalDirector)
            
            if @retval = null 
              set @retval = 3
              
            return @retval
        end
        