/****** Object:  UserDefinedFunction [dbo].[getTaxRateOnPrice]    Script Date: 09/22/2018 12:47:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create function [dbo].[getTaxRateOnPrice](@pRateVl money) returns table AS Return ( select case when @pRateVl < 1000 then 5 else 12 end taxRate )
GO
