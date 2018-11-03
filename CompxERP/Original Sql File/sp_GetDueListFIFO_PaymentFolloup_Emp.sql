--[sp_GetDueListFIFO_PaymentFolloup_Emp] @pUptoDate = '2018-10-22', @pCompCd = null
--drop function [dbo].[udf_GetDueListFIFO_PaymentFolloup]                                              
Alter procedure [dbo].[sp_GetDueListFIFO_PaymentFolloup_Emp](@pUptoDate smalldatetime, @pCompCd int, @pBranchWs int = 1, @pBrncCd int = null, @pAcctList varchar(max) ='' ,@State int =0 , @City int =0 ,   @CommitOnly int =0 ,@isPlanning bit =0 ,@empID int  =0 , @All int = 0 )                              
-- 0 = Plan,Commit,Due / 1 = All Due / 2 = Over Due      
                             
as                                                    
begin              
          
/* #################### */             
Declare @tblAcct  table (AcctCode int)           
insert @tblAcct  Select b.acctCode from EmpAllotMst a inner join account b on a.DealerID = b.acctDeal and b.acctgrou = 21 inner join Employee c on a.empID = c.EmpCode where c.UseCode =@empID             
--Select acctCode   from @tblAcct          
/* #################### */             
          
                                               
 declare @finalList Table(billSr int identity (1, 1), compcode int, brnccode int, acctcode int, agencode int, voucdate smalldatetime, vouctype int, vouccode int, vouccond int, voucdued smalldatetime, voucdrvl money, vouccrvl money, voucblvl money, voucdays int, inteamou money, voucsrno int, voucstat int, areacode int, intedays int, voucrefn varchar(50), balancev money, rowidnum int)                                              
                                               
 Declare @BalTable  table                                              
   (Id int identity(1, 1), compcode int, mstbrnc int, trnitem int, trnledg int, sumopen money, sumdram money, sumcram money, sumbala money)        
 insert into @BalTable (compcode, mstbrnc, trnitem, trnledg, sumopen, sumdram, sumcram, sumbala) select compcode, mstbrnc, trnitem, trnledg, 0 sumopen, sumdram, sumcram, sumbala from dbo.udf_GetBalanceLdg(@pCompCd, @pUptoDate, @pBranchWs, @pBrncCd, null) 
   
x inner join @tblAcct y on y.acctCode = x.trnitem                    
                 
 insert @finalList (compcode, brnccode, acctcode, agencode, voucdate, vouctype, vouccode, vouccond, voucdued, voucdrvl, vouccrvl, voucblvl, voucdays , inteamou, voucsrno, voucstat, areacode, intedays, voucrefn, rowidnum)          
 select b.compcode, b.mstbrnc, b.trnitem, b.trnledg, y.mstdate, y.msttype, y.mstcode, datediff(dd, y.mstdate, y.mstduedate), y.mstduedate, trndram, 0 as trncram, 0 as balamou, datediff(dd, y.mstduedate, getdate())+1, 0, trnsrno, 0, 0, 0, y.mstchno, row_number() over(partition by x.trnitem order by x.trnitem, x.trndate desc, x.trncode desc) as rownumb from @BalTable b inner join jourtrn x on x.trnitem = b.trnitem and x.compcode = b.compcode and x.trnledg = b.trnledg inner join jourmst y on  y.compcode = x.
  
    
compcode and  y.mstcode = x.trncode  and y.mstdate = x.trndate and y.msttype = x.trntype where trndram > 0 and datediff(d, trndate, @pUptoDate) >= 0                                                      
 update y set y.voucblvl = case when z.runningt <= x.sumbala then z.voucdrvl else case when z.runningt - x.sumbala > z.voucdrvl then 0 else z.voucdrvl - (z.runningt - x.sumbala) end end, balancev = z.runningt from @BalTable x inner join  @finalList y on x
  
    
