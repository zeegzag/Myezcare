  CREATE FUNCTION [dbo].[CSVtoTableWithIdentity]    
(    
    @LIST nvarchar(MAX),    
    @Delimeter nvarchar(10)    
)    
RETURNS @RET1 TABLE (ReturnId INT, RESULT NVARCHAR(max))    
AS    
BEGIN    
    DECLARE @RET TABLE(ReturnId INT IDENTITY(1,1), RESULT NVARCHAR(max))    
        
    IF LTRIM(RTRIM(@LIST))='' RETURN      
    
    DECLARE @START BIGINT    
    DECLARE @LASTSTART BIGINT    
    SET @LASTSTART=0    
    SET @START=CHARINDEX(@Delimeter,@LIST,0)    
    
    IF @START=0    
    INSERT INTO @RET VALUES(SUBSTRING(@LIST,0,LEN(@LIST)+1))    
    
    WHILE(@START >0)    
    BEGIN    
        INSERT INTO @RET VALUES(SUBSTRING(@LIST,@LASTSTART,@START-@LASTSTART))    
        SET @LASTSTART=@START+1    
        SET @START=CHARINDEX(@Delimeter,@LIST,@START+1)    
        IF(@START=0)    
        INSERT INTO @RET VALUES(SUBSTRING(@LIST,@LASTSTART,LEN(@LIST)+1))    
    END    
        
    INSERT INTO @RET1 SELECT * FROM @RET    
    RETURN     
END