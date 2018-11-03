alter proc dbo.sp_GetDailyVisitPartyList(@CompCode int,@EmpID int,@UserType int=0)
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
				where dv.dvCompCode is null /*and vsEmpID=@EmpID and vtCompCode=@CompCode*/
			)a order by aName
	else
		select * from (
			select '--Select Party Name--' as aName,'-1' aCode
			union all
			select distinct ltrim(rtrim(vtPartyName+'-'+vtMobile)) as aName,ltrim(rtrim(vtPartyName+'-'+vtMobile)) as aCode
			from tblVisitSchTrn t
			inner join tblVisitSchMst m on t.vtcompcode=m.vscompcode and t.vsid=m.vscode
			left join tblDailyVisitEntry dv on dv.dvCompCode=m.vscompcode and ltrim(rtrim(dv.dvPartyName))=ltrim(rtrim(t.vtPartyName)) and ltrim(rtrim(dv.dvMobile))=ltrim(rtrim(t.vtMobile))
			inner join employee e on e.empcode=m.vsEmpID
			where dv.dvCompCode is null and e.UseCode=@EmpID /*and vtCompCode=@CompCode*/
		)a order by aName
end
go
sp_GetDailyVisitPartyList @CompCode=66,@EmpID=19,@UserType=8