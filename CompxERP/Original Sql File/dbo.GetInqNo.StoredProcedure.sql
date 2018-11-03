/****** Object:  StoredProcedure [dbo].[GetInqNo]    Script Date: 09/22/2018 12:46:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Proc [dbo].[GetInqNo]     
@PartyID int     
as    
Begin    
select a.MstChNo ,a.mstCode from ordeMst a    
Left join ordeMst b on a.compcode = b.compcode and b.msttype = 1163 and a.MstChNo = b.mstblno       
 where a.msttype = 1147   and a.mstCust = @PartyID and b.mstblno is null    
End
GO
