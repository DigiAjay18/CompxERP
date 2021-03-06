--sp_GetDueListFIFO @pUptoDate = '2018-09-22', @pCompCd = null, @pAcctList = '2605, 1523, 1712, 2605, 2607'
/****** Object:  StoredProcedure [dbo].[sp_GetDueListFIFO]    Script Date: 09/22/2018 12:47:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter procedure [dbo].[sp_GetDueListFIFO](@pUptoDate smalldatetime, @pCompCd int, @pBranchWs int = 1, @pBrncCd int = null, @pAcctList varchar(max) ,@State int =0 , @City int =0 )                          
--Returns                           
as                                
begin                               
 declare @finalList Table(billSr int identity (1, 1), compcode int, brnccode int, acctcode int, agencode int, voucdate smalldatetime, vouctype int, vouccode int, vouccond int, voucdued smalldatetime, voucdrvl money, vouccrvl money, voucblvl money, voucdays int, inteamou money, voucsrno int, voucstat int, areacode int, intedays int, voucrefn varchar(50), balancev money, rowidnum int)                          
                           
 Declare @BalTable  table                          
   (Id int identity(1, 1), compcode int, mstbrnc int, trnitem int, trnledg int, sumopen money, sumdram money, sumcram money, sumbala money)         
 
  print 'Get Balance'
  print CAST(GETDATE() as Datetime2(7))
 insert into @BalTable (compcode, mstbrnc, trnitem, trnledg, sumopen, sumdram, sumcram, sumbala) select compcode, mstbrnc, trnitem, trnledg, 0 sumopen, sumdram, sumcram, sumbala from dbo.udf_GetBalanceLdg(@pCompCd, @pUptoDate, @pBranchWs, @pBrncCd, null) x inner join dbo.SplitStringtoIntTable(@pAcctList) y on y.value = x.trnitem                          
                   
  print 'Get Ledger'
  print CAST(GETDATE() as Datetime2(7))

 insert @finalList (compcode, brnccode, acctcode, agencode, voucdate, vouctype, vouccode, vouccond, voucdued, voucdrvl, vouccrvl, voucblvl, voucdays, inteamou, voucsrno, voucstat, areacode, intedays, voucrefn, rowidnum)                           
 select b.compcode, b.mstbrnc, b.trnitem, b.trnledg, y.mstdate, y.msttype, y.mstcode, datediff(dd, y.mstdate, y.mstduedate), y.mstduedate, trndram, 0 as trncram, 0 as balamou, datediff(dd, y.mstduedate, getdate()), 0, trnsrno, 0, 0, 0, y.mstchno, row_number() over(partition by x.trnitem order by x.trnitem, x.trndate desc, x.trncode desc) as rownumb from @BalTable b inner join jourtrn x on x.trnitem = b.trnitem and x.compcode = b.compcode and x.trnledg = b.trnledg inner join jourmst y on  y.compcode = x.compcode and  y.mstcode = x.trncode  and y.mstdate = x.trndate and y.msttype = x.trntype where trndram > 0 and datediff(d, trndate, @pUptoDate) >= 0                          
 
  print 'Upd Balance days'
  print CAST(GETDATE() as Datetime2(7))
                          
 update y set y.voucblvl = case when z.runningt <= x.sumbala then z.voucdrvl else case when z.runningt - x.sumbala > z.voucdrvl then 0 else z.voucdrvl - (z.runningt - x.sumbala) end end, balancev = z.runningt from @BalTable x inner join  @finalList y on x.compcode = y.compcode and x.trnitem = y.acctcode and x.mstbrnc = y.brnccode and x.trnledg = y.agencode inner join (select a.*, (select sum(voucdrvl) from @finalList where compcode = a.compcode and acctcode = a.acctcode and brnccode = a.brnccode and agencode = a.agencode and rowidnum <= a.rowidnum) runningt from @finalList a) z on z.billsr = y.billsr                 
 
 /*book2*/                   
  print 'Select finallist'
  print CAST(GETDATE() as Datetime2(7))
	select acc.acctdesc,d.OrdValue , isnull(d.OrdValue,0) + isnull(sumBala,0) Commited , d.compcode ordcompc, isnull(Dues,0)  Dues , a.* ,isnull(d.Compcode , a.Compcode) compcode , acc.acctcode ,comp.CompNAme  from vewLedgerMst acc              
	inner join dbo.SplitStringtoIntTable(@pAcctList) y on y.value = acc.acctCode                
	left Join @BalTable a on acc.acctCode = a.trnledg and acc.cexicomp = a.compcode               
	left Join (select sum(voucblvl)Dues ,acctCode ,compcode from @finalList where voucdays >0 group by acctCode ,compcode ) _a on a.trnledg = _a.acctCode and a.compcode = _a.Compcode                  
	--inner join vewAccountMst  b on a.trnledg = b.acctCode      
	left join (select Compcode ,mstcust ,sum(OrdValue) OrdValue from getOrderValue (@pCompCd) group by Compcode ,mstcust  ) d on acc.acctCode = d.mstcust  and acc.cexicomp = d.compcode 
	Inner join Company comp on isnull(d.Compcode , a.Compcode)  = comp.Compcode        /*book1*/
	where Acc.AcctGrou =  21 and (isnull(d.OrdValue,0) >0 or isnull(sumBala,0) >0 ) Order by acc.acctname            
                 
  print 'Select payfollowup'	
  print CAST(GETDATE() as Datetime2(7))
	select b.acctdesc,c.FollowUp, a.* from @finalList a                        
	inner join vewLedgerMst  b on a.acctCode = b.acctCode and b.cexicomp = a.compcode                                     
	Left JOin (select CompCode , VoucType , VoucCode ,Count(*) FollowUp from tblPayFollowUP group by CompCode ,VoucType ,VoucCode )c on a.vouctype =c.VoucType and a.vouccode  =c.VoucCode and a.compcode =c.CompCode                      
	where acctgrou = 21 And voucblvl > 0 and 1= case when @State = 0 then 1 else case when b.acctStat = @State then 1 else 0 end end                    and 1= case when @City = 0 then 1 else case when b.acctCity = @City then 1 else 0 end end order By b.acctdesc asc , rowidnum desc                          
	print 'end'
	print CAST(GETDATE() as Datetime2(7))                
