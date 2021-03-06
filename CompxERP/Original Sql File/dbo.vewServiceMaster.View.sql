/****** Object:  View [dbo].[vewServiceMaster]    Script Date: 09/22/2018 12:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[vewServiceMaster] as SELECT a.itemname AS itemdesc, c.itgptype, b.itgpname as ign1, b.itgpcode as igc1, b.itgpalia as iga1, b.itgpstat, c.itgpname as ign2, c.itgpalia as iga2, c.itgpcode as igc2 ,d.unitname, c.itgpbasi, c.itgpbcqt, b.itgpsort as igs1, c.itgpsort as igs2, a.*, b.itgpvalu, 0 as sumquan, case when ltrim(rtrim(a.itemalia)) = '' or a.itemalia is null then '' else a.itemalia + ' => ' end  + a.itemname as descalia FROM (((srvmain AS a inner join svgroup c on a.itemgrou = c.itgpcode And c.compcode = a.compcode) inner join (select * from svgroup where (itgpunde = 1 or itgpunde=0)) b on c.itgpunde = b.itgpcode And c.compcode = b.compcode) inner join unitdet d on d.compcode=c.compcode and d.unitcode=c.itgpbasi)
GO
