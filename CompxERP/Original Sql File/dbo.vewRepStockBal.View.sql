/****** Object:  View [dbo].[vewRepStockBal]    Script Date: 09/22/2018 12:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[vewRepStockBal] as select '' as grouname, 0 as groucode, '' as type, 0 as studcode, 0 as itemcode, '' descalia, convert(money, 0, 0) as tramount, convert(money, 0, 0) as dramount, convert(money, 0, 0) as cramount, convert(money, 0, 0) as balance, convert(money, 0, 0) as diff, convert(money, 0, 0) as rate, convert(money, 0, 0) as value
GO
