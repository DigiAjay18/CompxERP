/****** Object:  View [dbo].[vewCityMaster]    Script Date: 09/22/2018 12:47:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[vewCityMaster] as select * from citydet where citytype = 15
GO
