CREATE TABLE [dbo].[Employees] (
    [EmployeeID]                     BIGINT         IDENTITY (1, 1) NOT NULL,
    [EmployeeUniqueID]               VARCHAR (100)  NULL,
    [FirstName]                      VARCHAR (50)   NOT NULL,
    [MiddleName]                     VARCHAR (50)   NULL,
    [LastName]                       VARCHAR (50)   NOT NULL,
    [Email]                          VARCHAR (50)   NULL,
    [UserName]                       VARCHAR (50)   NOT NULL,
    [Password]                       VARCHAR (100)  NULL,
    [PasswordSalt]                   VARCHAR (MAX)  NULL,
    [PhoneWork]                      VARCHAR (20)   NULL,
    [PhoneHome]                      VARCHAR (20)   NULL,
    [DepartmentID]                   BIGINT         NULL,
    [IsDepartmentSupervisor]         BIT            NULL,
    [SecurityQuestionID]             BIGINT         NULL,
    [SecurityAnswer]                 VARCHAR (50)   NULL,
    [IsActive]                       BIT            NOT NULL,
    [IsVerify]                       BIT            NOT NULL,
    [RoleID]                         BIGINT         NOT NULL,
    [IsSecurityQuestionSubmitted]    BIT            NOT NULL,
    [CreatedDate]                    DATETIME       NOT NULL,
    [CreatedBy]                      BIGINT         NOT NULL,
    [UpdatedDate]                    DATETIME       NOT NULL,
    [UpdatedBy]                      BIGINT         NOT NULL,
    [SystemID]                       VARCHAR (100)  NOT NULL,
    [IsDeleted]                      BIT            NOT NULL,
    [EmpSignature]                   VARCHAR (MAX)  NULL,
    [EmployeeSignatureID]            BIGINT         NULL,
    [CredentialID]                   VARCHAR (50)   NULL,
    [Degree]                         VARCHAR (100)  NULL,
    [Department]                     VARCHAR (200)  NULL,
    [Roles]                          VARCHAR (200)  NULL,
    [MobileNumber]                   VARCHAR (20)   NULL,
    [Address]                        VARCHAR (100)  NULL,
    [City]                           VARCHAR (50)   NULL,
    [StateCode]                      VARCHAR (10)   NULL,
    [ZipCode]                        VARCHAR (15)   NULL,
    [Latitude]                       FLOAT (53)     NULL,
    [Longitude]                      FLOAT (53)     NULL,
    [IVRPin]                         VARCHAR (4)    NULL,
    [LoginFailedCount]               INT            CONSTRAINT [DF__Employees__Login__5C77A3CF] DEFAULT ((0)) NOT NULL,
    [HHA_NPI_ID]                     VARCHAR (50)   NULL,
    [IsFingerPrintAuth]              BIT            DEFAULT ((0)) NOT NULL,
    [ProfileImagePath]               VARCHAR (300)  NULL,
    [DeviceType]                     VARCHAR (10)   NULL,
    [FcmTokenId]                     NVARCHAR (MAX) NULL,
    [PayPerHour]                     INT            CONSTRAINT [DF__Employees__PayPe__28E2F130] DEFAULT ((0)) NOT NULL,
    [Designation]                    BIGINT         NULL,
    [CareTypeIds]                    VARCHAR (MAX)  NULL,
    [AssociateWith]                  NVARCHAR (MAX) NULL,
    [DeviceOSVersion]                VARCHAR (50)   NULL,
    [ApartmentNo]                    NVARCHAR (50)  NULL,
    [DateOfBirth]                    DATETIME       NULL,
    [FacilityID]                     BIGINT         NULL,
    [SocialSecurityNumber]           VARCHAR (MAX)  NULL,
    [DesignationName]                NVARCHAR (MAX) NULL,
    [RegularHours]                   DECIMAL (18)   NULL,
    [RegularPayHours]                FLOAT (53)     NULL,
    [OvertimePayHours]               FLOAT (53)     NULL,
    [GroupIDs]                       NVARCHAR (MAX) NULL,
    [RegularHourType]                INT            NULL,
    [HireDate]                       DATETIME       NULL,
    [EmpGender]                      NVARCHAR (MAX) NULL,
    [StateRegistrationID]            NVARCHAR (MAX) NULL,
    [ProfessionalLicenseNumber]      NVARCHAR (MAX) NULL,
    [HHAXLastSent]                   DATETIME       NULL,
    [IsTermsConditionMobileAccepted] BIT            DEFAULT ((0)) NOT NULL,
    [IsFirstTimeLogin]               BIT            DEFAULT ((0)) NOT NULL,
    [SandataLastSent]                DATETIME       NULL,
    [CanUpdateCoordinate] BIT NOT NULL DEFAULT ((0)), 
    CONSTRAINT [PK_Employees] PRIMARY KEY CLUSTERED ([EmployeeID] ASC),
    CONSTRAINT [FK_Employees_Departments] FOREIGN KEY ([DepartmentID]) REFERENCES [dbo].[Departments] ([DepartmentID]),
    CONSTRAINT [FK_Employees_EmployeeCredentials] FOREIGN KEY ([CredentialID]) REFERENCES [dbo].[EmployeeCredentials] ([CredentialID]),
    CONSTRAINT [FK_Employees_Roles] FOREIGN KEY ([RoleID]) REFERENCES [dbo].[Roles] ([RoleID]),
    CONSTRAINT [FK_Employees_SecurityQuestions] FOREIGN KEY ([SecurityQuestionID]) REFERENCES [dbo].[SecurityQuestions] ([SecurityQuestionID])
);








GO

CREATE TRIGGER [dbo].[tr_Employees_Deleted] ON [dbo].[Employees]
FOR Delete AS 

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
'D',GETUTCDATE() FROM deleted
GO

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
GO
CREATE NONCLUSTERED INDEX [IX_Employees01]
    ON [dbo].[Employees]([IsDeleted] ASC)
    INCLUDE([FirstName], [LastName]);


GO
CREATE NONCLUSTERED INDEX [IX_Employees003]
    ON [dbo].[Employees]([IsActive] ASC, [IsDeleted] ASC)
    INCLUDE([FirstName], [LastName], [MobileNumber]);


GO
CREATE NONCLUSTERED INDEX [IX_Employees002]
    ON [dbo].[Employees]([IsActive] ASC)
    INCLUDE([FirstName], [LastName], [IsDeleted]);


GO
CREATE NONCLUSTERED INDEX [IX_Employees001]
    ON [dbo].[Employees]([IsDeleted] ASC)
    INCLUDE([FirstName], [LastName], [MobileNumber]);

