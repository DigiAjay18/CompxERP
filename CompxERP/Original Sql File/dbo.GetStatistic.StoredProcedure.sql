alter Proc [dbo].[GetStatistic](@pFromDate varchar(50)='2018-04-01')
as    
Begin    
/*
 declare @TotalSales int,@TotalSaleValue varchar(50)  
 select @TotalSales=Count(msttota) , @TotalSaleValue=cast((sum(msttota)/10000000) as varchar)+' Cr' from jourmst where msttype = 42   
 declare @TotalCollection varchar(50)=(select cast((sum(trndram)/10000000) as varchar)+' Cr' from jourtrn where trntype = 3 )-- Receipt  
 declare @PendingComplaint  int=(select count(*) from tblComplaint where StatusID=1)  
 declare @ComplaintSolved  int=(select count(*) from tblComplaint where StatusID=2)  
 declare @RevenFromComplains  money=(select sum(Charge) from tblComplaint where StatusID = 2)
 
 declare @PendingOrder int=  
 declare @DispatchOrder int  
 declare @PendingDispatchOrder int  
 declare @TotalOutstanding varchar(50)  
 declare @TotalDue varchar(50)  
 
 declare @Q3Target  int=(select 0)  
 declare @Other  int=(select 0)  
 declare @AvaiStckQty  int=(select 0)  
 declare @ReqStckQty  int=(select 0)  
  */
 select   
 /*(  
  Select sum(isnull(abs(it.itdquan),0)) Dispatch from 
  itpursal it 
  where itdtype = 42 and itddate between @pFromDate and convert( varchar(10), Getdate() ,101)  
 ) as Dispatch,  */
 (  
  Select sum(cast((b.itdquan - abs(isnull(d.itdquan,0) )) as decimal(18,2))) PendingDispatch
  from OrdeMst a
  inner join ordeItd b on a.compcode = b.compcode and a.msttype =b.itdtype and a.mstcode = b.itdcode
  inner join account x on x.acctcode=a.mstprtc and x.acctgrou=21
  left join refetab c on a.compcode = c.compcode and b.itdtype = c.ms2type and itdcode = ms2code and a.mstchno = c.refRema  and  b.itditem = c.refitem
  left join itpursal d on c.compcode = d.compcode and c.ms1type =d.itdtype and c.ms1code = d.itdcode and c.refitem = d.itditem
  where msttype = 80 and (b.itdquan + isnull(d.itdquan,0))>0 and a.mstdate between @pFromDate and convert( varchar(10), Getdate() ,101)
 ) as PendingDispatch,  
 /*(  
	  select cast(isnull(count(m.compcode),0) as varchar)+'/'+cast((sum(m.msttota)/10000000) as varchar)+' Cr' OrdeQty
	  from /*ordeitd o  
	  inner join*/ordemst m /*on m.compcode=o.compcode and o.itdtype=m.msttype and o.itdcode=m.mstcode  */
	  /*left join company c on c.compcode=m.compcode  
	  left join employee a on a.usecode=m.mstexec  */
	  where mstType = 174 and mstDate between '2017-04-01' and convert(varchar(10), Getdate() ,101)  
 ) as OrdeQty,*/
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
    
  /*select isnull(Sum(itdQuan),0)ApprQty  
  from ordeitd  
  where itdType = 80 and itdDate between @pFromDate and convert( varchar(10), Getdate() ,101)*/  
 ) as ApprQty,
 /*0 as TotalSales,0 as TotalSaleValue,  */
   
 (select cast((sum(x.sumbala)/10000000) as varchar)+' Cr' from (select sum(trndram-trncram) sumbala, a.compcode, b.trnitem from jourmst a inner join jourtrn b on b.compcode = a.compcode and b.trncode = a.mstcode and b.trndate = a.mstdate and b.trntype = a
.msttype where a.mstdate <= getdate() group by a.compcode, b.trnitem) x inner join vewLedgerMst c on c.acctcode = x.trnitem where  c.groumain = 21 ) as TotalOutstanding,  

 (select cast((sum(x.sumbala)/10000000) as varchar)+' Cr' from (select sum(trndram-trncram) sumbala, a.compcode, b.trnitem from jourmst a inner join jourtrn b on b.compcode = a.compcode and b.trncode = a.mstcode and b.trndate = a.mstdate and b.trntype = a
.msttype where isnull(a.mstduedate, a.mstdate) <= getdate() group by a.compcode, b.trnitem) x inner join vewLedgerMst c on c.acctcode = x.trnitem where  c.groumain = 21) as TotalDue
 /*0 as TotalCollection,0 as ComplaintSolved,0 as PendingComplaint,0 as RevenFromComplains,0 as Q3Target,0 as Other,0 as AvaiStckQty,0 as ReqStckQty
 (select count(*) from tblDistributor where isnull(DistributorID,0) <> 0 and ApprovID>0)0 as Dealer,
 (select count(*) from company) Distributor  */
End  
go
GetStatistic @pFromDate='2018-04-01'