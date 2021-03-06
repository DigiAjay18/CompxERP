/****** Object:  View [dbo].[vewGetGstHSNCodeWSumm]    Script Date: 09/22/2018 12:47:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[vewGetGstHSNCodeWSumm] as SELECT itemhsnc, count(*) as vchcount, 0 as pendingsc, 0 as sumvchvalue, sum(sumitdamou) as sumtaxable, sum(sumigstamou) as sumigstamt, sum(sumcgstamou) as sumcgstamt, sum(sumsgstamou) as sumsgstamt, compcode, supplytype, itemcomp, category, y.hrcdName as GstType, y.hrcdEnum as GstTypeEnum, isnull(mstIpOp, '') as InOutType FROM vewGetGstVouchersDet x inner join (select * from tblhardcode where hrcdType = 'GstType') y on y.hrcdNameID = x.mstappl where 0 = 0 Group By itemhsnc, compcode, supplytype, itemcomp, category, y.hrcdName, y.hrcdEnum, isnull(mstIpOp, '')
GO
