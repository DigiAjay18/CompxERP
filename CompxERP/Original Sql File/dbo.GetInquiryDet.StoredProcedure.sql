/****** Object:  StoredProcedure [dbo].[GetInquiryDet]    Script Date: 09/22/2018 12:46:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[GetInquiryDet]  --60 ,1147 ,7                              
@Comp int =null      ,                                                                    
@Type int =null     ,                                                      
@Code int =null                                                            
as                                                                              
begin              
-- itdperd DisPer ,itdqtyd DisAmt   ( 300818 itdperd DisAmt )                     
Select a.itditem   , b.itemname ddlItem ,Abs(a.itdquan) itdquan , itdrate  , a.itdrema Remark , a.itdunit UnitID , c.unitname unitname ,  a.itdrema  ,         
d.itgpName CateNm  ,e.itgpName SubCateNm , CONVERT(VARCHAR(10),itdPODt ,103)  Sitdexpd  ,a.itdmill ,a.itdgodo,a.itdunit ,itdigstrt  ,itdgstrtv TaxPer,itdigstvl Tax_Amt, itddscp DisPer,gath.gathlabp DisAmt,itdvate,itdamou ItemNetAmt,itdquan  ,itdlabamt Amt
 ,mstContactPerson ,mstpofs ,msternv,mstfinc,mstrefno,mstrefc ,a.itdactprc         
from   ordeItd a                      
inner join OrdeMst OM on a.Compcode = OM.Compcode and a.itdType = OM.MstType and a.itdCode = OM.MstCode   
left join Gathdet gath on a.itdGath = gath.Gathcode                          
inner join itemain b on  a.itditem  = b.itemcode --and a.compcode = b.compcode                               
left join unitdet c on a.compcode =c.compcode and a.itdunit = c.unitcode                             
left join itGroup d on a.compcode =d.compcode and a.itdmill = d.itgpcode and d.itgpunde = 0                           
left join itGroup e on a.compcode =e.compcode and a.itdgodo = e.itgpcode and e.itgpunde <> 0                            
                              
where a.compcode = @Comp and a.itdtype = @Type ANd a.itdcode = @Code                               
End
GO
