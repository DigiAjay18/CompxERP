/****** Object:  StoredProcedure [dbo].[InsEmpAllotMst]    Script Date: 09/22/2018 12:46:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[InsEmpAllotMst]      
@EmpID int ,    
@DealerID int     
as    
Begin    
Declare @Code int    
 if not exists (select * from EmpAllotMst where EmpID = @EmpID and DealerID =@DealerID)  
 Begin  
 set @Code = (select isNull(mAx(MstCode),0) +1 from EmpAllotMst)    
  Insert into EmpAllotMst (MstCode ,EmpID , DealerID ) values ( @Code ,@EmpID , @DealerID )    
 End    
End
GO
