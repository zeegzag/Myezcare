
--UpdatedBy: Akhilesh kamal
--UpdatedDate:2 jun 2020
-- Decsription: Insert Billing information permission in permissions table
        
Declare @pID int
Declare @id int
declare @ParentID int 
declare @count int
select @ParentID=PermissionID from Permissions where PermissionCode='Reports'
Select @id = coalesce(MAX(PermissionID),0) + 1 FROM Permissions; 
select @count= COUNT(PermissionID) from Permissions where PermissionCode='SSRS Reports'

IF(@count=0)
BEGIN

INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted],
 [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) 
 VALUES (@id, N'SSRS Reports', N'SSRS reports access', @ParentID, 0, 0, 1, N'SSRS Reports', 1, N'Web')


 -- Mapping with RolePermissionMapping Table
 insert RolePermissionMapping (RoleID,PermissionID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,IsDeleted) 
 values(1,@id,getdate(),1,'','','',0)

 END
 ELSE
 BEGIN
 PRINT 'ERROR'
 END
 go