/****** Object:  StoredProcedure [dbo].[GetUserRight]    Script Date: 09/22/2018 12:46:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Alter Proc [dbo].[GetUserRight]       
@UserCode int,    
@MenGrou int    
as    
Begin    
select a.MenCode,Menname , isnull(menview,0) menview,isnull(menaddi,0) menaddi,isnull(menedit ,0)menedit,isnull(mendele ,0)mendele,isnull(menacce ,0)menacce   from Menuoption a     
left join (Select * from usermenust where MenUser = @UserCode) b on a.MenCode =b.mencode     
where MenGrou =@MenGrou and isCRM = 1     
End
GO
