/****** Object:  View [dbo].[SALEREPORTITEMS]    Script Date: 09/22/2018 12:47:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[SALEREPORTITEMS] AS SELECT sum(abs(itdpkin)) as sumbdls, sum(itdquan) as sumquan, case when month(mstdate) < 4 then year(mstdate) - 1 else year(mstdate) end as fiyear, month(mstdate) as monthv, case when month(mstdate) < 4 then month(mstdate) + 9 else month(mstdate) - 3 end as monthsort, a.compcode, itditem, ign1, ign2, igc1, igc2, itemdesc FROM jourmst a inner join itpursal b on b.compcode = a.compcode and b.itdtype = a.msttype and b.itdcode = a.mstcode and b.itddate = a.mstdate inner join iteminfo c on c.itemcode = b.itditem and c.compcode = b.compcode Where msttype = 42 Group By case when month(mstdate) < 4 then year(mstdate) - 1 else year(mstdate) end, month(mstdate), case when month(mstdate) < 4 then month(mstdate) + 9 else month(mstdate) - 3 end, a.compcode, itditem, ign1, ign2, igc1, igc2, itemdesc
GO
