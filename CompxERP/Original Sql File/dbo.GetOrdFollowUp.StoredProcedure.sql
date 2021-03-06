/****** Object:  StoredProcedure [dbo].[GetOrdFollowUp]    Script Date: 09/22/2018 12:46:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- GetOrdFollowUp  @DealerID = 0 ,        @StateID = 0  , @Executive = 33   , @CityID = 0     
CREATE Proc [dbo].[GetOrdFollowUp]    
@DealerID int = 0 ,        @StateID int = 0  , @Executive int = 0   , @CityID int = 0   , @From Datetime = null,    @To Datetime = null         
as    
Begin      
    
Select a.acctname ,a.acctCode ,a.acctphon,a.acctAddr, b.*   from Account a      
left join (    
Select a_.* from tblOrdFollowUp a_    
inner join (Select max(FDate)FDate, DealerID from tblOrdFollowUp group by DealerID ) b_ on a_.DealerID = b_.DealerID and a_. FDate = b_.FDate  where isnull(a_.Status ,0) =0) b on a.acctcode = b.DealerID     
where acctgrou= 21    
     
and 1= case when @DealerID = 0 then 1                                
else case when a.acctcode = @DealerID then 1 else 0 end end                             
                              
and 1= case when @StateID = 0 then 1                                
else case when a.acctStat = @StateID then 1 else 0 end end                               
                             
and 1= case when @Executive =0 then 1                                  
else case when a.acctDeal in (select DealerID from employee Emp inner join empAllotMst aEmp on Emp.Empcode = aEmp.EmpID  where Emp.useCode = @Executive ) then 1 else 0 end end                            
                                
and 1= case when @CityID =0 then 1                                  
else case when a.acctCity = @CityID then 1 else 0 end end                            
      
and 1= case when @From is null then 1                                
else case when cast(b.NextDt as date) between @From and @To then 1 else 0 end end                               
                                         
order by a.acctName           
    
end
GO
