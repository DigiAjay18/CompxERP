/****** Object:  StoredProcedure [dbo].[sp_GetLedger_PDF]    Script Date: 09/22/2018 12:47:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--sp_GetLedger @CompList = '60', @AcctList = '1707, 1236, 5093', @BdDateFrom = '2016-04-01', @BdDateTo = '2017-09-30'              
--select * from account where acctname like '%gst%'               
--Exec sp_GetLedger @CompList = '60', @AcctList = '1707', @BdDateFrom = '04/01/2017 12:00:00 AM', @BdDateTo = '11/10/2017 12:00:00 AM'          
--select mstrefc, mstprtc, mstsale,* from Jourmst Where msttype =3 and mstcode =14106              
--update jourmst set mstbrnc = null where mstbrnc = 0              
--select * from jourtrn where trncode = 14106 and trntype =3              
--sp_GetLedger @CompList = '66', @AcctList = '2460', @BdDateFrom = '2018-04-01', @BdDateTo = '2018-08-09'      , @pItemDtls =1        
        
Alter proc [dbo].[sp_GetLedger_PDF] @CompList varchar(250),  @AcctList varchar(max), @BrncList varchar(250)=null, @BdDateFrom smalldatetime, @BdDateTo smalldatetime, @pBranchWs int = 0, @pItemDtls int = 0              
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
  
    
      
 '0' else gododesc end) as groupnum, * from (select acctdesc, compname, '' as mstchno, trndate + 1 as trndate, case when sumcram - sumdram > 0 then sumcram - sumdram else 0 end sumcram, case when sumdram - sumcram > 0 then sumdram - sumcram else 0 end sumdram, 'Op. Bal.' as particular, '' as remark, '' as typehdr, gododesc, isnull(mstbrnc, 0) mstbrnc, 0 as trncode, 0 as trntype, sumbala, trnitem from udf_GetBalance(null, @BdDateFrom-1, @pBranchWs, null, null) tp inner join udf_AccountMst(@lCmpSngC) at on 
  
    
      
tp.trnitem = at.acctcode inner join company cp on cp.compcode = tp.compcode left join udf_BranchList(@lCmpSngC) bs on bs.godocode = tp.mstbrnc where tp.compcode in  (select * from dbo.SplitStringtoIntTable(@compList)) and tp.trnitem in (select * from dbo.
  
    
SplitStringtoIntTable(@AcctList)) and isnull(tp.mstbrnc, 0) in (isnull(@BrncList, isnull(tp.mstbrnc, 0)))              
 union                  
 select tp.acctdesc, tp.Compname, tp.mstchno, tp.trndate, tp.trncram, tp.trndram, tp.particular, tp.mstrema, dbo.udf_getVchTypeHdr(trntype) as typehdr, gododesc, isnull(mstbrnc, 0) mstbrnc, trncode, trntype, 0 sumbala, tp.trnitem from udf_GetLedgerDet(null, @lCmpSngC, @BdDateFrom, @BdDateTo) tp  where tp.compcode in (select * from dbo.SplitStringtoIntTable(@compList)) and tp.trnitem in (select * from dbo.SplitStringtoIntTable(@AcctList)) and isnull(tp.mstbrnc, 0) in (isnull(@BrncList, isnull(tp.mstbrnc, 0
  
    
      
)))              
 union                  
 select acctdesc, compname, '' as mstchno, tp.trndate as trndate, tp.sumcram, tp.sumdram, 'Total / Cl. Bal.' as particular, '' as remark, '' as typehdr, gododesc, isnull(tp.mstbrnc, 0) mstbrnc, 0 as trncode, 9999 as trntype, tp.sumbala, tp.trnitem from udf_GetBalance(null, @BdDateTo, @pBranchWs, null, null) tp /*left join udf_GetBalance(null, @lCmpSngC, @BdDateFrom-1, @pBranchWs, null, null) od on od.trnitem = tp.trnitem*/ inner join udf_AccountMst(@lCmpSngC) at on tp.trnitem = at.acctcode inner join company cp on cp.compcode = tp.compcode left join udf_BranchList(@lCmpSngC) bs on bs.godocode = tp.mstbrnc where tp.compcode in (select * from dbo.splitStringtoIntTable(@compList)) and tp.trnitem in (select * from dbo.SplitStringtoIntTable(@AcctList)) and isnull(tp.mstbrnc, 0) in (isnull(@BrncList, isnull(tp.mstbrnc, 0)))) tp                    
 order by acctdesc, compname, CASE when @pBranchWs = 0 then  '0' else gododesc end, trndate, trntype, trncode                  
         
 /*09-08-2018 */          
 /* <br/> */          
 if @pItemDtls = 1        
 begin  
 
 update a set a.remark = a.remark + b.particular from @temp_ledger a inner join (select STUFF((select Char(10)   + descalia + ' = ' +  convert(varchar, abs(itdquan), 0) + ' ' + iInf.unitname + ' = ' + convert(varchar, itdactprc, 0) from itpursal iTde inner   
   
join (select * from iteminfo where compcode = 66) iInf on iInf.itemcode = iTde.itditem where iTde.compcode = x.compcode and iTde.itdcode = x.itdcode and iTde.itdtype = x.itdtype and iTde.itddate = x.itddate order by itdsrno for xml path('')),1,1,'') as particular, compcode, itdcode, itdtype, itddate from itpursal x where x.compcode = @lCmpSngC group by compcode, itdcode, itdtype, itddate          
) b on a.trncode = b.itdcode and a.trntype = b.itdtype and a.vcdt = b.itddate         
        
 end        
        
 /*if @Partyid is not null    */              
 update a set a.bal = (SELECT SUM(dr-cr) FROM @temp_ledger b WHERE b.rowidnum <= a.rowidnum and trntype <> 9999 and b.groupnum = a.groupnum) from @temp_ledger a                   
              
 update a set a.dr= b.sumdr, a.cr = b.sumcr from @temp_ledger a inner join (SELECT SUM(dr) sumdr, sum(cr) sumcr, groupnum FROM @temp_ledger b WHERE trntype <> 9999 group by groupnum) b on b.groupnum = a.groupnum where trntype = 9999                  
                     
 select groupnum, ledger, company, [Branch], [VcDt], [VcType], trncode, [VcNo], [OnAccountName], [Remark], [Dr], [Cr], abs([bal]) [Bal], case when bal < 0 then 'Cr' else 'Dr' end as [DrCr], trntype, trnitem from @temp_ledger a order by rowidnum           
  
    
      
       
end
GO
