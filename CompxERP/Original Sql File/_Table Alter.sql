--181031
--alter table tblComplaint add StatusRemark varchar(1000)
--alter table tblComplaint add cmStatusDt datetime
--ALTER TABLE tblComplaint add cmIsRead int
update tblComplaint set cmIsRead=1 where isnull(EmpID,0)=0
--181027
--alter table tblComplaint add cmCategory int
--alter table tblComplaint add cmIsPaid int
--alter table tblComplaint add cmSrvcMode int

--181011
--alter table loginusers add UseHeadID int