/****** Object:  StoredProcedure [dbo].[GetChartData]    Script Date: 09/22/2018 12:46:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[GetChartData]          
@Compcode int   ,        
@msttype int  =42         
as        
Begin          
if @msttype =0       
Begin      
 Set @msttype = 42      
End      
select sum(msttota)   Amt,convert(varchar,cast(sum(msttota) as money),1) ItmRemark  ,convert(varchar (3) ,max(mstdate) ,100)  Remark from Ordemst         
where compcode =@Compcode and msttype = @msttype and mstDate between '2018/04/01' and '2019/03/31' group by convert(varchar (7) ,mstDate ,111)        
order by convert(varchar (7) ,mstDate ,111)        
      
select sum(msttota)   Amt,convert(varchar,cast(sum(msttota) as money),1) ItmRemark  , case when msttype = 42 then 'Sale' when msttype = 43 then 'Purchase' when msttype = 5 then 'Payment' when msttype = 3 then 'Receipt' when msttype = 6 then 'JV' when msttype = 108 then 'CV' else '' end Remark ,msttype ItemID from jourmst         
where compcode =@Compcode and msttype in (42,43,5,3 ) group by msttype         
        
End
GO
