-- Author: Fenil Gandhi
-- Date: 09 Jul 2020
-- Decsription: Insert 'Notification Configuration' and 'Notification Preferences' in permissions table
BEGIN     
 
DECLARE @ID INT, @ParentID INT;

--- Notification Preferences (Employee > Add/Update)

SELECT @ParentID = [PermissionID] FROM [dbo].[Permissions] WHERE PermissionCode = 'Masters_Employee_AddUpdate'
SELECT @ID = COALESCE(MAX(PermissionID),0) + 1 FROM [dbo].[Permissions]; 

INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) 
	VALUES (@ID, N'Notification Preferences', N'Notification Preferences Access', @ParentID, 0, 0, 1, N'Employee_Notification_Preferences', 1 , N'Web')
 
SELECT @ParentID = @ID
SELECT @ID = COALESCE(MAX(PermissionID),0) + 1 FROM [dbo].[Permissions]; 

INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) 
	VALUES (@ID, N'Update', N'Update Access', @ParentID, 1, 0, 1, N'Employee_Notification_Preferences_Update', 1 , N'Web')
 

SELECT @ID = COALESCE(MAX(PermissionID),0) + 1 FROM [dbo].[Permissions]; 

INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) 
	VALUES (@ID, N'View Only', N'View Only Access', @ParentID, 1, 0, 1, N'Employee_Notification_Preferences_View', 1 , N'Web')
 
 --- Notification Configuration (Masters)

SELECT @ParentID = [PermissionID] FROM [dbo].[Permissions] WHERE PermissionCode = 'Masters'
SELECT @ID = COALESCE(MAX(PermissionID),0) + 1 FROM [dbo].[Permissions]; 

INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) 
	VALUES (@ID, N'Notification Configuration', N'Notification Configuration Access', @ParentID, 29, 0, 1, N'Notification_Configuration', 1 , N'Web')
 
SELECT @ParentID = @ID
SELECT @ID = COALESCE(MAX(PermissionID),0) + 1 FROM [dbo].[Permissions]; 

INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) 
	VALUES (@ID, N'Update', N'Notification Configuration Update Access', @ParentID, 1, 0, 1, N'Notification_Configuration_Update', 1 , N'Web')
 

SELECT @ID = COALESCE(MAX(PermissionID),0) + 1 FROM [dbo].[Permissions]; 

INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) 
	VALUES (@ID, N'View Only', N'Notification Configuration View Only Access', @ParentID, 2, 0, 1, N'Notification_Configuration_ViewOnly', 1 , N'Web')


--- Web Notifications (Dashboard)

SELECT @ParentID = [PermissionID] FROM [dbo].[Permissions] WHERE PermissionCode = 'Dashboard'
SELECT @ID = COALESCE(MAX(PermissionID),0) + 1 FROM [dbo].[Permissions]; 

INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) 
	VALUES (@ID, N'Web Notifications', N'Dashboard Web Notifications Access', @ParentID, NULL, 0, 1, N'Dashboard_WebNotifications', 1 , N'Web')

END