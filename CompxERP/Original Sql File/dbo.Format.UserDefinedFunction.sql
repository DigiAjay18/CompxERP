/****** Object:  UserDefinedFunction [dbo].[Format]    Script Date: 09/22/2018 12:47:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[Format](@num int)
returns varChar(30)
As
Begin
Declare @out varChar(30) = ''

  while @num > 0 Begin
      Set @out = str(@num % 1000, 3, 0) + Coalesce(','+@out, '')
      Set @num = @num / 1000
  End
  Return @out
End
GO
