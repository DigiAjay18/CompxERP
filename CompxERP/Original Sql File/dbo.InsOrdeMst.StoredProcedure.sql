/****** Object:  StoredProcedure [dbo].[InsOrdeMst]    Script Date: 09/22/2018 12:47:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[InsOrdeMst](@compcode int=null              
,@msttype int=null              
,@Mastcode int=null              
,@mstdate datetime=null              
,@msttota money =null              
,@mstrema varchar(300) =null              
,@mstrefc varchar(50)  =null              
,@mstchno varchar(50)  =null              
,@mstptcode int  =null              
,@mstdrcode int  =null              
,@mstgncd varchar(30)  =null              
,@MSTEXTI varchar(50)  =null              
,@mstcfno varchar(50)  =null              
,@mstcfdt datetime  =null              
,@mststat int  =null              
,@msttime int  =null              
,@mstblno varchar(250) =null              
,@mstbldt smalldatetime =null              
,@mstclno varchar(250) =null              
,@mstcldt smalldatetime =null              
,@mstappr int  =null              
,@mstbala money =null              
,@mstneta money =null              
,@msttimR varchar(20)  =null              
,@msttimI varchar(20)  =null              
,@mstpaid money =null              
,@mstexca money =null              
,@msteces money =null              
,@mstsurc money =null              
,@mstfina money =null              
,@mstchnm varchar(150) =null              
,@mstqtyd money =null              
,@mstctyp int  =null              
,@oldwht money =null              
,@oldamt money =null              
,@mstpay money =null              
,@mstsite int  =null              
,@mstbrnc int  =null              
,@mstbrok int  =null              
,@mstsubt int  =null              
,@mstyldp money =null              
,@mstadde money =null              
,@mstdifq money =null              
,@mstschno int  =null              
,@mstlate money =null              
,@mstcust int  =null                
,@mstchnH varchar(10)  =null              
,@mstchnV int  =null              
,@mstvat1 money =null              
,@mstvat2 money =null              
,@mstvat3 money =null              
,@mstpdc int  =null              
,@mstautcd int  =null              
,@mstautdt datetime  =null              
,@mstautpr int  =null              
,@mstauttm datetime  =null              
,@mstincv money =null                 
,@msttrfr varchar(40)  =null              
,@mstgpno int  =null              
,@mstgpdt smalldatetime =null              
,@mstindno varchar(50)  =null              
,@mstinddat datetime  =null              
,@mstpono varchar(50)  =null              
,@mstpodate datetime  =null              
,@mstqno varchar(50)  =null              
,@mstqdt datetime  =null              
,@mstinvno varchar(50)  =null              
,@mstinvdt datetime  =null              
,@mstPerm varchar(20 ) =null              
,@mstaddr varchar(100) =null              
,@mstexcDes varchar(150) =null              
,@msttaxDes varchar(150) =null              
,@msttaxper money =null              
,@mstfrghtDes varchar(150)      =null              
,@mstfrghtper money  =null              
,@mstdeliDes varchar(150)      =null              
,@mstpayDes varchar(150) =null              
,@mstvaliDes varchar(150)      =null              
,@mstincome int  =null              
,@mstcurrcd int  =null              
,@mstcurrrat money  =null              
,@mstreftag varchar(100) =null              
,@mstempo int  =null              
,@mstdepa int  =null              
,@mststatus varchar(50)  =null              
,@mstactpay int  =null              
,@mstpayMode varchar(150)      =null               
,@mstContactPerson varchar(100)     =null              
,@mstContactType varchar(100)     =null              
,@mstDueDate datetime =null              
,@mstbuyerc int  =null              
,@MstPriceQuoted varchar(250)     =null              
,@mstfinc int  =null              
,@mstperd money =null               
,@mstexon money =null              
,@mstscon money =null              
,@mstcnfst int  =null              
,@mstincv7 money =null              
,@mstvincl int  =null              
,@mstrefno varchar(50)  =null              
,@mstCmpdt datetime  =null              
,@mstJobNo nvarchar(1000)  =null              
,@mstDrawNo nvarchar(1000)  =null              
,@mstjobEnd int  =null              
,@mstlotno nvarchar(1000)  =null               
,@lotcode int  =null              
,@mstsection nvarchar(1000)      =null                 
,@mstsale int  =null              
,@mstdsptoc int  =null              
,@mstuser int  =null              
,@mstincvz money =null              
,@mstvat0 money =null              
,@mstrfvc int  =null              
,@mstrfvt int  =null              
,@mstexec int =null              
,@mstprtc int  =null                
,@msternv varchar(50)  =null              
,@mstpofs varchar(50)  =null              
,@mstintr money =null              
,@mstdued int  =null                
,@mstincv13 money =null              
,@mstincv14 money =null             
,@mstvat4 money =null              
,@mstmultm int  =null              
,@mstrvsc int  =null              
,@mstappl int  =null              
,@mstadjt int  =null              
,@mstIorL varchar(1)  =null    ,        
@ItemID int =null    ,        
@ItemNM Varchar(200) =null )        
as                 
Begin                 
 if not exists ( select * from Ordemst where CompCode = @CompCode and Msttype = @Msttype and Mstcode = @Mastcode )                    
 Begin                    
  insert into Ordemst (compcode,msttype,mstcode,mstdate,msttota,mstrema,mstrefc,mstchno,mstptcode,mstdrcode,mstgncd,MSTEXTI,mstcfno,mstcfdt,mststat,msttime,mstblno,mstbldt,mstclno,mstcldt,mstappr,mstbala,mstneta,msttimR,msttimI,mstpaid,mstexca,msteces,mstsurc,mstfina,mstchnm,mstqtyd,mstctyp,oldwht,oldamt,mstpay,mstsite,mstbrnc,mstbrok,mstsubt,mstyldp,mstadde,mstdifq,mstschno,mstlate,mstcust,mstchnH,mstchnV,mstvat1,mstvat2,mstvat3,mstpdc,mstautcd,mstautdt,mstautpr,mstauttm,mstincv,msttrfr,mstgpno,mstgpdt, 
 
     
mstindno,mstinddat,mstpono,mstpodate,mstqno,mstqdt,mstinvno,mstinvdt,mstPerm,mstaddr,mstexcDes,msttaxDes,msttaxper,mstfrghtDes,mstfrghtper,mstdeliDes,mstpayDes,mstvaliDes,mstincome,mstcurrcd,mstcurrrat,mstreftag,mstempo,mstdepa,mststatus,mstactpay,mstpayMode,mstContactPerson,mstContactType,mstDueDate,mstbuyerc,MstPriceQuoted,mstfinc,mstperd,mstexon,mstscon,mstcnfst,mstincv7,mstvincl,mstrefno,mstCmpdt,mstJobNo,mstDrawNo,mstjobEnd,mstlotno,lotcode,mstsection,mstsale,mstdsptoc,mstuser,mstincvz,mstvat0,mstrfvc ,mstrfvt,mstexec,mstprtc,msternv,mstpofs,mstintr,mstdued,mstincv13,mstincv14,mstvat4,mstmultm,mstrvsc,mstappl,mstadjt,mstIorL ,ItemID ,ItemNM )                
  values(@compcode,@msttype,@Mastcode,@mstdate,@msttota,@mstrema,@mstrefc,@mstchno,@mstptcode,@mstdrcode,@mstgncd,@MSTEXTI,@mstcfno,@mstcfdt,@mststat,@msttime,@mstblno,@mstbldt,@mstclno,@mstcldt,@mstappr,@mstbala,@mstneta,@msttimR,@msttimI,@mstpaid,@mstexca,@msteces,@mstsurc,@mstfina,@mstchnm,@mstqtyd,@mstctyp,@oldwht,@oldamt,@mstpay,@mstsite,@mstbrnc,@mstbrok,@mstsubt,@mstyldp,@mstadde,@mstdifq,@mstschno,@mstlate,@mstcust,@mstchnH,@mstchnV,@mstvat1,@mstvat2,@mstvat3,@mstpdc,@mstautcd,@mstautdt,@mstautpr, 
     
@mstauttm,@mstincv,@msttrfr,@mstgpno,@mstgpdt,@mstindno,@mstinddat,@mstpono,@mstpodate,@mstqno,@mstqdt,@mstinvno,@mstinvdt,@mstPerm,@mstaddr,@mstexcDes,@msttaxDes,@msttaxper,@mstfrghtDes,@mstfrghtper,@mstdeliDes,@mstpayDes,@mstvaliDes,@mstincome,@mstcurrcd,@mstcurrrat,@mstreftag,@mstempo,@mstdepa,@mststatus,@mstactpay,@mstpayMode,@mstContactPerson,@mstContactType,@mstDueDate,@mstbuyerc,@MstPriceQuoted,@mstfinc,@mstperd,@mstexon,@mstscon,@mstcnfst,@mstincv7,@mstvincl,@mstrefno,@mstCmpdt,@mstJobNo,@mstDrawNo,@mstjobEnd,@mstlotno,@lotcode,@mstsection,@mstsale,@mstdsptoc,@mstuser,@mstincvz,@mstvat0,@mstrfvc,@mstrfvt,@mstexec,@mstprtc,@msternv,@mstpofs,@mstintr,@mstdued,@mstincv13,@mstincv14,@mstvat4,@mstmultm,@mstrvsc,@mstappl,@mstadjt,@mstIorL ,@ItemID ,@ItemNM )                 
 End                    
 Else                    
 Begin   

	
	insert into bak_ordemst 
	select *,getdate(),@mstuser,0 from ordemst where CompCode = @CompCode and Msttype = @Msttype and Mstcode = @Mastcode  
	  
  update Ordemst              
  set Mstdate =@Mstdate  ,mstneta =@mstneta ,msttota =@msttota ,mstrefc =@mstrefc,mstchno =@mstchno ,mstcust =@mstcust,mstrema =@mstrema ,mstadde =@mstadde ,mstdifq =@mstdifq ,mstblno =@mstblno ,mstpaid =@mstpaid,msttime =@msttime, mstbala =@mstbala , mstdepa=@mstdepa,Mstbldt = @Mstbldt ,mstbrnc =@mstbrnc,mstclno = @mstclno,mstpdc=@mstpdc,mstyldp=@mstyldp,mstactpay=@mstactpay ,mststat=@mststat,mstchnm=@mstchnm,@mstcurrcd =mstcurrcd,@mstptcode =mstptcode   ,mstchnV =@mstchnV  ,MstChnH =@MstChnH,mstdrcode=@mstdrcode,mstappr=@mstappr,msttimI=@msttimI,mstqtyd=@mstqtyd,mstctyp=@mstctyp,oldwht=@oldwht,mstsite=@mstsite,mstbrok=@mstbrok,mstsubt=@mstsubt,mstvat1=@mstvat1,mstvat2=@mstvat2,mstvat3=@mstvat3,mstcurrrat=@mstcurrrat,mstDueDate=@mstDueDate,mstbuyerc=@mstbuyerc,mstperd=@mstperd,mstsale=@mstsale,mstdsptoc=@mstdsptoc,mstprtc=@mstprtc,msternv=@msternv,mstpofs=@mstpofs,mstintr=@mstintr,mstdued=@mstdued,mstrvsc=@mstrvsc,mstIorL=@mstIorL   ,ItemID =@ItemID ,ItemNM   =@ItemNM  , mstexec =@mstexec          
  where CompCode = @CompCode and Msttype = @Msttype and Mstcode = @Mastcode                  
 End                
End
GO
