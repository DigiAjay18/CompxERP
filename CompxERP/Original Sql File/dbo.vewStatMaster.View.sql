/****** Object:  View [dbo].[vewStatMaster]    Script Date: 09/22/2018 12:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[vewStatMaster] as select cityname as statname, * from citydet where citytype = 67
GO
