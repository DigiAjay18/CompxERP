/****** Object:  View [dbo].[vewStockBal]    Script Date: 09/22/2018 12:47:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[vewStockBal] as select particular as grouname,  0 as groucode, c.studname, c.studcode, b.itemcode, b.descalia, sum(a.tramount) as tramount, sum(a.dramount) as dramount, sum(a.cramount) as cramount, sum(a.balance) as balance, convert(money, 0, 0) as diff, avg(a.chrate) as chrate, sum(a.chamou) as chamou from ((select * from stoctab where chrpcd = 'G') a inner join (select * from iteminfo where compcode = 8) b on b.itemcode=a.chacct) left outer join (select * from studdet where studtype = 61) c on c.studcode = a.chmill group by a.particular, descalia, c.studname, particular, c.studname, c.studcode, b.itemcode, b.descalia, a.particular having (sum(a.dramount) <> 0 or sum(a.cramount) <> 0 or sum(a.balance) <> 0)
GO
