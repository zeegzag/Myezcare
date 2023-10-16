CREATE PROCEDURE [dbo].[HC_BypassActionTaken]
@EmployeeVisitID BIGINT,
@RejectReason NVARCHAR(1000),
@ActionTaken INT
AS
BEGIN
	UPDATE EmployeeVisits SET  ActionTaken=@ActionTaken, RejectReason=@RejectReason WHERE EmployeeVisitID=@EmployeeVisitID
END
