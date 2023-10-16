CREATE PROCEDURE [dbo].[CreateAndSaveBatchForSchedule]
	@ScheduleID BIGINT
AS
BEGIN
	DECLARE @EmployeeVisitID BIGINT

	SELECT @EmployeeVisitID = EV.EmployeeVisitID
	FROM EmployeeVisits EV
	LEFT JOIN Notes N ON N.IsDeleted = 0
		AND N.EmployeeVisitID = EV.EmployeeVisitID
	LEFT JOIN BatchNotes BND
	    ON BND.NoteID = N.NoteID
	WHERE EV.IsDeleted = 0
		AND EV.ScheduleID = @ScheduleID
		AND EV.IsPCACompleted = 1
		AND BND.NoteID IS NULL

	IF ISNULL(@EmployeeVisitID, 0) > 0
	BEGIN
		DECLARE @PayorID BIGINT
			,@ReferralID BIGINT
			,@StartDate DATETIME
			,@EndDate DATETIME

		SELECT @PayorID = SM.PayorID
			,@ReferralID = SM.ReferralID
			,@StartDate = SM.StartDate
			,@EndDate = SM.EndDate
		FROM ScheduleMasters SM
		WHERE SM.ScheduleID = @ScheduleID
			AND ISNULL(SM.OnHold, 0) = 0 AND SM.IsDeleted = 0

		DECLARE @Result INT

		EXEC @Result = [dbo].[API_AddNoteByCareType] @EmployeeVisitID = @EmployeeVisitID, @ResultRequired = 0

		IF (@Result = 0)
		BEGIN
			EXEC HC_RefreshAndGroupingNotes @ResultRequired = 0

			EXEC [dbo].[HC_SaveNewBatch] @BatchID = 0
				,@BatchTypeID = 1
				,@PayorID = @PayorID
				,@ServiceCodeIDs = ''
				,@ReferralsIds = @ReferralID
				,@StartDate = @StartDate
				,@EndDate = @EndDate
				,@Comment = ''
				,@CreatedBy = 0
				,@BatchNoteStatusID = 1
				,@IsDayCare = 0                    
				,@IsCaseManagement = 0
				,@ResultRequired = 0
		END
	END
END
