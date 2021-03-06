/****** Object:  StoredProcedure [dbo].[GetInquiry]    Script Date: 09/22/2018 12:46:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[GetInquiry]                                             
@Comp int =null      ,                                                    
@Type int =null     ,                                      
@Code int =null      ,                                      
@UserCode int =null                                          
as                                                              
begin                                       
 if @Code is not null                                       
  begin                                       
  --select top 1 Itditem tpPartyID , Itdrema Remark, trndram tpDrAmt ,trncram tpCrAmt ,groumain acctgrou,c.acctname partyname ,e.acctname,f.acctname Broker1  , g.compname ,G.compadd1,G.compadd3,trntagv ,h.acctname Broker,f.acctname mstconnm,  a.* from  Ordemst a       
   select b.acctname partyname ,a.* from  Ordemst a   
left join account b on a.mstcust = b.acctCode and b.acctgrou  = 21  
--  left join OrdeItd b on a.compcode = b.compcode  and a.mstType =b.ItdType and a.mstcode = b.Itdcode                                        
--  left join Ordetrn trn on a.compcode = trn.compcode  and a.mstType =trn.trnType and a.mstcode = trn.trncode                                        
-- inner join account c on b.Itditem = c.acctCode and b.compcode =c.compcode                       
--Left join acgroup d on c.compcode =d.compcode and c.acctgrou = d.groucode                                   
--Left join account e on a.compcode =e.compcode and a.MstCust = e.Acctcode                                
--Left join account f on a.compcode =f.compcode and a.mstempo = f.Acctcode                               
--Left join account h on a.compcode =h.compcode and a.mstexec = h.Acctcode                                
--Inner join Company G on a.compcode =G.compcode                      
where a.msttype =@Type and a.compcode= @comp and a.mstcode = @Code   -- and a.mstuser = @UserCode                                     
   End                                       
else                                      
begin                                       
select compcode , msttype , mstcode , mstDate ,mstTota , mstChno , mstrema ,mstclno,mstchnm from  Ordemst a                                           
where a.msttype =@Type and a.compcode= @comp and a.mstuser = @UserCode order by mstdate desc                                                         
 End                                       
 End
GO
