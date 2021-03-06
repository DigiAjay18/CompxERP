/****** Object:  UserDefinedFunction [dbo].[ProperCase]    Script Date: 09/22/2018 12:47:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ProperCase] (@InputString VARCHAR(MAX) )  
RETURNS VARCHAR(4000)  
AS  
BEGIN  
DECLARE @Index INT  
DECLARE @Char CHAR(1)  
DECLARE @OutputString VARCHAR(MAX)  
  
SET @OutputString = LOWER(@InputString)  
SET @Index = 2  
SET @OutputString = STUFF(@OutputString, 1, 1,UPPER(SUBSTRING(@InputString,1,1)))  
  
WHILE @Index <= LEN(@InputString)  
BEGIN  
    SET @Char = SUBSTRING(@InputString, @Index, 1)  
    IF @Char IN (' ', ';', ':', '!', '?', ',', '.', '_', '-', '/', '&','''','(')  
    IF @Index + 1 <= LEN(@InputString)  
BEGIN  
    IF @Char != ''''  
    OR  
    UPPER(SUBSTRING(@InputString, @Index + 1, 1)) != 'S'  
    SET @OutputString =  
    STUFF(@OutputString, @Index + 1, 1,UPPER(SUBSTRING(@InputString, @Index + 1, 1)))  
END  
    SET @Index = @Index + 1  
END  
  
RETURN ISNULL(@OutputString,'')  
END
GO
