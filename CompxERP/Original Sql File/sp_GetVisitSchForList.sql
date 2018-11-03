alter proc dbo.sp_GetVisitSchForList(@CompCode int)
as
begin
	select empname as aName,empcode as aCode
	from employee
	/*where compcode=@CompCode*/
	order by empname
end