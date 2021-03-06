/****** Object:  UserDefinedFunction [dbo].[udf_GetBalanceLdg]    Script Date: 09/22/2018 12:47:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--select * from udf_GetBalanceDet(null, null, '2017-09-23', 0, null, null)  
CREATE function [dbo].[udf_GetBalanceLdg] (@pCompCd int, @pUptoDate smalldatetime, @pBranchWs int = 1, @pBranchId int = 0, @pAcctCode int = 0)      
returns table          
as                
return (             
select sum(trndram-trncram) sumbala, sum(trndram) sumdram, sum(trncram) sumcram, compcode, CASE when @pBranchWs = 0 then  0 else tp.mstbrnc end mstbrnc, trnitem, @pUptoDate as trndate, 0 as trntype, /*case when isnull(trnledg, 0) = 0 then trnitem else trn
ledg end*/ trnledg from udf_GetLedger(@pCompCd, '1900-04-01', @pUptoDate) tp where trndate <= @pUptoDate and tp.compcode = isnull(@pCompCd, tp.compcode) and tp.trnitem = isnull(@pAcctCode, tp.trnitem) and tp.mstbrnc = isnull(@pBranchId, tp.mstbrnc) group 
by tp.trnitem, tp.compcode, CASE when @pBranchWs = 0 then  0 else tp.mstbrnc end, /*case when isnull(trnledg, 0) = 0 then trnitem else trnledg end */trnledg  
)
GO
