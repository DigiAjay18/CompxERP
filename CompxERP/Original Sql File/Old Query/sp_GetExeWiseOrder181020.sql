alter proc dbo.sp_GetExeWiseOrder(@pFromDt varchar(50)='',@pToDt varchar(50)='',@pEmpCode int=0)
as
begin
declare @pMainSql nvarchar(max)='',@ColName nvarchar(max)='',@SumColName nvarchar(max)='',@TotSumColName nvarchar(max)='',@pWhere nvarchar(max)=''
	set @ColName=STUFF((SELECT distinct ',[' + cast(compcode as varchar)+']' from company FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)') ,1,1,'')   
	set @SumColName=STUFF((SELECT distinct ',isnull(sum([' + cast(compcode as varchar)+']),0) [a' + cast(compcode as varchar)+']' from company FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)') ,1,1,'')      
	set @TotSumColName=STUFF((SELECT distinct '+isnull(sum([' + cast(compcode as varchar)+']),0)' from company FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)') ,1,1,'')+' as Total'

	if(ltrim(rtrim(@pEmpCode))<>0)set @pWhere+='and m.mstexec='+cast(@pEmpCode as varchar)
	if(ltrim(rtrim(@pFromDt))<>'' and ltrim(rtrim(@pToDt))<>'')set @pWhere+='and o.itdDate between '''+@pFromDt+''' and '''+@pToDt+''''
	else if(ltrim(rtrim(@pFromDt))<>'' and ltrim(rtrim(@pToDt))='')set @pWhere+='and o.itdDate >= '''+@pFromDt+''''
	else if(ltrim(rtrim(@pFromDt))='' and ltrim(rtrim(@pToDt))<>'')set @pWhere+='and o.itdDate <='''+@pToDt+''''

set @pMainSql='select substring(s.studname,0,4) as aMonthNm,aMonth,aYear,aEmpCode,a.empname as aEmpName,'+@SumColName+','+@TotSumColName+' from ( 
	select aMonth,aYear,isnull(aEmpCode,0) as aEmpCode,'+@ColName+'
	FROM (
		select isnull(Sum(itdQuan),0) OrderQty,o.compcode,c.compname,month(o.itdDate) as aMonth,Year(o.itdDate) as aYear,m.mstexec as aEmpCode
		from ordeitd o
		inner join ordemst m on m.compcode=o.compcode and o.itdtype=m.msttype and o.itdcode=m.mstcode
		inner join company c on c.compcode=o.compcode
		where itdType = 174 '+@pWhere+'
		group by o.compcode,c.compname,month(o.itdDate),year(o.itdDate),m.mstexec
	) as s
	PIVOT
	(
		SUM(OrderQty)
		FOR [compcode] IN ('+@ColName+')
	)AS pvt
) as pvt
inner join employee a on a.empcode=pvt.aEmpCode
inner join studdet s on s.studType=237 and s.studstat=pvt.aMonth
group by s.studname,aMonth,aYear,aEmpCode,a.empname,s.studcode
order by aEmpName,s.studcode,aYear' 
print @pMainSql
execute(@pMainSql)
end
go
--sp_GetExeWiseOrder @pFromDt='2018-04-01',@pToDt='2019-03-31',@pEmpCode=2
sp_GetExeWiseOrder @pFromDt='9/10/2018 12:00:00 AM',@pToDt='10/20/2019 12:00:00 AM',@pEmpCode='19'
go
--select* from ordeitd
--select mstexec,* from ordemst
--select * from menuoption where menopti like '%order%'
--select* from studdet where studName like 'apr%'
