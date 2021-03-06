/****** Object:  StoredProcedure [dbo].[getCallSummary]    Script Date: 09/22/2018 12:46:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--getCallSummary '2018/05/20' ,'2018/05/30' ,21 ,141      
CREATE Proc [dbo].[getCallSummary]            
@From Datetime,            
@To Datetime  ,        
@UseType int  ,        
@UserID int        
as            
Begin            
        
if @UseType = 20        
 begin        
  select *,8 UseType from EmpJoin a        
  inner join udfGetCallSummary (0,@From,@To) b on a.EmpID = b.UserID        
  where a.ParentID = @UserID        
 End         
Else if @UseType = 21        
 begin          
 select ParentID  ,  max(c.UserNM) UseName, isnull(sum(_25),0)_25  ,isnull(sum(_50),0) _50,isnull(sum(_75),0)_75 ,isnull(sum(_100),0)_100 ,isnull(sum(_0),0)_0 ,isnull(sum(Done),0)Done ,isnull(sum(NotUseful),0)NotUseful, isnull(sum(CallAgain),0)CallAgain ,
  
    
isnull(sum(None),0)None ,isnull(sum(Total),0)Total , 20 UseType from EmpJoin a         
 inner join udfGetCallSummary (0,@From,@To) b on a.EmpID = b.UserID        
 inner join LoginUsers c on a.ParentID = c.UseCode and Usetype = 20      
 where a.ParentID in ( select EmpID from EmpJoin where ParentID = 141 )        
 group by ParentID  ,  c.UseName      
 End         
   else       
 Begin      
  select '0' ParentID  ,  '' UseName , 0[_25]  ,0[_50] ,0 [_75] ,0 [_100] ,0 [_0]  , 0[Done] , 0[NotUseful] , 0[CallAgain]  ,0[None]  ,0 [Total]       
 End      
End
GO
