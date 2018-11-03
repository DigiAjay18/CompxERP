alter proc dbo.sp_GetExeWiseOrder(@pFromDt varchar(50)='',@pToDt varchar(50)='',@pEmpCode int=0)
as
begin
declare @pMainSql nvarchar(max)='',@ColName nvarchar(max)='',@SumColName nvarchar(max)='',@TotSumColName nvarchar(max)='',@pWhere nvarchar(max)=''
	set @ColName=STUFF((SELECT distinct ',[' + cast(compcode as varchar)+']' from company FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)') ,1,1,'')   
	set @SumColName=STUFF((SELECT distinct ',isnull(sum([' + cast(compcode as varchar)+']),0) [a' + cast(compcode as varchar)+']' from company FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)') ,1,1,'')      
	set @TotSumColName=STUFF((SELECT distinct '+isnull(sum([' + cast(compcode as varchar)+']),0)' from company FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)') ,1,1,'')+' as Total'

	if(ltrim(rtrim(@pEmpCode))<>0)set @pWhere+='and m.mstexec='+cast(@pEmpCode as varchar)
	if(ltrim(rtrim(@pFromDt))<>'' and ltrim(rtrim(@pToDt))<>'')set @pWhere+='and m.mstDate between '''+@pFromDt+''' and '''+@pToDt+''''
	else if(ltrim(rtrim(@pFromDt))<>'' and ltrim(rtrim(@pToDt))='')set @pWhere+='and m.mstDate >= '''+@pFromDt+''''
	else if(ltrim(rtrim(@pFromDt))='' and ltrim(rtrim(@pToDt))<>'')set @pWhere+='and m.mstDate <='''+@pToDt+''''

set @pMainSql='select substring(s.studname,0,4) as aMonthNm,aMonth,aYear,aEmpCode,a.empname as aEmpName,'+@SumColName+','+@TotSumColName+' from ( 
	select aMonth,aYear,isnull(aEmpCode,0) as aEmpCode,'+@ColName+'
	FROM (
		select isnull(count(m.compcode),0) OrderQty,m.compcode,c.compname,month(m.mstDate) as aMonth,Year(m.mstDate) as aYear,m.mstexec as aEmpCode
		from ordemst m 
		left join company c on c.compcode=m.compcode
		where m.mstType = 174 '+@pWhere+'
		group by m.compcode,c.compname,month(m.mstDate),year(m.mstDate),m.mstexec
	) as s
	PIVOT
	(
		SUM(OrderQty)
		FOR [compcode] IN ('+@ColName+')
	)AS pvt
) as pvt
left join employee a on a.usecode=pvt.aEmpCode
inner join studdet s on s.studType=237 and s.studstat=pvt.aMonth
group by s.studname,aMonth,aYear,aEmpCode,a.empname,s.studcode
order by aEmpName,s.studcode,aYear' 
print @pMainSql
execute(@pMainSql)
end
go
--sp_GetExeWiseOrder @pFromDt='2018-04-01',@pToDt='2019-03-31',@pEmpCode=2
sp_GetExeWiseOrder @pFromDt='04/01/2018 12:00:00 AM'--,@pToDt='10/20/2019 12:00:00 AM'--,@pEmpCode='19'
go
--select* from ordeitd
--select mstexec,* from ordemst
--select * from menuoption where menopti like '%order%'
--select* from studdet where studName like 'apr%'
