/****** Object:  StoredProcedure [dbo].[InsEmployee]    Script Date: 09/22/2018 12:46:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter proc [dbo].[InsEmployee]                              
@compcode smallint ,                              
@empgrou smallint ,                              
@empcode int ,                              
@empname nvarchar (100)=null,                              
@empalia nvarchar (100)=null,                              
@empaddr nvarchar  (200)=null,                              
@empcity int =null,                              
@empstat nvarchar (100)=null,                              
@empphon nvarchar (100)=null,                              
@empopdr money =null,                              
@empopcr money =null,                              
@empcldr money =null,                              
@empclcr money =null,                              
@empjmbl money =null,                              
@empagen smallint =null,                              
@emprema nvarchar (100)=null,                              
@emppath nvarchar (200)=null,                              
@empsort nvarchar (300)=null,                              
@empledg int =null,                              
@emparea int =null,                              
@acwithbl money =null,                              
@empnomi int =null,                              
@empdsap int =null,                              
@empprty int =null,                              
@emprate money =null,                              
@empcate int =null,                              
@empitrd int =null,                              
@empdsa int =null,                              
@emppinc varchar (50)=null,                              
@emprefn varchar (50)=null,                              
@empbudget int =null,                              
@emphind nvarchar (300)=null,                              
@empincome int =null  ,                            
@PAN varchar(50) =null,                            
@ESI varchar(50)=null,                            
@BirthDt datetime=null,                            
@AnvDt datetime=null,                            
@Gender int=null,                            
@CastID int=null,                            
@CategoryID int=null,                            
@IDProof varchar(200)=null,                            
@ProofNo varchar(200)=null,                            
@AcNo varchar(200)=null,                            
@PfAcNo varchar(200)=null,                            
@BankID varchar(200)=null,                            
@pfBankID varchar(200)=null    ,                    
@VetanMaan varchar (150)   =null ,              
@BranchID int = null  ,            
@Official_No varchar(50) = null ,            
@Residential_No varchar(50) = null ,            
@ReferenceNm varchar(150) = null ,            
@ReferenceNo varchar(50) = null ,            
@FatherNo varchar(50) = null ,            
@TemporaryAdd varchar(250) = null ,            
@PermanentAdd varchar(250) = null ,            
@Old_PF_No varchar(70) = null ,            
@UNA_No  varchar(70) = null ,            
@EmailID varchar(70) = null ,            
@BloodGroup varchar(20) = null  ,  @empDepa int = null ,   @empDesi int= null   ,@Father varchar(150)= null  , @Aadhar varchar(150) = null , @IFSC varchar(150) = null , @DegreeNM varchar(150) = null , @DegreeID int = null ,@IMG_Path  varchar(150)= null  ,
   
@AadharCard bit = 0  , @VoterId bit= 0  , @Licence bit= 0  , @Passport bit= 0  , @Statement bit= 0  , @Degree bit= 0  , @usetype int =8 ,@usename VARCHAR(50) = NULL ,@usepass VARCHAR(50) = NULL        
as                    
begin                        
 if not exists ( select * from Employee where empcode= @empcode  )                      
 Begin        
declare @UseCode int                
set @UseCode =(select Max(usecode) +1 usecode from [loginusers])                
                        
  insert into Employee                              
  (compcode ,empgrou ,empcode ,empname ,empalia ,empaddr, empcity ,empstat ,empphon ,empopdr ,empopcr ,empcldr ,                              
  empclcr ,empjmbl ,empagen ,emprema ,emppath , empsort , empledg ,emparea ,acwithbl ,empnomi ,empdsap,empprty ,                              
  emprate,empcate ,empitrd,empdsa ,emppinc ,emprefn ,empbudget , emphind ,empincome ,                            
  PAN, ESI, BirthDt ,AnvDt, Gender  ,CastID  ,CategoryID, IDProof , ProofNo , AcNo , PfAcNo , BankID , pfBankID,                    
VetanMaan ,BranchID ,Official_No , Residential_No , ReferenceNm , ReferenceNo ,FatherNo , TemporaryAdd ,            
PermanentAdd , Old_PF_No , UNA_No  , EmailID , BloodGroup ,empDepa ,empDesi ,Father , Aadhar ,IFSC ,DegreeNM   , DegreeID ,        
IMG_Path ,AadharCard , VoterId , Licence , Passport , Statement , Degree , UseCode )                               
  values                              
  (@compcode ,@empgrou ,@empcode ,@empname ,@empalia ,@empaddr,  @empcity ,@empstat ,@empphon ,@empopdr ,@empopcr ,@empcldr ,  @empclcr ,@empjmbl ,@empagen ,                  
@emprema ,@emppath , @empsort , @empledg ,@emparea ,@acwithbl ,@empnomi ,@empdsap,@empprty ,                        
                        
  @emprate,@empcate ,@empitrd,@empdsa ,@emppinc ,@emprefn ,@empbudget , @emphind ,@empincome,                            
  @PAN, @ESI, @BirthDt ,@AnvDt, @Gender  ,@CastID  ,@CategoryID, @IDProof , @ProofNo , @AcNo , @PfAcNo , @BankID , @pfBankID ,            
@VetanMaan ,@BranchID  , @Official_No , @Residential_No , @ReferenceNm , @ReferenceNo ,@FatherNo , @TemporaryAdd ,            
@PermanentAdd , @Old_PF_No , @UNA_No  , @EmailID , @BloodGroup ,@empDepa   ,   @empDesi ,@Father , @Aadhar ,@IFSC ,@DegreeNM  , @DegreeID ,        
@IMG_Path ,@AadharCard , @VoterId , @Licence , @Passport , @Statement , @Degree ,@UseCode )                  
      
if @usename is not Null            
begin            
 INSERT INTO [loginusers] (compcode,usecode,usename,usepass,usetype )                                          
 VALUES (62 ,@UseCode,@usename,dbo.encrypt(@usepass),@usetype )                  
 End      
                  
 end                       
 else                       
 Begin                         
  update Employee set empname =@empname,empalia =@empalia,empaddr = @empaddr,empcity = @empcity ,empstat = @empstat ,empphon= @empphon ,empopdr =@empopdr ,                  
empopcr=@empopcr ,empcldr =@empcldr , empclcr =@empclcr ,empjmbl =@empjmbl ,empagen=@empagen ,emprema=@emprema ,emppath =@emppath ,empsort =@empsort ,                  
empledg =@empledg ,emparea =@emparea ,acwithbl =@acwithbl,empnomi =@empnomi ,empdsap =@empdsap,empprty =@empprty , emprate =@emprate,empcate =@empcate ,                  
empitrd =@empitrd,empdsa =@empdsa ,emppinc = @emppinc                   
 ,emprefn =@emprefn ,empbudget =@empbudget , emphind =@emphind ,empincome = @empincome,                          
  PAN =@PAN, ESI =@ESI, BirthDt =@BirthDt ,AnvDt =@AnvDt, Gender =@Gender  ,CastID =@CastID  ,CategoryID =@CategoryID, IDProof =@IDProof , ProofNo =@ProofNo ,                  
 AcNo =@AcNo, PfAcNo = @PfAcNo ,  BankID = @BankID ,  pfBankID = @pfBankID  ,VetanMaan= @VetanMaan   ,BranchID=@BranchID  ,Official_No =@Official_No ,Residential_No =  @Residential_No ,ReferenceNm =@ReferenceNm ,ReferenceNo =@ReferenceNo ,FatherNo =@FatherNo,TemporaryAdd =@TemporaryAdd ,PermanentAdd =@PermanentAdd ,Old_PF_No =@Old_PF_No ,UNA_No =@UNA_No  , EmailID =@EmailID, BloodGroup  =@BloodGroup ,empDepa =@empDepa  ,  empDesi =@empDesi    ,Father =@Father , Aadhar =@Aadhar , IFSC =@IFSC ,DegreeNM =@DegreeNM , DegreeID =@DegreeID ,IMG_Path =@IMG_Path,AadharCard = @AadharCard, VoterId= @VoterId,Licence =@Licence ,Passport =@Passport ,Statement =@Statement ,Degree =@Degree         
  where compcode = @compcode and empcode= @empcode

  update loginusers
  set usetype=@usetype
  where usecode=(select usecode from Employee where compcode = @compcode and empcode= @empcode )
 End                       
End
GO
