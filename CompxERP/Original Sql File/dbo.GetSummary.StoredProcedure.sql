/****** Object:  StoredProcedure [dbo].[GetSummary]    Script Date: 09/22/2018 12:46:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[GetSummary]    
@UserType int = null ,    
@UserCode int     
as    
Begin    
if @UserType = 0 or @UserType = 1    
 select (select  count(*) from Ticketent  ) TotTicket, (select  count(*) from Ticketent where Status = 1 ) ResolvTicket ,(select Count(*) from OrdeMst where MstType=1147)TotalLead  ,(select count(*) from tblFollow  )TotalFolloUp  
else    
 Begin    
  select     
  (select  count(*) from Ticketent where CreateUserID =@UserCode ) TotTicket,     
  (select  count(*) from Ticketent where Status = 1 and CreateUserID =@UserCode  ) ResolvTicket ,    
  (select count(*) from OrdeMst where MstType=1147 and mstuser =@UserCode )TotalLead   ,  
  (select count(*) from tblFollow where   f_By =@UserCode )TotalFolloUp   
 End    
End
GO
