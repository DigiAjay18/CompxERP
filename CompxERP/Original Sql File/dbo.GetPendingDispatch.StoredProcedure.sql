/****** Object:  StoredProcedure [dbo].[GetPendingDispatch]    Script Date: 09/22/2018 12:46:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- GetPendingDispatch @From ='2018-04-05' ,@To= '2019-07-06' , @SubCateID = 49                                  
                              
CREATE Proc GetPendingDispatch                                            
@BrandID varchar(100) = '' ,  @DealerID int = 0 ,          @StateID int = 0  ,                                           
@SubCateID int = 0  ,      @Executive int = 0  ,    @CompanyID   int = 0  ,                                
@From Datetime = null,       @To Datetime = null                                    
as                                              
Begin                                              
Select itm.itemname ItemNm,acc.Acctname ,a.mstchno,b.itdquan Qty, (b.itdquan - abs(isnull(cc.itdquan,0) ))oldwht,brnd.studName mstbrhd ,a.mstdate ,Comp.compname mstrefno ,gath.gathpuri Remark ,ct.cityname Cityname,st.cityname Statename  , cast((b.itdquan -  
 abs(isnull(cc.itdquan,0) )) as decimal(18,2)) TotQtyFini , a.mstrema from OrdeMst a            
inner join ordeItd b on a.compcode = b.compcode and a.msttype =b.itdtype and a.mstcode = b.itdcode                         
inner join Account acc on a.Mstcust = acc.Acctcode and acc.acctgrou = 21                        
left join Citydet ct on acc.acctcity = ct.citycode and ct.cityType = 15              
left join Citydet st on acc.acctstat = st.citycode and st.cityType = 67              
inner join Itemain itm on b.itdItem = itm.Itemcode                          
inner join company Comp on a.Compcode = Comp .Compcode                          
inner join gathdet gath on gath.gathcode = b.itdgath                
Left join studdet brnd on b.itdmill = brnd.Studcode and brnd.Studtype  = 61   --  itm.itemnumb                       
----left join refetab c on b.itdtype = c.ms2type and itdcode = ms2code                        
--left join refetab c on a.compcode = c.compcode and b.itdtype = c.ms2type and itdcode = ms2code and a.mstchno = c.refRema  and  b.itditem = c.refitem           
--left join itpursal d on c.compcode = d.compcode and c.ms1type =d.itdtype and c.ms1code = d.itdcode     and c.refitem = d.itditem                     
  
Left join (SELECT sum(isnull(d.itdquan,0) ) itdquan,   c.compcode , c.ms2type , ms2code , c.refRema , c.refitem  from refeTab c  
left join  itpursal d on c.compcode = d.compcode and c.ms1type =d.itdtype and c.ms1code = d.itdcode     and c.refitem = d.itditem      
group by c.compcode , c.ms2type , ms2code , c.refRema , c.refitem )cc  on a.compcode = cc.compcode and b.itdtype = cc.ms2type and itdcode = cc.ms2code and a.mstchno = cc.refRema  and  b.itditem = cc.refitem   
  
where msttype = 80 and (b.itdquan + isnull(cc.itdquan,0))>0 --and c.refrema is null                            
                                      
--and 1= case when @BrandID = 0 then 1                                          
--else case when itm.itemnumb = @BrandID then 1 else 0 end end                                       
                         
and 1= case when @BrandID = '' then 1                                        
else case when b.itdMill in (Select * from SplitStringtoIntTable(@BrandID)) then 1 else 0 end end                                     
                                           
and 1= case when @DealerID = 0 then 1                                          
else case when a.Mstcust = @DealerID then 1 else 0 end end                                       
                                        
and 1= case when @StateID = 0 then 1                                          
else case when acc.acctStat = @StateID then 1 else 0 end end                                         
                                          
and 1= case when @SubCateID = 0 then 1                                          
else case when b.itdItem in (Select ItemCode from itemain where itemgrou =@SubCateID) then 1 else 0 end end                                         
                                          
and 1= case when @From is null then 1                      
else case when a.mstdate between @From and @To then 1 else 0 end end                                      
                                   
--and 1= case when @Executive =0 then 1                                          
--else case when a.mstexec = @Executive then 1 else 0 end end                                    
                                     
and 1= case when @CompanyID =0 then 1                                          
else case when a.compcode = @CompanyID then 1 else 0 end end                                           
          
order by acc.Acctname --a.mstdate ,a.mstchno          
                                            
End 
GO
