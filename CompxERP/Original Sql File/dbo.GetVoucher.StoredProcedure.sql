/****** Object:  StoredProcedure [dbo].[GetVoucher]    Script Date: 09/22/2018 12:46:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[GetVoucher]                                                
@Comp int =null      ,                                                  
@Type int =null     ,                                    
@Code int =null                                          
as                                                            
begin                                    
 if @Code is not null                                     
  begin                                     
  select trnitem tpPartyID , trnrema Remark, trndram tpDrAmt ,trncram tpCrAmt ,groumain acctgrou,c.acctname partyname ,e.acctname,f.acctname [Broker],b.trnAdju , g.compname ,G.compadd1,G.compadd3,isnull(trntagv ,0)trntagv  ,isnull(trninde,0)trninde , isnull(trnexpa,0) trnexpa,   isnull(trnaddv,0)trnaddv ,  a.* from  jourmst a       
  left join Jourtrn b on a.compcode = b.compcode  and a.mstType =b.trnType and a.mstcode = b.trncode                                      
Left join account c on b.trnitem = c.acctCode and b.compcode =c.compcode                     
Left join acgroup d on c.compcode =d.compcode and c.acctgrou = d.groucode                                 
Left join account e on a.compcode =e.compcode and a.mstprtc = e.Acctcode                              
Left join account f on a.compcode =f.compcode and a.mstbrok = f.Acctcode                                
Inner join Company G on a.compcode =G.compcode                    
where a.msttype =@Type and a.compcode= @comp and a.mstcode = @Code                                     
   End                                     
else                                    
begin                                     
select a.compcode , msttype , mstcode , mstDate ,mstTota , mstChno , mstrema ,mstclno,mstchnm ,e.acctname partyname from  jourmst a          
Left join account e on a.compcode =e.compcode and a.mstprtc = e.Acctcode                                         
where a.msttype =@Type 
                
and 1= case when @comp = 0 then 1                              
else case when a.compcode= @comp then 1 else 0 end end   

order by mstdate desc                                                       
 End                                     
 End
GO
