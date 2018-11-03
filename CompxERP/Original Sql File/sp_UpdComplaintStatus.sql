create proc dbo.sp_UpdComplaintStatus(@CompID int,@StatusID int,@cmStatusRemark varchar(1000)=null)
as
begin
	update tblComplaint
	set StatusID=@StatusID,cmStatusRemark=@cmStatusRemark,cmStatusDt=GETDATE()
	where CompID=@CompID
end