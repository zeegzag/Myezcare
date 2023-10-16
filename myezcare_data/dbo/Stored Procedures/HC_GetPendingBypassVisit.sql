CREATE PROCEDURE [dbo].[HC_GetPendingBypassVisit]
@ActionTaken INT,
@IsApprovalRequired BIT
AS
BEGIN
	SELECT COUNT(EmployeeVisitID) FROM EmployeeVisits WHERE IsApprovalRequired=@IsApprovalRequired AND ActionTaken=@ActionTaken
END
