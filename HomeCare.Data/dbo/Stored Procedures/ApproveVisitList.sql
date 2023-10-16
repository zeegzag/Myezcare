CREATE PROCEDURE [dbo].[ApproveVisitList]  
 @SystemID NVARCHAR(MAX) = NULL  
 ,@LoggedInID BIGINT = NULL  
 ,@List  [dbo].[ApproveVisit] READONLY  
AS  
BEGIN  
	
	DECLARE @Output TABLE (  
		ScheduleID bigint
	  ) 
	  
	 INSERT INTO @Output   
	 SELECT EV.ScheduleID
	 FROM EmployeeVisits EV 
	 INNER JOIN @List L ON EV.EmployeeVisitID = L.[EmployeeVisitID]
	 WHERE
		EV.IsDeleted = 0 AND EV.IsApproved IS NOT NULL


	 UPDATE EV
		SET 
		ClockInTime = L.[ClockInTime],
		ClockOutTime = L.[ClockOutTime],
		IsApproved = 1,
		ApproveNote = L.[ApproveNote],
		UpdatedBy = @LoggedInID,
		UpdatedDate = GETUTCDATE()
	 FROM EmployeeVisits EV 
	 INNER JOIN @List L ON EV.EmployeeVisitID = L.[EmployeeVisitID]
	 WHERE
		EV.IsDeleted = 0 AND EV.IsApproved IS NOT NULL

	DECLARE @CurScheduleID bigint;                         
      DECLARE eventCursor CURSOR FORWARD_ONLY FOR
            SELECT ScheduleID FROM @Output;
        OPEN eventCursor;
        FETCH NEXT FROM eventCursor INTO @CurScheduleID;
        WHILE @@FETCH_STATUS = 0 BEGIN
            EXEC [dbo].[ScheduleEventBroadcast] 'EditSchedule', @CurScheduleID,'',''
            FETCH NEXT FROM eventCursor INTO @CurScheduleID;
        END;
        CLOSE eventCursor;
        DEALLOCATE eventCursor;

END
GO

