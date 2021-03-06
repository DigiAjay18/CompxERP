/****** Object:  StoredProcedure [dbo].[insOrdeItd]    Script Date: 09/22/2018 12:47:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[insOrdeItd]                        
@compcode int  ,            @itdtype int   =null ,                        
@itdcode int   =null ,            @itddate smalldatetime   =null ,            @itdsrno int   =null ,            @itditem int   =null ,                        
@itdquan decimal(18,2)  =null ,            @itdunit int   =null ,            @itdrate decimal(18,2)  =null ,            @itdamou decimal(18,2)  =null ,            
@itddime int   =null ,            @itdpkin decimal(18,2)  =null ,            @itdmill int   =null ,            @itdgodo int   =null ,                        
@itdgath varchar (15)  =null ,            @itdrema varchar (100)  =null ,            @itdtagno varchar (250)  =null ,@status varchar (1)  =null ,                        
@itdrefq decimal(18,2)  =null ,            @itdrefs int   =null ,            @itdpenq decimal(18,2)  =null ,            @itdtime int   =null ,                        
@itdvate decimal(18,2)  =null ,            @itdempo int   =null ,                        
@itdlabamt decimal(18,2)  =null ,           @itdtowt decimal(18,2)  =null  , @itdthickness decimal(18,2)  =null ,@itdlength decimal(18,2)  =null ,             
@itdwidth decimal(18,2)  =null ,            @itdweight decimal(18,2)  =null    ,       @itdinde int   =null   ,              
@itdPODt Datetime = null ,                   
@itdgstrtv  decimal(18,2)  =null ,    @itdcgstrt decimal(18,2)  =null ,    @itdcgstvl decimal(18,2)  =null ,    @itdsgstrt decimal(18,2)  =null ,                
@itdsgstvl decimal(18,2)  =null ,    @itdigstrt decimal(18,2)  =null ,    @itdigstvl decimal(18,2)  =null ,    @itdactprc decimal(18,2)  =null ,                
@itdcessrt decimal(18,2)  =null ,    @itdcessvl decimal(18,2)  =null ,    @itdugstrt decimal(18,2)  =null ,    @itdugstvl decimal(18,2)  =null   ,            
@itdperd decimal(18,3)  =null ,    @itdqtyd decimal(18,3)  =null    ,    @itddscp decimal(18,3)  =null         
as                        
begin                         
insert into  OrdeItd (                        
compcode ,itdtype ,itdcode ,itddate ,itdsrno ,itditem ,itdquan ,                        
itdunit ,itdrate ,itdamou ,itddime ,itdpkin ,itdmill ,itdgodo ,                        
itdgath ,itdrema ,itdtagno ,status ,itdrefq ,itdrefs ,itdpenq ,                        
itdtime ,itdvate ,itdempo ,itdlabamt ,itdtowt ,                      
itdthickness ,itdlength, itdwidth ,itdweight ,itdinde ,itdPODt,            
itdgstrtv,itdcgstrt,itdcgstvl,itdsgstrt,itdsgstvl,itdigstrt,            
itdigstvl,itdactprc,itdcessrt,itdcessvl,itdugstrt,itdugstvl ,itdperd,itdqtyd ,itddscp ) values                        
(@compcode ,@itdtype ,@itdcode ,@itddate ,@itdsrno ,@itditem ,@itdquan ,                        
@itdunit ,@itdrate ,@itdamou ,@itddime ,@itdpkin ,@itdmill ,@itdgodo ,                        
@itdgath ,@itdrema ,@itdtagno ,@status ,@itdrefq ,@itdrefs ,@itdpenq ,                        
@itdtime ,@itdvate ,@itdempo ,@itdlabamt ,@itdtowt                       
,@itdthickness ,@itdlength, @itdwidth ,@itdweight ,@itdinde ,@itdPODt              
,@itdgstrtv,@itdcgstrt,@itdcgstvl,@itdsgstrt,@itdsgstvl,@itdigstrt            
,@itdigstvl,@itdactprc,@itdcessrt,@itdcessvl,@itdugstrt,@itdugstvl ,@itdperd,@itdqtyd ,@itddscp)                        
                        
end
GO
