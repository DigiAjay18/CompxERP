/****** Object:  UserDefinedFunction [dbo].[Encrypt]    Script Date: 09/22/2018 12:47:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[Encrypt](@StrIn nvarchar(50)) 
returns nvarchar(50)
as
begin 
	declare @i int 
	declare @ii int 
	declare @III int 
	declare @iv int 
	declare @v int 
	declare @VI int 
	declare @VII int 
	declare @VIII int 
	declare @LenStr  int
	declare @mrVal int
	declare @Z int
	declare @Y int
	declare @j int
	declare @k int
	declare @SglChar nvarchar(100)
	declare @SglAsc int
	declare @CvrtChr int
	declare @ReturnV nvarchar(50)

	set @i = 67
	set @ii = 38
	set @III = 123
	set @iv = 94
	set @v = 81
	set @VI = 101
	set @VII = 30
	set @VIII = 97
	set @LenStr = Len(@StrIn)
	set @j = 1
	while @j <= @LenStr    
	begin
		If @j = 1 
		begin
			set @SglChar = Left(@StrIn, 1)
			set @SglAsc = ascii(@SglChar)
			set @CvrtChr = @SglAsc + @i
			set @ReturnV = char(@CvrtChr)
		end
		Else If @j = 2 
		begin
			set @SglChar = substring(@StrIn, 2, 1)
			set @SglAsc = ascii(@SglChar)
			set @CvrtChr = @SglAsc + @ii
			set @ReturnV = @ReturnV + char(@CvrtChr)
		end
		Else If @j = 3 
		begin
			set @SglChar = substring(@StrIn, 3, 1)
			set @SglAsc = ascii(@SglChar)
			set @CvrtChr = @SglAsc + @III
			set @ReturnV = @ReturnV + char(@CvrtChr)
		end
		Else If @j = 4 
		begin
			set @SglChar = substring(@StrIn, 4, 1)
			set @SglAsc = ascii(@SglChar)
			set @CvrtChr = @SglAsc + @iv
			set @ReturnV = @ReturnV + char(@CvrtChr)
		end
		Else If @j = 5 
		begin
			set @SglChar = substring(@StrIn, 5, 1)
			set @SglAsc = ascii(@SglChar)
			set @CvrtChr = @SglAsc + @v
			set @ReturnV = @ReturnV + char(@CvrtChr)
		end
		Else If @j = 6 
		begin
			set @SglChar = substring(@StrIn, 6, 1)
			set @SglAsc = ascii(@SglChar)
			set @CvrtChr = @SglAsc + @VI
			set @ReturnV = @ReturnV + char(@CvrtChr)
		end
		Else If @j = 7 
		begin
			set @SglChar = substring(@StrIn, 7, 1)
			set @SglAsc = ascii(@SglChar)
			set @CvrtChr = @SglAsc + @VII
			set @ReturnV = @ReturnV + char(@CvrtChr)
		end
		Else If @j = 8 
		begin
			set @SglChar = substring(@StrIn, 8, 1)
			set @SglAsc = ascii(@SglChar)
			set @CvrtChr = @SglAsc + @VIII
			set @ReturnV = @ReturnV + char(@CvrtChr)
		end
		Else If @j > 8 
		begin
			set @Z = 17
			set @Y = 17
			set @k = 9
			while @k <= @LenStr
			begin
				set @SglChar = substring(@StrIn, @k, 1)
				set @SglAsc = ascii(@SglChar)
				set @CvrtChr = @SglAsc + @Z
				set @ReturnV = @ReturnV + char(@CvrtChr)
				If @Y > 0 
				begin
					set @Y = @Y - 2
				end
				Else
				begin
					set @Z = 17
					set @Y = 17
				End 
				set @Z = @Z + @Y
				set @k = @k + 1
			end
			break
		end 
		set @j = @j + 1
	end
	return @ReturnV
end

/*select dbo.encrypt('samarsingh012983*1')
drop function encrypt
select * from loginusers where usepass = dbo.encrypt('A') COLLATE SQL_Latin1_General_Cp1_CS_AS
select * from loginusers where usepass = dbo.encrypt('' or 0 = 0 --A' COLLATE SQL_Latin1_General_Cp1_CS_AS*/
GO
