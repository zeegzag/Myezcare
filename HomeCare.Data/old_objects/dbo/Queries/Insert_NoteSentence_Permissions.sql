
--UpdatedBy: Akhilesh kamal
--UpdatedDate: 8 may 2020
-- Decsription: Insert NoteSentence permission in permissions table
 BEGIN     
 
Declare @pID int
Declare @id int
declare @ParentID int 
select @ParentID=ParentID from Permissions where PermissionCode='Masters_VisitTask_List'
Select @id = coalesce(MAX(PermissionID),0) + 1 FROM Permissions; 

print @ParentID
print @id
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted],
 [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) 
 VALUES (@id, N'NoteSentence', N'NoteSentenceAccess', @ParentID, 0, 0, 1, N'Masters_NoteSentence', 1, N'Web')


 select @pID = @id
 Select @id = coalesce(MAX(PermissionID),0) + 1 FROM Permissions;

 
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID],
 [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform])
 VALUES (@id, N'Add/Update', N'NoteSentence add/update access', @pID, 0, 0, 1, N'Masters_NoteSentence_AddUpdate', 1, N'Web')

 
 Select @id = coalesce(MAX(PermissionID),0) + 1 FROM Permissions;
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID],
 [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform])
 VALUES (@id, N'List', N'NoteSentence list', @pID, 0, 0, 1, N'Masters_NoteSentence_List', 1, N'Web')

END