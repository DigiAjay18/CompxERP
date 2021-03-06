/****** Object:  UserDefinedFunction [dbo].[udf_getVchTypeHdr]    Script Date: 09/22/2018 12:47:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[udf_getVchTypeHdr](@pVchType int)              
returns varchar(50)              
as              
begin              
 declare @lRetV varchar(50)              
 if @pVchType = 42 set @lRetV = 'SL'              
 else if @pVchType =  43 set @lRetV = 'PU'              
 else if @pVchType =  3 set @lRetV = 'RE'              
 else if @pVchType =  6 set @lRetV = 'JV'              
 else if @pVchType =  5 set @lRetV = 'PA'              
 else if @pVchType =  320 set @lRetV = 'OE'              
 else if @pVchType =  46 set @lRetV = 'DN'            
 else if @pVchType =  81 set @lRetV = 'Ret.'               
 else if @pVchType =  47 set @lRetV = 'CN'              
 else set @lRetV = convert(varchar, @pVchType, 0)          
 return @lRetV              
end
GO
