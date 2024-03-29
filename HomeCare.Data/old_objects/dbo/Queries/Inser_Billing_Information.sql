
--UpdatedBy: Akhilesh kamal
--UpdatedDate:2 jun 2020
-- Decsription: Insert Billing information permission in permissions table
        
Declare @pID int
Declare @id int
declare @ParentID int 
select @ParentID=PermissionID from Permissions where PermissionName='Masters' and PermissionCode='Masters'
Select @id = coalesce(MAX(PermissionID),0) + 1 FROM Permissions; 


INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted],
 [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) 
 VALUES (@id, N'Billing_Information', N'Billing_Information', @ParentID, 0, 0, 1, N'Billing_Information', 1, N'Web')


 select @pID = @id
 Select @id = coalesce(MAX(PermissionID),0) + 1 FROM Permissions;

INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID],
 [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform])
 VALUES (@id, N'View', N'View_Billing_Information', @pID, 0, 0, 1, N'View_Billing_Information', 1, N'Web')

 -- Mapping with RolePermissionMapping Table
 insert RolePermissionMapping (RolePermissionMappingID,RoleID,PermissionID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,IsDeleted) 
 values('',1,@id,getdate(),1,'','',0)


 go