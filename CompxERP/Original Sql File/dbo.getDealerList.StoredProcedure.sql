/****** Object:  StoredProcedure [dbo].[getDealerList]    Script Date: 09/22/2018 12:46:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[getDealerList]       
@EmpID int =0    
as    
Begin    
 if @EmpID  >0     
  Begin    
   select empname EmpNm ,b.DisName + ' - ' + isnull(d.cityName ,'') DisName,ContactPerson ,Mob1 ,a.*  from EmpAllotMst a     
   inner join tblDistributor b on b.DistributorID >0 and a.DealerID = b.MstCode    
   inner join employee c on a.EmpID = c.EmpCode 
	left Join CityDet d on d.CityType = 15 and b.CityID_I = d.CityCode
Where EmpID = @EmpID      
   Order by  b.DisName     
  End     
 else    
  Begin    
   select empname EmpNm,b.DisName ,ContactPerson ,Mob1 ,a.*  from EmpAllotMst a     
   inner join tblDistributor b on b.DistributorID >0 and a.DealerID = b.MstCode    
   inner join employee c on a.EmpID = c.EmpCode    
   Order by  empname     
  End     
End
GO
