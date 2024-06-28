USE [HPTAMasterDb]
GO
/****** Object:  UserDefinedFunction [dbo].[SplitStringToInt]    Script Date: 25-06-2024 18:38:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER FUNCTION [dbo].[SplitStringToInt]
(
    @Input NVARCHAR(MAX),
    @Delimiter CHAR(1)
)
RETURNS @Output TABLE (Value INT)
AS
BEGIN
    DECLARE @Start INT, @End INT

    SET @Start = 1
    SET @End = CHARINDEX(@Delimiter, @Input)

    WHILE @Start <= LEN(@Input)
    BEGIN
        IF @End = 0 
            SET @End = LEN(@Input) + 1

        INSERT INTO @Output (Value)
        VALUES (CAST(SUBSTRING(@Input, @Start, @End - @Start) AS INT))

        SET @Start = @End + 1
        SET @End = CHARINDEX(@Delimiter, @Input, @Start)
    END

    RETURN
END