USE [Unitech1617]
GO
/****** Object:  StoredProcedure [dbo].[GetApprovedOrder]    Script Date: 09/22/2018 12:46:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--GetApprovedOrder @From ='2018-04-05' ,@To= '2018-07-06' , @SubCateID = 55                    
                    
CREATE Proc [dbo].[GetApprovedOrder]                          
@BrandID int = 0 ,  @DealerID int = 0 ,          @StateID int = 0  ,                         
@SubCateID int = 0  ,      @Executive int = 0  ,    @CompanyID   int = 0  ,              
@From Datetime = null,       @To Datetime = null                  
as                            
Begin                            
Select a.compcode ,a.MstCode ,a.msttype ,acc.acctname, a.mstdate , a.mstchno, ltrim(rtrim(c.itemname)) ddlItem, b.itdquan  , Brnd.studName trnrema ,  State.cityname Statename ,City.cityname Cityname,b.itditem ,b.itdquan Qty ,a.mstprtc ,gath.gathpuri Remark ,  
a.mstrema from  ordemst a          
inner join account acc on  a.mstprtc = acc.acctcode  -- a.compcode = acc.compcode and                              
Left join citydet State on  acc.acctstat = State.citycode and State.cityType =67                                  
Left join citydet City on  acc.acctCity = City.citycode and City.cityType =15                            
inner join ordeitd b on   a.compcode= b.compcode and a.msttype = b.itdtype and a.mstcode = b.itdcode                                 
inner join studdet Brnd on  Brnd.studType =61 and b.itdmill = Brnd.studCode                                 
inner join itemain c on c.itemcode = b.itditem --  c.compcode = b.compcode and                               
left join gathdet gath on b.itdGath = gath.gathCode                       
where a.msttype = 80                 
                    
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
