/****** Object:  View [dbo].[vewPlacMaster]    Script Date: 09/22/2018 12:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[vewPlacMaster] as select cityname as statname, * from citydet where citytype = 70
GO
