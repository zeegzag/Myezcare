
--CreatedBy: Viswash
--CreatedDate:06 Feb 2020
--Description: For get complete timesheet report




--Declare @PermissionID int

--INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted],
--[UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform])
-- VALUES ((select max(permissionid)+1 from permissions), N'ActivePatient', N'ActivePatient', 2036, 94, 0, 1, N'ActivePatient', 1, N'Web')
--select  @permissionID=max(permissionid) from permissions

 
-- INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted],
--[UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform])
-- VALUES ((select max(permissionid)+1 from permissions), N'EmployeeTimeSheetReoprt', N'EmployeeTimeSheetReoprt', 2036, 94, 0, 1, N'EmployeeTimeSheetReoprt', 1, N'Web')
--select  @permissionID=max(permissionid) from permissions

