/****** Object:  StoredProcedure [dbo].[GetSubCategoryInfo]    Script Date: 09/22/2018 12:46:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[GetSubCategoryInfo] (@Name varchar(100),@CompCode int,@CatCode int)    
as    
begin    
 select itgpcode as Code,itgpname as Name from itgroup    
 where compcode=@CompCode and itgpname like '%'+@Name+'%' and itgpunde=@CatCode    
 order by itgpname    
end
GO
