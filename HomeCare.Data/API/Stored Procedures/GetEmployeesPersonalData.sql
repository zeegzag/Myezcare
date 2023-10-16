--  EXEC [API].[GetEmployeesPersonalData] 11  
  
CREATE PROCEDURE [API].[GetEmployeesPersonalData]                                     
    @OrganizationID INT=0   
AS                                                          
BEGIN                     
    DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()              
 SELECT                           
  E.[EmployeeID],  
  E.[FirstName],  
  ISNULL(E.MiddleName, 'NA') AS [MiddleName],                    
  E.[LastName],  
  dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat) AS [FullName],  
  E.[UserName], E.[EmployeeUniqueID],  
  ISNULL(E.Email, 'NA') AS [Email],  
  CONVERT(date, DateOfBirth) AS [DateOfBirth],  
  E.[Designation],  
  ISNULL(E.Designation, 'NA') AS [Designation],  
  DATEDIFF(hour,DateOfBirth,GETDATE())/8766 AS Age,  
  ISNULL(E.MobileNumber, 'NA') AS [MobileNumber],  
  ISNULL(E.PhoneHome, 'NA') AS [PhoneHome],  
  ISNULL(E.PhoneWork, 'NA') AS [PhoneWork],  
  E.[AssociateWith], E.[RoleID],  
  CONCAT(E.ApartmentNo,' ',E.Address,' ',E.City,' ',E.StateCode,' ',E.ZipCode) AS [EmployeeAddress],  
  CONCAT(dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat),',',E.ApartmentNo,' ',E.Address,' ',E.City,' ',E.StateCode,' ',E.ZipCode) AS [EmployeeNameAndAddress],  
  ISNULL(E.[ApartmentNo], 'NA') AS [ApartmentNo],  
  E.[Address],  
  E.[City],  
  E.[StateCode],  
  E.[ZipCode],  
  CONCAT('https://','test','.myezcare.com', ISNUll(E.EmpSignature, '/Assets/images/DefaultSignatureImage.jpg')) AS [EmpSignature],  
  CONCAT('https://','test','.myezcare.com', ISNUll(E.ProfileImagePath, '/Assets/images/DefaultProfileImage.jpg')) AS [ProfileImagePath]  
         
 FROM [dbo].[Employees] E  
  WHERE E.[IsDeleted] = 0                        
                        
END 