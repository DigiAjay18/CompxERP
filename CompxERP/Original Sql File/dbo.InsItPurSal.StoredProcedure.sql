/****** Object:  StoredProcedure [dbo].[InsItPurSal]    Script Date: 09/22/2018 12:46:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[InsItPurSal](@compcode int  ,         
@itdtype int   =null ,@itdcode int   =null ,        
@itddate smalldatetime   =null ,        
@itdsrno int   =null ,        
@itditem int   =null ,        
@itdquan decimal(18,2)  =null ,        
@itdunit int   =null ,        
@itdrate decimal(18,2)  =null ,         
@itdamou decimal(18,2)  =null ,         
@itddime nvarchar(30)=null ,         
@itdpkin decimal(18,2)  =null ,         
@itdmill int   =null ,         
@itdgodo int   =null ,         
@itdgath varchar (15)  =null ,         
@itdrema varchar (100)  =null ,         
@itdtagno varchar (250)  =null ,         
@status varchar (1)  =null ,         
@itdrefq decimal(18,2)  =null ,         
@itdrefs int   =0 ,         
@itdpenq decimal(18,2)  =null ,         
@itdtime int   =null ,         
@itdvate decimal(18,2)  =null ,         
@itdempo int   =null ,         
@itdlabamt decimal(18,2)  =null ,         
@itdtowt decimal(18,2)  =null  ,            
@itdthickness decimal(18,2)  =null ,         
@itdlength decimal(18,2)  =null ,         
@itdwidth decimal(18,2)  =null ,         
@itdweight decimal(18,2)  =null ,          
@itdinde int   =0,@itdgstrtv money=0,@itdcgstrt money=0,@itdcgstvl money=0,@itdsgstrt money=0,@itdsgstvl money=0,    
@itdigstrt money=0,@itdigstvl money=0,@itdactprc money=0,@itdcessrt money=0,@itdcessvl money=0,@itdugstrt money=0,    
@itdugstvl money=0 ,@itdcasert money=0  ,@itdlabonw money=0 , @itdvatinc money=0  , @itdlswt money=0 ,@itdperd money=0 ,    
@itdqtyd money=0 ,@itddscp money=0 )        
as         
begin          
insert into ItPurSal (         
compcode ,itdtype ,itdcode ,itddate ,itdsrno ,itditem ,itdquan , itdunit ,itdrate ,itdamou ,itddime ,itdpkin ,itdmill ,itdgodo ,         
itdgath ,itdrema ,itdtagno ,status ,itdrefq ,itdrefs ,itdpenq , itdtime ,itdvate ,itdempo ,itdlabamt ,itdtowt ,              
itdthickness ,itdlength, itdwidth ,itdweight ,itdinde ,itdgstrtv,itdcgstrt,itdcgstvl,itdsgstrt,itdsgstvl,itdigstrt,    
itdigstvl,itdactprc,itdcessrt,itdcessvl,itdugstrt,itdugstvl ,itdcasert ,itdlabonw ,itdvatinc ,itdlswt ,itdperd ,itdqtyd ,itddscp )         
values(@compcode ,@itdtype ,@itdcode ,@itddate ,@itdsrno ,@itditem ,@itdquan , @itdunit ,@itdrate ,@itdamou ,@itddime ,    
@itdpkin ,@itdmill ,@itdgodo , @itdgath ,@itdrema ,@itdtagno ,@status ,@itdrefq ,@itdrefs ,@itdpenq ,@itdtime ,@itdvate ,    
@itdempo ,@itdlabamt ,@itdtowt ,@itdthickness ,@itdlength, @itdwidth ,@itdweight ,@itdinde,@itdgstrtv,@itdcgstrt,@itdcgstvl,    
@itdsgstrt,@itdsgstvl,@itdigstrt,@itdigstvl,@itdactprc,@itdcessrt,@itdcessvl,@itdugstrt,@itdugstvl ,@itdcasert ,@itdlabonw ,@itdvatinc     
,@itdlswt ,@itdperd ,@itdqtyd ,@itddscp )    
end
GO
