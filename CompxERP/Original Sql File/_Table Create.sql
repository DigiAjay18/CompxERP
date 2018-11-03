--insert into tblVisitSchTrn(vtTypeName,vtPartyName,vtMobile,vtAddress,vtModelNo,vtMachineNo,vtWeightName,vtValidFor,vtMachineType,vtDueYear,vtDueMonths,vtValidUpToDate,vtVcType)select TypeName,PartyName,Mobile+'',Address,ModelNo+'',MachineNo+'',WeightName,ValidFor,MachineType,aDueYear,DueMonths,ValidUpToDate,crtmVcType from neverstampyet$ order by ValidUpToDate
/*Start 181012 %temp% */

create table dbo.tblVisitSchMst(vsCompCode int,vsDate datetime,vsCode int,vsEmpID int,vsSchDate datetime)
go
create table dbo.tblDailyVisitEntry(dvCompCode int,dvDate datetime,dvCode int,dvPartyName varchar(500),dvMobile varchar(15),dvVisitDetail nvarchar(max),dvEstCost money,dvNextFollowUp datetime,dvRemark nvarchar(max))
go
create table dbo.tblDailyVisitReport(dvCompCode int,dvDate datetime,dvCode int,dvPartyName varchar(500),dvMobile varchar(15),dvVisitDetail nvarchar(max),dvQty int,dvEstCost money,dvNextFollowUp datetime,dvRemark nvarchar(max))
go
create table dbo.tblVisitSchTrn(vtCompCode int,vtCode int,vsID int,vtTypeName varchar(20),vtPartyName varchar(500),vtArea varchar(100),vtMobile varchar(15),vtAddress varchar(2000),vtModelNo varchar(100),vtMachineNo nvarchar(200),vtWeightName varchar(100),vtValidFor int,vtMachineType varchar(200),vtDueYear int,vtDueMonths int,vtValidUpToDate datetime,vtVcType varchar(50))
go
create proc dbo.sp_InsVisitSchMst(@vsCompCode int,@vsDate datetime,@vsEmpID int,@vsSchDate datetime)
as
begin
	declare @vsCode int=(select isnull(max(vsCode),0)+1 from tblVisitSchMst)
	insert into tblVisitSchMst(vsCompCode,vsDate ,vsCode ,vsEmpID ,vsSchDate)
	values(@vsCompCode,@vsDate ,@vsCode ,@vsEmpID ,@vsSchDate )
	select @vsCode as aCode,0 as aType ,'Record Insert Sucessfully...' as aMsg
end
go
create proc dbo.sp_InsVisitSchTrn(@vtCompCode int=null,@vsID int=null,@vtTypeName varchar(20)=null,@vtPartyName varchar(500)=null,@vtArea varchar(100)=null,@vtMobile varchar(15)=null,@vtAddress varchar(2000)=null,@vtModelNo varchar(100)=null,@vtMachineNo nvarchar(200)=null,@vtWeightName varchar(100)=null,@vtValidFor int=null,@vtMachineType varchar(200)=null,@vtDueYear int=null,@vtDueMonths int=null,@vtValidUpToDate datetime=null,@vtVcType varchar(50)=null)
as
begin
	declare @vtCode int=(select isnull(max(vtCode),0)+1 from tblVisitSchTrn)
	insert into tblVisitSchTrn(vtCompCode,vtCode,vsID,vtTypeName,vtPartyName,vtArea,vtMobile,vtAddress,vtModelNo,vtMachineNo,vtWeightName,vtValidFor,vtMachineType,vtDueYear,vtDueMonths,vtValidUpToDate,vtVcType)
	values(@vtCompCode,@vtCode,@vsID,@vtTypeName,@vtPartyName,@vtArea,@vtMobile,@vtAddress,@vtModelNo,@vtMachineNo,@vtWeightName,@vtValidFor,@vtMachineType,@vtDueYear,@vtDueMonths,@vtValidUpToDate,@vtVcType)
	select @vtCode as aCode,0 as aType ,'Record Insert Sucessfully...' as aMsg
