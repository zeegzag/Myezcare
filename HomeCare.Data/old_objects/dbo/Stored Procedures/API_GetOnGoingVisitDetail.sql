--EXEC API_GetOnGoingVisitDetail 133441      
CREATE PROCEDURE [dbo].[API_GetOnGoingVisitDetail] @EmployeeID BIGINT = 0,
  @ScheduleID BIGINT,
  @CurrentDateTime DATETIME NULL
AS
BEGIN
  DECLARE @EmployeeVisitID BIGINT
  DECLARE @ReferralID BIGINT
  DECLARE @Count INT

  SET @EmployeeVisitID = (
      SELECT EmployeeVisitID
      FROM EmployeeVisits
      WHERE ScheduleID = @ScheduleID
      )

  SELECT @ReferralID = ReferralID
  FROM ScheduleMasters
  WHERE ScheduleID = @ScheduleID

  SET @Count = (
      SELECT COUNT(ReferralTaskMappingID)
      FROM ReferralTaskMappings RTM
      INNER JOIN VisitTasks V
        ON V.VisitTaskID = RTM.VisitTaskID
          AND V.VisitTaskType = 'Conclusion'
      WHERE RTM.ReferralID = @ReferralID
        AND RTM.IsDeleted = 0
      )

  IF (@EmployeeVisitID IS NOT NULL)
    SELECT ClockInTime,
      ClockOutTime,
      SurveyCompleted,
      IsSigned,
      IsPCACompleted,
      EmployeeVisitID,
      IVRClockOut,
      --CASE WHEN UsedInScheduling=1 THEN 0 ELSE 1 END AS IsDenied,          
      IsDenied,
      Notes AS DeniedReason,
      CASE 
        WHEN @Count > 0
          THEN 1
        ELSE 0
        END AS HasConclusion
    FROM EmployeeVisits EV
    INNER JOIN ScheduleMasters SM
      ON SM.ScheduleID = EV.ScheduleID
    LEFT JOIN ReferralTimeSlotDates RTD
      ON RTD.ReferralTSDateID = SM.ReferralTSDateID 
        --Join may be changed in future    
    WHERE EV.EmployeeVisitID = @EmployeeVisitID 
      --EV.ScheduleID=@ScheduleID                
  ELSE
    SELECT 
      --CASE WHEN UsedInScheduling=1 THEN 0 ELSE 1 END AS IsDenied,          
      IsDenied,
      Notes AS DeniedReason
    FROM ScheduleMasters SM
    INNER JOIN ReferralTimeSlotDates RTD
      ON RTD.ReferralTSDateID = SM.ReferralTSDateID
    --inner join EmployeeVisits EV on ev.ScheduleID=sm.ScheduleID    
    WHERE sm.ScheduleID = @ScheduleID
END
