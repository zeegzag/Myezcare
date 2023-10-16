
CREATE FUNCTION [dbo].[GetGenericNameFormat]    
(     
 @FirstName varchar(max),    
 @MiddleName varchar(max),
 @LastName varchar(max),
 @nameformat nvarchar(max)
)    
RETURNS VARCHAR(MAX)    
AS    
   
BEGIN
 
Declare @Name varchar(50);   
--Declare @OrgID bigint  
--declare @nameformat nvarchar(max)  
--SELECT TOP 1 @OrgID=OrganizationID FROM DEVAdmin.[dbo].[Organizations] where DBName = DB_NAME()  
--select @nameformat= op.NameDisplayFormat from DEVAdmin.dbo.OrganizationPreference op where op.OrganizationID=@OrgID  

IF(@nameformat='First Last')    
	SELECT @Name = ISNULL(LTRIM(RTRIM(@FirstName))+' ','') + isNull(LTRIM(RTRIM(@LastName)),'');      
ELSE IF(@nameformat='Last First')    
	SELECT @Name=ISNULL(LTRIM(RTRIM(@LastName))+' ','') +  ISNULL(LTRIM(RTRIM(@FirstName)),'');
ELSE IF(@nameformat='First, Last')    
	SELECT @Name=ISNULL(LTRIM(RTRIM(@FirstName))+', ','')+ ISNULL(LTRIM(RTRIM(@LastName)),'');
ELSE IF(@nameformat='Last, First Middle')    
	SELECT @Name=ISNULL(LTRIM(RTRIM(@LastName))+', ','')+ ISNULL(LTRIM(RTRIM(@FirstName))+' ','')+ ISNULL(LTRIM(RTRIM(@MiddleName)),'');
ELSE IF(@nameformat='First Middle Last')    
	SELECT @Name=ISNULL(LTRIM(RTRIM(@FirstName))+' ','')+ ISNULL(LTRIM(RTRIM(@MiddleName))+' ','')+ ISNULL(LTRIM(RTRIM(@LastName))+' ','');	
ELSE
	SELECT @Name=ISNULL(LTRIM(RTRIM(@LastName))+', ','')+ ISNULL(LTRIM(RTRIM(@FirstName)),'');
RETURN @Name    
  
END
