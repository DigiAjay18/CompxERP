/****** Object:  StoredProcedure [dbo].[spEmployeeLogin]    Script Date: 09/22/2018 12:47:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spEmployeeLogin] (                                        
 @compcode int       = NULL                           
,@usecode int               = NULL                
,@usetype int                      = NULL     
,@useshowtr int                           = NULL            
,@usename VARCHAR(50) = NULL                                    
,@usepass VARCHAR(50) = NULL                       
,@useref int       = NULL   
      
,@UsePhone varchar(50) = NULL                                    
,@UseEmail varchar(250) = NULL                                    
,@UseDesi Int= NULL                                    
,@UseDepa Int= NULL                                    
,@UseDealer Int= NULL         
      
,@UserNM varchar(50) = NULL      
,@Remark varchar(250)= NULL      
                                  
 )                                        
AS                                        
 begin                                   
            
 INSERT INTO [loginusers] (compcode,usecode,useshowtr,usename,usepass,usetype,useref ,usecrdt ,UsePhone  ,UseEmail  ,UseDesi ,UseDepa ,UseDealer ,UserNM ,Remark)                                        
 VALUES (@compcode ,@usecode,@useshowtr,@usename,dbo.encrypt(@usepass),@usetype,@useref ,GetDate() ,@UsePhone  ,@UseEmail  ,@UseDesi ,@UseDepa ,@UseDealer,@UserNM ,@Remark)                                   
             
insert into usermenust            
select Mencode ,@usecode , 1,1,1,1,0 from Menuoption            
             
END
GO
