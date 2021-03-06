/****** Object:  View [dbo].[vewRcptPymtAccount]    Script Date: 09/22/2018 12:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[vewRcptPymtAccount] as SELECT acct.acctdesc, acct.acctgstin, acct1.acctdesc as ledgname, trntype, trndram, trncram, trnrema, acct.acctgrou, acct.groumain, jtrn.compcode, jtrn.trncode, jtrn.trndate, acct.compcode as acctcomp, case when trntype =3 then trncram-trndram else trndram-trncram end crdrvl, case when trndram > 0 then 'Dr' else 'Cr' end crdrhd  FROM ((jourtrn jtrn INNER JOIN vewAccountMst acct ON jtrn.trnitem=acct.acctcode) LEFT OUTER JOIN vewAccountMst acct1 ON jtrn.trnledg=acct1.acctcode and acct.compcode = acct1.compcode) --WHERE acct.groumain not in (23, 24, 43) AND acct.acctgrou in (23, 24, 43)
GO
