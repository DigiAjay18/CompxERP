/****** Object:  View [dbo].[mahabillInvGST_Order]    Script Date: 09/22/2018 12:47:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create VIEW [dbo].[mahabillInvGST_Order] AS SELECT parN.acctname + ', ' + citN.cityname AS acctdesc, parN.acctname AS pname, parN.acctcode AS Expr1, parN.acctaddr AS addr, parN.acctphon, citN.cityname AS partyCity, sttN.cityname AS partyState, It_Main.itemName itemdesc, a.msttype AS Expr3, a.mstcode AS Expr4, a.mstdate AS Expr5, b.itdsrno AS Expr53, b.itdquan AS Expr55, b.itdrate AS Expr57, b.itdamou AS Expr58, a.compcode AS Expr6, b.itdtype AS Expr78, parN.compcode AS acctcom, itmN.compcode AS itemcom, a.mstchno AS               
refnum, a.mstblno AS bilno, a.mstclno AS chalno, a.mstcldt AS chaldt, a.mstbldt, ABS(b.itdpkin) AS cas, itmN.itemalia AS code, itmN.unitname AS wunit, gDet.gathwtst AS mrp, gDet.gathwtnt AS mrpvalue, isnull(gDet.gathqtdi, 0) AS dis, isnull(gDet.gathwtdi,0) AS                  
Expr8, gDet.gathlabp AS ItemDiscount, gDet.gathdesc AS rema, gDet.gathcity AS station, gDet.gathavgw AS abetvalue, gDet.gathtruc AS exciseValue, gDet.gathdisq AS ecessvalue, gDet.gathsehp AS sehpValue, brk.acctname + ', Ph. ' + isnull(brk.acctphon, '') as
  
    
 acctname, brk.compcode AS BKCOMP, U.unitname, U.compcode AS UNICOMP, b.itdrema, itmN.itgpbcqt, cmpN.compname, cmpN.compadd1, cmpN.compadd2, cmpN.compadd3, cmpN.compcity, cmpN.compmail, cmpN.compfax, cmpN.compphoo, cmpN.compstat AS state, cmpN.compitno AS
  
    
 TinNo, cmpN.compctno AS compctno, parN.acctrema as acctrema, emP.acctname AS empoName, det.unitname AS unit1, det.compcode AS detcomp, s.studName, tr.mstrpno, tr.mstdrin, tr.mstgrno AS grno, tr.mstgrdt AS grdt, sizeD.compcode AS sizedcomp, sizeD.unitname
  
    
 AS unit2, uni.unitqty2, emP.compcode AS empComp, uni.compcode AS ucnvcomp, isnull(buyer.studName, parN.acctname) AS buyername, a.oldwht AS totalwt, itmN.igs2 AS chaptar, trans.studname as transport, tr.mstmode, tr.mstcase, a.mstrema as mstrema, b.itditem
  
    
 as itditem, b.itdgath as itdgath, gd.gododesc as godown, isnull(custd.studname, '') as custname, isnull(custd.studcode, 0) as custcode, custd.studadd1 as custadd, custd.studphon as custphon, gcity.cityname as custcity, b.itdvatinc as vatinc, gathwast as 
  
    
vatpercent, itmN.itemdisc As Weight, isnull(gdet.gathdion, 0) as schdisc, a.mstrfvc as mstrfvc, a.mstrfvt as mstrfvt, itmN.itemhsnc as itemhsnc, itmN.itemgstr as itemgstr, b.itdgstrtv as itdgstrtv, b.itdcgstrt as itdcgstrt, b.itdcgstvl as itdcgstvl, b.itdsgstrt as itdsgstrt, b.itdsgstvl as itdsgstvl, b.itdigstrt as itdigstrt, b.itdigstvl as itdigstvl, isnull(buyer.studcode, 0) as buyercode, isnull(buyer.studadd1, parN.acctaddr) as buyeradd, isnull(buyer.studphon, parN.acctgstin) as buyergstin, isnull(bCity.cityname, citN.cityname) as buyercity, isnull(bSttN.cityname, sttN.cityname) as buyerState, parN.acctgstin as partyGstin, msternv as msternv, mstpofs as mstpofs, msttimr as msttimr, round(gDet.gathwtdi * abs(b.itdquan) / itmN.itgpbcqt, 2) as itdtotv, 
  round(gDet.gathlabp * abs(b.itdquan) / itmN.itgpbcqt, 2) as discvl, sttN.cityexti AS partyStateAlias, isnull(bSttN.cityexti, sttN.cityexti) as buyerStateAlias, b.itdactprc as actprice, isnull(b.itdperd, 0) as itdperd, a.mstintr as mstintr, a.mstdued as 
mstdued, a.mstduedate as mstduedate, compgstin as compgstin, b.itddscp as itddscp, tr.mstlicn, itmn.itemdisc, tr.mstmobno as dmob, ct.cityname as acctcityname, brk.acctphon as bkmob, a.mstgpno AS mstgpno, tr.trpcrvl as frtoutrd, dspto.studname as dsptoname, dspto.studadd1 as dsptoadd1, dspto.dsptocity, dspto.dsptostate, dspto.studphon as dsptophone, mstneta as mstneta, btype.studname as billtype, gDet.gatlabg As freeqty, isnull(gDet.gathcdam, 0) As cdamt, gDet.gathwtdf As billqty, brk.acctname as bkname, 
b.itdmill as itdmill, a.mstsale, itmN.itemtext, isnull(b.itdqtyd, 0) as itdqtyd, isnull(gDet.gathwtdi, 0) - isnull(gDet.gathlabp, 0) - isnull(gDet.gathcdam, 0) as taxinclprc, isnull(itdvate, 0) as itdvate, round((isnull(gDet.gathwtdi, 0) - isnull(gDet.gathlabp, 0) - isnull(gDet.gathcdam, 0)) * isnull(gDet.gathwtdf, 0) / itmN.itgpbcqt, 2) as taxinclamt, compdtpn as compdtpn, itdlabamt as itdlabamt, itdamou + isnull(itdlabamt, 0) as labinclamt, oppN.acformreq as acformreq, oppN.compcode as salecom, itmN.itemname as itemname, sttI.statname as plcofsply, a.mstbala as balance, a.mstpaid as paidamt, gdet.gathpuri as gathpuri ,parN.acctpanc FROM dbo.Ordemst a                   
INNER JOIN  (SELECT * From account  WHERE compcode <> 0) parN ON parN.acctcode = CONVERT(integer, LEFT(a.mstrefc, 6), 0) /* and parN.Compcode = a.compcode     */              
LEFT OUTER JOIN (SELECT * From citydet WHERE citytype = 15) citN ON citN.citycode = parN.acctcity                   
LEFT OUTER JOIN (SELECT * From citydet WHERE citytype = 67) sttN ON sttN.citycode = parN.acctstat                   
INNER JOIN dbo.company cmpN ON cmpN.compcode = a.compcode                   
LEFT OUTER JOIN OrdeItd b ON b.compcode = a.compcode AND b.itdcode = a.mstcode AND b.itddate = a.mstdate AND b.itdtype = a.msttype                   
LEFT OUTER JOIN dbo.gathdet gDet ON gDet.gathcode = b.itdgath                   
LEFT OUTER JOIN (SELECT * From iteminfo WHERE compcode <> 0) itmN ON /* itmN.Compcode = b.Compcode and*/ itmN.itemcode = b.itditem                   
LEFT OUTER JOIN (SELECT * From Itemain WHERE compcode <> 0) It_Main ON  It_Main.itemcode = b.itditem /* It_Main.Compcode = b.Compcode and  */               
LEFT OUTER JOIN  dbo.ordetrn ot ON ot.compcode = a.compcode AND ot.trncode = a.mstcode AND ot.trndate = a.mstdate AND ot.trntype = a.msttype AND ot.trnitem = isnull(a.mstbrok , 0)                   
LEFT OUTER JOIN (SELECT * From account  WHERE compcode <> 0) brk ON /*brk.Compcode = ot.Compcode and*/ brk.acctcode = ot.trnitem                   
LEFT OUTER JOIN (SELECT * From account WHERE compcode <> 0) emP ON /*emP.Compcode = a.Compcode and*/ emP.acctcode = a.mstptcode                   
LEFT OUTER JOIN (SELECT * From unitdet WHERE compcode <> 0) U ON /*U.Compcode = b.Compcode and*/ b.itdunit = U.unitcode                   
LEFT OUTER JOIN dbo.transta tr ON tr.compcode = a.compcode AND tr.mstcode = a.mstcode AND tr.mstdate = a.mstdate AND tr.msttype = a.msttype                   
LEFT OUTER JOIN (SELECT * From Studdet wHERE studtype = 61) s ON s.studCode = b.itdmill                   
LEFT OUTER JOIN (SELECT * From uniconv WHERE compcode <> 0) uni ON b.itdunit = uni.unitcode                   
LEFT OUTER JOIN (SELECT * From unitdet WHERE compcode <> 0) det ON uni.unitcod1 = det.unitcode                   
LEFT OUTER JOIN (SELECT * From unitdet  WHERE compcode <> 0) sizeD ON uni.unitcod2 = sizeD.unitcode                   
LEFT OUTER JOIN (SELECT * From Studdet WHERE studtype = 62) buyer ON buyer.studCode = a.mstbuyerc                   
LEFT OUTER JOIN (SELECT * FROM Studdet WHERE studtype = 78) trans ON trans.studCode = tr.msttrcd                   
left outer join dbo.godowns gd on gd.compcode=b.compcode and gd.godocode = b.itdgodo                   
left outer join (select * from studdet where studtype = 68) custd on isnull(a.mstcust, 0)  =custd.studcode                   
left join (select * from citydet where citytype = 15) Gcity on gcity.citycode = custd.studcity                   
left join (select * from citydet where citytype = 15) bCity on bCity.citycode = buyer.studcity                   
left join (select * from citydet where citytype = 67) bSttN on bSttN.citycode = buyer.studstat                   
LEFT OUTER JOIN (select * from citydet where citytype =15) ct on brk.acctcity =ct.citycode                    
left outer join (                     
SELECT  d1.*, cd1.cityname as dsptocity, cd2.cityname as dsptostate FROM Studdet d1                   
left outer join citydet cd1 on cd1.citycode = isnull(d1.studcity, 0) and cd1.citytype = 15                   
left outer join citydet cd2 on cd2.citycode = isnull(d1.studstat, 0) and cd2.citytype = 67                    
WHERE studtype = 62) dspto ON dspto.studcode = isnull(a.mstdsptoc, 0)                   
left outer join (select * from studdet where studtype = 387) btype on btype.studcode = a.mstctyp                   
Left JOIN  (SELECT * From account  WHERE compcode <> 0) oppN ON oppN.acctcode = isnull(mstsale, 0) and oppN.Compcode = a.compcode                   
left join vewStatMaster sttI on sttI.citycode = mstposc    
GO
