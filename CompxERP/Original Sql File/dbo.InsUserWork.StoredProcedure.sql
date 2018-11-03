/****** Object:  StoredProcedure [dbo].[InsUserWork]    Script Date: 09/22/2018 12:47:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[InsUserWork]              
@worsrno varchar(15) = null ,              
@woruser int = null ,              
@wormode tinyint = null ,              
@worcomp int = null ,              
@wordate smalldatetime = null ,              
@wortype int = null ,              
@worcode int = null ,              
@worrema varchar(150) = null ,              
@worcudt datetime = null ,              
@worrfsr varchar(15) = null ,              
@worsysn varchar(50) = null ,    
@IP_Add Varchar(100) = null , @WorChNo Varchar(100) = null , @WorNarr Varchar(500) = null     
as            
Begin              
 INSERT INTO [dbo].[userworkin]               
   ([worsrno],[woruser] ,[wormode] ,[worcomp] ,[wordate] ,[wortype] ,[worcode]              
           ,[worrema] ,[worcudt] ,[worrfsr] ,[worsysn] ,IP_Add , WorChNo , WorNarr )              
     VALUES              
   ( @worsrno,@woruser ,@wormode ,@worcomp ,@wordate ,@wortype ,@worcode              
           ,@worrema ,@worcudt ,@worrfsr ,@worsysn ,@IP_Add , @WorChNo , @WorNarr )              
END
GO
