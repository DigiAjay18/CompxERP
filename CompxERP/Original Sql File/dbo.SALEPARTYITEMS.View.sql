/****** Object:  View [dbo].[SALEPARTYITEMS]    Script Date: 09/22/2018 12:47:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[SALEPARTYITEMS] AS SELECT sum(abs(itdpkin)) as sumbdls, sum(itdquan) as sumquan, case when month(mstdate) < 4 then year(mstdate) - 1 else year(mstdate) end as fiyear, month(mstdate) as monthv, case when month(mstdate) < 4 then month(mstdate) + 9 else month(mstdate) - 3 end as monthsort, a.compcode, itditem, ign1, ign2, igc1, igc2, itemdesc, acctcode as partycode, itdmill, studname as millname, acctname as partyname, cityname FROM jourmst a inner join itpursal b on b.compcode = a.compcode and b.itdtype = a.msttype and b.itdcode = a.mstcode and b.itddate = a.mstdate inner join iteminfo c on c.itemcode = b.itditem inner join account d on d.acctcode = convert(int, left(mstrefc, 6), 0) left outer join vewBrandQuality e on e.studcode = b.itdmill left outer join vewCityMaster f on f.citycode = d.acctcity Where msttype = 42 Group By case when month(mstdate) < 4 then year(mstdate) - 1 else year(mstdate) end, month(mstdate), case when month(mstdate) < 4 then month(mstdate) + 9 else month(mstdate) - 3 end, a.compcode, itditem, ign1, ign2, igc1, igc2, itemdesc, acctcode, itdmill, studname, acctname, cityname
GO
