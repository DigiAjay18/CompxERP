/****** Object:  StoredProcedure [dbo].[InsertAndUpdateCatItem]    Script Date: 09/22/2018 12:46:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[InsertAndUpdateCatItem](@compcode smallint=NULL,@itemcode smallint= 0,@itemname varchar(500)=NULL,@itemgrou smallint= NULL,                  
@itemnumb int=NULL,@itemsrno int=NULL,@itemopbl money=NULL,@itemclbl money=NULL,@itemmini money=NULL,@itemmaxi money=NULL                  
,@itemrate money=NULL,@itlastrat money=NULL,@itemsort int=NULL,@itemalia varchar(50)= NULL,@itempart varchar (50) = NULL,                  
@itemdisc money= NULL,@itemtext varchar (50) = NULL,@itemvatr money=NULL,@itemmrpv money=NULL,@itemhind varchar(250)=NULL,@itemrefn                   
varchar (50) = NULL,@itemtype varchar(50)=NULL,@itemvalu int =NULL,@iteper money = NULL,@itebesic int = NULL,@itemPrcnt money = NULL,                  
@itemimg varchar(max)=NULL,@SrNo int = NULL,@category int=NULL,@subcategory int=NULL,@itgpcode smallint= NULL,@itgpnamec                     
varchar(100)=NULL,@itgpnamesc varchar(100)=NULL,@itgpaliac varchar(50)=NULL,@itgpaliasc varchar(50)=NULL,@itgpunde smallint= NULL,                  
@itgpbasi smallint= NULL,@itgpbcqt MONEY=1,@itgpnumb smallint= NULL,@itgpsortc int= NULL,@itgpsortsc int= NULL,@itgptext varchar(50)=NULL,                  
@itgptyp varchar(50)=0,@itemgrou1 smallint= NULL,@itgpcode1 smallint=0,@StkVlAsPerPrchPrcLst smallint=0,@itemhsnc varchar(100)=null,                  
@itemgstr money=null,@itemcess money=null,@itgprefn varchar(50)=null,@itgpcart money=null,@itgprefn1 varchar(50)=null,                  
@itgpcart1 money=null ,@Brand varchar (150)=null,@UnitNm varchar (150)=null , @itmSize int = 0)                                
AS                                  
Begin                                  
 Begin tran t1                        
                      
 Exec InsUnitDet @compcode ,@itgpbasi Output, @UnitNm                                  
                   
if @itgpcode is null                   
 Begin                  
  exec sp_InsCategory @compcode,0,'',@itgpnamec,@itgpaliac,@itgpsortc,@itgptyp ,@itgpcode,1,@itgpcode output,@itgprefn,@itgpcart                                
 End                  
if ( @itgpcode1 is null)                  
 Begin                  
  exec sp_InsSubCategory @compcode,0,'',@itgpnamesc,@itgpaliasc,@itgpsortsc,@itgptyp,@itgpcode,@itgpcode1,@itgpbasi,@itgpbcqt,@StkVlAsPerPrchPrcLst,1,@itgpcode1 output,@itgprefn1,@itgpcart1                                  
 End                  
                  
 Exec InsStud 61,@itemnumb  Output ,@Brand                        
                        
  if not exists(select * from itemain where compcode=@compcode and itemname=@itemname and itemgrou=@itgpcode1) and @itemcode =0                               
   begin                                  
    set @itemsrno=(select isnull(max(itemsrno),0)+1 from itemain where compcode=@compcode and itemgrou=@itgpcode1)                                  
    set @itemcode=(select isnull(max(itemcode),0)+1 from itemain where compcode=@compcode)                                  
    
    Insert into itemain(compcode ,itemcode ,itemname ,itemgrou ,itemnumb ,itemsrno ,itemmini ,itemmaxi,itemsort ,itemdisc ,    
  itemalia ,itemtext ,itemvatr,itemhind,itemtype,itemhsnc,itemgstr,itemcess ,itemRate ,itemOpbl ,UnitId ,itmSize ,itemRefn ,itempart)          
    VALUES(@CompCode,@itemcode,@itemname,@itgpcode1,@itemnumb,@itemsrno,@itemmini,@itemmaxi,@itemsort ,@itemdisc,    
  @itemalia ,@itemtext,@itemvatr,@itemhind,'General',@itemhsnc,@itemgstr,@itemcess ,@itemRate ,@itemOpbl ,@itgpbasi ,@itmSize ,@itemRefn ,@itempart)                                        
    insert into iteminfo(compcode,itemcode,itemdesc,itemalia ,itgpbcqt,igs2 ,itemhsnc ,itemgstr ,itemdisc,    
  igc1,igc2 , itemname ,itemgrou ,itgptype ,descalia ,itgpbasi,unitname)--,unitname                                
    values(@CompCode,@itemcode,@itemname,@itemalia,@itgpbcqt,@itgpsortsc,@itemhsnc,@itemgstr,@itemdisc,    
  @itgpcode,@itgpcode1 , @itemname ,@itgpcode1 ,@itgptyp ,@itemname ,@itgpbasi ,@UnitNm)--,'Pcs.'                  
   
  Select @itemcode                                
   end                 
  else                    
 Select @itemcode                       
  begin                       
    if @itemcode is NULL                       
  begin                   
   set @itemcode=(select itemcode from itemain where compcode=@compcode and itemname=@itemname and itemgrou=@itgpcode1)                      
  End                 
                      
   update itemain                                
   set itemname=@itemname,itemmini=@itemmini,itemmaxi=@itemmaxi,itemsort=@itemsort ,itemdisc=@itemdisc,itemalia=@itemalia,    
 itemtext=@itemtext ,itemvatr=@itemvatr,itemhind=@itemhind,itemhsnc=@itemhsnc,itemgstr=@itemgstr,itemcess=@itemcess,    
 itemnumb=@itemnumb  ,itemRate =@itemRate ,itemOpbl = @itemOpbl  ,UnitId =@itgpbasi        ,itmSize =@itmSize ,itemRefn =@itemRefn   ,    
 itempart =@itempart    
 where compcode=@CompCode and itemcode=@itemcode                               
    
   if exists(select * from iteminfo where compcode=@CompCode and itemcode=@itemcode)                                
    update iteminfo                                
    set itemdesc=@itemname,itemalia=@itemalia,itgpbcqt=@itgpbcqt,igs2 =@itgpsortsc,itemdisc =@itemdisc,itemhsnc =@itemhsnc,    
 itemgstr =@itemgstr  ,igc1=@itgpcode,igc2=@itgpcode1    ,itemname =@itemname     ,itgpbasi =@itgpbasi  ,unitname = @UnitNm ,    
 itgptype = @itgptyp ,itemgrou= @itgpcode1    
    where compcode=@CompCode and itemcode=@itemcode                                  
   else                                
    insert into iteminfo(compcode,itemcode,itemdesc,itemalia ,itgpbcqt,igs2 ,itemhsnc ,itemgstr ,itemdisc,igc1,igc2)--,unitname                                
   values(@CompCode,@itemcode,@itemname,@itemalia,@itgpbcqt,@itgpsortsc,@itemhsnc,@itemgstr,@itemdisc,@itgpcode,@itgpcode1)--,'Pcs.'                                
  end                                  
 COMMIT TRAN t1                                    
End
GO
