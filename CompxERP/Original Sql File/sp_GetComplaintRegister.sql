alter proc dbo.sp_GetComplaintRegister(@pHeadID int=0,@pFromDt varchar(50)='',@pToDt varchar(50)='',@pEmpID int=0,@pStatusID int=0)
as
begin
	select 
	a.EmpID,lu.empname as aEmpName,a.StatusID as aStatusID,CompNo as aCompName,Remark_Cust as aDesc,case statusid when 1 then 'Pending' when 2 then 'Resolve' when 3 then 'Reject' when 3 then 'Halt' end as aStatus
	from tblComplaint a
	inner join employee lu on lu.usecode=a.EmpID
	inner join loginusers hn on hn.usecode=lu.usecode
	where 1=1 
	and 1= case when @pHeadID= '' then 1 else case when hn.UseHeadID=@pHeadID then 1 else 0 end end
	and 1= case when @pEmpID= 0 then 1 else case when a.EmpID=@pEmpID then 1 else 0 end end
	and 1= case when @pStatusID= 0 then 1 else case when a.StatusID=@pStatusID then 1 else 0 end end
	order by lu.empname
end
go
sp_GetComplaintRegister 0