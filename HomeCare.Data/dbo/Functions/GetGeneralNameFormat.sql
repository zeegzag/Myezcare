 --  PatientName=dbo.GetGeneralNameFormat(r.FirstName,r.LastName)  
CREATE FUNCTION [dbo].[GetGeneralNameFormat]    
(     
 @FirstName varchar(max),    
 @LastName varchar(max)    
)    
RETURNS VARCHAR(MAX)    
AS    
--select @FirstName='Akhil',@LastName='kamal'  
  
begin    
Declare @LastNameLen bit  
if(len(ltrim(rtrim(@lastname)))<=0 ) Begin set @LastName=null end  
Declare @Name varchar(50);   
Declare @OrgID bigint  
declare @nameformat nvarchar(max)  
SELECT TOP 1 @OrgID=OrganizationID FROM [Admin_Myezcare_Live].[dbo].[Organizations] where DBName = DB_NAME()  
select @nameformat= op.NameDisplayFormat from Admin_Myezcare_Live.dbo.OrganizationPreference op where op.OrganizationID=@OrgID  
  
if( @nameformat='First Last')  
begin  
SELECT @Name=ISNULL(LTRIM(RTRIM(@FirstName)),'')+ isNull(' '+LTRIM(RTRIM(@LastName)),'');    
end  
else  
begin  
SELECT @Name=ISNULL(LTRIM(RTRIM(@LastName))+', ','')+ LTRIM(RTRIM(@FirstName));    
  end  
return @Name    
  
end