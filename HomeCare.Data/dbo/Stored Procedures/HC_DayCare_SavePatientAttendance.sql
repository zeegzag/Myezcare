CREATE PROCEDURE [dbo].[HC_DayCare_SavePatientAttendance]   
  @ScheduleID BIGINT,  
  @ReferralID BIGINT,  
  @IsPatientAttendedSchedule BIT,  
  @PateintAbsentReason NVARCHAR(MAX),  
  @loggedInId BIGINT,  
  @SystemID NVARCHAR(MAX)  
AS  
BEGIN  
  DECLARE @EmployeeVisitID BIGINT,  
    @ClockInTime DATETIME,  
    @ClockOutTime DATETIME,  
 @CurrDateTime DATETIME = [dbo].[GetOrgCurrentDateTime]();  
  
  IF (  
      @IsPatientAttendedSchedule = 1  
      OR LEN(@PateintAbsentReason) = 0  
      )  
  BEGIN  
    SET @PateintAbsentReason = NULL;  
  
    SELECT @ClockInTime = SM.StartDate,  
      @ClockOutTime = SM.EndDate  
    FROM ScheduleMasters SM  
    WHERE ScheduleID = @ScheduleID  
  END  
  
  --DECLARE @IsPastDayClockIn BIT = (SELECT CASE WHEN CONVERT(DATE, @ClockInTime) <= CONVERT(DATE, DATEADD(DD, -1, @CurrDateTime)) THEN 1 ELSE 0 END)
  DECLARE @IsPastDayClockIn BIT = (SELECT CASE WHEN CONVERT(DATE, @ClockInTime) <= CONVERT(DATE, @CurrDateTime) THEN 1 ELSE 0 END) 
  print @IsPastDayClockIn
  IF (@IsPatientAttendedSchedule = 0 OR CONVERT(DATE, @ClockInTime) <= @CurrDateTime)  
  BEGIN  
    UPDATE ScheduleMasters  
    SET IsPatientAttendedSchedule = CASE WHEN @IsPastDayClockIn = 1 OR @IsPatientAttendedSchedule = 0 THEN @IsPatientAttendedSchedule ELSE NULL END,  
      AbsentReason = @PateintAbsentReason ,  
      UpdatedBy = @loggedInId,  
      UpdatedDate = GETDATE(),  
      SystemID = @SystemID  
    WHERE ScheduleID = @ScheduleID  
  
 SELECT @EmployeeVisitID = EV.EmployeeVisitID  
      FROM EmployeeVisits EV  
      WHERE ScheduleID = @ScheduleID  
        AND IsDeleted = 0  
  
 IF(@IsPastDayClockIn = 1)  
 BEGIN  
     
      IF (ISNULL(@EmployeeVisitID, 0) = 0)  
      BEGIN  
	  print 'insert'
        INSERT INTO EmployeeVisits (  
          ScheduleID,  
          ClockInTime,  
          ClockOutTime,  
          IsDeleted,  
          IsSigned,  
          CreatedDate,  
          CreatedBy,  
          UpdatedDate,  
          UpdatedBy,  
          SystemID  
          )  
        VALUES (  
          @ScheduleID,  
          @ClockInTime,  
          @ClockOutTime,  
          0,  
          0,  
          GETDATE(),  
          @LoggedInID,  
          GETDATE(),  
          @LoggedInID,  
          @SystemID  
          )  
      END  
      ELSE  
      BEGIN  
        UPDATE EmployeeVisits  
        SET ClockInTime = @ClockInTime,  
          ClockOutTime = @ClockOutTime,  
          UpdatedDate = GETDATE(),  
          UpdatedBy = @LoggedInID,  
          IsPCACompleted = 1,  
          SystemID = @SystemID  
        WHERE EmployeeVisitID = @EmployeeVisitID  
      END  
  
 END  
 ELSE  
 BEGIN  
      UPDATE EmployeeVisits  
  SET IsDeleted = 1  
        WHERE EmployeeVisitID = ISNULL(@EmployeeVisitID, 0)  
 END  
  
  END  
END
