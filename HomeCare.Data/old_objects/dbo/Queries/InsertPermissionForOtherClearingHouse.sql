
--UpdatedBy: Akhilesh kamal
--UpdatedDate:09 OCT 2020
-- Decsription: Insert Other permission for Billing & Claim Processing in permissions table
        
Declare @pID int
Declare @id int
declare @ParentID int 
select @ParentID=PermissionID from Permissions where-- PermissionName='Billing' and
 PermissionCode='Billing'
Select @id = coalesce(MAX(PermissionID),0) + 1 FROM Permissions; 

print @ParentID
print @id

INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID],
 [OrderID], [IsDeleted], [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform])
 VALUES (@id, N'Other', N'OtherClearingHouse', @ParentID, 0, 0, 1, N'OtherClearingHouse', 1, N'Web')

 -- Mapping with RolePermissionMapping Table
 insert RolePermissionMapping (RoleID,PermissionID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,IsDeleted) 
 values(1,@id,getdate(),1,'','',1,0)


 go