/****** Object:  View [dbo].[SALEREPORT]    Script Date: 09/22/2018 12:47:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[SALEREPORT] AS SELECT sum(mstneta) as sumsales, case when month(mstdate) < 4 then year(mstdate) - 1 else year(mstdate) end as fiyear, month(mstdate) as monthv, case when month(mstdate) < 4 then month(mstdate) + 9 else month(mstdate) - 3 end as monthsort, compcode From jourmst Where msttype = 42 Group By case when month(mstdate) < 4 then year(mstdate) - 1 else year(mstdate) end, month(mstdate), case when month(mstdate) < 4 then month(mstdate) + 9 else month(mstdate) - 3 end, compcode
GO
