create procedure [dbo].[BulkGroupUpdate]
(    
      @Input NVARCHAR(MAX),
      @Character CHAR(1),
	  @GroupIds nvarchar(max)
)
as
BEGIN
      DECLARE @StartIndex INT, @EndIndex INT
 
      SET @StartIndex = 1
      IF SUBSTRING(@Input, LEN(@Input) - 1, LEN(@Input)) <> @Character
      BEGIN
            SET @Input = @Input + @Character
      END
 
      WHILE CHARINDEX(@Character, @Input) > 0
      BEGIN
            SET @EndIndex = CHARINDEX(@Character, @Input)
           
           update Employees set GroupIDs=@GroupIds where EmployeeID=SUBSTRING(@Input, @StartIndex, @EndIndex - 1)
           
            SET @Input = SUBSTRING(@Input, @EndIndex + 1, LEN(@Input))
			
      END
 
      RETURN
END