/****** Object:  UserDefinedFunction [dbo].[udf_GetLedger]    Script Date: 09/22/2018 12:47:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[udf_GetLedger](@pCompCd int, @pOpenDate smalldatetime ='1900-06-01', @pClosDate smalldatetime ='2016-06-30')          
returns table          
as                
return (          
 select b.*, a.mstbrnc, a.mstprtc, a.mstsale from jourmst a inner join jourtrn b on b.compcode = a.compcode and b.trncode = a.mstcode and b.trndate = a.mstdate and b.trntype = a.msttype where a.compcode = isnull(@pCompCd, a.compcode) and a.mstdate between
  
    
      
 @pOpenDate and @pClosDate        
)
GO
