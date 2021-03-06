/****** Object:  UserDefinedFunction [dbo].[getOrderValue]    Script Date: 09/22/2018 12:47:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter function [dbo].[getOrderValue] (@Compcode int  )  
returns table  
as return (  
	Select   isnull(bb.itdquan,0) * isnull(bb.itdactprc,0) - isnull(g.itdAmou,0)    OrdValue ,mstcust ,aa.compcode ,aa.mstchno ,aa.mstdate from  ordemst aa  
	inner join ordeitd bb on   aa.compcode= bb.compcode and aa.msttype = bb.itdtype and aa.mstcode = bb.itdcode and aa.mstdate = bb.itddate  
	Left join (  
	Select OM.CompCode ,OI.itdType ,Sum(abs(IT.itdquan))itdQuan ,OI.itdItem ,(Sum(abs(IT.itdquan))* OI.itdactprc) itdAmou,rf.refRema MstChno from ordemst OM   
	Inner join ordeitd OI on om.compcode = om.compcode and om.msttype = OI.ItdType and om.mstCode = OI.ItdCode   and om.mstdate = oi.itddate
	left join refetab RF on OM.Compcode = rf.Compcode  and rf.Ms1Type = 42 and ms2type = OM.msttype and OM.mstchno = rf.refRema  and rf.ms2srno = OM.mstprtc and rf.refItem = OI.itdItem   and rf.ms2date = om.mstdate
	inner join itpursal IT on RF.compcode = IT.compcode and RF.ms1Type = IT.itdtype and RF.ms1Code = IT.itdCode and RF.refItem = IT.itdItem and rf.ms1date = it.itddate 
	where msttype = 80 group by OM.CompCode ,OI.itdType,OI.itdItem ,rf.refRema,OI.itdactprc ) g      
	on   aa.compcode = g.compcode  and aa.MstChno = g.MstChno and  bb.itditem = g.itditem                                                
	where aa.msttype = 174  and aa.compcode = isnull(@Compcode, aa.compcode)    and (bb.itdquan - isnull(g.itdquan ,0)) > 0 --and  g.mstchno  is null    
 )
GO