.compcode = y.compcode and x.trnitem = y.acctcode and x.mstbrnc = y.brnccode and x.trnledg = y.agencode inner join (select a.*, (select sum(voucdrvl) from @finalList where compcode = a.compcode and acctcode = a.acctcode and brnccode = a.brnccode and agencode = a.agencode and rowidnum <= a.rowidnum) runningt from @finalList a) z on z.billsr = y.billsr                             
               
/* #################### */                 
              
 if (@All = 0 )              
  begin          
Select acctdesc , OrdValue ,   Commited ,  ordcompc, Dues , compcode ,  acctcode , CompNAme ,  DueDays, MaxDues , StatusID ,sumbala from(                
            
select acc.acctdesc +' - '+ isnull(acc.acctphon,'') acctdesc ,d.OrdValue , isnull(d.OrdValue,0) + isnull(sumBala,0) Commited , d.compcode ordcompc, isnull(Dues,0)  Dues ,isnull(d.Compcode , a.Compcode) compcode , acc.acctcode ,comp.CompNAme ,MDay.maxDays 
  
       
DueDays,MValue.MaxDues ,1 StatusID ,a.sumBala from vewLedgerMst acc                                  
--left join dbo.SplitStringtoIntTable(@pAcctList) y on y.value = acc.acctCode                                    
left Join @BalTable a on acc.acctCode = a.trnledg and acc.cexicomp = a.compcode                                   
left Join (select sum(voucblvl)Dues ,acctCode ,compcode from @finalList where voucdays >0 group by acctCode ,compcode ) _a on a.trnledg = _a.acctCode and a.compcode = _a.Compcode                      
left join (select Compcode ,mstcust ,sum(OrdValue) OrdValue from getOrderValue (@pCompCd) group by Compcode ,mstcust  ) d on acc.acctCode = d.mstcust  and acc.cexicomp = d.compcode                     
Inner join Company comp on isnull(d.Compcode , a.Compcode)  = comp.Compcode                   
left Join (select CompCode ,acctCode ,isnull(max(VoucDays),0) maxDays from @finalList where VoucType = 42 and VoucDays >0  group by CompCode ,acctCode ) MDay on acc.acctCode = MDay.acctCode  and acc.cexicomp = MDay.CompCode              
left Join (select Max(voucblvl)MaxDues ,acctCode ,compcode from @finalList where  VoucType = 42 and voucdays >0 group by acctCode ,compcode )MValue on acc.acctCode = MValue.acctCode  and acc.cexicomp = MValue.CompCode               
where Acc.AcctGrou =  21 and (isnull(d.OrdValue,0) >0 or isnull(sumBala,0) >0 ) and MDay.maxDays = 1 and acc.acctCode in ( select * from @tblAcct )--( select * from dbo.SplitStringtoIntTable(@pAcctList) )   and MValue.MaxDues >0          
--Order by MValue.MaxDues desc ,maxDays desc ,acc.acctname             
            
union              
            
select acc.acctdesc +' - '+ isnull(acc.acctphon,'') acctdesc ,d.OrdValue , isnull(d.OrdValue,0) + isnull(sumBala,0) Commited , d.compcode ordcompc, isnull(Dues,0)  Dues   ,isnull(d.Compcode , a.Compcode) compcode , acc.acctcode ,comp.CompNAme ,MDay.maxDays DueDays,MValue.MaxDues ,2 StatusID ,a.sumBala from vewLedgerMst acc                                                         
left Join @BalTable a on acc.acctCode = a.trnledg and acc.cexicomp = a.compcode                                   
left Join (select sum(voucblvl)Dues ,acctCode ,compcode from @finalList where voucdays >0 group by acctCode ,compcode ) _a on a.trnledg = _a.acctCode and a.compcode = _a.Compcode                      
left join (select Compcode ,mstcust ,sum(OrdValue) OrdValue from getOrderValue (@pCompCd) group by Compcode ,mstcust  ) d on acc.acctCode = d.mstcust  and acc.cexicomp = d.compcode                     
Inner join Company comp on isnull(d.Compcode , a.Compcode)  = comp.Compcode                   
left Join (select CompCode ,acctCode ,isnull(max(VoucDays),0) maxDays from @finalList where VoucType = 42 and VoucDays >0  group by CompCode ,acctCode ) MDay on acc.acctCode = MDay.acctCode  and acc.cexicomp = MDay.CompCode              
left Join (select Max(voucblvl)MaxDues ,acctCode ,compcode from @finalList where  VoucType = 42 and voucdays >0 group by acctCode ,compcode )MValue on acc.acctCode = MValue.acctCode  and acc.cexicomp = MValue.CompCode               
where Acc.AcctGrou =  21 and (isnull(d.OrdValue,0) >0 or isnull(sumBala,0) >0 ) and acc.acctCode in (Select DealerID from tblPayFolloupPlanning where IsUse = 0 and PlanningDT <= @pUptoDate group by DealerID  )  and acc.acctCode in ( select * from @tblAcct
  
    
      
        
 ) --( select * from dbo.SplitStringtoIntTable(@pAcctList) )             
--Order by MValue.MaxDues desc ,maxDays desc ,acc.acctname             
            
 union             
select acc.acctdesc +' - '+ isnull(acc.acctphon,'') acctdesc ,d.OrdValue , isnull(d.OrdValue,0) + isnull(sumBala,0) Commited , d.compcode ordcompc, isnull(Dues,0)  Dues   ,isnull(d.Compcode , a.Compcode) compcode , acc.acctcode ,comp.CompNAme ,MDay.maxDays DueDays,MValue.MaxDues ,3 StatusID ,a.sumBala from vewLedgerMst acc                                                         
left Join @BalTable a on acc.acctCode = a.trnledg and acc.cexicomp = a.compcode                                   
left Join (select sum(voucblvl)Dues ,acctCode ,compcode from @finalList where voucdays >0 group by acctCode ,compcode ) _a on a.trnledg = _a.acctCode and a.compcode = _a.Compcode                      
left join (select Compcode ,mstcust ,sum(OrdValue) OrdValue from getOrderValue (@pCompCd) group by Compcode ,mstcust  ) d on acc.acctCode = d.mstcust  and acc.cexicomp = d.compcode                     
Inner join Company comp on isnull(d.Compcode , a.Compcode)  = comp.Compcode                   
left Join (select CompCode ,acctCode ,isnull(max(VoucDays),0) maxDays from @finalList where VoucType = 42 and VoucDays >0  group by CompCode ,acctCode ) MDay on acc.acctCode = MDay.acctCode  and acc.cexicomp = MDay.CompCode              
left Join (select Max(voucblvl)MaxDues ,acctCode ,compcode from @finalList where  VoucType = 42 and voucdays >0 group by acctCode ,compcode )MValue on acc.acctCode = MValue.acctCode  and acc.cexicomp = MValue.CompCode               
where Acc.AcctGrou =  21 and (isnull(d.OrdValue,0) >0 or isnull(sumBala,0) >0 ) and acc.acctCode in (select PartyID from tblPayFollowUP Where convert( varchar(20) ,NextDt , 111) = convert( varchar(20) ,getDate() , 111)) and acc.acctCode in ( select * from
  
    
      
        
 @tblAcct ) --( select * from dbo.SplitStringtoIntTable(@pAcctList) )             
                
) PF             
Order by StatusID ,MaxDues desc ,DueDays desc ,acctdesc                                     
              
