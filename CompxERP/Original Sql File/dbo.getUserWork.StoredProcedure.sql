/****** Object:  StoredProcedure [dbo].[getUserWork]    Script Date: 09/22/2018 12:46:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[getUserWork]                        
@UserID int =0,                         
@ModeID int =5,                         
@To varchar(20)  = null,                    
@From varchar(20)  = null ,                    
@CompCode int =0                 
as                              
Begin                          
declare @Query varchar(max) , @Where varchar(Max)                                                                                      
set @Where =''                                  
if ltrim(rtrim(@CompCode)) <> 0 set @Where =@Where +' and a.worcomp = '+ cast(@CompCode as varchar(10))        
if ltrim(rtrim(@UserID)) <> 0 set @Where =@Where +' and a.WorUser = '+ cast(@UserID as varchar(10))        
if ltrim(rtrim(@ModeID)) <> 5 set @Where =@Where +' and a.WorMode = '+ cast(@ModeID as varchar(10))        
if ltrim(rtrim(@From)) <> '' set @Where =@Where +' and a.Worcudt between  ''' + @From + ''' and  ''' + @To + ''''                             
                                  
set @Query = 'select WorType  ,WorCode  ,b.MstChno  , CONVERT(varchar, wordate, 103)  swordate   ,WorRema   ,MstRema   ,case When Wormode = 0 then ''ADD'' When Wormode = 1 then ''EDIT'' When Wormode = 2 then ''DEL'' else '''' end  sMode , CONVERT(varchar,
   
 worcudt, 100) sworcudt  , Worsysn   from userworkin a            
Left join JourMst b on a.WorComp =b.compcode and a.worCode = MstCode and a.WorType =b.Msttype where 1 = 1 '            
             
set @Query =@Query+' '+@Where + ' order by a.Worcudt'                                  
                                       
print @Query                                                                           
execute(@Query)               
            
End
GO
