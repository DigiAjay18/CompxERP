/****** Object:  UserDefinedFunction [dbo].[udf_getDDMMYY]    Script Date: 09/22/2018 12:47:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create function [dbo].[udf_getDDMMYY] (@pEntryD smalldatetime) returns VarChar(7) as begin  return left(convert(varchar, @pEntryD, 105), 6) + right(convert(varchar, @pEntryD, 105), 2) End
GO
