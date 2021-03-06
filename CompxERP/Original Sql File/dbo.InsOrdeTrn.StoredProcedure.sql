/****** Object:  StoredProcedure [dbo].[InsOrdeTrn]    Script Date: 09/22/2018 12:47:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[InsOrdeTrn](@SqlType int=0,@compcode smallint,@trntype int,@trncode int,@trndate datetime,@trnsrno smallint=null,@trnitem int=null,@trnrema nvarchar (100)=null,@trndram money=null,@trncram money=null,@trnrefc int=null,@trnrefd datetime=null,@trnadju money=null,@trncmdt tinyint=null,@trnchno nvarchar(40)= null,@trnchdt datetime=null,@trnbank varchar(150)=null,@trnledg int=null,@trnexpa money= null,@trntime int=null,@trnexpr int=null,@trnaddv money=null,@trncity int = null , @trntagv int = null,@trnshow int = null, @trninde int=null)  
as           
begin           
 Insert into jourtrn(compcode,trntype,trncode,trndate,trnsrno,trnitem,trnrema,trndram,trncram,trnrefc,trnrefd,trnadju,trncmdt,trnchno,trnchdt,trnbank,trnledg,trnexpa,trntime,trnexpr,trnaddv,trncity,trntagv,trnshow,trninde)  
 values (@compcode,@trntype,@trncode,@trndate,@trnsrno,@trnitem,@trnrema,@trndram,@trncram,@trnrefc,@trnrefd,@trnadju,@trncmdt,@trnchno,@trnchdt,@trnbank,@trnledg,@trnexpa,@trntime,@trnexpr,@trnaddv,@trncity,@trntagv,@trnshow,@trninde)  
 select @@identity        
end
GO
