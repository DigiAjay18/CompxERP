/****** Object:  UserDefinedFunction [dbo].[formatAmount]    Script Date: 09/22/2018 12:47:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[formatAmount]  (
@amtIn as varchar(20)
) RETURNS varchar(20)
AS
BEGIN 

return cast(REPLACE(SUBSTRING(CONVERT(varchar(20), CAST(@amtIn AS money), 1),1,
LEN(CONVERT(varchar(20), CAST(@amtIn AS money), 1))-3), ',','.')
 + replace(RIGHT(CONVERT(varchar(20), CAST(@amtIn AS money), 1),3), '.',',') AS VARCHAR(20))

END
GO