End           
else if (@All = 1 )           
          
Begin          
           
select acc.acctdesc +' - '+ isnull(acc.acctphon,'') acctdesc ,d.OrdValue , isnull(d.OrdValue,0) + isnull(sumBala,0) Commited , d.compcode ordcompc, isnull(Dues,0)  Dues ,isnull(d.Compcode , a.Compcode) compcode , acc.acctcode ,comp.CompNAme ,MDay.maxDays 
  
    
      
        
          
DueDays,MValue.MaxDues ,1 StatusID ,a.sumBala from vewLedgerMst acc                                  
--left join dbo.SplitStringtoIntTable(@pAcctList) y on y.value = acc.acctCode                                    
left Join @BalTable a on acc.acctCode = a.trnledg and acc.cexicomp = a.compcode                                   
left Join (select sum(voucblvl)Dues ,acctCode ,compcode from @finalList where voucdays >0 group by acctCode ,compcode ) _a on a.trnledg = _a.acctCode and a.compcode = _a.Compcode                      
left join (select Compcode ,mstcust ,sum(OrdValue) OrdValue from getOrderValue (@pCompCd) group by Compcode ,mstcust  ) d on acc.acctCode = d.mstcust  and acc.cexicomp = d.compcode                     
Inner join Company comp on isnull(d.Compcode , a.Compcode)  = comp.Compcode                   
left Join (select CompCode ,acctCode ,isnull(max(VoucDays),0) maxDays from @finalList where VoucType = 42 and VoucDays >0  group by CompCode ,acctCode ) MDay on acc.acctCode = MDay.acctCode  and acc.cexicomp = MDay.CompCode              
left Join (select Max(voucblvl)MaxDues ,acctCode ,compcode from @finalList where  VoucType = 42 and voucdays >0 group by acctCode ,compcode )MValue on acc.acctCode = MValue.acctCode  and acc.cexicomp = MValue.CompCode               
where Acc.AcctGrou =  21 and (isnull(d.OrdValue,0) >0 or isnull(sumBala,0) >0 )   and acc.acctCode in ( select * from @tblAcct )           
          
 Order by MValue.MaxDues desc ,maxDays desc ,acc.acctname             
