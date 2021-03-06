/****** Object:  StoredProcedure [dbo].[GetPool]    Script Date: 09/22/2018 12:46:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[GetPool]      
@Host varchar (150) =null      
as      
Begin      
 SELECT  des.program_name , des.login_name , des.host_name , COUNT(des.session_id) [Connections]      
 FROM    sys.dm_exec_sessions des      
 INNER JOIN sys.dm_exec_connections DEC ON des.session_id = DEC.session_id      
 WHERE   des.is_user_process = 1 AND des.status != 'running'       
 and  1 = case when @Host is null then 1 else case when des.host_name = @Host then 1 else 0 end end      
 GROUP BY des.program_name      
    , des.login_name      
    , des.host_name      
 HAVING  COUNT(des.session_id) > 1      
 ORDER BY COUNT(des.session_id) DESC      
      
End
GO
