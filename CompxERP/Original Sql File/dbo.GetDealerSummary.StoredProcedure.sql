/****** Object:  StoredProcedure [dbo].[GetDealerSummary]    Script Date: 09/22/2018 12:46:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[GetDealerSummary]      
@CompCode int,      
@Msttype int,      
@DealerID varchar (10)       
as      
Begin      
Declare @L_Total decimal(18,2)      
Declare @L_Date Datetime      
Declare @Total decimal(18,2)      
Declare @TotalOrd int       
      
select top 1 @L_Total = msttota ,@L_Date =MstDate from ordemst where compcode=@CompCode and mstType = @Msttype and mstrefc = @DealerID order by mstCode desc   
select @TotalOrd = count(*) ,@Total = sum(msttota) from ordemst where compcode = @CompCode and mstType = @Msttype and mstrefc = @DealerID  and month(mstdate) = Month(getdate()) and Year(mstdate) = Year(getdate())      
       
select @L_Total L_Total ,@L_Date L_Date,@Total Total,@TotalOrd TotalOrd       
      
End
GO
