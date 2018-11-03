alter proc dbo.sp_GetExeWiseOrder(@pFromDt varchar(50)='',@pToDt varchar(50)='',@pEmpCode int=0)
as
begin
declare @pMainSql nvarchar(max)='',@ColName nvarchar(max)='',@SumColName nvarchar(max)='',@TotSumQColName nvarchar(max)='',@TotSumCColName nvarchar(max)='',@pWhere nvarchar(max)=''
	set @ColName=STUFF((SELECT distinct ',[Q' + cast(compcode as varchar)+'],[C' + cast(compcode as varchar)+']' from company FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)') ,1,1,'')
	set @SumColName=STUFF((SELECT distinct ',isnull(sum([Q' + cast(compcode as varchar)+']),0) as [Q' + cast(compcode as varchar)+'],isnull(sum([C' + cast(compcode as varchar)+']),0) as [C' + cast(compcode as varchar)+']' from company FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)') ,1,1,'')
	set @TotSumQColName=STUFF((SELECT distinct '+isnull(sum([Q' + cast(compcode as varchar)+']),0)' from company FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)') ,1,1,'')+' as QTotal'
	set @TotSumCColName=STUFF((SELECT distinct '+isnull(sum([C' + cast(compcode as varchar)+']),0)' from company FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)') ,1,1,'')+' as CTotal'

	if(ltrim(rtrim(@pEmpCode))<>0)set @pWhere+='and m.mstexec='+cast(@pEmpCode as varchar)
	if(ltrim(rtrim(@pFromDt))<>'' and ltrim(rtrim(@pToDt))<>'')set @pWhere+='and m.mstDate between '''+@pFromDt+''' and '''+@pToDt+''''
	else if(ltrim(rtrim(@pFromDt))<>'' and ltrim(rtrim(@pToDt))='')set @pWhere+='and m.mstDate >= '''+@pFromDt+''''
	else if(ltrim(rtrim(@pFromDt))='' and ltrim(rtrim(@pToDt))<>'')set @pWhere+='and m.mstDate <='''+@pToDt+''''

set @pMainSql='select substring(s.studname,0,4) as aMonthNm,aMonth,aYear,case when isnull(a.empname,'''')<>'''' then aEmpCode else 0 end as aEmpCode,case when isnull(a.empname,'''')<>'''' then a.empname else '' NA'' end as aEmpName,'+@SumColName+','+@TotSumQColName+','+@TotSumCColName+' FROM (
	select aMonth, aYear, aEmpCode,col + convert(varchar, compcode, 0)new_col,value
	from (
		select isnull(count(m.compcode),0) OrderQty,isnull(sum(m.msttota),0) OrderCharge,m.compcode,c.compname,month(m.mstDate) as aMonth,Year(m.mstDate) as aYear,m.mstexec as aEmpCode 
		from ordemst m 
		left join company c on c.compcode=m.compcode
		where m.mstType = 174 '+@pWhere+'
		group by m.compcode,c.compname,month(m.mstDate),year(m.mstDate),m.mstexec
	) t
	cross apply
	(
		VALUES
			(OrderQty, ''Q''),
			(OrderCharge, ''C'')
	) x (value, col)
) as s
PIVOT
(
	max(value)
	FOR new_col IN ('+@ColName+')
)AS pvt
left join employee a on a.usecode=pvt.aEmpCode
inner join studdet s on s.studType=237 and s.studstat=pvt.aMonth
group by s.studname,aMonth,aYear,case when isnull(a.empname,'''')<>'''' then aEmpCode else 0 end,case when isnull(a.empname,'''')<>'''' then a.empname else '' NA'' end,s.studcode
order by aEmpName,s.studcode,aYear'

print @pMainSql
execute(@pMainSql)
end
go
--sp_GetExeWiseOrder @pFromDt='2018-04-01',@pToDt='2019-03-31',@pEmpCode=2
--sp_GetExeWiseOrder @pFromDt='04/01/2018 12:00:00 AM'--,@pToDt='10/20/2019 12:00:00 AM'--,@pEmpCode='19'
go
--select* from ordemst where compcode=0
--select mstexec,* from ordemst
--select * from menuoption where menopti like '%order%'
--select* from studdet where studName like 'apr%'
