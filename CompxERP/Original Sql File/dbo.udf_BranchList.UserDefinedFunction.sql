/****** Object:  UserDefinedFunction [dbo].[udf_BranchList]    Script Date: 09/22/2018 12:47:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[udf_BranchList] (@pCompCd int)        
RETURNS TABLE        
AS        
RETURN        
(        
 select a.* from branches a inner join compexi cexi on cexi.compunde = a.compcode where cexi.compcode = @pCompCd         
)
GO
