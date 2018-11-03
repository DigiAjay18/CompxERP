
Select * from loginusers where usename like 'himansh%' usetype = 8
 
Select * from employee where useCode = 25
717
tbl _emp
sp_help tblcomplaint

 
sp_help tblPayFolloupPlanning

Alter table tblPayFolloupPlanning add ExecID int

Select * from tblcomplaint where compDt between '2017/11/01' and '2019/11/01'    and isnull(empID,0) > 0 =1

13	Himanshu Mukhekar 34 
17
 var Complaint = from a in dba.tblComplaints
                            join b in dba.employees on a.EmpID equals b.UseCode into c
                            from d in c.DefaultIfEmpty()
                            where (UserType == 4 ? a.DealerID == UserID : UserType == 7 ? a.EmpID == UserID : a.CustID == UserID)
                            

 Select a.CustID ,a.CompNo,a.CompDt, a.EmpID,  empname, a.StatusID, a.CompID, a.CustNM, a.ModelNo, a.SrvType,a.ItemID,a.InvNo,
 a.TentetiveTm,a.Charge, a.Remark_Cust,a.Remark_Eng, a.cmCategory,a.cmIsPaid,a.Remark_Mgr, a.cmSrvcMode from tblcomplaint a
left join   employee b on a.EmpID = b.UseCode

where a.CustID =0

17 /34 