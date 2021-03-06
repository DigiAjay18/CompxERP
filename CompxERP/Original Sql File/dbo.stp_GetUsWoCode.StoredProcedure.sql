/****** Object:  StoredProcedure [dbo].[stp_GetUsWoCode]    Script Date: 09/22/2018 12:47:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[stp_GetUsWoCode] (@pFieldN nvarchar(20), @pResult nvarchar(20) output )      
as      
begin      
 declare @lMainSql nvarchar(2000), @lDateSql nvarchar(1000)      
 declare @gatCode nvarchar(20), @rCountV int, @prvCode nvarchar(20)      
 declare @ne int, @ts nvarchar(20)      
 set @gatCode = 'A'      
 ---------------------------below query only to check whether ther is record or not      
 set @rCountV = 0      
 select @rCountV = count(*) from charcodege       
 ---------------------------      
 if @rCountV >= 1      
 begin      
  set @lMainSql = 'select @Value = isnull(' + @pFieldN + ', '''') from charCodeGe'      
  exec sp_executesql @lMainSql , N'@Value nvarchar(20) output', @Value = @prvCode output      
  if @prvCode <>''      
  begin      
   set @ne = 0      
   set @gatCode = ''      
   While Len(@prvCode) > 0      
   Begin      
    set @ts = Right(@prvCode, 1)      
    set @prvCode = substring(@prvCode, 1, Len(@prvCode) - 1)      
    If @ts = 'Z'      
    begin       
      set @ne = 1      
      set @gatCode = 'A' + @gatCode      
    end       
    Else      
    begin      
      set @gatCode = Char(Ascii(@ts) + 1) + @gatCode      
      set @ne = 0      
      break      
    End       
   End      
   If @ne = 1       
    set @gatCode = 'A' + @gatCode      
   Else      
    set @gatCode = @prvCode + @gatCode      
  end      
  set @lMainSql = 'update charcodege set ' + @pFieldN + '  = ''' + @gatCode + ''''      
 end      
 else      
 begin      
  set @lMainSql = 'insert into charcodege (' + @pFieldN + ') values (''' + @gatCode + ''')'      
 end      
 exec sp_executesql @lMainSql      
 ---------------      
 set @pResult = @gatCode      
    Return       
end
GO
