
BEGIN
DECLARE @PermissionIDMAX BIGINT;
DECLARE @PermissionIDParent BIGINT;
SELECT @PermissionIDParent=PermissionID FROM [dbo].[Permissions] WHERE PermissionName='Masters' AND UsedInHomeCare=1 
--print @PermissionIDParent

SET @PermissionIDMAX=(SELECT MAX(PermissionID) FROM [dbo].[Permissions])
--print @PermissionIDMAX


INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES (@PermissionIDMAX+1, N'CaseManager', N'CaseManager access', @PermissionIDParent, 28, 0, 1, N'Masters_CaseManager', 1, N'Web')


INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES ((@PermissionIDMAX+2), N'Add/Update', N'CaseManager add/update access',( @PermissionIDMAX+1), NULL, 0, 1, N'Masters_CaseManager_AddUpdate', 1, N'Web')

INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) VALUES ((@PermissionIDMAX+3), N'List', N'CaseManager list access', (@PermissionIDMAX+1), NULL, 0, 1, N'Masters_CaseManager_List', 1, N'Web')

END

--
