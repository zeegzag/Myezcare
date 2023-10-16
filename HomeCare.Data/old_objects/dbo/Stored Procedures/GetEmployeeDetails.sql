--Exec GetEmployeeDetails @EmployeeID=167  
CREATE PROCEDURE [dbo].[GetEmployeeDetails]
 @EmployeeID BIGINT=0           
AS              
BEGIN              
 SELECT EmployeeID,EmployeeUniqueID,FirstName,MiddleName,LastName,Email,UserName,Password,PasswordSalt,PhoneWork,PhoneHome,DepartmentID,IsDepartmentSupervisor,SecurityQuestionID,SecurityAnswer,
  IsActive,IsVerify,RoleID,IsSecurityQuestionSubmitted,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,IsDeleted,EmpSignature,EmployeeSignatureID,CredentialID,  
  Degree,Department,Roles,MobileNumber,Address,City,StateCode,ZipCode  
 FROM Employees Where EmployeeID=@EmployeeID  
END 
