/****** Object:  StoredProcedure [dbo].[tbl]    Script Date: 09/22/2018 12:47:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[tbl]                   
@Name varchar (100)                  
as                  
select Name from sysobjects where name like '%' + @Name + '%'
GO
