/****** Object:  StoredProcedure [dbo].[sp_InsSubCategory]    Script Date: 09/22/2018 12:47:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_InsSubCategory](@CompCode INT,@LoginID INT=0,@SysIP VARCHAR(100)='',@SubName NVARCHAR(50),@SubAlia NVARCHAR(50)='',@SubSortingNo INT=0,@CatType int=0,@CatCode int,@SubCode int=0,@BasicUnit smallint=0,@RateonPer money=1,@StkVlAsPerPrchPrcLst smallint=0,@OnlyCode int=0,@itgpcode int=0 output,@itgprefn varchar(50)=null,@itgpcart money=null)              
as              
begin              
 IF (EXISTS(SELECT * FROM itgroup WHERE ItgpName = @SubName AND compcode=@CompCode and itgpunde=@CatCode)) -- or @SubCode<>0 ItgpType=@CatType and              
  begin              
    update ItGroup            
    set ItgpName=@SubName,ItgpAlia=@SubAlia,ItgpSort=@SubSortingNo,itgpbasi=@BasicUnit,itgpbcqt=@RateonPer,itgpnumb=@StkVlAsPerPrchPrcLst        
   ,ItgpType=@CatType    ,itgprefn=@itgprefn,itgpcart=@itgpcart            
    where compcode=@CompCode and itgpcode=@SubCode            
   if(@OnlyCode=0)              
  Begin         
     SELECT -1 AS iType,'Already Exists...' AS Msg,itgpcode as Code            
     from itgroup            
     WHERE ItgpName = @SubName AND compcode=@CompCode and ItgpType=@CatType and itgpunde=@CatCode              
  End        
   else            
  Begin              
   set @itgpcode=(SELECT itgpcode as Code from itgroup WHERE ItgpName = @SubName AND compcode=@CompCode and ItgpType=@CatType and itgpunde=@CatCode)        
  End           
  End         
 ELSE              
  Begin               
   if(ltrim(rtrim(@SubName))<>'')              
    begin            
 Print 'ItgpName = '+ @SubName +' AND compcode='+ cast(@CompCode as varchar(20)) +'and itgpunde=' + Cast(@CatCode as varchar(20))        
      set @SubCode=(Select isnull(max(itgpcode),0)+1 as maxitgpcode from itgroup where compcode=@CompCode)              
      INSERT INTO ItGroup(compcode,itgpcode,ItgpName,ItgpAlia,ItgpUnde,ItgpSort,ItgpType,itgpbasi,itgpbcqt,itgpnumb,itgprefn,itgpcart)              
      VALUES(@CompCode,@SubCode,@SubName,@SubAlia,@CatCode,@SubSortingNo,@CatType,@BasicUnit,@RateonPer,@StkVlAsPerPrchPrcLst,@itgprefn,@itgpcart)              
             
    if(@OnlyCode=0)              
     SELECT 0 AS iType,'Saved Successfully...' AS Msg,@SubCode as Code              
    ELSE              
     set @itgpcode=(SELECT @SubCode as Code)              
    end              
 end        
End
GO
