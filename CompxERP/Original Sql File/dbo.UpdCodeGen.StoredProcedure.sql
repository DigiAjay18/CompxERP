/****** Object:  StoredProcedure [dbo].[UpdCodeGen]    Script Date: 09/22/2018 12:47:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create Proc [dbo].[UpdCodeGen]  @comp int ,  @Type int ,  @Prev  int,  @Curr int ,  @Date datetime   , @StDate datetime  =null , @LsDate datetime  =null as   Begin   if not exists (select * from codegen where codetype =@Type and compcode =@comp ) begin insert into codegen values( @comp,@Type,@Prev,@Curr,@Date,@StDate,@LsDate) end else begin update codegen set codeprev =@Prev , codecurr =@Curr where codetype = @Type and compcode = @comp  End   End
GO
