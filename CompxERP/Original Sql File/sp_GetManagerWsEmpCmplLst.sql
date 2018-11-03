alter proc dbo.sp_GetManagerWsEmpCmplLst(@pStatus int=0,@pEmpID int=0)
as
begin
	select hn.UseHeadID as aHeadID,isnull(eh.empname,'') as aHeadNm,a.EmpID,lu.empname as aEmpName,count(a.statusid) as aCompCount,case statusid when 1 then 'Pending' when 2 then 'Resolve' when 3 then 'Reject' when 3 then 'Halt' end as aStatus
	from tblComplaint a
	inner join employee lu on lu.usecode=a.EmpID
	inner join loginusers hn on hn.usecode=lu.usecode
	left join employee eh on hn.UseHeadID=eh.UseCode
	where 1=1 and 1= case when @pStatus= '' then 1 else case when a.StatusID=@pStatus then 1 else 0 end end
	 and 1= case when @pEmpID= '' then 1 else case when hn.UseHeadID=@pEmpID then 1 else 0 end end
	group by hn.UseHeadID,a.EmpID,lu.empname,a.statusid,eh.empname
	order by eh.empname,lu.empname
end
go
sp_GetManagerWsEmpCmplLst 0,33