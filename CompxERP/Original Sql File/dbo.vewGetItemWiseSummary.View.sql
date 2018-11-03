/****** Object:  View [dbo].[vewGetItemWiseSummary]    Script Date: 09/22/2018 12:47:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[vewGetItemWiseSummary] as SELECT itemname, count(*) as vchcount, sum(itdquan) as sumquantity, sum(itdamou) as sumamount, avg(itdrate) as avgrate, compcode, itemcomp, itditem FROM vewGetStdVouchersDet Group By itemname, compcode, itemcomp, itditem
GO
