
CREATE TRIGGER [dbo].[tr_Employees_Updated] ON [dbo].[Employees]
FOR UPDATE AS 
Declare @Cnt int
select @cnt=isDeleted from inserted
--print @Cnt
IF(@cnt= 1)
BEGIN
	UPDATE Employees SET IsActive = 0 where EmployeeID in (SELECT TOP 1 EmployeeID FROM deleted )
	DELETE FROM ScheduleMasters WHERE EmployeeID in (SELECT TOP 1 EmployeeID FROM deleted ) AND StartDate > GETDATE()
	--DELETE FROM EmployeeVisits WHERE ScheduleID IN (SELECT ScheduleID FROM ScheduleMasters WHERE EmployeeID = (SELECT TOP 1 EmployeeID FROM deleted ) AND StartDate > GETDATE())
END

INSERT INTO JO_Employees( 
EmployeeID,
FirstName,
MiddleName,
LastName,
Email,
UserName,
Password,
PasswordSalt,
PhoneWork,
PhoneHome,
DepartmentID,
IsDepartmentSupervisor,
SecurityQuestionID,
SecurityAnswer,
IsActive,
IsVerify,
RoleID,
IsSecurityQuestionSubmitted,
CreatedDate,
CreatedBy,
UpdatedDate,
UpdatedBy,
SystemID,
IsDeleted,
EmpSignature,
EmployeeSignatureID,
CredentialID,
Degree,
RegularHours,
RegularPayHours,
OvertimePayHours,
Action,ActionDate
)

SELECT  
EmployeeID,
FirstName,
MiddleName,
LastName,
Email,
UserName,
Password,
PasswordSalt,
PhoneWork,
PhoneHome,
DepartmentID,
IsDepartmentSupervisor,
SecurityQuestionID,
SecurityAnswer,
IsActive,
IsVerify,
RoleID,
IsSecurityQuestionSubmitted,
CreatedDate,
CreatedBy,
UpdatedDate,
UpdatedBy,
SystemID,
IsDeleted,
EmpSignature,
EmployeeSignatureID,
CredentialID,
Degree,
RegularHours,
RegularPayHours,
OvertimePayHours,
'U',GETUTCDATE()
  FROM deleted
