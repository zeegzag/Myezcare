CREATE PROCEDURE [dbo].[API_GetProfileDetail]              
 @EmployeeID BIGINT                            
AS                            
BEGIN                      
   DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()                    
DECLARE @TimeZone NVARCHAR(200)              
DECLARE @OrganizationTwilioNumber NVARCHAR(20)          
DECLARE @AndroidMinimumVersion VARCHAR(100)          
DECLARE @AndroidCurrentVersion VARCHAR(100)          
              
SELECT TOP 1 @TimeZone=TimeZone,@OrganizationTwilioNumber=TwilioFromNo,@AndroidMinimumVersion=AndroidMinimumVersion,          
@AndroidCurrentVersion=AndroidCurrentVersion FROM OrganizationSettings          
              
                      
SELECT e.EmployeeID,e.FirstName,e.MiddleName,e.LastName,e.EmployeeSignatureID,es.SignaturePath AS EmployeeSignatureURL,e.IsFingerPrintAuth,                    
 e.ProfileImagePath AS EmployeeProfileImgURL,FullName=dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat),@TimeZone AS TimeZone,e.MobileNumber,e.IVRPin,e.UserName,            
 @OrganizationTwilioNumber AS OrganizationTwilioNumber,@AndroidCurrentVersion AS AndroidCurrentVersion,@AndroidMinimumVersion AS AndroidMinimumVersion,HireDates=CONVERT(DATE,E.HireDate)      
 FROM Employees e                            
 LEFT JOIN EmployeeSignatures es ON es.EmployeeSignatureID=e.EmployeeSignatureID                            
 WHERE e.EmployeeID=@EmployeeID                            
END 