create Proc [dbo].[GetStatisticEmp](@pFromDate varchar(50)='2018-04-01',@pEmpCode int)
as    
Begin    

 select   
 (  
  Select sum(cast((b.itdquan - abs(isnull(d.itdquan,0) )) as decimal(18,2))) PendingDispatch
  from OrdeMst a
  inner join ordeItd b on a.compcode = b.compcode and a.msttype =b.itdtype and a.mstcode = b.itdcode                         
  left join refetab c on a.compcode = c.compcode and b.itdtype = c.ms2type and itdcode = ms2code and a.mstchno = c.refRema  and  b.itditem = c.refitem
  left join itpursal d on c.compcode = d.compcode and c.ms1type =d.itdtype and c.ms1code = d.itdcode and c.refitem = d.itditem
  where msttype = 80 and (b.itdquan + isnull(d.itdquan,0))>0 and a.mstdate between @pFromDate and convert( varchar(10), Getdate() ,101)
 ) as PendingDispatch,  
 (  
 select sum(aCount) from (  
  select count(o.mstchno) as aCount  
  from ordemst o  
  left join ordemst ap on ap.msttype = 80 and ap.compcode=o.compcode and o.mstchno=ap.mstchno  
  where o.msttype = 174 and ap.compcode is null and o.mstdate between @pFromDate and convert( varchar(10), Getdate() ,101)  
  union all  
  select count(x.compcode) as aCount from (select sum(ot.itdquan) as Qty,ot.itditem,o.compcode,o.mstcode,o.mstchno  
  from ordemst o  
  inner join ordeitd ot on ot.compcode=o.compcode and o.msttype=ot.itdtype and o.mstcode=ot.itdcode  
  where o.msttype = 174 and o.mstdate between @pFromDate and convert( varchar(10), Getdate() ,101)  
  group by ot.itditem,o.compcode,o.mstcode,o.mstchno--3834  
  ) x   
  inner join (  
   select sum(ot.itdquan) as Qty,ot.itditem,o.compcode,o.mstcode,o.mstchno  
   from ordemst o  
   inner join ordeitd ot on ot.compcode=o.compcode and o.msttype=ot.itdtype and o.mstcode=ot.itdcode  
   where o.msttype = 80 and o.mstdate between @pFromDate and convert( varchar(10), Getdate() ,101)  
   group by ot.itditem,o.compcode,o.mstcode,o.mstchno--3455  
  ) y on x.itditem=y.itditem and x.compcode=y.compcode and x.mstchno=y.mstchno  
  where (x.Qty-y.Qty)>0  
  ) z  
 ) as ApprQty,
   
 (select cast((sum(x.sumbala)/10000000) as varchar)+' Cr' from (select sum(trndram-trncram) sumbala, a.compcode, b.trnitem from jourmst a inner join jourtrn b on b.compcode = a.compcode and b.trncode = a.mstcode and b.trndate = a.mstdate and b.trntype = a
.msttype where a.mstdate <= getdate() group by a.compcode, b.trnitem) x inner join vewLedgerMst c on c.acctcode = x.trnitem where  c.groumain = 21 ) as TotalOutstanding,  

 (select cast((sum(x.sumbala)/10000000) as varchar)+' Cr' from (select sum(trndram-trncram) sumbala, a.compcode, b.trnitem from jourmst a inner join jourtrn b on b.compcode = a.compcode and b.trncode = a.mstcode and b.trndate = a.mstdate and b.trntype = a
.msttype where isnull(a.mstduedate, a.mstdate) <= getdate() group by a.compcode, b.trnitem) x inner join vewLedgerMst c on c.acctcode = x.trnitem where  c.groumain = 21) as TotalDue
 
End  
go
--GetStatisticEmp @pFromDate='2018-04-01',@pEmpCode=19