/****** Object:  StoredProcedure [dbo].[sp_AreaSignUp]    Script Date: 09/22/2018 12:47:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[sp_AreaSignUp] (@useId nvarchar(1000),@usePass nvarchar(1000))                
as                  
begin                  
select * from loginusers a left join Company b on a.compcode=b.compcode where usename=@useId and usepass=dbo.encrypt(@usePass)          
end
GO
