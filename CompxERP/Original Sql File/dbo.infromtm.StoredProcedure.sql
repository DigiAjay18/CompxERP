/****** Object:  StoredProcedure [dbo].[infromtm]    Script Date: 09/22/2018 12:46:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[infromtm] as insert into account select * from tempback insert into jourmst select * from tempmast insert into jourtrn select * from temptran
GO
