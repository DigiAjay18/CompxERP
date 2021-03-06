/****** Object:  StoredProcedure [dbo].[sp_GetSalesBill]    Script Date: 09/22/2018 12:47:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[sp_GetSalesBill] @mstcode int,@compcode int,@msttype int                            
as                            
begin   

Declare @compunde int 
set @compunde = (Select compunde  from compexi where compcode = @compcode )

 select distinct acctdesc,pname,Expr1,addr,acctphon,partyCity,partyState,itemdesc,Expr3,Expr4,Expr5,Expr53,Expr55,Expr57,Expr58,Expr6,Expr78,                          
acctcom,itemcom,refnum,bilno,chalno,chaldt,mstbldt,cas,code,wunit,mrp,mrpvalue,dis,Expr8,ItemDiscount,rema,station                          
,abetvalue,exciseValue,ecessvalue,sehpValue,acctname,BKCOMP,unitname   /*,UNICOMP*/,itdrema,itgpbcqt,compname,compadd1,compadd2,compadd3,                          
compcity,compmail,compfax,compphoo,state,TinNo,compctno,acctrema,empoName,unit1,detcomp,studName,mstrpno,mstdrin,grno,grdt,sizedcomp,unit2,                          
unitqty2,empComp,ucnvcomp,buyername,totalwt,chaptar,transport,mstmode,mstcase,mstrema,itditem,itdgath,godown,custname,custcode,custadd,                          
custphon,custcity,vatinc,vatpercent,Weight,schdisc,mstrfvc,mstrfvt,itemhsnc,itemgstr,itdgstrtv,itdcgstrt,itdcgstvl,itdsgstrt,itdsgstvl,                          
itdigstrt,itdigstvl,buyercode,buyeradd,buyergstin,buyercity,buyerState,partyGstin,msternv,mstpofs,msttimr,itdtotv,discvl,partyStateAlias                          
,buyerStateAlias,actprice,itdperd,mstintr,mstdued,mstduedate,compgstin,itddscp,mstlicn,itemdisc,dmob,acctcityname,bkmob,mstgpno,frtoutrd,                          
dsptoname,dsptoadd1,dsptocity,dsptostate,dsptophone,mstneta,billtype,freeqty,cdamt,billqty,bkname,itdmill,mstsale,itemtext,itdqtyd,                          
taxinclprc,itdvate,taxinclamt,compdtpn,itdlabamt,labinclamt,acformreq,salecom,itemname ,(select compbank from compexi where compcode =a.Expr6 )compbank                
 from mahabillinvgst a                 
 where Expr6=@compcode and Expr3=@msttype and Expr4=@mstcode   and itemcom=@compunde and acctcom =@compunde  and salecom =@compunde and (empcomp=@compunde or empcomp is null)   and                     
(bkcomp=@compunde or bkcomp is null)  and (unicomp=@compcode or unicomp is null)                      
and (detcomp=@compcode or detcomp is null)  and (ucnvcomp=@compcode or ucnvcomp is null)  and (sizedcomp=@compcode or sizedcomp is null)                         
--and  expr5 = '2018-01-29'                         
 order by itemdesc                            
                             
 select * from mahabillinvgstsubreport                            
 where Expr6=@compcode and Expr3=@msttype and Expr4=@mstcode                            
                            
 select * from mahabillinvgsthsnsummary                            
 where Expr6=@compcode and Expr3=@msttype and Expr4=@mstcode                            
                        
                        
select MstTota Amount,'Total' Head,0 trnItem from jourmst a where a.compcode=@compcode and mstType=@msttype and MstCode=@mstcode                        
Union all                        
select                     
case when msttype = 42 then                    
case when trncram >0 then trncram else - trndram  end                      
else                     
case when trncram >0 then - trncram else trndram  end end Amount                     
, case when trnItem = 1 then 'Paid' else Acctname end Head ,b.trnItem from jourmst a                        
inner join jourtrn b on a.compcode =b.compcode  and mstType=b.trntype and MstCode = b.trncode                        
inner join Account c on  b.trnItem = c.acctcode and c.acctcode >1000 --and c.compcode =b.compcode                        
where a.compcode=@compcode and trnType=@msttype and trnCode=@mstcode and b.trnItem not in (isnull(mstprtc,0) ,isnull(mstsale,0) ,isnull(mstbrok,0))                   
order by  trnItem                        
                         
end
GO