End          
    else if (@All = 2 )           
          
Begin         
                 
select acc.acctdesc +' - '+ isnull(acc.acctphon,'') acctdesc ,d.OrdValue , isnull(d.OrdValue,0) + isnull(sumBala,0) Commited , d.compcode ordcompc, isnull(Dues,0)  Dues ,isnull(d.Compcode , a.Compcode) compcode , acc.acctcode ,comp.CompNAme ,MDay.maxDays 
  
    
       
DueDays,MValue.MaxDues ,1 StatusID ,a.sumBala from vewLedgerMst acc                                  
--left join dbo.SplitStringtoIntTable(@pAcctList) y on y.value = acc.acctCode                                    
left Join @BalTable a on acc.acctCode = a.trnledg and acc.cexicomp = a.compcode                                   
left Join (select sum(voucblvl)Dues ,acctCode ,compcode from @finalList where voucdays >0 group by acctCode ,compcode ) _a on a.trnledg = _a.acctCode and a.compcode = _a.Compcode                 
left join (select Compcode ,mstcust ,sum(OrdValue) OrdValue from getOrderValue (@pCompCd) group by Compcode ,mstcust  ) d on acc.acctCode = d.mstcust  and acc.cexicomp = d.compcode                     
Inner join Company comp on isnull(d.Compcode , a.Compcode)  = comp.Compcode                   
left Join (select CompCode ,acctCode ,isnull(max(VoucDays),0) maxDays from @finalList where VoucType = 42 and VoucDays >0  group by CompCode ,acctCode ) MDay on acc.acctCode = MDay.acctCode  and acc.cexicomp = MDay.CompCode              
left Join (select Max(voucblvl)MaxDues ,acctCode ,compcode from @finalList where  VoucType = 42 and voucdays >0 group by acctCode ,compcode )MValue on acc.acctCode = MValue.acctCode  and acc.cexicomp = MValue.CompCode               
where Acc.AcctGrou =  21 and (isnull(d.OrdValue,0) >0 or isnull(sumBala,0) >0 ) and MDay.maxDays > 0 and acc.acctCode in ( select * from @tblAcct )      
        
Order by MValue.MaxDues desc ,maxDays desc ,acc.acctname             
            
End       
End 