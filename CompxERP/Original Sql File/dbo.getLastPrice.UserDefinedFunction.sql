/****** Object:  UserDefinedFunction [dbo].[getLastPrice]    Script Date: 09/22/2018 12:47:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create function [dbo].[getLastPrice](@p_dateVa smalldatetime, @p_compCd int, @p_PriceT int, @p_ListTy int, @p_CmpItm int) 
Returns @priceList Table(itemCd int, prRate money, itBcQt money) 
 as
BEGIN
   declare @itemC int
  declare @iBcQt money
 
   DECLARE item_list CURSOR FAST_FORWARD
  FOR SELECT itemcode, itgpbcqt from iteminfo where compcode = @p_CmpItm and itgptype = 0
   OPEN item_list
  FETCH NEXT FROM item_list into @itemC, @iBcQt
   WHILE @@FETCH_STATUS = 0
  BEGIN
       insert @priceList select top 1 @itemC, itdrate, @iBcQt from pricitd a inner join pricmst b on b.compcode = a.compcode
    and b.mstcode = a.itdcode and b.mstdate = a.itddate and b.msttype = a.itdtype  where datediff(d, a.itddate, @p_dateVa) >= 0
    and a.compcode = @p_compCd and a.itdtype = @p_PriceT and b.mstplty = @p_ListTy and a.itditem = @itemC
    order by itddate desc, itdcode desc, itdsrno desc
    FETCH NEXT FROM item_list into @itemC, @iBcQt
  End
  Close item_list
  DEALLOCATE item_list
  Return
End
GO
