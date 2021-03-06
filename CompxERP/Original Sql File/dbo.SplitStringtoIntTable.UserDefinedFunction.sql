/****** Object:  UserDefinedFunction [dbo].[SplitStringtoIntTable]    Script Date: 09/22/2018 12:47:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[SplitStringtoIntTable](@input AS Varchar(MAX) )    
RETURNS    
      @Result TABLE(Value INT)    
AS    
BEGIN    
      DECLARE @str VARCHAR(20)    
      DECLARE @ind Int    
      IF(@input is not null)    
      BEGIN    
            SET @ind = CharIndex(',',@input)    
            WHILE @ind > 0    
            BEGIN    
                  SET @str = SUBSTRING(@input,1,@ind-1)    
                  SET @input = SUBSTRING(@input,@ind+1,LEN(@input)-@ind)    
                  INSERT INTO @Result values (@str)    
                  SET @ind = CharIndex(',',@input)    
            END    
            SET @str = @input    
            INSERT INTO @Result values (@str)    
      END    
      RETURN    
END
GO
