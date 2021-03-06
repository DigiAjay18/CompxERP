/****** Object:  StoredProcedure [dbo].[InsFollow]    Script Date: 09/22/2018 12:46:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[InsFollow]             
@MstCode Int ,            
@MstDate datetime = null,            
@F_by int,            
@L_ID int,            
@L_No varchar(100) = null ,            
@F_Date datetime = null ,            
@Status_I int = null ,            
@Status_II int = null ,            
@Remark varchar(500) = null   ,           
@isLead int =0   ,@F_Next_Date datetime = null   ,           
@F_Time int  = null   ,           
@Feedback varchar(500) = null     ,  
@UseCode int =0   
as            
Begin            
Insert into tblFollow ( MstCode   ,MstDate   ,F_by  ,L_ID  ,L_No   , F_Date   ,Status_I   ,Status_II   ,Remark ,isLead ,F_Next_Date,F_Time,Feedback ,UseCode)            
Values (@MstCode   ,@MstDate   ,@F_by  ,@L_ID  ,@L_No   , @F_Date   ,@Status_I   ,@Status_II   ,@Remark  ,@isLead,@F_Next_Date,@F_Time,@Feedback ,@UseCode)             
End
GO
