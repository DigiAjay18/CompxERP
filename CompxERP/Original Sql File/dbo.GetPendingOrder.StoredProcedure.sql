/****** Object:  StoredProcedure [dbo].[GetPendingOrder]    Script Date: 09/22/2018 12:46:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--GetPendingOrder @From ='2018-07-05' ,@To= '2018-07-06' , @SubCateID = 55                        
                         
CREATE Proc [dbo].[GetPendingOrder]                              
@BrandID int = 0 ,       @DealerID int = 0 ,        @StateID int = 0  ,                             
@SubCateID int = 0  ,      @Executive int = 0  ,    @CompanyID   int = 0      ,                    
@From Datetime = null,    @To Datetime = null                        
as                                
Begin                                
 Select a.compcode , a.MstCode ,a.msttype ,acc.acctname, a.mstdate , a.mstchno, ltrim(rtrim(c.itemname)) ddlItem, itdquan = (b.itdquan - isnull(g.itdquan ,0)), Brnd.studName trnrema,  State.cityname Statename ,City.cityname Cityname,b.itditem ,b.itdquan Qty,g.itdquan v ,b.itdactprc Rate ,b.itdrema Remark ,b.itdrema  ,cast(b.itdamou as decimal(16,2)) msttota ,cast(b.itddscp as decimal(16,2)) DisPer ,a.mstrema from  ordemst a                                
inner join account acc on  a.mstprtc = acc.acctcode  --and  a.compcode = acc.compcode                              
Left join citydet State on  acc.acctstat = State.citycode and State.cityType =67                                  
Left join citydet City on  acc.acctCity = City.citycode and City.cityType =15                            
inner join ordeitd b on   a.compcode= b.compcode and a.msttype = b.itdtype and a.mstcode = b.itdcode                                 
left join studdet Brnd on  Brnd.studType =61 and b.itdmill = Brnd.studCode                                 
inner join itemain c on   c.itemcode = b.itditem      --and     c.compcode = b.compcode                      
Left join (Select OM.CompCode ,itdType ,Sum(itdQuan)itdQuan ,itdItem ,MstChno from ordemst OM Left join ordeitd OI on om.compcode = om.compcode and om.msttype = OI.ItdType and om.mstCode = OI.ItdCode where msttype = 80 group by OM.CompCode ,itdType,itdItem ,MstChno) g                                
--Left join (Select CompCode ,itdType,itdCode ,Sum(itdQuan)itdQuan ,itdItem from ordeitd where itdType = 80 group by CompCode ,itdType,itdCode,itdItem) g                           
on   a.compcode = g.compcode  and a.MstChno = g.MstChno and  b.itditem = g.itditem                            
where a.msttype = 174 and (b.itdquan - isnull(g.itdquan ,0)) > 0                                
                        
and 1= case when @BrandID = 0 then 1                            
else case when b.itdMill = @BrandID then 1 else 0 end end                         
                            
and 1= case when @DealerID = 0 then 1                            
else case when a.mstprtc = @DealerID then 1 else 0 end end                         
                          
and 1= case when @StateID = 0 then 1                            
else case when acc.acctStat = @StateID then 1 else 0 end end                           
                            
and 1= case when @SubCateID = 0 then 1                            
else case when b.itdItem in (Select ItemCode from itemain where itemgrou =@SubCateID) then 1 else 0 end end                           
                            
and 1= case when @From is null then 1                            
else case when a.mstDate between @From and @To then 1 else 0 end end                           
                        
and 1= case when @Executive =0 then 1                              
else case when a.mstexec = @Executive then 1 else 0 end end                        
                         
and 1= case when @CompanyID =0 then 1                              
else case when a.compcode = @CompanyID then 1 else 0 end end                       
                    
order by acc.acctName                            
                              
End
GO
