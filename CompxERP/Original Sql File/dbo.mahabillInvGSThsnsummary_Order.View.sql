/****** Object:  View [dbo].[mahabillInvGSThsnsummary_Order]    Script Date: 09/22/2018 12:47:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[mahabillInvGSThsnsummary_Order] AS      
SELECT a.msttype AS Expr3, a.mstcode AS Expr4, a.mstdate AS Expr5, sum(b.itdquan) AS sumquan, sum(b.itdamou) AS sumamout, a.compcode AS Expr6, itmN.compcode AS itemcom, a.mstchno AS refnum, sum(b.itdpkin) AS    
 sumcase, cmpN.compname, itmN.itemhsnc as itemhsnc, isnull(itdgstrtv, 0) as itdgstrtv, isnull(itdcgstrt, 0) as itdcgstrt, sum(isnull(b.itdcgstvl,0)) as sumcgstvl, isnull(itdsgstrt, 0) as itdsgstrt, sum(isnull(b.itdsgstvl, 0)) as sumsgstvl, isnull(itdigstrt, 0) as itdigstrt, sum(isnull(b.itdigstvl, 0)) as sumigstvl, sum(isnull(b.itdcgstvl,0) + isnull(b.itdsgstvl, 0) + isnull(b.itdigstvl, 0)) as sumgstvl, sum(isnull(b.itdperd, 0)) as sumperd, sum(isnull(b.itdqtyd, 0)) as sumqtyd FROM dbo.ordemst a     
INNER JOIN dbo.company cmpN ON cmpN.compcode = a.compcode     
INNER JOIN OrdeItd b ON b.compcode = a.compcode AND b.itdcode = a.mstcode AND b.itddate = a.mstdate AND b.itdtype = a.msttype     
INNER JOIN (SELECT * From iteminfo WHERE compcode <> 0) itmN ON b.compcode = itmN.compcode AND itmN.itemcode = b.itditem     
GROUP BY a.msttype, a.mstcode, a.mstdate, a.compcode, itmN.compcode, a.mstchno, cmpN.compname, itmN.itemhsnc, itdgstrtv, itdcgstrt, itdsgstrt, itdigstrt
GO
