/****** Object:  View [dbo].[mahaorderInvGSTsubreport]    Script Date: 09/22/2018 12:47:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create VIEW [dbo].[mahaorderInvGSTsubreport] AS SELECT parN.acctname + ', ' + citN.cityname AS acctdesc, parN.acctname AS pname, parN.acctcode AS Expr1, parN.acctaddr AS addr, parN.acctphon, citN.cityname AS partyCity, sttN.cityname AS partyState, a.msttype AS Expr3, a.mstcode AS Expr4, a.mstdate AS Expr5, a.compcode AS Expr6, parN.compcode AS acctcom, parN.acctrema as acctrema, ot.trnadju as trnadju, ot.trnaddv as trnaddv, ot.trncram as trncram, ot.trndram as trndram, ot.trncram - ot.trndram + ot.trnexpa as trntotv, ot.trnexpr as trnexpr, parN.acctname + ' (On ' + str(trnaddv, 10, 2) + ') ' as trnhead,  str(trnadju, 6, 2) + '%' as trnperc, ot.trnsrno as trnsrno FROM dbo.ordemst a LEFT OUTER JOIN  dbo.ordetrn ot ON ot.compcode = a.compcode AND ot.trncode = a.mstcode AND ot.trndate = a.mstdate AND ot.trntype = a.msttype and isnull(ot.trninde, 0) = 3 INNER JOIN  (SELECT * From account  WHERE compcode <> 0) parN ON parN.acctcode = ot.trnitem LEFT OUTER JOIN (SELECT * From citydet WHERE citytype = 15) citN ON citN.citycode = parN.acctcity LEFT OUTER JOIN (SELECT * From citydet WHERE citytype = 67) sttN ON sttN.citycode = parN.acctstat
GO
