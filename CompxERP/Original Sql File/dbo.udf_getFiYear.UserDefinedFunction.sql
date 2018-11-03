/****** Object:  UserDefinedFunction [dbo].[udf_getFiYear]    Script Date: 09/22/2018 12:47:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create function [dbo].[udf_getFiYear] (@pEntryD smalldatetime) returns VarChar(7) as begin   declare @retValue varchar(7)   if month(@pEntryD) >= 4 and month(@pEntryD) <= 12     set @retValue = cast(year(@pEntryD) as varchar) + '-' +right(cast(year(@pEntryD)+1 as varchar),2)   Else     set @retValue = cast(year(@pEntryD)-1 as varchar) + '-' +right(cast(year(@pEntryD) as varchar),2)   return @retValue End
GO
