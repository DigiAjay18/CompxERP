alter proc dbo.sp_GetChildTypeUser(@CompCode int=0,@UseCode int=0,@TypeID int=0)
as
begin
	select usename,usecode,usernm,case isnull(useheadid,0) when 0 then 0 else 1 end isExistsUser
	from loginusers l 
	where l.compcode=@CompCode and isnull(useheadid,0) in (0,@UseCode) and l.usetype in (select hrcdnameid from tblhardcode h where h.hrcdtype='UserType' and h.hrcdgroupcode=@TypeID)
end
go
sp_GetChildTypeUser @CompCode=66,@TypeID=22,@UseCode=143

select * from loginusers where compcode=66 and usetype=23