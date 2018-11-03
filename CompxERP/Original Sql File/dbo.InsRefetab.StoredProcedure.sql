/****** Object:  StoredProcedure [dbo].[InsRefetab]    Script Date: 09/22/2018 12:47:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Proc [dbo].[InsRefetab] 
 @compcode	int,
@ms1code	int,
@ms1type	int,
@ms1date	datetime= null,
@ms1srno	int= null,
@ms2code	int= null,
@ms2type	int= null,
@ms2date	smalldatetime= null,
@ms2srno	int= null,
@reftype	int= null,
@refamou	money= null,
@refrema	varchar(500)= null,
@refitem	int= null,
@refmill	int= null
as
Begin 
    insert into refetab 
        (compcode,ms1code	,ms1type	,ms1date	,ms1srno	,ms2code	,ms2type	,ms2date	,ms2srno	,
reftype	,refamou	,refrema	,refitem	,refmill)	
    Values (@compcode	,@ms1code	,@ms1type	,@ms1date	,@ms1srno	,@ms2code	,@ms2type	,@ms2date	
,@ms2srno	,@reftype	,@refamou	,@refrema	,@refitem	,@refmill	)
End
GO
