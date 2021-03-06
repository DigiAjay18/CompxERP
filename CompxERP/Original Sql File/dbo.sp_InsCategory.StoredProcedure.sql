/****** Object:  StoredProcedure [dbo].[sp_InsCategory]    Script Date: 09/22/2018 12:47:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_InsCategory](@CompCode INT,@LoginID INT=0,@SysIP VARCHAR(100)='',@CatName NVARCHAR(50),@CatAlia NVARCHAR(50)='',        
@CatSortingNo INT=0,@CatType int=0,@CatCode int=0,@OnlyCode int=0,@itgpcode int=0 output,@itgprefn varchar(50)=null,@itgpcart money=null)            
as            
begin            
 IF (EXISTS(SELECT * FROM itgroup WHERE ItgpName = @CatName AND compcode=@CompCode and itgpunde=0) or @CatCode<>0)  --and ItgpType=@CatType         
 begin            
  update ItGroup          
  set ItgpName=@CatName,ItgpAlia=@CatAlia,ItgpSort=@CatSortingNo,itgprefn=@itgprefn,itgpcart=@itgpcart          
  where compcode=@CompCode and itgpcode=@CatCode          
  if(@OnlyCode=0)            
   SELECT -1 AS iType,'Already Exists...' AS Msg,itgpcode as Code          
   from itgroup          
   WHERE ItgpName = @CatName AND compcode=@CompCode and ItgpType=@CatType and itgpunde=0            
  else            
 Begin      
   set @itgpcode=(SELECT itgpcode as Code from itgroup WHERE ItgpName = @CatName AND compcode=@CompCode and ItgpType=@CatType and itgpunde=0)            
  Select @itgpcode      
 End       
 end            
 ELSE            
 begin             
  if(ltrim(rtrim(@CatName))<>'')            
  begin            
   set @CatCode=(Select isnull(max(itgpcode),0)+1 as maxitgpcode from itgroup where compcode=@CompCode)            
   INSERT INTO ItGroup(compcode,itgpcode,ItgpName,ItgpAlia,ItgpUnde,ItgpSort,ItgpType,itgprefn,itgpcart)          
   VALUES(@CompCode,@CatCode,@CatName,@CatAlia,0,@CatSortingNo,@CatType,@itgprefn,@itgpcart)          
   if(@OnlyCode=0)            
    SELECT 0 AS iType,'Saved Successfully...' AS Msg,@CatCode as Code            
   else            
    set @itgpcode=(SELECT @CatCode as Code)            
  end            
 end            
end
GO
