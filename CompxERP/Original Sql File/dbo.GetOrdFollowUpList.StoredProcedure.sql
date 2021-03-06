/****** Object:  StoredProcedure [dbo].[GetOrdFollowUpList]    Script Date: 09/22/2018 12:46:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- GetOrdFollowUp  @DealerID = 0 ,        @StateID = 0  , @Executive = 33   , @CityID = 0     
CREATE Proc [dbo].[GetOrdFollowUpList]    
@DealerID int = 0 ,        @StateID int = 0  , @Executive int = 0   , @CityID int = 0   , @From Datetime = null,    @To Datetime = null         
as    
Begin      
    
Select b.acctname ,b.acctCode ,acctphon, a.* from tblOrdFollowUp a     
inner join Account   b on a.DealerID = b.acctCode      
where acctgrou= 21    
     
and 1= case when @DealerID = 0 then 1                                
else case when b.acctcode = @DealerID then 1 else 0 end end                             
                              
and 1= case when @StateID = 0 then 1                                
else case when b.acctStat = @StateID then 1 else 0 end end                               
                             
and 1= case when @Executive =0 then 1                                  
else case when b.acctDeal in (select DealerID from employee Emp inner join empAllotMst aEmp on Emp.Empcode = aEmp.EmpID  where Emp.useCode = @Executive ) then 1 else 0 end end                            
                                
and 1= case when @CityID =0 then 1                                  
else case when b.acctCity = @CityID then 1 else 0 end end                            
      
and 1= case when @From is null then 1                                
else case when  cast (a.FDate as date) between @From  and @To then 1 else 0 end end                               
                                         
order by b.acctName           
    
end
GO
