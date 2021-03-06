/****** Object:  StoredProcedure [dbo].[insGathDet]    Script Date: 09/22/2018 12:46:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[insGathDet]                   
@gathcode varchar (15)  ,                  
@gathdesc varchar (1000) = null ,                  
@gathstat varchar (50) = null ,                  
@gathwtdf decimal(18,2) = null ,                  
@gathwtst decimal(18,2) = null ,                  
@gathwtnt decimal(18,2) = null ,                  
@gathwast varchar (50) = null ,                  
@gathpuri varchar (50) = null ,                  
@gathqtdi decimal(18,2) = null ,                  
@gathwtdi decimal(18,2) = null  ,            
@itdtype int = null    ,        
@gathlabp  decimal(18,3) = null ,    
@gathdscv decimal(18,3) = null           
as                  
Begin                  
insert into GathDet                   
(gathcode ,gathdesc ,gathstat ,gathwtdf ,gathwtst ,                  
gathwtnt ,gathwast ,gathpuri ,gathqtdi ,gathwtdi ,itdtype ,gathlabp ,gathdscv) Values                   
(@gathcode ,@gathdesc ,@gathstat ,@gathwtdf ,@gathwtst ,                  
@gathwtnt ,@gathwast ,@gathpuri ,@gathqtdi ,@gathwtdi ,@itdtype ,@gathlabp ,@gathdscv)                  
end
GO
