/****** Object:  UserDefinedFunction [dbo].[udfGetCallSummary]    Script Date: 09/22/2018 12:47:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[udfGetCallSummary](@UserID int ,@From Datetime, @To Datetime )        
returns Table      
as       
return       
(       
select empName UseName,UserID ,        
(select isnull(count(*),0) Total from tblCallDet a where Ratio = 25 and a.UserID = CD.UserID and a.CreatedOn between @From and @To group by UserID ,Ratio   )'_25',        
(select isnull(count(*),0) Total from tblCallDet a where Ratio = 50 and a.UserID = CD.UserID and a.CreatedOn between @From and @To  group by UserID ,Ratio )'_50' ,        
(select isnull(count(*),0) Total from tblCallDet a where Ratio = 75 and a.UserID = CD.UserID  and a.CreatedOn between @From and @To group by UserID ,Ratio )'_75' ,        
(select isnull(count(*),0) Total from tblCallDet a where Ratio = 100 and a.UserID = CD.UserID and a.CreatedOn between @From and @To  group by UserID ,Ratio )'_100' ,        
(select isnull(count(*),0) Total from tblCallDet a where isnull(a.Ratio , 0) = 0 and a.UserID = CD.UserID and a.CreatedOn between @From and @To  group by UserID ,isnull(a.Ratio , 0)  )'_0' ,         
(select isnull(count(*),0) Total from tblCallDet a where status = 2 and a.UserID = CD.UserID and a.CreatedOn between @From and @To  group by UserID ,status )'Done',        
(select isnull(count(*),0) Total from tblCallDet a where status = 3 and a.UserID = CD.UserID and a.CreatedOn between @From and @To  group by UserID ,status )'NotUseful',        
(select isnull(count(*),0) Total from tblCallDet a where status = 4 and a.UserID = CD.UserID and a.CreatedOn between @From and @To  group by UserID ,status )'CallAgain',        
(select isnull(count(*),0) Total from tblCallDet a where isnull(a.status , 0) = 0 and a.UserID = CD.UserID and a.CreatedOn between @From and @To  group by UserID ,isnull(a.status , 0)  )'None' ,        
(select isnull(count(*),0) Total from tblCallDet a where a.UserID = CD.UserID and a.CreatedOn between @From and @To  group by UserID )'Total'         
from tblCallDet CD Left join Employee b on CD.UserID = b.UseCode         
where CD.CreatedOn between @From and @To        
group by UserID ,empName        
       
)
GO
