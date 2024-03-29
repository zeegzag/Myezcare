--UpdatedBy: Abhishek Gautam
--UpdatedDate: 8 Dec 2020
-- Decsription: Insert Notes permission in Patient list
 BEGIN     
 
Declare @pID int
Declare @id int
declare @ParentID int 
--select @ParentID=PermissionID from Permissions where PermissionCode='PatientIntake'
Select @id = coalesce(MAX(PermissionID),0) + 1 FROM Permissions; 

--print @ParentID
print @id
INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted],
 [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) 
 VALUES (@id, N'Invoice', N'Invoice Access', 0, 0, 0, 1, N'Invoice', 1, N'Web')

 select @ParentID=PermissionID from Permissions where PermissionCode='Invoice'
Select @id = coalesce(MAX(PermissionID),0) + 1 FROM Permissions; 

INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted],
 [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) 
 VALUES (@id, N'InvoiceList', N'InvoiceList Access', @ParentID, 0, 0, 1, N'InvoiceList', 1, N'Web')

   insert RolePermissionMapping (RoleID,PermissionID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,IsDeleted)
 values(1,@id,getdate(),1,'','','',0)

END