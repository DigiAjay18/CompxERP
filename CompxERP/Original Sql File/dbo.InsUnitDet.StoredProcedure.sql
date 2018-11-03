/****** Object:  StoredProcedure [dbo].[InsUnitDet]    Script Date: 09/22/2018 12:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsUnitDet](@compcode int=0,@unitcode int=0 output ,@unitname nvarchar(max)='' )                
as                
begin                
 IF (EXISTS(SELECT * FROM UnitDet WHERE compcode = @compcode AND unitname=@unitname ))            
 Begin          
print '1'      
  SELECT @unitcode =unitcode FROM UnitDet WHERE compcode = @compcode AND unitname=@unitname               
  Select @unitcode          
 End              
 ELSE                
  begin                 
   if(ltrim(rtrim(@unitname))<>'')                
    begin                
     set @unitcode=(Select isnull(max(unitcode),0)+1 as Maxunitcode from UnitDet  )                
     INSERT INTO UnitDet(unitcode , compcode  , unitname)              
     VALUES(@unitcode , @compcode  , @unitname)              
              
   --set @unitcode=(SELECT @unitcode as Code)                
      SELECT @unitcode unitcode           
   end                
 end          
end
GO
