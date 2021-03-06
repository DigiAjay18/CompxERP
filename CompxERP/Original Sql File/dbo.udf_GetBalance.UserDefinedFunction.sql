/****** Object:  UserDefinedFunction [dbo].[udf_GetBalance]    Script Date: 09/22/2018 12:47:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--select * from udf_GetBalance(null, null, '2017-09-23', 0, null, null)        
CREATE function [dbo].[udf_GetBalance] (@pCompCd int, @pUptoDate smalldatetime, @pBranchWs int = 1, @pBranchId int, @pAcctCode int)               
returns table                
as                      
return (                   
select sum(trndram-trncram) sumbala, sum(trndram) sumdram, sum(trncram) sumcram, compcode, CASE when @pBranchWs = 0 then  0 else tp.mstbrnc end mstbrnc, trnitem, @pUptoDate as trndate, 0 as trntype from udf_GetLedger(@pCompCd, '1900-04-01', @pUptoDate) tp where trndate <= @pUptoDate and tp.compcode = isnull(@pCompCd, tp.compcode) and tp.trnitem = isnull(@pAcctCode, tp.trnitem) and isnull(tp.mstbrnc, 0) = isnull(@pBranchId, isnull(tp.mstbrnc, 0)) group by tp.trnitem, tp.compcode, CASE when @pBranchWs = 0 then  0 else tp.mstbrnc end        
)
GO
