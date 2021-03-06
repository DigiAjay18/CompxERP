/****** Object:  StoredProcedure [dbo].[getProduction]    Script Date: 09/22/2018 12:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[getProduction]     
@IsProduction bit =0  
as    
begin    
 if (@IsProduction = 0)  
 Begin  
  select  a.MstCode,a.msttype,a.MstDate, a.mstrefc ,a.mstrefno ,a.mstchno ,a.mstFinc , d.itemname ItemNm,itdQuan Qty  from OrdeMst a    
  inner join Ordeitd c on a.CompCode = c.CompCode and  c.itdtype = a.Msttype and a.mstCode = c.itdCode  
  inner Join Itemain d on c.Compcode = d.Compcode and c.itdItem = itemCode   
  left join tblOrderApprove b on a.CompCode = b.CompCode and  b.msttype = a.Msttype and a.mstCode = b.OrderID     
  where a.msttype = 174 and b.OrderID is null     
 End    
 else  
 Begin  
  select  a.MstCode,a.msttype,a.MstDate, a.mstrefc ,a.mstrefno ,a.mstchno , a.mstContactPerson ,a.mstTota ,a.mstFinc ,     
  case when a.mstFinc =1 then 0 else (select top 1 Opening   from tblOpening where DealerID = a.mstrefc order by  CreatedOn desc)   
  End msttopay  from OrdeMst a    
  left join tblOrderApprove b on a.CompCode = b.CompCode and  b.msttype = a.Msttype and a.mstCode = b.OrderID     
  where a.msttype = 174 and b.OrderID is null     
 End    
End
GO
