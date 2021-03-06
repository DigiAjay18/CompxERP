/****** Object:  UserDefinedFunction [dbo].[udf_AccountMst]    Script Date: 09/22/2018 12:47:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--drop function  dbo.udf_AcctCityVw        
--drop function  dbo.udf_AcctOnlyVw        
--drop function  dbo.udf_AccountMst        
--alter table compexi add constraint uq_compexi unique(compcode)        
CREATE FUNCTION [dbo].[udf_AccountMst] (@pCompCd int) /*acct compcode replace with pcompcode 26-09-2017*/        
RETURNS TABLE        
AS        
RETURN        
(        
    --SELECT * FROM vewAccountMst WHERE compcode = @pAcctCompCd        
 select pacT.*,  acctdesc =  case when isnull(pciT.cityname,'')='' then pacT.acctname else pacT.acctname + ' , ' + pciT.cityname end  ,  pciT.cityname, pciT.citycode, pgrT.groumain, pgrT.grouunde, pgrT.grouaddr, pgrT.grouname, pgrT.grourepo, pgrT.grouposi
  
      
 from account pacT left outer join (select * from citydet where citytype = 15) pciT on pciT.citycode = acctcity inner join acgroup pgrT on pgrT.compcode = pacT.compcode and pgrT.groucode = pacT.acctgrou         
inner join compexi cexi on cexi.compunde = pacT.compcode where cexi.compcode = @pCompCd        
)
GO
