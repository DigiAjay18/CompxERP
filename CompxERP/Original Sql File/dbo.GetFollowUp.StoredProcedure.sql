/****** Object:  StoredProcedure [dbo].[GetFollowUp]    Script Date: 09/22/2018 12:46:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[GetFollowUp]            
@Status int =0,       
@EmpID int =0 ,             
@Days int =0   ,          
@IsLead int =2 ,    
@isDue int =2          
as            
Begin            
            
declare @Query varchar(max) , @Where varchar(Max)                                                                    
set @Where =''                
if ltrim(rtrim(@Status)) <> 0 set @Where =@Where +' and a.Status_I = '+ cast(@Status as varchar(10))                                                           
if ltrim(rtrim(@IsLead)) <> 2 set @Where =@Where +' and a.IsLead = '+ cast(@IsLead as varchar(10))                                                           
if ltrim(rtrim(@EmpID)) <> 0 set @Where =@Where +' and a.F_by = '+ cast(@EmpID as varchar(10))        
      
if ltrim(rtrim(@Days)) <> 0 set @Where =@Where +' and a.MstDate between getDate()-'+ cast(@Days as varchar(10)) +'  and   getDate()'                                     
set @Query = 'select UseName ,L_No ,F_Date,case when Status_I = 1 then ''Closed'' when Status_I = 2 then ''Fake'' when Status_I = 3 then ''Under Process'' when Status_I = 4 then ''Converted'' end  Status,a.Remark ,isLead ,isNull(msternv ,DisName) Company 
,F_Time ,a.MstDate from tblFollow a              
inner join loginUsers b on a.F_by = b.UseCode            
Left join ordeMst Itd on a.L_ID = Itd.MstCode and MstType = 1147                 
Left join tblDistributor Dist on a.L_ID = Dist.MstCode                       
where 1=1'            
            
set @Query =@Query+' '+@Where + ' order by a.MstDate desc'                
            
                    
print @Query                                                         
execute(@Query)             
            
End
GO
