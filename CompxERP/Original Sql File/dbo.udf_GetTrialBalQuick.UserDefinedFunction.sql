/****** Object:  UserDefinedFunction [dbo].[udf_GetTrialBalQuick]    Script Date: 09/22/2018 12:47:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[udf_GetTrialBalQuick](@pFromDt smalldatetime, @pUptoDt smalldatetime, @pCompCd int, @pBrncCd int)              
returns table              
as              
return              
(              
select mTbl.grouname, mTbl.acctcode, mTbl.acctdesc acctname, mTbl.cityname, IsNull(oTbl.sumbala, 0) [opbl], isnull(tTbl.sumdram, 0) [dr], isnull(tTbl.sumcram, 0) [cr], IsNull(cTbl.sumbala, 0) [crdrbl], mTbl.grourepo, mTbl.acctgrou as groucode, mTbl.grouposi, @pCompCd as compcode, @pBrncCd as brnccode ,acctstat, acctdeal from  udf_AccountMst(@pCompCd) mTbl               
/*closing bal.*/              
inner join               
(select sum(trndram - trncram) as sumbala, trnitem from jourmst aC inner join jourtrn bC on aC.compcode = bC.compcode and  aC.mstcode = bC.trncode  and aC.mstdate = bC.trndate  and aC.msttype = bC.trntype  where aC.compcode = @pCompCd and  datediff(d, mstdate, @pUptoDt) >= 0 and  isnull(aC.mstbrnc, 0) = isnull(@pBrncCd, isnull(aC.mstbrnc, 0)) /*and aC.msttype = isnull(@pVoucTp, ac.msttype)*/ group by trnitem) cTbl on cTbl.trnitem = mTbl.acctcode              
/*opening bal.*/              
left outer join               
(select sum(trndram - trncram) as sumbala, trnitem from jourmst aO inner join jourtrn bO on aO.compcode = bO.compcode and  aO.mstcode = bO.trncode  and aO.mstdate = bO.trndate  and aO.msttype = bO.trntype  where aO.compcode = @pCompCd and  datediff(d, mstdate, @pFromDt-1) >= 0 and  isnull(aO.mstbrnc, 0) = isnull(@pBrncCd, isnull(aO.mstbrnc, 0)) /*and aO.msttype = isnull(@pVoucTp, aO.msttype)*/ group by trnitem) oTbl on oTbl.trnitem = cTbl.trnitem               
/*transaction*/              
left outer join               
(select sum(trndram) as sumdram, sum(trncram) as sumcram, trnitem from jourmst aT    inner join jourtrn bT on aT.compcode = bT.compcode and  aT.mstcode = bT.trncode  and aT.mstdate = bT.trndate  and aT.msttype = bT.trntype  where aT.compcode = @pCompCd and  datediff(d, mstdate, @pFromDt) <= 0  and datediff(d, mstdate, @pUptoDt) >= 0 and  isnull(aT.mstbrnc, 0) = isnull(@pBrncCd, isnull(aT.mstbrnc, 0)) /*and aT.msttype = isnull(@pVoucTp, aT.msttype)*/ group by trnitem) tTbl on tTbl.trnitem = cTbl.trnitem   
  
    
      
        
            
)
GO
