/****** Object:  UserDefinedFunction [dbo].[udfGetTran1Details]    Script Date: 09/22/2018 12:47:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[udfGetTran1Details] (@pCompanyCodeV int, @pAcctCompCd int) RETURNS table AS Return (select ROW_NUMBER() OVER(ORDER BY itemhsnc) AS tmp7aRank, itemhsnc as tmp7aHSNc, unitname as tmp7aUnit, sum(itdquan) as tmp7aBalQ, 0 as tmp7aValu FROM jourmst a inner join itpursal b on b.compcode = a.compcode and b.itdtype = a.msttype and b.itdcode = a.mstcode and b.itddate = a.mstdate inner join iteminfo itMn on b.itditem = itMn.itemcode group by itemhsnc, unitname)
GO
