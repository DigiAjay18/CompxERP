--[sp_GetDueListFIFO_PaymentFolloup] @pUptoDate = '2018-10-22', @pCompCd = null, @pAcctList = '2605, 1523, 1712, 2605, 2607'
--sp_GetDueListFIFO_PaymentFolloup @pUptoDate ='2018-04-01',@pCompCd =66, @pBranchWs = 1, @pBrncCd = null, @pAcctList  ='' ,@State =0 , @City =0 , @DueOnly =0 , @CommitOnly =0 
--drop function [dbo].[udf_GetDueListFIFO_PaymentFolloup]                                  
alter procedure [dbo].[sp_GetDueListFIFO_PaymentFolloup](@pUptoDate smalldatetime, @pCompCd int, @pBranchWs int = 1, @pBrncCd int = null, @pAcctList nvarchar(max) ='' ,@State int =0 , @City int =0 , @DueOnly int =0 , @CommitOnly int =0 ,@isPlanning bit =0 )                  
                 
as                                        
begin                                       
        
 	declare @SqlS nvarchar(max)

 declare @finalList Table(billSr int identity (1, 1), compcode int, brnccode int, acctcode int, agencode int, voucdate smalldatetime, vouctype int, vouccode int, vouccond int, voucdued smalldatetime, voucdrvl money, vouccrvl money, voucblvl money, voucdays int, inteamou money, voucsrno int, voucstat int, areacode int, intedays int, voucrefn varchar(50), balancev money, rowidnum int)                                  

   Declare @BalTable  as BalTableType
	print 'Get Balance'
  print CAST(GETDATE() as Datetime2(7))                          
  set @Sqls = N'insert into @BalTable (compcode, mstbrnc, trnitem, trnledg, sumopen, sumdram, sumcram, sumbala) select compcode, mstbrnc, trnitem, trnledg, 0 sumopen, sumdram, sumcram, sumbala from dbo.udf_GetBalanceLdg(@pCompCd, @pUptoDate, @pBranchWs, @pBrncCd, null) 
x '        
if  @pAcctList <> '' 
	set @SqlS = @SqlS + N' inner join dbo.SplitStringtoIntTable(@pAcctList) y on y.value = x.trnitem ' 
 
	execute sp_executesql @Sqls, N'@BalTable BalTableType, @pCompCd int ', @BalTable, @pCompCd
      print 'Get Ledger'
  print CAST(GETDATE() as Datetime2(7)) 
 insert @finalList (compcode, brnccode, acctcode, agencode, voucdate, vouctype, vouccode, vouccond, voucdued, voucdrvl, vouccrvl, voucblvl, voucdays , inteamou, voucsrno, voucstat, areacode, intedays, voucrefn, rowidnum)                                   
 select b.compcode, b.mstbrnc, b.trnitem, b.trnledg, y.mstdate, y.msttype, y.mstcode, datediff(dd, y.mstdate, y.mstduedate), y.mstduedate, trndram, 0 as trncram, 0 as balamou, datediff(dd, y.mstduedate, getdate())+1, 0, trnsrno, 0, 0, 0, y.mstchno, row_number() over(partition by x.trnitem order by x.trnitem, x.trndate desc, x.trncode desc) as rownumb from @BalTable b inner join jourtrn x on x.trnitem = b.trnitem and x.compcode = b.compcode and x.trnledg = b.trnledg inner join jourmst y on  y.compcode = x.
compcode and  y.mstcode = x.trncode  and y.mstdate = x.trndate and y.msttype = x.trntype where trndram > 0 and datediff(d, trndate, @pUptoDate) >= 0                                          
  print 'Upd Balance days'
  print CAST(GETDATE() as Datetime2(7))                          
 update y set y.voucblvl = case when z.runningt <= x.sumbala then z.voucdrvl else case when z.runningt - x.sumbala > z.voucdrvl then 0 else z.voucdrvl - (z.runningt - x.sumbala) end end, balancev = z.runningt from @BalTable x inner join  @finalList y on x.compcode = y.compcode and x.trnitem = y.acctcode and x.mstbrnc = y.brnccode and x.trnledg = y.agencode inner join (select a.*, (select sum(voucdrvl) from @finalList where compcode = a.compcode and acctcode = a.acctcode and brnccode = a.brnccode and agencode = a.agencode and rowidnum <= a.rowidnum) runningt from @finalList a) z on z.billsr = y.billsr                 
   
