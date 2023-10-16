--Exec AddEtsDetail @EmployeeTimeSlotMasterID=6,@EmployeeID=56,@StartDate='2007-03-01',@EndDate='2022-12-31',@loggedInUserId=1,@SystemID='::1'          
CREATE PROCEDURE [dbo].[AddEtsDetail]  
  @EmployeeTimeSlotDetailID bigint,  
  @EmployeeTimeSlotMasterID bigint,  
  @EmployeeID bigint,  
  @Day int = 0,  
  @StartTime time(7) = NULL,  
  @EndTime time(7) = NULL,  
  @AllDay bit,  
  @Is24Hrs bit,  
  @Notes nvarchar(1000),  
  @loggedInUserId bigint,  
  @SystemID varchar(100),  
  @SelectedDays varchar(20),  
  @TodayDate date,  
  @SlotEndDate date  
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
          Is24Hrs,
          Notes,  
          CreatedBy,  
          CreatedDate,  
          UpdatedBy,  
          UpdatedDate,  
          SystemID  
        )  
          SELECT  
            @EmployeeTimeSlotMasterID,  
            T.Day,  
            @StartTime,  
            @EndTime,  
            @AllDay,
            @Is24Hrs,
            @Notes,  
            @loggedInUserId,  
            GETUTCDATE(),  
            @loggedInUserId,  
            GETUTCDATE(),  
            @SystemID  
          FROM @TempTable T  
          WHERE  
            ExcludeFromInsert = 0  
        SET @Result =  
                       CASE  
                         WHEN @ExcludedCount = 0 THEN 1  
                         ELSE 2  
                       END  
  
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
              EmployeeTSStartTime = TS.TSStartTime,  
              EmployeeTSEndTime = CASE WHEN TS.TSStartTime <= TS.TSEndTime AND ISNULL(ETSD.Is24Hrs, 0) = 0 THEN TS.TSEndTime ELSE TS.TSEndTimeNextDay END, 
              DayNumber = T1.DayNameInt,  
              ETSD.EmployeeTimeSlotDetailID  
            FROM DateRange(CASE  
              WHEN @StartDate < @TodayDate THEN @TodayDate  
              ELSE @StartDate  
            END, @SlotEndDate) T1  
            INNER JOIN EmployeeTimeSlotDetails ETSD  
              ON ETSD.Day = T1.DayNameInt  
              AND ETSD.IsDeleted = 0 
			CROSS APPLY (
			  SELECT
                TSStartTime = CONVERT(datetime, CONVERT(char(8), T1.IndividualDate, 112) + ' ' + ISNULL(CONVERT(char(8), StartTime, 108), '00:00:00')),  
				TSEndTime = CONVERT(datetime, CONVERT(char(8), T1.IndividualDate, 112) + ' ' + ISNULL(CONVERT(char(8), EndTime, 108), '23:59:59')), 
				TSEndTimeNextDay = CONVERT(datetime, CONVERT(char(8), DATEADD(D, 1,T1.IndividualDate), 112) + ' ' + ISNULL(CONVERT(char(8), EndTime, 108), '11:59:59'))
			) TS
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
          ORDER BY T.EmployeeID ASC, T.EmployeeTimeSlotMasterID ASC  
  
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
  
  
      --IF(@EmployeeTimeSlotDetailID=0)                    
      --BEGIN                    
      --  INSERT INTO EmployeeTimeSlotDetails                    
      --  (EmployeeTimeSlotMasterID,Day,StartTime,EndTime,Notes,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,SystemID)                    
      --  VALUES                    
      --  (@EmployeeTimeSlotMasterID,@Day,@StartTime,@EndTime,@Notes,@loggedInUserId,GETUTCDATE(),@loggedInUserId,GETUTCDATE(),@SystemID);                     
  
      --SET @TablePrimaryId = @@IDENTITY;             
      --END                    
      --ELSE                    
      --BEGIN                    
      --  UPDATE EmployeeTimeSlotDetails                     
      --  SET                          
      --     EmployeeTimeSlotMasterID=@EmployeeTimeSlotMasterID,          
      --  Day=@Day,                    
      --     StartTime=@StartTime,                  
      --  EndTime=@EndTime,      
      --  Notes=@Notes,             
      --     UpdatedBy=@loggedInUserId,                    
      --     UpdatedDate=GETUTCDATE(),                    
      --     SystemID=@SystemID                    
      --  WHERE EmployeeTimeSlotDetailID=@EmployeeTimeSlotDetailID;                    
      --END                    
  
      SELECT  
        @Result AS TransactionResultId,  
        @TablePrimaryId AS TablePrimaryId;  
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