-- EXEC UpdateRtsDetail @SortExpression = 'Day', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50', @ReferralTimeSlotDetailID = '30038', @ReferralTimeSlotMasterID = '1966', @StartTime = '06:00:00', @EndTime = '07:00:00', @UsedInScheduling = 'True', @Notes = '', @loggedInUserID = '1', @SystemID = '::1', @SelectedDays = '', @ListOfIdsInCsv = '30038', @IsShowList = 'True'      

CREATE PROCEDURE [dbo].[UpdateEtsDetail]
  @EmployeeTimeSlotDetailID bigint,
  @EmployeeTimeSlotMasterID bigint,
  @EmployeeID bigint,
  @Day int = 0,
  @StartTime time(7),
  @EndTime time(7),
  @AllDay bit,
  @Notes nvarchar(1000),
  @loggedInUserId bigint,
  @SystemID varchar(100),
  @SelectedDays varchar(20),
  @TodayDate date,
  @SlotEndDate date,
  @SortExpression nvarchar(100),
  @SortType nvarchar(10),
  @FromIndex int,
  @PageSize int,
  @IsShowList bit,
  @ListOfIdsInCsv varchar(300),
  @AddDay datetime = NULL

AS
BEGIN
  DECLARE @TablePrimaryId bigint;
  DECLARE @StartDate date;
  DECLARE @EndDate date;
  DECLARE @MaxDate date = '2099-12-31';
  DECLARE @EmployeeTimeSlotMasterIds varchar(max);

  BEGIN TRANSACTION trans
    BEGIN TRY

      SELECT
        @StartDate = StartDate,
        @EndDate = EndDate
      FROM EmployeeTimeSlotMaster
      WHERE
        EmployeeTimeSlotMasterID = @EmployeeTimeSlotMasterID

      SELECT
        @EmployeeTimeSlotMasterIds =
        STUFF(
        (
          SELECT
            ', ' + CONVERT(varchar(50), EmployeeTimeSlotMasterID)
          FROM EmployeeTimeSlotMaster
          WHERE
            EmployeeID = @EmployeeID
            AND ((StartDate >= @StartDate
                AND StartDate <= ISNULL(@EndDate, @MaxDate))
              OR (EndDate >= @StartDate
                AND EndDate <= ISNULL(@EndDate, @MaxDate))
              OR (@StartDate >= StartDate
                AND @StartDate <= ISNULL(EndDate, @MaxDate))
              OR (@EndDate

            >= StartDate
                AND @EndDate <= ISNULL(EndDate, @MaxDate)))
          FOR xml PATH ('')
        )
        , 1, 2, '')

      -- CHECK FOR EXIST      

      DECLARE @TempTable TABLE (
        DayID bigint,
        Day int,
        ExcludeFromInsert bit DEFAULT 0
      )

      DECLARE @ExcludedCount int = 0,
              @TotalCount int = 0,
              @Result int = 0;

      INSERT INTO @TempTable
        SELECT
          ReturnId,
          RESULT,
          0
        FROM [dbo].[CSVtoTableWithIdentity](@SelectedDays, ',')

      DECLARE @ExistTimeSlotTable TABLE (
        EmployeeTimeSlotID bigint
      )

      INSERT INTO @ExistTimeSlotTable
        SELECT DISTINCT
          EmployeeTimeSlotMasterID
        FROM EmployeeTimeSlotDetails E
        INNER JOIN @TempTable T
          ON E.Day = T.Day
        WHERE
          (
          (E.StartTime > @StartTime
              AND E.StartTime < @EndTime)
            OR (E.EndTime > @StartTime
              AND E.EndTime < @EndTime)
            OR (@StartTime > E.StartTime
              AND @StartTime < E.EndTime)
            OR (@EndTime > E.StartTime
              AND @EndTime < E.EndTime)
            OR (@StartTime = E.StartTime
              AND @EndTime = E.EndTime)
          )
          AND E.EmployeeTimeSlotMasterID IN
          (
            SELECT
              val
            FROM GetCSVTable(@EmployeeTimeSlotMasterIds)
          )
          AND E.EmployeeTimeSlotDetailID != @EmployeeTimeSlotDetailID
          AND E.IsDeleted = 0
          AND E.Day = T.Day

      IF EXISTS
        (
          SELECT
            *
          FROM @ExistTimeSlotTable
        )
      BEGIN

        UPDATE T
        SET
          ExcludeFromInsert = 1
        FROM EmployeeTimeSlotDetails E
        INNER JOIN @TempTable T
          ON E.Day = T.Day
        WHERE
          (

          (E.StartTime > @StartTime
          AND E.StartTime < @EndTime)
          OR (E.EndTime > @StartTime
          AND E.EndTime < @EndTime)
          OR (@StartTime > E.StartTime
          AND @StartTime < E.EndTime)
          OR (@EndTime > E.StartTime
          AND @EndTime < E.EndTime)
          OR (@StartTime = E.StartTime
          AND @EndTime = E.EndTime)
          )
          AND E.EmployeeTimeSlotMasterID IN
          (
            SELECT
              val
            FROM GetCSVTable(@EmployeeTimeSlotMasterIds)
          )
          AND E.EmployeeTimeSlotDetailID != @EmployeeTimeSlotDetailID
          AND E.IsDeleted = 0


      END

      -- SELECT * FROm @TempTable      

      -- EXEC UpdateRtsDetail @SortExpression = 'Day', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50', @ReferralTimeSlotDetailID = '50076', @ReferralTimeSlotMasterID = '1966', @StartTime = '04:00:00', @EndTime = '06:00:00', @UsedInScheduling = 'True', @Notes = '', @loggedInUserID = '1', @SystemID = '::1', @SelectedDays = '1', @ListOfIdsInCsv = '50076', @IsShowList = 'True'      


      SELECT
        @ExcludedCount = COUNT(*)
      FROM @TempTable
      WHERE
        ExcludeFromInsert = 1
      SELECT
        @TotalCount = COUNT(*)
      FROM @TempTable


      IF (@TotalCount > @ExcludedCount)
      BEGIN


        INSERT INTO EmployeeTimeSlotDetails
        (
          EmployeeTimeSlotMasterID,
          Day,
          StartTime,
          EndTime,
          AllDay,
          Notes,
          CreatedBy,
          CreatedDate,
          UpdatedBy,
          UpdatedDate,
          SystemID,
          AddDay
        )
          SELECT
            @EmployeeTimeSlotMasterID,
            T.Day,
            @StartTime,
            @EndTime,
            @AllDay,
            @Notes,
            @loggedInUserId,
            GETUTCDATE(),
            @loggedInUserId,
            GETUTCDATE(),
            @SystemID,
            @AddDay
          FROM @TempTable T
          WHERE
            ExcludeFromInsert = 0

        -- DELETE      
        EXEC DeleteEtsDetail @EmployeeTimeSlotMasterID,
                             NULL,
                             NULL,
                             -1,
                             @SortExpression,
                             @SortType,
                             @FromIndex,
                             @PageSize,
                             @ListOfIdsInCsv,
                             @IsShowList,
                             @loggedInUserId

        --Create Slots    
        INSERT INTO EmployeeTimeSlotDates
          SELECT
            T.EmployeeID,
            T.EmployeeTimeSlotMasterID,
            T.EmployeeTSDate,
            T.EmployeeTSStartTime,
            T.EmployeeTSEndTime,
            T.DayNumber,
            T.EmployeeTimeSlotDetailID
          FROM
          (
            SELECT
              E.EmployeeID,
              ETM.EmployeeTimeSlotMasterID,
              EmployeeTSDate = IndividualDate,
              ETMEndDate = ETM.EndDate,
              EmployeeTSStartTime = CONVERT(datetime, CONVERT(char(8), T1.IndividualDate, 112) + ' ' + CONVERT(char(8), StartTime, 108)),
              EmployeeTSEndTime = CONVERT(datetime, CONVERT(char(8), T1.IndividualDate, 112) + ' ' + CONVERT(char(8), EndTime, 108)),
              DayNumber = T1.DayNameInt,
              ETSD.EmployeeTimeSlotDetailID
            FROM DateRange(CASE
              WHEN @StartDate < @TodayDate THEN @TodayDate
              ELSE @StartDate
            END, @SlotEndDate) T1
            INNER JOIN EmployeeTimeSlotDetails ETSD
              ON ETSD.Day = T1.DayNameInt
              AND ETSD.IsDeleted = 0
              INNER JOIN EmployeeTimeSlotMaster ETM
                ON ETM.EmployeeTimeSlotMasterID = ETSD.EmployeeTimeSlotMasterID
                AND ETM.IsDeleted = 0
              INNER JOIN Employees E
                ON E.EmployeeID = ETM.EmployeeID
          ) AS T
          LEFT JOIN EmployeeTimeSlotDates ETSDT
            ON ETSDT.EmployeeTSStartTime = T.EmployeeTSStartTime
            AND ETSDT.EmployeeTSEndTime = T.EmployeeTSEndTime
            AND ETSDT.EmployeeID = T.EmployeeID
          WHERE
            ETSDT.EmployeeTSDateID IS NULL
            AND T.EmployeeTSDate <= ISNULL(ETMEndDate, @SlotEndDate)
            AND (@EmployeeID = 0
              OR T.EmployeeID = @EmployeeID)
            AND T.EmployeeTimeSlotMasterID = @EmployeeTimeSlotMasterID
            AND T.DayNumber IN
            (
              SELECT
                val
              FROM GetCSVTable(@SelectedDays)
            )
          ORDER BY T.EmployeeID ASC, T.EmployeeTimeSlotMasterID ASC

        SET @Result =
                       CASE
                         WHEN @ExcludedCount = 0 THEN 1
                         ELSE 2
                       END

      END
      ELSE
      BEGIN
        IF (
          (
            SELECT
              COUNT(*)
            FROM @ExistTimeSlotTable
          )
          > 1)
          OR NOT EXISTS
          (
            SELECT
              *
            FROM @ExistTimeSlotTable
            WHERE
              EmployeeTimeSlotID = @EmployeeTimeSlotMasterID
          )
          SET @Result = -3
        ELSE
          SET @Result = -2
      END






      SELECT
        @Result AS TransactionResultId,
        1 AS TablePrimaryId;
      IF @@TRANCOUNT > 0
      BEGIN
      COMMIT TRANSACTION trans
    END
  END TRY
  BEGIN CATCH
    SELECT
      -1 AS TransactionResultId,
      ERROR_MESSAGE() AS ErrorMessage;
    IF @@TRANCOUNT > 0
    BEGIN
      ROLLBACK TRANSACTION trans
    END
  END CATCH

END