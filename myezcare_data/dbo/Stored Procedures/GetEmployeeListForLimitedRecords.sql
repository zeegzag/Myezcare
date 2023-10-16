 CREATE PROCEDURE [dbo].[GetEmployeeListForLimitedRecords] 
 @Name VARCHAR(100) = NULL,        
 @Email VARCHAR(50) = NULL,        
 @DepartmentID BIGINT = 0,        
 @RoleID BIGINT=0,        
 @IsDepartmentSupervisor INT = -1,        
 @Degree varchar(100)=null,      
 @CredentialID varchar(50)=null,  
 @MobileNumber varchar(10)=null,      
 @Address varchar(50)=null,      
 @IsDeleted int=-1,        
 @SortExpression NVARCHAR(100),          
 @SortType NVARCHAR(10),        
 @FromIndex INT,        
 @PageSize INT  ,                
 @EmployeeId INT        
AS        
BEGIN            
    
  SELECT e.EmployeeID, dbo.GetGeneralNameFormat(e.FirstName ,e.LastName) as Name,e.UserName,e.Email,e.IsSecurityQuestionSubmitted,        
   e.IsDepartmentSupervisor, d.DepartmentName, R.RoleName,e.IsDeleted,C.CredentialName,E.Degree, E.EmployeeSignatureID, E.MobileNumber,E.Address,E.City,E.ZipCode,E.StateCode       
   FROM  employees e         
   LEFT JOIN Departments d on e.DepartmentID=d.DepartmentID        
   LEFT JOIN Roles R ON R.RoleID = E.RoleID        
   LEFT JOIN EmployeeCredentials C ON C.CredentialID=E.CredentialID 
   WHERE e.EmployeeID  = @EmployeeId
END