/****** Object:  StoredProcedure [dbo].[tblData]    Script Date: 09/22/2018 12:47:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create proc [dbo].[tblData]                       
@Name varchar (100)                      
as                      
Declare @sQry varchar(500)
set @sQry = 'select * from ' +  @Name    

exec (@sQry)
GO
