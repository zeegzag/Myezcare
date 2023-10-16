Declare @id int
declare @ParentID int 
select @ParentID=PermissionID from Permissions where PermissionCode='Scheduling' and UsedInHomeCare=1 and ParentID=0 and IsDeleted=0
Select @id = coalesce(MAX(PermissionID),0) + 1 FROM Permissions; 

INSERT [dbo].[Permissions] ([PermissionID], [PermissionName], [Description], [ParentID], [OrderID], [IsDeleted],
 [UsedInHomeCare], [PermissionCode], [CompanyHasAccess], [PermissionPlatform]) 
 VALUES (@id, N'Visitor Attendance', N'Visitor Attendance', @ParentID, 0, 0, 1, N'Scheduling_Visitor_Attendance', 1, N'Web')