/****** Object:  View [dbo].[vewGetGstVouchersMst]    Script Date: 09/22/2018 12:47:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[vewGetGstVouchersMst] as SELECT partyname, mstneta as mstneta, sum(sumitdamou) as sumitdamou, sum(sumigstamou) as sumigstamou, sum(sumcgstamou) as sumcgstamou, sum(sumsgstamou) as sumsgstamou, compcode, mstprtc, mstdate, msttype, mstsale, acctgrou, groumain, mstsite, mstbrnc, mstbuyerc, acctcomp, mstchno, mstcode, y.hrcdName as GstType, y.hrcdEnum as GstTypeEnum, isnull(mstIpOp, '') as InOutType, mstchnh, mstchnv FROM vewGetGstVouchersDet x inner join (select * from tblhardcode where hrcdType = 'GstType') y on y.hrcdNameID = x.mstappl where 0 = 0 group by partyname, mstneta, compcode, mstprtc, mstdate, msttype, mstsale, acctgrou, groumain, mstsite, mstbrnc, mstbuyerc, mstchno, mstcode, acctcomp, y.hrcdName, y.hrcdEnum, isnull(mstIpOp, ''), mstchnh, mstchnv
GO
