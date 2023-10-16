CREATE PROCEDURE [dbo].[API_CreateCareTypeSchedule] @CareTypeID BIGINT,
  @EmployeeID BIGINT,
  @ReferralID BIGINT,
  @ServerCurrentDate DATETIME,
  @SystemID VARCHAR(100),
  @StartTime TIME(7),
  @EndTime TIME(7)
AS
BEGIN
  DECLARE @ScheduleID BIGINT;

  BEGIN TRANSACTION trans

  BEGIN TRY
    DECLARE @MaxDate DATE = '2099-12-31'
    DECLARE @CareTypeTimeSlotID BIGINT
    DECLARE @PayorID BIGINT

    --DECLARE @ScheduleID BIGINT                
    SELECT @CareTypeTimeSlotID = CareTypeTimeSlotID
    FROM CareTypeTimeSlots
    WHERE CareTypeID = @CareTypeID
      AND ReferralID = @ReferralID
      AND IsDeleted = 0
      AND CONVERT(DATE, @ServerCurrentDate) BETWEEN CONVERT(DATE, StartDate)
        AND CONVERT(DATE, ISNULL(EndDate, @MaxDate))

    SELECT @PayorID = PayorID
    FROM ReferralPayorMappings
    WHERE ReferralID = @ReferralID
      AND Precedence = 1
      AND (
        CONVERT(DATE, @ServerCurrentDate) BETWEEN PayorEffectiveDate
          AND PayorEffectiveEndDate
        )

    --SELECT ScheduleID=@ScheduleID FROM ScheduleMasters WHERE ReferralID=@ReferralID AND CareTypeTimeSlotID=@CareTypeTimeSlotID            
    --AND CONVERT(DATE,@ServerCurrentDate) BETWEEN CONVERT(DATE,StartDate) AND CONVERT(DATE,ISNULL(EndDate,@MaxDate))            
    IF (@CareTypeTimeSlotID > 0)
    BEGIN
      INSERT INTO ScheduleMasters (
        ReferralID,
        EmployeeID,
        CareTypeTimeSlotID,
        StartDate,
        EndDate,
        ScheduleStatusID,
        CreatedBy,
        CreatedDate,
        UpdatedBy,
        UpdatedDate,
        SystemID,
        IsDeleted,
        PayorID,
        StartTime,
        EndTime,
        CareTypeId
        )
      VALUES (
        @ReferralID,
        @EmployeeID,
        @CareTypeTimeSlotID,
        @ServerCurrentDate --+CAST('00:01:00' AS DATETIME)        
        ,
        @ServerCurrentDate + CAST('23:59:00' AS DATETIME),
        2,
        @EmployeeID,
        @ServerCurrentDate,
        @EmployeeID,
        @ServerCurrentDate,
        @SystemID,
        0,
        @PayorID,
        @StartTime,
        @EndTime,
        @CareTypeID
        )

      SET @ScheduleID = @@IDENTITY

      SELECT @ScheduleID AS TablePrimaryId,
        1 AS TransactionResultId
    END
    ELSE
    BEGIN
      INSERT INTO CareTypeTimeSlots (
        ReferralID,
        CareTypeID,
        Count,
        Frequency,
        StartDate,
        EndDate,
        IsDeleted,
        CreatedDate,
        CreatedBy,
        UpdatedDate,
        UpdatedBy,
        SystemID
        )
      VALUES (
        @ReferralID,
        @CareTypeID,
        1,
        1,
        @ServerCurrentDate,
        @ServerCurrentDate,
        0,
        @ServerCurrentDate,
        @EmployeeID,
        @ServerCurrentDate,
        @EmployeeID,
        @SystemID
        )

      INSERT INTO ScheduleMasters (
        ReferralID,
        EmployeeID,
        CareTypeTimeSlotID,
        StartDate,
        EndDate,
        ScheduleStatusID,
        CreatedBy,
        CreatedDate,
        UpdatedBy,
        UpdatedDate,
        SystemID,
        IsDeleted,
        PayorID,
        StartTime,
        EndTime,
        CareTypeId
        )
      VALUES (
        @ReferralID,
        @EmployeeID,
        @@IDENTITY,
        @ServerCurrentDate --+CAST('00:01:00' AS DATETIME)        
        ,
        @ServerCurrentDate + CAST('23:59:00' AS DATETIME),
        2,
        @EmployeeID,
        @ServerCurrentDate,
        @EmployeeID,
        @ServerCurrentDate,
        @SystemID,
        0,
        @PayorID,
        @StartTime,
        @EndTime,
        @CareTypeID
        )

      SET @ScheduleID = @@IDENTITY

      SELECT @ScheduleID AS TablePrimaryId,
        1 AS TransactionResultId
    END

    IF @@TRANCOUNT > 0
    BEGIN
      COMMIT TRANSACTION trans

      EXEC [dbo].[ScheduleEventBroadcast] 'CreateSchedule',
        @ScheduleID,
        '',
        ''
    END
  END TRY

  BEGIN CATCH
    SELECT - 1 AS TransactionResultId,
      ERROR_MESSAGE() AS ErrorMessage;

    IF @@TRANCOUNT > 0
    BEGIN
      ROLLBACK TRANSACTION trans
    END
  END CATCH
END
