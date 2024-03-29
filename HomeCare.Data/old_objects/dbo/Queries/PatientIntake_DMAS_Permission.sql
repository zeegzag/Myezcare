
--UpdatedBy: Akhilesh kamal
--UpdatedDate: 8 may 2020
-- Decsription: Insert NoteSentence permission in permissions table
 BEGIN     
 
Declare @pID int
Declare @id int
declare @ParentID int 
select @ParentID=PermissionID from Permissions where PermissionCode='PatientIntake_Forms'
Select @id = coalesce(MAX(PermissionID),0) + 1 FROM Permissions; 

print @ParentID
print @id
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted],
 [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) 
 VALUES (@id, N'DMAS97', N'Forms Access', @ParentID, 0, 0, 1, N'PatientIntake_DMAS97', 1, N'Web')


 select @pID = @id
 Select @id = coalesce(MAX(PermissionID),0) + 1 FROM Permissions;

 
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID],
 [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform])
 VALUES (@id, N'View Only', N'View Only Access', @pID, 0, 0, 1, N'PatientIntake_DMAS97_View', 1, N'Web')

  Select @id = coalesce(MAX(PermissionID),0) + 1 FROM Permissions;
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID],
 [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform])
 VALUES (@id, N'Add/Update', N'Add/Update Access', @pID, 0, 0, 1, N'PatientIntake_DMAS97_Add', 1, N'Web')

   Select @id = coalesce(MAX(PermissionID),0) + 1 FROM Permissions;
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID],
 [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform])
 VALUES (@id, N'Delete', N'Delete', @pID, 0, 0, 1, N'PatientIntake_DMAS97_Delete', 1, N'Web')
 ----------------------------------------------------------------------------------------------------------
 select @ParentID=PermissionID from Permissions where PermissionCode='PatientIntake_Forms'
Select @id = coalesce(MAX(PermissionID),0) + 1 FROM Permissions; 
 INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted],
 [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) 
 VALUES (@id, N'DMAS99', N'Forms Access', @ParentID, 0, 0, 1, N'PatientIntake_DMAS99', 1, N'Web')


 select @pID = @id
 Select @id = coalesce(MAX(PermissionID),0) + 1 FROM Permissions;

 
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID],
 [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform])
 VALUES (@id, N'View Only', N'View Only Access', @pID, 0, 0, 1, N'PatientIntake_DMAS99_View', 1, N'Web')

  Select @id = coalesce(MAX(PermissionID),0) + 1 FROM Permissions;
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID],
 [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform])
 VALUES (@id, N'Add/Update', N'Add/Update Access', @pID, 0, 0, 1, N'PatientIntake_DMAS99_Add', 1, N'Web')

   Select @id = coalesce(MAX(PermissionID),0) + 1 FROM Permissions;
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID],
 [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform])
 VALUES (@id, N'Delete', N'Delete', @pID, 0, 0, 1, N'PatientIntake_DMAS99_Delete', 1, N'Web')
 -------------------------------------------------------------------------------------------------------------------
 select @ParentID=PermissionID from Permissions where PermissionCode='PatientIntake_Forms'
Select @id = coalesce(MAX(PermissionID),0) + 1 FROM Permissions; 
 INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted],
 [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) 
 VALUES (@id, N'CMS485', N'Forms Access', @ParentID, 0, 0, 1, N'PatientIntake_CMS485', 1, N'Web')


 select @pID = @id
 Select @id = coalesce(MAX(PermissionID),0) + 1 FROM Permissions;

 
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID],
 [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform])
 VALUES (@id, N'View Only', N'View Only Access', @pID, 0, 0, 1, N'PatientIntake_CMS485_View', 1, N'Web')

  Select @id = coalesce(MAX(PermissionID),0) + 1 FROM Permissions;
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID],
 [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform])
 VALUES (@id, N'Add/Update', N'Add/Update Access', @pID, 0, 0, 1, N'PatientIntake_CMS485_Add', 1, N'Web')

  Select @id = coalesce(MAX(PermissionID),0) + 1 FROM Permissions;
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID],
 [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform])
 VALUES (@id, N'Delete', N'Delete', @pID, 0, 0, 1, N'PatientIntake_CMS485_Delete', 1, N'Web')

 


END