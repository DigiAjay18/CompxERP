

alter proc [dbo].[sp_GetLedger] @CompList varchar(250),  @AcctList varchar(max), @BrncList varchar(250)=null, @BdDateFrom smalldatetime, @BdDateTo smalldatetime, @pBranchWs int = 0, @pItemDtls int = 0, @pAcctDtls int = 0          
as                          
begin                          
 declare @pMainSql varchar(max)='', @pWhereSql varchar(2000)=' where 1 = 1 '                
 declare @lCmpSngC int          
 DECLARE @temp_ledger table (rowidnum int identity(1, 1), groupnum int, ledger varchar(250), company varchar(150), [VcNo] varchar(50), [VcDt] smalldatetime, [Cr] money, [Dr] money, [OnAccountName] varchar(250), [Remark] nvarchar(max), [VcType] varchar(25)
  
, [Branch] varchar(150), trnbrnc int, trncode int, trntype int, [Bal] money, trnitem int)  
 if @BdDatefrom is null              
 set @BdDateFrom = '2016-04-01'              
 if @BdDateTo is null               
 set @BdDateTo = getdate()              
 if charindex(',', @CompList) > 0           
 set @lCmpSngC = convert(int, left(@CompList, charindex(',', @CompList)-1), 0)          
 else          
 set @lCmpSngC = convert(int, @CompList, 0)          
          
 insert into @temp_ledger (groupnum, ledger, company, [VcNo], [VcDt], [Cr], [Dr], [OnAccountName], [Remark], [VcType], [Branch], trnbrnc, trncode, trntype, [bal], trnitem) select dense_rank() over(order by acctdesc, compname, CASE when @pBranchWs = 0 then 
 '0' else gododesc end) as groupnum, * from (select acctdesc, compname, '' as mstchno, trndate + 1 as trndate, case when sumcram - sumdram > 0 then sumcram - sumdram else 0 end sumcram, case when sumdram - sumcram > 0 then sumdram - sumcram else 0 end sumdram, 'Op. Bal.' as particular, '' as remark, '' as typehdr, gododesc, isnull(mstbrnc, 0) mstbrnc, 0 as trncode, 0 as trntype, sumbala, trnitem from udf_GetBalance(null, @BdDateFrom-1, @pBranchWs, null, null) tp inner join udf_AccountMst(@lCmpSngC) at on tp.trnitem = at.acctcode inner join company cp on cp.compcode = tp.compcode left join udf_BranchList(@lCmpSngC) bs on bs.godocode = tp.mstbrnc inner join  (select * from dbo.SplitStringtoIntTable(@compList)) tComp on tComp.Value = tp.compcode inner join (select * from dbo.
SplitStringtoIntTable(@AcctList)) tAcct on tAcct.Value = tp.trnitem where isnull(tp.mstbrnc, 0) in (isnull(@BrncList, isnull(tp.mstbrnc, 0)))          
 union              
 select tp.acctdesc, tp.Compname, tp.mstchno, tp.trndate, tp.trncram, tp.trndram, tp.particular, tp.mstrema, dbo.udf_getVchTypeHdr(trntype) as typehdr, gododesc, isnull(mstbrnc, 0) mstbrnc, trncode, trntype, 0 sumbala, tp.trnitem from udf_GetLedgerDet(null, @lCmpSngC, @BdDateFrom, @BdDateTo) tp inner join  (select * from dbo.SplitStringtoIntTable(@compList)) tComp on tComp.Value = tp.compcode inner join (select * from dbo.SplitStringtoIntTable(@AcctList)) tAcct on tAcct.Value = tp.trnitem where isnull(tp.mstbrnc, 0) in (isnull(@BrncList, isnull(tp.mstbrnc, 0
  
)))          
 union              
 select acctdesc, compname, '' as mstchno, tp.trndate as trndate, tp.sumcram, tp.sumdram, 'Total / Cl. Bal.' as particular, '' as remark, '' as typehdr, gododesc, isnull(tp.mstbrnc, 0) mstbrnc, 0 as trncode, 9999 as trntype, tp.sumbala, tp.trnitem from udf_GetBalance(null, @BdDateTo, @pBranchWs, null, null) tp /*left join udf_GetBalance(null, @lCmpSngC, @BdDateFrom-1, @pBranchWs, null, null) od on od.trnitem = tp.trnitem*/ inner join udf_AccountMst(@lCmpSngC) at on tp.trnitem = at.acctcode inner join company cp on cp.compcode = tp.compcode left join udf_BranchList(@lCmpSngC) bs on bs.godocode = tp.mstbrnc inner join  (select * from dbo.SplitStringtoIntTable(@compList)) tComp on tComp.Value = tp.compcode inner join (select * from dbo.SplitStringtoIntTable(@AcctList)) tAcct on tAcct.Value = tp.trnitem where  isnull(tp.mstbrnc, 0) in (isnull(@BrncList, isnull(tp.mstbrnc, 0)))) tp order by acctdesc, compname, CASE when @pBranchWs = 0 then  '0' else gododesc end, trndate, trntype, trncode              
     
 /*09-08-2018 */      
 if @pItemDtls = 1    
 begin    
 update a set a.remark = a.remark + b.particular from @temp_ledger a inner join (select STUFF((select N' <br/> ' + descalia + ' = ' +  convert(varchar, abs(itdquan), 0) + ' ' + iInf.unitname + ' = ' + convert(varchar, abs(itdactprc), 0) from itpursal iTde inner join (select * from iteminfo where compcode = 66) iInf on iInf.itemcode = iTde.itditem where iTde.compcode = x.compcode and iTde.itdcode = x.itdcode and iTde.itdtype = x.itdtype and iTde.itddate = x.itddate order by itdsrno for xml path('')),1,1,'') as particular, compcode, itdcode, itdtype, itddate from itpursal x where x.compcode = @lCmpSngC group by compcode, itdcode, itdtype, itddate      
) b on a.trncode = b.itdcode and a.trntype = b.itdtype and a.vcdt = b.itddate     
    
 end    
 
 /*24-10-2018 */      
 if @pAcctDtls = 1    
 begin    
 update a set a.remark = a.remark + b.particular from @temp_ledger a inner join (select STUFF((select N' <br/> ' + acctdesc + ' = ' +  convert(varchar, abs(trndram-trncram), 0) + ' ' + case when trndram-trncram > 0 then 'Dr.' else 'Cr.' end from jourtrn trnD inner join vewLedgerMst aInf on aInf.acctcode = trnD.trnitem left join (select * from dbo.SplitStringtoIntTable(@AcctList)) tList on tList.Value = trnD.trnitem where trnD.compcode = x.compcode and trnD.trncode = x.trncode and trnD.trntype = x.trntype and trnD.trndate = x.trndate and tList.Value is null order by trnsrno for xml path('')),1,1,'') as particular, compcode, trncode, trntype, trndate from jourtrn x where x.compcode = @lCmpSngC group by compcode, trncode, trntype, trndate
) b on a.trncode = b.trncode and a.trntype = b.trntype and a.vcdt = b.trndate     
    
 end    
    
 /*if @Partyid is not null    */          
 update a set a.bal = (SELECT SUM(dr-cr) FROM @temp_ledger b WHERE b.rowidnum <= a.rowidnum and trntype <> 9999 and b.groupnum = a.groupnum) from @temp_ledger a               
          
 update a set a.dr= b.sumdr, a.cr = b.sumcr from @temp_ledger a inner join (SELECT SUM(dr) sumdr, sum(cr) sumcr, groupnum FROM @temp_ledger b WHERE trntype <> 9999 group by groupnum) b on b.groupnum = a.groupnum where trntype = 9999              
                 
 select groupnum, ledger, company, [Branch], [VcDt], [VcType], trncode, [VcNo], [OnAccountName], [Remark], [Dr], [Cr], abs([bal]) [Bal], case when bal < 0 then 'Cr' else 'Dr' end as [DrCr], trntype, trnitem from @temp_ledger a order by rowidnum           
  
   
end
GO
