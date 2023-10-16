-- CHK_GetVisitChecklistItemDetail 30242, 24254
CREATE PROCEDURE [dbo].[CHK_GetVisitChecklistItemDetail]
	@EmployeeVisitID BIGINT,
	@ScheduleID BIGINT,
	@ReferralID BIGINT
AS
BEGIN

	DECLARE @IsCareTypeAssigned BIT
	SELECT
		@IsCareTypeAssigned = CASE WHEN CareTypeIds IS NOT NULL THEN 1 ELSE 0 END
	FROM
		Referrals
	WHERE
		ReferralID = @ReferralID

	IF(@EmployeeVisitID != 0)
	BEGIN
		SELECT
			EV.EmployeeVisitID,
			IsCareTypeAssigned = @IsCareTypeAssigned,
			CASE WHEN ClockInTime IS NOT NULL THEN 1 ELSE 0 END AS IsClockInCompleted,
			CASE WHEN ClockOutTime IS NOT NULL THEN 1 ELSE 0 END AS IsClockOutCompleted,
			IsSigned,
			IsPCACompleted AS IsCompleted,
			SM.StartDate,
			SM.EndDate
		FROM
			EmployeeVisits EV
			INNER JOIN ScheduleMasters SM ON EV.ScheduleID = SM.ScheduleID
		WHERE
			EV.EmployeeVisitID = @EmployeeVisitID
	END
	ELSE
	BEGIN
		SELECT
			0 AS EmployeeVisitID,
			IsCareTypeAssigned = @IsCareTypeAssigned,
			0 AS IsClockInCompleted,
			0 AS IsClockOutCompleted,
			0 AS IsSigned,
			0 AS IsCompleted,
			SM.StartDate,
			SM.EndDate
		FROM
			ScheduleMasters SM
		WHERE
			SM.ScheduleID = @ScheduleID
	END
END
