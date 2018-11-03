alter Proc dbo.getCustDet                
@Mobile  varchar(50)='',                
@Model varchar(50)='',                
@InvNo varchar(50)=''   ,    
@ItemID varchar(50)='',
@PartyID varchar(50)=''                
as
Begin
Select b.mstprtc,AcctCode ,AcctName ,Acctphon,Acctaddr ,mstChno InvNo, mstdate InvDt, dateadd( year , 1, mstdate) ExpDt ,datediff(dd , dateadd( year , 1, mstdate) , getdate()) RemDays ,itdItem  ,itemname  ,   CONVERT(varchar, mstdate, 105 )InvDate  from Account a
                
--Select AcctName  from Account a                
Left join jourMst b on a.acctCode  = b.mstprtc
--Left join jourMst b on a.mstprtc = b.acctCode                 
Left join ITPursal c on b.compcode = c.compcode and b.mstCode = c.itdcode and b.msttype = c.itdType                  
left join itemain d on itdItem = d.itemcode                   
Where msttype = 42                
                 
and 1= case when @Mobile = '' then 1
else case when a.acctphon = @Mobile then 1 else 0 end end
                    
and 1= case when @Model = '' then 1
else case when itdrema = @Model then 1 else 0 end end
                    
and 1= case when @InvNo = '' then 1
else case when mstChno = @InvNo then 1 else 0 end end
     
and 1= case when @ItemID = '' then 1
else case when c.itdItem = @ItemID then 1 else 0 end end

and 1= case when @PartyID= '' then 1
else case when a.AcctUser=@PartyID then 1 else 0 end end
         
order by InvNo ,ItemName
End
go
--getCustDet @Mobile ='' , @Model ='', @InvNo ='2260', @ItemID ='', @PartyID='1716'
