CREATE PROCEDURE [dbo].[HC_GetEmployeeDetails]                    
@EmployeeID BIGINT,                    
@PreferenceType_Preference VARCHAR(100),                    
@PreferenceType_Skill VARCHAR(100) ,            
@DDType_CareType int =1,                                         
@DDType_Designation int =18                                         
AS                    
BEGIN                    
                    
 SELECT           
 EmployeeID,EmployeeUniqueID,FirstName,MiddleName,LastName,Email,UserName,Password,PasswordSalt,IVRPin,PhoneWork,PhoneHome,DepartmentID,          
 IsDepartmentSupervisor,SecurityQuestionID,SecurityAnswer,IsActive,IsVerify,RoleID,IsSecurityQuestionSubmitted,CreatedDate,CreatedBy,          
 UpdatedDate,UpdatedBy,SystemID,IsDeleted,EmpSignature,EmployeeSignatureID,CredentialID,Degree,Department,Roles,MobileNumber,Address,          
 City,StateCode,ZipCode,Latitude,Longitude,HHA_NPI_ID,ProfileImagePath ,Designation ,CareTypeIds,AssociateWith,ApartmentNo      
 FROM Employees Where EmployeeID=@EmployeeID                     
                    
 SELECT * FROM Departments WHERE IsDeleted=0 ORDER BY DepartmentName ASC                    
 SELECT * FROM SecurityQuestions                    
 SELECT * FROM Roles ORDER BY RoleName ASC                    
 SELECT * FROM EmployeeCredentials ORDER BY CredentialID ASC                    
 SELECT * FROM States ORDER BY StateName ASC                    
                    
                    
 -- EMPLOYEE PREFERENCES                    
 SELECT EP.*,P.PreferenceName FROM EmployeePreferences EP                      
 INNER JOIN Preferences P ON EP.PreferenceID=p.PreferenceID                      
 WHERE EmployeeID=@EmployeeID AND P.KeyType=@PreferenceType_Preference                    
                    
                    
                    
 -- SKILLS MASTER                    
 IF(@EmployeeID=0)                    
 SELECT * FROM Preferences P WHERE P.KeyType=@PreferenceType_Skill AND IsDeleted=0                    
 ELSE    
 SELECT DISTINCT P.* FROM Preferences P    
 LEFT JOIN EmployeePreferences EP ON EP.PreferenceID=P.PreferenceID    
 WHERE P.KeyType=@PreferenceType_Skill AND ((EP.PreferenceID IS NOT NULL AND EP.EmployeeID=@EmployeeID) OR P.IsDeleted=0)  
 --AND IsDeleted=0                    
                    
                    
 SELECT EP.PreferenceID FROM EmployeePreferences EP                      
 INNER JOIN Preferences P ON EP.PreferenceID=p.PreferenceID                      
 WHERE EmployeeID=@EmployeeID AND P.KeyType=@PreferenceType_Skill                    
                    
 SELECT DocumentTypeID=ComplianceID,DocumentTypeName=DocumentName FROM Compliances WHERE UserType=1 AND IsDeleted=0              
               
 --SELECT Name=Title,ID=DDMasterID FROM DDMaster where IsDeleted=0 and ItemType=@DDType_CareType         
 --SELECT Name=Title,ID=DDMasterID FROM DDMaster where IsDeleted=0 and ItemType=@DDType_Designation   
  
 EXEC GetGeneralMasterList @DDType_CareType  
 EXEC GetGeneralMasterList @DDType_Designation   
                    
END  