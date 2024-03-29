
--CreatedBy: Akhilesh kamal
--CreatedDate: 20 jan 2021
-- Decsription: Insert Permission_ReferralTracking permission in permissions table
 BEGIN     
 
Declare @pID int
Declare @id int
declare @ParentID int 
Select @id = coalesce(MAX(PermissionID),0) + 1 FROM Permissions; 
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted],
 [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) 
 VALUES (@id, N'Referral Tracking', N'Referral Tracking', 0, 0, 0, 1, N'ReferralTracking', 1, N'Web')

   insert RolePermissionMapping (RoleID,PermissionID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,IsDeleted)
 values(1,@id,getdate(),1,'','','',0)
 ---------------------------------------------------------------------------------------------------------------
select @ParentID=PermissionID from Permissions where PermissionCode='ReferralTracking'
Select @id = coalesce(MAX(PermissionID),0) + 1 FROM Permissions; 


INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted],
 [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) 
 VALUES (@id, N'Add/Update', N'Referral Tracking Add/Update', @ParentID, 0, 0, 1, N'ReferralTrackingAdd', 1, N'Web')

   insert RolePermissionMapping (RoleID,PermissionID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,IsDeleted)
 values(1,@id,getdate(),1,'','','',0)
 ------------------------------------------------------------------------------------------------------------------
 select @ParentID=PermissionID from Permissions where PermissionCode='ReferralTracking'
Select @id = coalesce(MAX(PermissionID),0) + 1 FROM Permissions; 


INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted],
 [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) 
 VALUES (@id, N'List', N'Referral Tracking List', @ParentID, 0, 0, 1, N'ReferralTrackingList', 1, N'Web')

   insert RolePermissionMapping (RoleID,PermissionID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,IsDeleted)
 values(1,@id,getdate(),1,'','','',0)
 --------------------------------------------------------------------------------------------------------------------
  select @ParentID=PermissionID from Permissions where PermissionCode='ReferralTracking'
Select @id = coalesce(MAX(PermissionID),0) + 1 FROM Permissions; 


INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted],
 [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) 
 VALUES (@id, N'Delete', N'Referral Tracking Delete', @ParentID, 0, 0, 1, N'ReferralTrackingDelete', 1, N'Web')

   insert RolePermissionMapping (RoleID,PermissionID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,IsDeleted)
 values(1,@id,getdate(),1,'','','',0)
 ---------------------------------------------------------------------------------------------------------------------
 select @ParentID=PermissionID from Permissions where PermissionCode='ReferralTracking'
Select @id = coalesce(MAX(PermissionID),0) + 1 FROM Permissions; 
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted],
 [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) 
 VALUES (@id, N'Attach/View Form', N'Attach/View Form', @ParentID, 0, 0, 1, N'AttachForm', 1, N'Web')

   insert RolePermissionMapping (RoleID,PermissionID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,IsDeleted)
 values(1,@id,getdate(),1,'','','',0)
END