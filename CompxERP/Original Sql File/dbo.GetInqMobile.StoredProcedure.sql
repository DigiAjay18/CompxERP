/****** Object:  StoredProcedure [dbo].[GetInqMobile]    Script Date: 09/22/2018 12:46:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[GetInqMobile]                    
@Product varchar(200)='',                  
@ExeName varchar(200)='',                  
@CityNM varchar(200)=''   ,                
@UserID int=1    ,               
@UserType int=0              
as                    
Begin                    
Select a.InqName +' , '+ isnull(CityNM,'') +' , '+ isnull(CD.CityName,'') +' , '+ isnull(Add2,'') +'~'+convert(varchar(10) ,isnull(a.ProductID, ''))  NameDesc ,a.Mobile NameNM  from tblCallMst a        
left Join EmpProduct c on c.ProductID = a.ProductID                    
left Join tblCallDet b on a.Mobile = b.Mobile                    
left Join CityDet CD on a.StateID = CD.CityCode and CD.CityType = 67      
where b.Mobile is  null                   
and 1= case when @Product = '' then 1 else                  
case when a.Product = @Product then 1 else 0 end end                   
                  
and 1= case when @ExeName = '' then 1 else                  
case when a.ExeName = @ExeName then 1 else 0 end end                   
                  
and 1= case when @CityNM = '' then 1 else                  
case when a.CityNM = @CityNM then 1 else 0 end end              
                   
and 1= case when @UserType = 0 then 1 else                  
case when c.EmpID = @UserID then 1 else 0 end end                   
                  
ORDER BY NEWID()                    
End
GO