end
go
create proc dbo.sp_UpdVisitSchTrn(@vtCompCode int=null,@vsID int=null,@vtPartyName varchar(500)=null,@vtArea varchar(100)=null,@vtMobile varchar(15)=null)
as
begin
	declare @vtCode int=(select isnull(max(vtCode),0)+1 from tblVisitSchTrn)
	update tblVisitSchTrn
	set vsID=@vsID,vtCode=@vtCode,vtCompCode=@vtCompCode,vtArea=@vtArea
	where ltrim(rtrim(vtPartyName))=@vtPartyName and ltrim(rtrim(vtMobile))=@vtMobile
	select @vtCode as aCode,0 as aType ,'Record Insert Sucessfully...' as aMsg
end
go
create proc dbo.sp_GetVisitPartyData(@vtCompCode int,@vtPartyName varchar(200))
as
begin
	select convert(varchar,t.vtValidUpToDate,106) as ValidUpToDate,t.*
	from tblVisitSchTrn t
	where ltrim(rtrim(vtPartyName))=@vtPartyName and vsID is null and vtTypeName='Self'--and vtCompCode=@vtCompCode
end
go
create proc dbo.sp_GetVisitPartyMobData(@vtCompCode int,@vtPartyName varchar(200),@vtMobile varchar(50))
as
begin
	select convert(varchar,t.vtValidUpToDate,106) as ValidUpToDate,t.*
	from tblVisitSchTrn t
	where ltrim(rtrim(vtPartyName))=@vtPartyName and ltrim(rtrim(vtMobile))=@vtMobile and vtTypeName='Self'--and vtCompCode=@vtCompCode
end
go
create proc dbo.sp_GetVisitScheduleList(@CompCode int,@EmpID int,@UserType int=0)
as
begin
	if (@UserType=0 or @UserType=1)
		select * from(select 
		 distinct vsSchDate,convert(varchar,vsSchDate,106) as ScheduleDate,vsCompCode as aCompCode,vtCode as aCode,vsID as aID,vtTypeName as aTypeName,ltrim(rtrim(vtPartyName)) as aPartyName,vtArea as aArea,vtMobile as aMobile,a.empname as aEmpName/*,vtAddress as aAddress,vtModelNo as aModelNo,vtMachineNo as aMachineNo,vtWeightName as aWeightName,vtValidFor as aValidFor,vtMachineType as aMachineType,vtDueYear as aDueYear,vtDueMonths as aDueMonths,vtValidUpToDate as aValidUpToDate,vtVcType as aVcType*/
		from tblVisitSchTrn t
		inner join tblVisitSchMst m on t.vtcompcode=m.vscompcode and t.vsid=m.vscode
		inner join employee a on a.empcode=m.vsEmpID
		/*where m.vsCompCode=@CompCode and a.UseCode=@EmpID*/
		)x
		order by x.vsSchDate
	else
		select * from(select 
		 distinct vsSchDate,convert(varchar,vsSchDate,106) as ScheduleDate,vsCompCode as aCompCode,vtCode as aCode,vsID as aID,vtTypeName as aTypeName,ltrim(rtrim(vtPartyName)) as aPartyName,vtArea as aArea,vtMobile as aMobile,a.empname as aEmpName/*,vtAddress as aAddress,vtModelNo as aModelNo,vtMachineNo as aMachineNo,vtWeightName as aWeightName,vtValidFor as aValidFor,vtMachineType as aMachineType,vtDueYear as aDueYear,vtDueMonths as aDueMonths,vtValidUpToDate as aValidUpToDate,vtVcType as aVcType*/
		from tblVisitSchTrn t
		inner join tblVisitSchMst m on t.vtcompcode=m.vscompcode and t.vsid=m.vscode
		inner join employee a on a.empcode=m.vsEmpID
		where /*m.vsCompCode=@CompCode and */a.UseCode=@EmpID
		)x
		order by x.vsSchDate
end
go

/*End 181012 %temp% */
go
create proc dbo.sp_GetVisitPartyList(@CompCode int)
as
begin
	select * from (
		select '--Select Party Name--' as aName,'-1' aCode
		union all
		select distinct ltrim(rtrim(vtPartyName)) as aName,ltrim(rtrim(vtPartyName)) as aCode
		from tblVisitSchTrn
		where vsId is null and vtTypeName='Self'/*vtCompCode=@CompCode*/
	)a order by aName
