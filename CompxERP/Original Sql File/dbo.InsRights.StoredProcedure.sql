/****** Object:  StoredProcedure [dbo].[InsRights]    Script Date: 09/22/2018 12:47:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Proc [dbo].[InsRights]    
@mencode int ,    
@menuser int,    
@menview bit,    
@menaddi bit,    
@menedit bit,    
@mendele bit,    
@menacce bit    
as    
Begin    
if not exists (select * from usermenust where  mencode = @mencode  and menuser =@menuser)      
 Begin    
  insert into usermenust (mencode ,menuser,menview,menaddi,menedit,mendele,menacce )    
  values (@mencode ,@menuser,@menview,@menaddi,@menedit,@mendele,@menacce )    
 End    
Else    
 Begin    
  update usermenust     
  set  menview =@menview,menaddi =@menaddi,menedit = @menedit,mendele =@mendele,menacce =@menacce     
  where  mencode = @mencode  and menuser =@menuser    
 End    
End
GO
