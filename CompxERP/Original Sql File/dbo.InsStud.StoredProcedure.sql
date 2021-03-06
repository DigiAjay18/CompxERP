/****** Object:  StoredProcedure [dbo].[InsStud]    Script Date: 09/22/2018 12:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsStud](@studType int=0,@studCode int=0 output ,@studName nvarchar(max)='' )              
as              
begin              
 IF (EXISTS(SELECT * FROM studdet WHERE studtype = @studType AND studName=@studName ))         
 Begin        
  SELECT @studCode =studCode FROM studdet WHERE studtype = @studType AND studName=@studName             
  --Select @studCode        
 End            
 ELSE              
  begin               
   if(ltrim(rtrim(@studName))<>'')              
    begin              
     set @studCode=(Select isnull(max(studCode),0)+1 as MaxStudCode from studdet where studType  = 61 )              
     INSERT INTO studdet(studCode , studtype  , studName)            
     VALUES(@studCode , @studtype  , @studName)            
            
   --set @studCode=(SELECT @studCode as Code)              
      SELECT @studCode studCode         
   end              
 end        
end
GO
