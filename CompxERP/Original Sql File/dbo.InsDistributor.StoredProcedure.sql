/****** Object:  StoredProcedure [dbo].[InsDistributor]    Script Date: 09/22/2018 12:46:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[InsDistributor]  @MstCode int ,@MstDate datetime = null,@DistributorID int =null, @CountryID int =null,@StateID Int =null,@CityID int =null, @CityID_I int =null,@Add_I nvarchar (200) =null,                  
@Add_II nvarchar (200) =null,@Add_III nvarchar (200) =null,@Add_IV nvarchar (200) =null,@PO_Box nvarchar (200) =null,@DisName varchar(150) =null ,@ContactPerson varchar(200) =null,@Mob1 varchar(200) =null,@Mob2 varchar(200)=null ,@LandLine varchar(200)=null ,@Email  varchar(200)=null ,  
@Skype varchar(200)=null ,@GSTIN varchar(100)=null , @compcode int =62  , @usetype int ,@usename VARCHAR(50) = NULL ,@usepass VARCHAR(50) = NULL  ,  
@DealCode varchar(100) = null ,@CityNM varchar(100) = null , @AdminID int =0 ,@Brand varchar(300)=null ,@Product varchar(300)=null  ,  
@ChequeNo varchar(50) = null ,@CP_I varchar(150)  = null ,@CP_II varchar(150)   = null ,@CP_NO_I varchar(50)  = null ,@CP_NO_II varchar(50)    = null ,  
@DOB datetime  = null ,@DOJ datetime  = null ,@AnniDt datetime = null    
as                  
Begin                  
if not exists(select * from tblDistributor where MstCode =@MstCode )                  
 begin                   
declare @UseCode int                
set @UseCode =(select Max(usecode) +1 usecode from [loginusers])                
                
  Insert into tblDistributor  (MstCode,MstDate ,DistributorID , CountryID ,StateID  ,CityID  , CityID_I  ,Add_I , Add_II ,Add_III ,Add_IV ,    
PO_Box ,DisName ,ContactPerson ,Mob1,Mob2,LandLine ,Email  ,Skype ,GSTIN  ,DealCode ,CityNM ,UseCode ,AdminID , Brand ,Product,  
ChequeNo ,CP_I   ,CP_II   ,CP_NO_I  ,CP_NO_II  ,DOB  ,DOJ  ,AnniDt  )                  
  values                   
  (@MstCode,@MstDate ,@DistributorID , @CountryID ,@StateID  ,@CityID  , @CityID_I  ,@Add_I , @Add_II ,@Add_III ,@Add_IV ,@PO_Box ,    
@DisName ,@ContactPerson ,@Mob1,@Mob2,@LandLine ,@Email  ,@Skype ,@GSTIN ,@DealCode ,@CityNM ,@UseCode ,@AdminID ,@Brand  ,@Product,  
@ChequeNo ,@CP_I   ,@CP_II   ,@CP_NO_I  ,@CP_NO_II  ,@DOB  ,@DOJ  ,@AnniDt  )                  
                
if @usename is not Null            
begin            
 INSERT INTO [loginusers] (compcode,usecode,usename,usepass,usetype )                                          
 VALUES (@compcode ,@UseCode,@usename,dbo.encrypt(@usepass),@usetype )                  
 End                  
                
 End                  
Else                  
 begin                   
  Update tblDistributor set                   
  MstDate =@MstDate ,DistributorID=@DistributorID , CountryID =@CountryID ,StateID =@StateID  ,                  
  CityID =@CityID  ,CityID_I =@CityID_I  ,Add_I =@Add_I,                  
  Add_II =@Add_II,Add_III =@Add_III,Add_IV =@Add_IV,PO_Box =@PO_Box,DisName =@DisName ,@CityNM =CityNM   ,            
ContactPerson= @ContactPerson ,Mob1 = @Mob1,Mob2 =@Mob2,LandLine =@LandLine ,Email =@Email   ,Skype =@Skype,GSTIN =@GSTIN ,DealCode   =@DealCode   ,    
Brand = @Brand,Product  = @Product     ,ChequeNo =@ChequeNo,CP_I =@CP_I  ,CP_II = @CP_II ,CP_NO_I =@CP_NO_I ,CP_NO_II =@CP_NO_II ,DOB= @DOB ,DOJ =@DOJ ,AnniDt = @AnniDt  
  Where MstCode =@MstCode                   
                
--if @usename is not Null            
-- begin            
--  update [loginusers] set usepass=@usepass where usename = @usename          
-- End                  
          
 End                  
                  
End
GO
