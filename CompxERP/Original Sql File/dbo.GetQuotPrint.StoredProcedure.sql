/****** Object:  StoredProcedure [dbo].[GetQuotPrint]    Script Date: 09/22/2018 12:46:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[GetQuotPrint]         
@MstCode int          
as        
Begin        
      
declare @Query varchar(max) , @Select varchar(Max)  , @Join varchar(Max)                                                                            
declare @iLead int      
set @iLead = (select mstfinc from ordeMst where Msttype = 1163 and mstCode = @MstCode)      
      
if @iLead = 1       
 begin      
  set @Select = 'OM.MstChNo ParkLead,OM.MstChNo ParkLead1,OM.mstsection Mobile,OM.mstlotno EMail,OM.mstJobNo Address,CD.CityName CityName ,'      
  set @Join = ' Left join OrdeMst OM on OM.Msttype = 1147 and a.mstrefc = OM.MstCode         
Left join CityDet CD on OM.mstrvsc = CD.CityCode and CD.CityType = 67'      
 End      
Else      
 begin      
  set @Select = 'TD.DealCode ParkLead,TD.DealCode  ParkLead1,TD.Mob2  Mobile, TD.Email EMail, TD.Add_I Address, DD.CityName CityName,'      
  set @Join = 'Left join tblDistributor TD On TD.DistributorID >0 and a.mstrefc = TD.MstCode         
Left join CityDet DD on TD.CityID = DD.CityCode and DD.CityType = 67'      
 End      
         
 set @Query = 'select ' + @Select + ' a.*,b.itdRate, c.ItemName , d.* from ordeMst a        
 inner join OrdeItd b on a.MstType = b.ItdType and a.MstCode =b.ItdCode         
 left join IteMain c On b.ItdItem = c.itemCode and a.compcode = c.Compcode         
 left join IteMExin d On d.ItexCode = c.itemCode and a.compcode = d.Compcode '       
 set @Query =@Query + @Join + ' Where a.msttype = 1163 and a.MstCode = ' + cast( @MstCode as varchar(15))      
       
print @Query                                                                   
execute(@Query)                       
       
select OT.studName OTerms    from ordeMst a       
CROSS APPLY dbo.SplitStringtoIntTable( a.msternv ) spl_O        
Left join studdet OT on OT.studtype = 44 and spl_O.value = OT.studcode         
where Msttype = 1163 and mstCode = @MstCode       
      
select FT.studName FTerms   from ordeMst a        
CROSS APPLY dbo.SplitStringtoIntTable( a.mstpofs ) spl_F        
Left join studdet FT on FT.studtype = 55 and spl_F.value = FT.studcode        
where Msttype = 1163 and mstCode = @MstCode       
      
End
GO
