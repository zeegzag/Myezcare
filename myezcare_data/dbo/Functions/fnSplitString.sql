
CREATE FUNCTION [dbo].[fnSplitString] 
( 
    @string NVARCHAR(MAX), 
    @delimiter CHAR(1) 
) 
RETURNS @output TABLE(RowID int,splitdata NVARCHAR(MAX) 
) 
BEGIN 
    DECLARE @start INT, @end INT, @index INT 
	SET @index=0;
    SELECT @start = 1, @end = CHARINDEX(@delimiter, @string) 
    WHILE @start < LEN(@string) + 1 BEGIN 
		SET @index=@index+1;
        IF @end = 0  
            SET @end = LEN(@string) + 1
       
        INSERT INTO @output (RowID,splitdata)  
        VALUES(@index,SUBSTRING(@string, @start, @end - @start)) 
        SET @start = @end + 1 
        SET @end = CHARINDEX(@delimiter, @string, @start)
        
    END 
    RETURN 
END
