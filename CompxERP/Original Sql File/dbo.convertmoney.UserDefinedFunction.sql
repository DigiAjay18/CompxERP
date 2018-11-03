/****** Object:  UserDefinedFunction [dbo].[convertmoney]    Script Date: 09/22/2018 12:47:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create function [dbo].[convertmoney] (@sting varchar(100))
returns 
varchar(100)
as
begin 
declare @ptr int,
      @str varchar(100),
      @ptr2 int,
      @rup  int,
      @coma int
set @ptr = len(@sting)
set @ptr2= 0
while right(@sting,@ptr2)  not like '.%' and @ptr2 < @ptr
begin 
set @ptr2= @ptr2 + 1
end 
set @str = '.00'
set @ptr = @ptr-@ptr2
set @coma = 0
set @rup = 0 
while  @ptr >0
begin 
      if @rup is null
      begin 
      if @coma = 2 
      begin set @str = ','+@str set @coma = 0 end 
      set @coma = @coma + 1
      set @str = substring(@sting,@ptr,1)+@str
      set @ptr = @ptr -1
      end
      else 
            begin   
                  if @rup = 3
                  begin set @str = ','+@str set @rup = null end 
                  set @rup = @rup + 1
                  set @str = substring(@sting,@ptr,1)+@str
                  set @ptr = @ptr -1
            end 
      
end

return  @str 
end
GO