End
GO

/*book1*/
/* left Join (               
Select  sum(bb.itdamou) - sum(isnull(g.itdAmou,0)) OrdValue ,mstcust ,aa.compcode from  ordemst aa                                                  
inner join ordeitd bb on   aa.compcode= bb.compcode and aa.msttype = bb.itdtype and aa.mstcode = bb.itdcode  
Left join (
Select OM.CompCode ,OI.itdType ,Sum(abs(IT.itdquan))itdQuan ,OI.itdItem ,(Sum(abs(IT.itdquan))* OI.itdactprc) itdAmou,rf.refRema MstChno from ordemst OM 
Inner join ordeitd OI on om.compcode = om.compcode and om.msttype = OI.ItdType and om.mstCode = OI.ItdCode 
left join refetab RF on OM.Compcode = rf.Compcode  and rf.Ms1Type = 42 and ms2type = OM.msttype and OM.mstchno = rf.refRema  and rf.ms2srno = OM.mstprtc and rf.refItem = OI.itdItem 
inner join itpursal IT on RF.compcode = IT.compcode and RF.ms1Type = IT.itdtype and RF.ms1Code = IT.itdCode and RF.refItem = IT.itdItem
where msttype = 80 group by OM.CompCode ,OI.itdType,OI.itdItem ,rf.refRema,OI.itdactprc ) g    
                                              
on   aa.compcode = g.compcode  and aa.MstChno = g.MstChno and  bb.itditem = g.itditem                                              
where aa.msttype = 174  and (bb.itdquan - isnull(g.itdquan ,0)) > 0 and aa.compcode = @pCompCd /* and g.mstchno is null */ group by mstcust ,aa.compcode) d on acc.acctCode = d.mstcust */              

/*book2*/
--                  
--select b.acctdesc,d.OrdValue , isnull(d.OrdValue,0) + isnull(sumBala,0) Commited ,isnull(Dues,0)  Dues  ,a.*  from @BalTable a                        
--left Join (select sum(voucblvl)Dues ,acctCode ,compcode from @finalList where voucdays >0 group by acctCode ,compcode ) _a on a.trnledg = _a.acctCode and a.compcode = _a.Compcode       
--inner join vewAccountMst  b on a.trnledg = b.acctCode                --left Join (select sum(b.itdamou)OrdValue ,mstcust ,a.compcode from ordemst a                  
--inner join ordeitd b on a.msttype =b.itdType and a.mstcode = b.itdcode                    
--left join refetab c on a.mstchno = c.refRema and b.itdItem = c.refItem                    
--where a.msttype = 174 and c.compcode is null                  
--group by mstcust ,a.compcode ) d on a.trnledg = d.mstcust and d.compcode = a.compcode                 

/*book3*/
-- select b.acctdesc,d.OrdValue , isnull(d.OrdValue,0) + isnull(sumBala,0) Commited ,a.*  from @BalTable a                        
--inner join vewAccountMst  b on a.trnledg = b.acctCode                     
--left Join (select sum(b.itdamou)OrdValue ,mstcust ,a.compcode from ordemst a                  
--inner join ordeitd b on a.msttype =b.itdType and a.mstcode = b.itdcode                    
--left join refetab c on a.mstchno = c.refRema and b.itdItem = c.refItem                    
--where a.msttype = 174 and c.compcode is null                  
--group by mstcust ,a.compcode ) d on a.trnledg = d.mstcust and d.compcode = a.compcode                  
