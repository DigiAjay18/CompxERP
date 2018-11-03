/****** Object:  View [dbo].[vewLedgerMst]    Script Date: 09/22/2018 12:47:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[vewLedgerMst] as select cexi.compcode cexicomp, pacT.*, acctdesc = case when isnull(pciT.cityname,'')='' then pacT.acctname else pacT.acctname + ' , ' + pciT.cityname end, pciT.cityname, pciT.citycode, pgrT.groumain, pgrT.grouunde from account pacT left outer join vewCityMaster pciT on pciT.citycode = acctcity inner join acgroup pgrT on pgrT.compcode = pacT.compcode and pgrT.groucode = pacT.acctgrou inner join compexi cexi on cexi.compunde = pacT.compcode
GO


