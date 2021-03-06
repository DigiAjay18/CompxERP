/****** Object:  StoredProcedure [dbo].[GetLeadList]    Script Date: 09/22/2018 12:46:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[GetLeadList] -- 1163                    
@MstType int =0,                   
@ItemID int =0,                   
@EmpID int =0 ,            @Industries int =0 ,                    
@To varchar(20)  = null,              
@From varchar(20)  = null               
as                        
Begin                        
                        
declare @Query varchar(max) , @Where varchar(Max)                                                                                
set @Where =''                            
if ltrim(rtrim(@ItemID)) <> 0 set @Where =@Where +' and a.itemID = '+ cast(@ItemID as varchar(10))                                                                     
if ltrim(rtrim(@MstType)) <> 0 set @Where =@Where +' and a.msttype = '+ cast(@MstType as varchar(10))                                                                       
if ltrim(rtrim(@EmpID)) <> 0 set @Where =@Where +' and a.mstuser = '+ cast(@EmpID as varchar(10))                  
if ltrim(rtrim(@From)) <> '' set @Where =@Where +' and a.mstdate between  ''' + @From + ''' and  ''' + @To + ''''               
if ltrim(rtrim(@Industries)) <> 0 set @Where =@Where +' and a.mstexec = '+ cast(@Industries as varchar(10))                  
                            
set @Query = 'select a.MstCode ,a.mstdate,a.MstChNo ,a.mstrefno ,a.mstContactPerson ,a.mstDrawNo ,a.MstRema,mstdate,mstDueDate ChallanDate ,mstsection NewMobile ,mstchnV ,msternv acctname  ,ItemNM ItemNm_Fini ,e.cityname Cityname, ''M.P'' Statename ,MstRema ,case when a.mstfinc =1 then ''ParkLead'' else ''Dealer'' end TypeName from OrdeMst a                    
left Join CityDet e on a.mstrvsc = e.citycode and e.citytype = 15               
Where a.CompCode = 66 '                        
                        
set @Query =@Query+' '+@Where + ' order by a.MstDate desc'                            
                        
                                
print @Query                                                                     
execute(@Query)                         
                        
End
GO
