-- =============================================
-- Author:		Ashar
-- Create date: 23 Nov 2021
-- Description:	SP to get employee caretypes
-- =============================================
CREATE PROCEDURE [dbo].[API_GetEmployeeCareTypes]
	@DDTypeCareTypeID INT,
	@EmployeeID bigint
AS

DECLARE @EmployeeCareTypeIds varchar(max)
SELECT @EmployeeCareTypeIds = CareTypeIds from Employees WHERE EmployeeID = @EmployeeID

SELECT CareTypeID=DDMasterID,CareTypeName=Title FROM DDMaster WHERE 
ItemType=@DDTypeCareTypeID AND IsDeleted=0 AND DDMasterID IN (SELECT GL.[val]
						FROM dbo.GetCSVTable(@EmployeeCareTypeIds) GL)