CREATE FUNCTION [dbo].[GetGeneralNameFormat]
(	
	@FirstName varchar(max),
	@LastName varchar(max)
)
RETURNS VARCHAR(MAX)
AS
begin
Declare @Name varchar(50);
SELECT @Name=@LastName+', '+ @FirstName;

return @Name
end

