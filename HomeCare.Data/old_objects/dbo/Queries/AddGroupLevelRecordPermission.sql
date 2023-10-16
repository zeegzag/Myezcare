BEGIN     
 
DECLARE @ID INT, @ParentID INT, @ParentCode NVARCHAR(MAX), @PermissionCode NVARCHAR(MAX);

--- Notification Preferences (Employee > Add/Update)

SELECT @ParentID = NULL, @ParentCode = 'Masters_Employee';
SELECT @PermissionCode = 'EmployeeRecord_Access_Group_And_Limited_Record';
SELECT @ParentID = [PermissionID] FROM [dbo].[Permissions] WHERE PermissionCode = @ParentCode
SELECT @ID = COALESCE(MAX(PermissionID),0) + 1 FROM [dbo].[Permissions]; 

INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) 
SELECT @ID, N'Group Level Record', N'Group Level Record Access', @ParentID, 0, 0, 1,@PermissionCode, 1 , N'Web' 
	WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Permissions] WHERE PermissionCode = @PermissionCode)
 
SELECT @ParentID = NULL, @ParentCode = 'PatientIntake';
SELECT @PermissionCode = 'PatientRecord_Access_Group_And_Limited_Record';
SELECT @ParentID = [PermissionID] FROM [dbo].[Permissions] WHERE PermissionCode = @ParentCode
SELECT @ID = COALESCE(MAX(PermissionID),0) + 1 FROM [dbo].[Permissions]; 

INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) 
SELECT @ID, N'Group Level Record', N'Group Level Record Access', @ParentID, 1, 0, 1, @PermissionCode, 1 , N'Web' 
	WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Permissions] WHERE PermissionCode = @PermissionCode)
 

END