end
go
create proc dbo.sp_GetDailyVisitPartyList(@CompCode int,@EmpID int,@UserType int=0)
as
begin
	if (@UserType=0 or @UserType=1)
		select * from (
				select '--Select Party Name--' as aName,'-1' aCode
				union all
				select distinct ltrim(rtrim(vtPartyName+'-'+vtMobile)) as aName,ltrim(rtrim(vtPartyName+'-'+vtMobile)) as aCode
				from tblVisitSchTrn t
				inner join tblVisitSchMst m on t.vtcompcode=m.vscompcode and t.vsid=m.vscode
				left join tblDailyVisitEntry dv on dv.dvCompCode=m.vscompcode and ltrim(rtrim(dv.dvPartyName))=ltrim(rtrim(t.vtPartyName)) and ltrim(rtrim(dv.dvMobile))=ltrim(rtrim(t.vtMobile))
				where dv.dvCompCode is null/*vsEmpID=@EmpID and vtCompCode=@CompCode*/
			)a order by aName
	else
		select * from (
			select '--Select Party Name--' as aName,'-1' aCode
			union all
			select distinct ltrim(rtrim(vtPartyName+'-'+vtMobile)) as aName,ltrim(rtrim(vtPartyName+'-'+vtMobile)) as aCode
			from tblVisitSchTrn t
			inner join tblVisitSchMst m on t.vtcompcode=m.vscompcode and t.vsid=m.vscode
			left join tblDailyVisitEntry dv on dv.dvCompCode=m.vscompcode and ltrim(rtrim(dv.dvPartyName))=ltrim(rtrim(t.vtPartyName)) and ltrim(rtrim(dv.dvMobile))=ltrim(rtrim(t.vtMobile))
			where dv.dvCompCode is null and vsEmpID=@EmpID /*and vtCompCode=@CompCode*/
		)a order by aName
end
go
create proc dbo.sp_GetVisitPartyAreaList(@CompCode int)
as
begin
	select distinct vtArea as aName,vtArea as aCode
	from tblVisitSchTrn
	--where vtCompCode=@CompCode
end
go
create proc dbo.sp_GetVisitSchForList(@CompCode int)
as
begin
	select empname as aName,empcode as aCode
	from employee
	/*where compcode=@CompCode*/
end
go
create proc dbo.sp_InsDailyVisitEntry(@dvCompCode int=null,@dvPartyName varchar(500)=null,@dvMobile varchar(15)=null,@dvVisitDetail nvarchar(max)=null,@dvEstCost money=null,@dvNextFollowUp datetime=null,@dvRemark nvarchar(max)=null)
as
begin
	declare @dvCode int=(select isnull(max(dvCode),0)+1 from tblDailyVisitEntry)
	insert into tblDailyVisitEntry(dvCompCode,dvDate,dvCode,dvPartyName,dvMobile,dvVisitDetail,dvEstCost,dvNextFollowUp,dvRemark)
	values(@dvCompCode,getdate(),@dvCode,@dvPartyName,@dvMobile,@dvVisitDetail,@dvEstCost,@dvNextFollowUp,@dvRemark)
	select @dvCode as aCode,0 as aType ,'Record Insert Sucessfully...' as aMsg
end
go
create proc dbo.sp_InsDailyVisitReport(@dvCompCode int=null,@dvPartyName varchar(500)=null,@dvMobile varchar(15)=null,@dvVisitDetail nvarchar(max)=null,@dvQty money=null,@dvEstCost money=null,@dvNextFollowUp datetime=null,@dvRemark nvarchar(max)=null)
as
begin
	declare @dvCode int=(select isnull(max(dvCode),0)+1 from tblDailyVisitReport)
	insert into tblDailyVisitReport(dvCompCode,dvDate,dvCode,dvPartyName,dvMobile,dvVisitDetail,dvQty,dvEstCost,dvNextFollowUp,dvRemark)
	values(@dvCompCode,getdate(),@dvCode,@dvPartyName,@dvMobile,@dvVisitDetail,@dvQty,@dvEstCost,@dvNextFollowUp,@dvRemark)
	select @dvCode as aCode,0 as aType ,'Record Insert Sucessfully...' as aMsg
end