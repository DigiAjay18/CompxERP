/****** Object:  UserDefinedFunction [dbo].[udf_GetLedgerDet]    Script Date: 09/22/2018 12:47:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[udf_GetLedgerDet](@pCompCd int, @pAcctCompCd int, @pOpenDate smalldatetime ='2016-06-01', @pClosDate smalldatetime ='2016-06-30')          
returns table          
as                
return (          
 select b.*, a.mstchno, a.mstchnh, a.mstchnv,  a.mstbrnc, a.mstprtc, e.acctdesc, bkdet.acctdesc as brokernm, isnull(case when b.trnitem <> isnull(a.mstprtc, 0) then acdet.acctdesc else sldet.acctdesc end, '') particular, a.mstrema, compname, gododesc from
  
    
      
 jourmst a inner join jourtrn b on b.compcode = a.compcode and b.trncode = a.mstcode and b.trndate = a.mstdate and b.trntype = a.msttype          
left join udf_BranchList(@pAcctCompCd) c on a.mstbrnc = c.godocode        
inner join company d on a.compcode = d.compcode             
inner join udf_AccountMst(@pAcctCompCd) e on e.acctcode = b.trnitem         
left join udf_AccountMst(@pAcctCompCd) bkdet on bkdet.acctcode = b.trnledg         
left join udf_AccountMst(@pAcctCompCd) acdet on acdet.acctcode = a.mstprtc         
left join udf_AccountMst(@pAcctCompCd) sldet on sldet.acctcode = a.mstsale         
where a.compcode = isnull(@pCompCd, a.compcode) and a.mstdate between @pOpenDate and @pClosDate        
)
GO