/* #################### */     

  print 'Select finallist'
  print CAST(GETDATE() as Datetime2(7))
   
	set @Sqls = N'select acc.acctdesc +'' - ''+ isnull(acc.acctphon,'''') acctdesc ,d.OrdValue , isnull(d.OrdValue,0) + isnull(sumBala,0) Commited , d.compcode ordcompc, isnull(Dues,0)  Dues , a.* ,isnull(d.Compcode , a.Compcode) compcode , acc.acctcode ,comp.CompNAme ,MDay.maxDays DueDays,MValue.MaxDues from vewLedgerMst acc                      
left Join @BalTable a on acc.acctCode = a.trnledg and acc.cexicomp = a.compcode                       
left Join (select sum(voucblvl)Dues ,acctCode ,compcode from @finalList where voucdays >0 group by acctCode ,compcode ) _a on a.trnledg = _a.acctCode and a.compcode = _a.Compcode          
left join (select Compcode ,mstcust ,sum(OrdValue) OrdValue from getOrderValue (@pCompCd) group by Compcode ,mstcust  ) d on acc.acctCode = d.mstcust  and acc.cexicomp = d.compcode         
Inner join Company comp on isnull(d.Compcode , a.Compcode)  = comp.Compcode      
'  
 if  @pAcctList <> '' 
	set @SqlS = @SqlS + N' inner join (select * from dbo.SplitStringtoIntTable(@pAcctList)) acctL on acc.acctCode = acctL.value ' 
 set @SqlS = @SqlS + 
 N'
left Join (select CompCode ,acctCode ,isnull(max(VoucDays),0) maxDays from @finalList where VoucType = 42 and VoucDays >0  group by CompCode ,acctCode ) MDay on acc.acctCode = MDay.acctCode  and acc.cexicomp = MDay.CompCode  
left Join (select Max(voucblvl)MaxDues ,acctCode ,compcode from @finalList where  VoucType = 42 and voucdays >0 group by acctCode ,compcode )MValue on acc.acctCode = MValue.acctCode  and acc.cexicomp = MValue.CompCode   
where Acc.AcctGrou =  21 and (isnull(d.OrdValue,0) >0 or isnull(sumBala,0) >0 )  
    
and 1= case when @DueOnly = 0 then 1                                                      
else case when Dues >0 then 1 else 0 end end      
     
   
and 1= case when @isPlanning =0 then 1                                                      
else case when acc.acctCode in (Select DealerID from tblPayFolloupPlanning where IsUse = 0 and PlanningDT <= @pUptoDate group by DealerID  ) then 1 else 0 end end      
    
and 1= case when @CommitOnly = 0 then 1                                                      
else case when acc.acctCode in (select PartyID from tblPayFollowUP Where /*convert( varchar(20) ,NextDt , 111) = convert( varchar(20) ,getDate() , 111)*/ NextDt = convert(date, getdate()) ) then 1 else 0 end end      
    

Order by MValue.MaxDues desc ,maxDays desc ,acc.acctname         '              

execute sp_executesql @Sqls, N'@BalTable table ', @BalTable
/* #################### */     

  print 'Select payfollowup'	
  print CAST(GETDATE() as Datetime2(7))
                         
 set @Sqls = N'select b.acctdesc,c.FollowUp, a.* from @finalList a                                
inner join vewLedgerMst  b on a.acctCode = b.acctCode and b.cexicomp = a.compcode                                             
Left JOin (select CompCode , VoucType , VoucCode ,Count(*) FollowUp from tblPayFollowUP group by CompCode ,VoucType ,VoucCode )c on a.vouctype =c.VoucType and a.vouccode  =c.VoucCode and a.compcode =c.CompCode      

where acctgrou = 21 And voucblvl > 0                            
                            
and 1= case when @State = 0 then 1                                                      
else case when b.acctStat = @State then 1 else 0 end end                                                   
             
and 1= case when @City = 0 then 1                                                      
else case when b.acctCity = @City then 1 else 0 end end                                
'
if  @pAcctList <> '' 
	set @SqlS = @SqlS + N' inner join (select * from dbo.SplitStringtoIntTable(@pAcctList)) acctL on acc.acctCode = acctL.value ' 
 set @SqlS = @SqlS + 
 N'    
and 1= case when @CommitOnly = 0 then 1                                                      
else case when a.acctCode in (select PartyID from tblPayFollowUP Where /*convert( varchar(20) ,NextDt , 111) = convert( varchar(20) ,getDate() , 111)*/ nextdt = convert(date, getdate())) then 1 else 0 end end      
                            
order By b.acctdesc asc , rowidnum desc                                  '
-- return                                            
execute sp_executesql @Sqls     
	print 'end'
	print CAST(GETDATE() as Datetime2(7))                

End 
go
