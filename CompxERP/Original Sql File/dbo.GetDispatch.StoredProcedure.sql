/****** Object:  StoredProcedure [dbo].[GetDispatch]    Script Date: 09/22/2018 12:46:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- GetDispatch @From ='2018-04-05' ,@To= '2019-07-06' , @SubCateID = 49                          
                          
CREATE Proc [dbo].[GetDispatch]                                
@BrandID varchar(100) = '' ,  @DealerID int = 0 ,          @StateID int = 0  ,                               
@SubCateID int = 0  ,      @Executive int = 0  ,    @CompanyID   int = 0  ,                    
@From Datetime = null,       @To Datetime = null                        
as                                  
Begin           
      
select b.itemname ItemNm, c.Acctname , d.studName mstbrhd, cast( isnull(abs(it.itdquan),0) as decimal(18,2))  oldwht ,ms1date mstdate,refrema mstchno,om.itdquan Qty  ,Comp.compname mstrefno  ,gath.gathpuri Remark,ct.cityname Cityname,st.cityname Statename
  
    
 ,cast(om.itdquan -  isnull(abs(it.itdquan),0) as decimal(18,2)) TotQtyFini ,mstrema     
 from refetab a              
inner join ordeitd om on a.compcode = om.compcode and a.ms2Code = om.itdCode and a.ms2Type = om.itdType and a.refItem = om.ItdItem               
inner join itpursal it on a.compcode = it.compcode and a.ms1Code = it.itdCode and a.ms1Type = it.itdType and a.refItem = it.ItdItem               
inner join ordeMst omst on om.compcode = omst.compcode and om.itdCode = omst.MstCode and om.itdType = omst.MstType                 
inner join company Comp on a.Compcode = Comp .Compcode                
inner join Itemain b on a.refItem = b.Itemcode              
inner join gathdet gath on gath.gathcode = om.itdgath          
inner join Account c on a.Ms2SrNo = c.Acctcode and c.acctgrou = 21         
left join Citydet ct on c.acctcity = ct.citycode and ct.cityType = 15        
left join Citydet st on c.acctstat = st.citycode and st.cityType = 67              
Left join studdet d on om.itdmill = d.Studcode and d.Studtype  = 61  -- b.itemnumb             
Where ms1Type = 42 and ms2Type = 80                 
                        
--and 1= case when @BrandID = 0 then 1                              
--else case when b.itemnumb = @BrandID then 1 else 0 end end                           
          
and 1= case when @BrandID = '' then 1                              
else case when om.itdMill in (Select * from SplitStringtoIntTable(@BrandID)) then 1 else 0 end end                           
 
and 1= case when @DealerID = 0 then 1                              
else case when a.Ms2SrNo = @DealerID then 1 else 0 end end                           
                            
and 1= case when @StateID = 0 then 1                              
else case when c.acctStat = @StateID then 1 else 0 end end                             
                              
and 1= case when @SubCateID = 0 then 1                              
else case when om.itdItem in (Select ItemCode from itemain where itemgrou =@SubCateID) then 1 else 0 end end                             
                              
and 1= case when @From is null then 1                              
else case when a.ms1date between @From and @To then 1 else 0 end end                          
                       
--and 1= case when @Executive =0 then 1                              
--else case when a.mstexec = @Executive then 1 else 0 end end                        
                         
and 1= case when @CompanyID =0 then 1                              
else case when a.compcode = @CompanyID then 1 else 0 end end                               
                       
order by a.ms1date ,refrema                               
                                
End
GO
