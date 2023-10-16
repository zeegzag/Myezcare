-- =============================================
-- Author:		Ashar
-- Create date: 23 Nov 2021
-- Description:	SP to get employee groups
-- =============================================
CREATE PROCEDURE [dbo].[API_GetEmployeeGroups]
	@GroupName varchar(max),
	@EmployeeID bigint
AS

	DECLARE @EmployeeGroupIds varchar(max)
	SELECT @EmployeeGroupIds = GroupIDs from Employees WHERE EmployeeID = @EmployeeID

	select d.Title as Title,d.DDMasterID AS Value
	from DDMaster d
	inner join lu_DDMasterTypes ld on ld.DDMasterTypeID=d.ItemType
	where d.ItemType=ld.DDMasterTypeID
	and ld.Name=@GroupName
	and d.isdeleted=0 AND DDMasterID IN (SELECT GL.[val] FROM dbo.GetCSVTable(@EmployeeGroupIds) GL)
	order by d.Title