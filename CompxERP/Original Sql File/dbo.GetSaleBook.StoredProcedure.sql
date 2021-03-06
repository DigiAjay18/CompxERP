/****** Object:  StoredProcedure [dbo].[GetSaleBook]    Script Date: 09/22/2018 12:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[GetSaleBook]            
@Compcode int,        
@From datetime ,            
@TO datetime ,            
@PartyID varchar(500) =null,            
@TypeID varchar(500) =42              
as            
Begin            
SELECT  tp.Acctname ,partyname, mstneta as mstneta, sum(sumitdamou) as sumitdamou, sum(sumigstamou) as sumigstamou, sum(sumcgstamou) as sumcgstamou, sum(sumsgstamou) as sumsgstamou, x.compcode,   mstdate, msttype, mstsale,    mstbrnc, mstbuyerc, x.acctcomp      
        
, mstchno, mstcode,   mstchnh, mstchnv FROM vewGetGstVouchersDet x             
inner join account tp on tp.AcctCode = x.mstsale            
Where x.acctcomp = @Compcode and salecomp = @Compcode and msttype =@TypeID and x.compcode = @Compcode  and  datediff(d, mstdate, @From) <= 0  and datediff(d, mstdate, @TO) >= 0  and tp.Compcode = @Compcode              
group by tp.Acctname , partyname,x.compcode, msttype,mstneta,mstdate ,mstsale, mstbrnc, mstbuyerc, x.acctcomp, mstchno, mstcode, acctcomp, mstchnh, mstchnv           
order by tp.Acctname ,mstDate,partyname            
            
End
GO
