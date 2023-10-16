INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted],
 [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) 
 VALUES (5000, N'RAL', N'RAL', 0, 0, 0, 1, N'Mobile_RAL', 1, N'Mobile')

INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID],
 [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform])
 VALUES (5001, N'GeoFencing Required', N'RAL', 5000, 0, 0, 1, N'Mobile_RAL_GeoFencing_Required', 1, N'Mobile')

INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID],
 [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform])
 VALUES (5002, N'Default Schedule Time', N'RAL', 5000, 0, 0, 1, N'Mobile_RAL_Default_Schedule', 1, N'Mobile')

INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID],
[OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) 
VALUES (5003, N'1 hr', N'Default Schedule Time', 5002, 0, 0, 1, N'Mobile_RAL_Default_Schedule_1_hr', 1, N'Mobile')

INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID],
[OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) 
VALUES (5004, N'2 hr', N'Default Schedule Time', 5002, 0, 0, 1, N'Mobile_RAL_Default_Schedule_2_hr', 1, N'Mobile')

INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID],
[OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) 
VALUES (5005, N'3 hr', N'Default Schedule Time', 5002, 0, 0, 1, N'Mobile_RAL_Default_Schedule_3_hr', 1, N'Mobile')

INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID],
[OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) 
VALUES (5006, N'4 hr', N'Default Schedule Time', 5002, 0, 0, 1, N'Mobile_RAL_Default_Schedule_4_hr', 1, N'Mobile')

--AdminDB
Update Organizations Set OrganizationType='HomeCare,CaseManagement,DayCare,RAL' where DomainName='localhost'

