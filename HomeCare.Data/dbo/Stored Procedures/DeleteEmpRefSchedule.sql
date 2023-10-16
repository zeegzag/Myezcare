--EXEC DeleteEmpRefSchedule @ReferralTimeSlotDetailID = '51', @ReferralTimeSlotMasterID = '41', @Day = '1', @loggedInId = '47', @StartDate = '2021/01/24', @EndDate = '2022/01/24'
--EXEC DeleteEmpRefSchedule @ReferralTimeSlotDetailID = '50078', @ReferralTimeSlotMasterID = '1966', @Day = '1', @StartDate = '5/21/2018 12:00:00 AM', @EndDate = '5/21/2019 12:00:00 AM', @StartTime = '08:00:00', @EndTime = '10:00:00'          
--EXEC DeleteEmpRefSchedule @ReferralTimeSlotDetailID = '50078', @ReferralTimeSlotMasterID = '1966', @Day = '1', @StartDate = '5/21/2018 12:00:00 AM', @EndDate = '5/21/2019 12:00:00 AM', @StartTime = '08:00:00', @EndTime = '10:00:00'      
CREATE PROCEDURE [dbo].[DeleteEmpRefSchedule] @ReferralTimeSlotDetailID BIGINT,
  @ReferralTimeSlotMasterID BIGINT,
  @Day INT,
  @StartDate DATE,
  @EndDate DATE,
  @loggedInId BIGINT
AS
BEGIN
  DECLARE @TodayDateTime DATETIME = dbo.GetOrgCurrentDateTime();
  DECLARE @Output TABLE (ScheduleID BIGINT)

  UPDATE SM
  SET IsDeleted = 1,
    UpdatedDate = GETDATE(),
    UpdatedBy = @loggedInId
  OUTPUT deleted.ScheduleID
  INTO @Output
  FROM ScheduleMasters SM
  INNER JOIN ReferralTimeSlotDates RSD
    ON RSD.ReferralTSDateID = SM.ReferralTSDateID
  WHERE RSD.ReferralTimeSlotDetailID = @ReferralTimeSlotDetailID
    AND RSD.ReferralTimeSlotMasterID = @ReferralTimeSlotMasterID
    AND DayNumber = @Day
    AND RSD.ReferralTSDate BETWEEN @StartDate
      AND @EndDate
    AND ISNULL(SM.IsDeleted, 0) = 0
    AND RSD.ReferralTSDate > @TodayDateTime

  DECLARE @CurScheduleID BIGINT;

  DECLARE eventCursor CURSOR FORWARD_ONLY
  FOR
  SELECT ScheduleID
  FROM @Output;

  OPEN eventCursor;

  FETCH NEXT
  FROM eventCursor
  INTO @CurScheduleID;

  WHILE @@FETCH_STATUS = 0
  BEGIN
    EXEC [dbo].[ScheduleEventBroadcast] 'DeleteSchedule',
      @CurScheduleID,
      '',
      ''

    FETCH NEXT
    FROM eventCursor
    INTO @CurScheduleID;
  END;

  CLOSE eventCursor;

  DEALLOCATE eventCursor;

  SELECT 1;
END