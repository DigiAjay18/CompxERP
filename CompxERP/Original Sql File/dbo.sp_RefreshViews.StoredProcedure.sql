/****** Object:  StoredProcedure [dbo].[sp_RefreshViews]    Script Date: 09/22/2018 12:47:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[sp_RefreshViews] as begin declare @mschName varchar(100), @mvewName varchar(100) declare curViewsList cursor for select schema_name(schema_id)  as schema_name, name from sys.views  open curViewsList fetch next from curViewsList into @mschName, @mvewName while (@@fetch_status = 0) begin exec sp_refreshview @mvewName fetch next from curViewsList into @mschName, @mvewName End Close curViewsList deallocate curViewsList End
GO
