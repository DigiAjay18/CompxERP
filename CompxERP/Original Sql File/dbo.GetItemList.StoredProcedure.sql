/****** Object:  StoredProcedure [dbo].[GetItemList]    Script Date: 09/22/2018 12:46:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[GetItemList]             
@Compcode int  ,          
@SubCatID int  =null          
as            
Begin            
if @SubCatID is null          
Begin          
 select c.ItGpName Cate, b.ItGpName SubCate, a.Itemname ,isnull(' ', UnitName )UnitName,a.compcode ,ItemSrno ItemRate ,a.itemCode ,a.ItemAlia            
 from Itemain a            
 Left join Itgroup b on a.itemgrou = b.itgpcode and Itgpunde <> 0 and a.Compcode =b.compcode            
 Left join Itgroup c on b.Itgpunde = c.itgpcode and c.Compcode =b.compcode            
 left join unitdet d on a.compcode = d.compcode and a.itemmaxi = d.unitcode             
 where a.Compcode= @Compcode            
End            
Else          
Begin          
 select c.ItGpName Cate, b.ItGpName SubCate, a.Itemname ,isnull(' ', UnitName )UnitName,a.compcode ,ItemSrno ItemRate ,a.itemCode ,a.ItemAlia ,itemhsnc ,itemgstr ,itemcess ,itemsort ,itemtext  ,itemnumb         
 from Itemain a            
 Left join Itgroup b on a.itemgrou = b.itgpcode and Itgpunde <> 0 and a.Compcode =b.compcode            
 Left join Itgroup c on b.Itgpunde = c.itgpcode and c.Compcode =b.compcode            
 left join unitdet d on a.compcode = d.compcode and a.itemmaxi = d.unitcode             
 where a.Compcode= @Compcode  and a.itemgrou = @SubCatID          
End            
          
End
GO
