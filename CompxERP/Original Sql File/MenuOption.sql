alter table menuoption add meniscrm int

select * from menuoption where   meniscrm = 1 
select * from menuoption where   MenCode = 1322>1281 
Delete from menuoption where   MenCode >1281 
update menuoption set meniscrm = 1 where   MenCode >1281 
select max(MenCode)+1 from menuoption  


insert into menuoption (MenCode ,MenGrou ,MenName ,Mendesc,menopti ,meniscrm ,mensort) values (1283 ,731,'Dealer Master' ,'#MapDealer','MnDealer',1,1)  
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1284 ,731,'#SourceMaster' ,'btnBrand' ,'Brand Master',1,5) 
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1285 ,731,'#SourceMaster' ,'btnFinancialTerms' ,'Financial Terms',1,6)
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1286 ,731,'#SourceMaster' ,'btnOtherTerms' ,'Other Terms',1,7) 
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1287 ,731,'#SourceMaster' ,'btnRegion' ,'Bank Master',1,8)
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1288 ,731,'#SourceMaster' ,'btnDept' ,'Department Master',1,9)  
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1289 ,731,'#DealerApprove' ,'btnApproveDealer' ,'Dealer Approve',1,10)
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1290 ,731,'#EmployeeMaster' ,'btnEmployeeMaster' ,'Employee Setup',1,11)
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1291 ,731,'#departmentSetup' ,'btnDepartmentSetup' ,'Designation Setup',1,12) 
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1292 ,731,'#CityMaster' ,'btnCityMaster' ,'Route Map',1,13) 
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1293 ,731,'#DealerAllotment' ,'btnDealerAllotment' ,'Dealer Allocation',1,14) 
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1294 ,731,'#DealerApprove' ,'btnOrdApprove' ,'Pending Order',1,15) 
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1295 ,731,'' ,'/itemsetup/createtype?MenCode=1' ,'Category',1,2) 
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1296 ,731,'' ,'/itemsetup/createsubtype?MenCode=1' ,'Sub Category',1,3)  
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1297 ,731,'' ,'/itemsetup/itemain?MenCode=1' ,'Item Setup',1,4) 
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1298 ,731,'' ,'/User/Rights?MenCode=1' ,'User Setup',1,16) 
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1299 ,731,'' ,'/Master/City' ,'City Master',1,17)  
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1300 ,731,'#DealerAllotment' ,'btnDealerMapAcct' ,'Account Mapping',1,15) 

insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values ((select max(MenCode)+1 from menuoption) ,731,'#Calling' ,'btnCallingMst' ,'Calling',1,(select max(MenCode)+1 from menuoption))  

insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1301 ,732,'#Calling' ,'btnCalling' ,'Calling',1,1) 
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1302 ,732,'#ParkLead' ,'btnParkLead' ,'Park Lead',1,2) 
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1303 ,732,'#DealerApprove' ,'MnFollow' ,'Follow Up Entry',1,3) 
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1304 ,732,'#ParkLead' ,'btnQuotation' ,'Quotation Entry',1,4) 
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1305 ,732,'#ParkLead' ,'btnOrder' ,'Order Entry',1,5) 
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1306 ,732,'#ParkLead' ,'btnPlanDispatch' ,'Plan Dispatch',1,6) 
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1307 ,732,'#DealerApprove' ,'btnPayFollowUp' ,'Payment FollowUp',1,7) 
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1308 ,732,'#DealerApprove' ,'btnBankReceipt' ,'Bank Receipt',1,8) 
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1309 ,732,'#DealerApprove' ,'btnOrdFollowUp' ,'Order FollowUp',1,8)  
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1310 ,732,'#AllotTask' ,'btnAllotTask' ,'Allot Task',1,8) 
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1311 ,732,'#RaiseTicket' ,'btnRaiseTicket' ,'Raise Ticket',1,9) 
 
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1312 ,733,'#ParkLead' ,'btnDealerList' ,'Dealer List',1,1) 
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1313 ,733,'#ParkLead' ,'btnCallingSummary' ,'Calling Summary',1,2) 
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1314 ,733,'#LeadList' ,'MnLeadList' ,'Park Lead List',1,3) 
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1315 ,733,'#DealerApprove' ,'FollowUpList' ,'Follow Up List',1,4) 
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1316 ,733,'#QuotList' ,'bntQuotList' ,'Quotation List',1,5) 
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1317 ,733,'#DealerApprove' ,'bntOrderList' ,'Order List',1,6) 
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1318 ,733,'#DealerApprove' ,'btnOrdApprove_I' ,'Approved Order',1,7) 
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1319 ,733,'#DealerApprove' ,'btnOrdDispatch' ,'Dispatch List',1,8) 
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1320 ,733,'#ParkLead' ,'btnProductionList' ,'Production List',1,11) 
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1321 ,733,'#DealerApprove' ,'btnOrdApprove_II' ,'Pending Order',1,12) 
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1322 ,733,'#DealerApprove' ,'btnPayFollowUpList' ,'Payment FollowUp List',1,9) 
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1323 ,733,'#EmpList' ,'btnEmpList' ,'Employee List',1,10) 
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1324 ,733,'#EmpDealerList' ,'btnEmpDealerList' ,'Employee-Dealer List',1,11) 
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1325 ,733,'' ,'/Report/Filter' ,'Report Viewer',1,11) 


insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1326 ,733,'#DealerApprove' ,'btnTrialBal' ,'Trial Balance',1,10) 
 
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1328 ,733,'#DealerApprove' ,'btnMnUserWork' ,'User Work',1,10) 
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1329 ,733,'#DealerApprove' ,'btnOrdFollowUpList' ,'Order FollowUp List',1,10) 
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1330 ,733,'#ParkLead' ,'btnLedger' ,'Ledger',1,10)   
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort ) values (1331 ,733,'#DealerApprove' ,'btnCallingList' ,'Calling List',1,10)  


insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,meniscrm, mensort  ) values (1332 ,0,'Service' ,'Service' , 1 , 1332 ) 
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm,mensort  ) values (1333 ,1332,'#SourceMaster' ,'btnSrvMaster' ,'Service Master' , 1,1333) 
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm,mensort  ) values (1334 ,1332,'#DealerApprove' ,'btnSrvCycle' ,'Service Cycle' , 1,1334) 
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm,mensort  ) values (1335 ,1332,'#DealerApprove ' ,'btnSrvSchedule' ,'Service Schedule' ,1, 1335 ) 
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort  ) values (1336 ,1332,'#DealerApprove' ,'btnSrvComplement' ,'Complement Entry',1 ,1336 ) 

--insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort  ) values (1337 ,731,'#DealerApprove' ,'btnGatePass' ,'Gate Pass',1 ,1337 ) 

--insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort  ) values (1338 ,731,'#DealerApprove' ,'btnForecast' ,'Forecast',1 ,1338 )
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort  ) values (1340 ,1333,'#DealerApprove' ,'btnComplaintList' ,'Complaint List',1 ,1340) 
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort  ) values (1344 ,1333,'#DealerApprove' ,'btnSchemeMaster' ,'Scheme Manager',1 ,1344) 

insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort  ) values (1346 ,1333,'#DealerApprove' ,'btnFloatScheme' ,'Offer / Float Scheme',1 ,1346) 
 
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort  ) values ((select max(MenCode)+1 from menuoption) ,1333,'#DealerApprove' ,'btnPaymentFollowupPlanning' ,'Payment Followup Planning',1 ,(select max(MenCode)+1 from menuoption)) 
  
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort , IsCrm ) values ((select max(MenCode)+1 from menuoption) ,1333,'#DealerApprove' ,'btnPaymentFollowupPlanningSearch' ,'Payment Planning Search',1 ,(select max(MenCode)+1 from menuoption),1) 
 
update Menuoption set MenDesc = 'Masters'  where MenGrou = 0 and menCode = 731
update Menuoption set MenDesc = 'Transactions'  where MenGrou = 0 and menCode = 732 
update Menuoption set MenDesc = 'Reports'  where MenGrou = 0 and menCode = 733


select * from menuoption  where mengrou <> 0 and menCode = (select max(MenCode)  from menuoption)
Update  menuoption set IsCrm = 1 ,menGrou = 1333 where mengrou <> 0 and menCode = (select max(MenCode)  from menuoption)

select max(MenCode)+1 from menuoption  

insert into usermenust values (1282 ,1,1,1,1,1,1)  
select * from usermenust
select * from menuoption where   meniscrm = 1
Select * from loginUsers where useType = 4 8 

Select * from menuoption 
Insert into usermenust 
Select Mencode ,a.useCode ,1,1,1,1,1 from loginUsers a
join menuoption b on b.meniscrm = 1 --1243
where useType = 4 and useCode = 651 and mencode = 1322

select * from menuoption where   meniscrm = 1

SP_helptext GetUserRight GetUserRight

/* start 181012 %temp%*/
declare @mencode int=(select max(MenCode)+1 from menuoption  )
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort  ) values (@mencode ,1332,'' ,'/Service/VisitScheduleMst?MenCode=1' ,'Visit Schedule Entry',1 ,@mencode) 
go
declare @mencode int=(select max(MenCode)+1 from menuoption  )
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort  ) values (@mencode ,1332,'' ,'/Service/DailyVisitEntry?MenCode=1' ,'Daily Visit Entry',1 ,@mencode) 
go
declare @mencode int=(select max(MenCode)+1 from menuoption  )
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort  ) values (@mencode ,1332,'' ,'/Service/InwardEntry?MenCode=1' ,'Inward Entry',1 ,@mencode) 
go
declare @mencode int=(select max(MenCode)+1 from menuoption  )
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort  ) values (@mencode ,1332,'' ,'/Service/VisitScheduleList?MenCode=1' ,'Inward Entry',1 ,@mencode) 
go
declare @mencode int=(select max(MenCode)+1 from menuoption  )
insert into menuoption (MenCode ,MenGrou ,Mendesc,menopti  ,MenName,meniscrm ,mensort  ) values (@mencode ,1332,'' ,'/Service/DailyVisitReport?MenCode=1' ,'Daily Visit Report',1 ,@mencode)
go
update menuoption set menopti='btnVisitScheduleList' where menopti='/Service/VisitScheduleList?MenCode=1'
go
insert into usermenust(mencode,menuser,menview,menaddi,menedit,mendele,menacce)
select distinct m.mencode,1,1,1,1,1,1 from menuoption m
left join usermenust u on u.mencode=m.mencode
where u.mencode is null
/* end 181012 %temp%*/