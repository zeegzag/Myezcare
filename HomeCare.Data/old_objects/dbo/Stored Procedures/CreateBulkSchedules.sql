  
--UpdateBy: Akhilesh Kamal    
--UpdatedDate: 15 sep 2020    
--Description: Insert AnyTimeClockIn boolean data in scheduleMaster table     
-- UpdatedBy: Kundan Rai    
-- Update 20 Sept, 2020    
-- Details: To manage the is virtual virtual visit    
CREATE PROCEDURE [dbo].[CreateBulkSchedules]  
  @PayorID bigint = 0,  
  @ReferralID bigint,  
  @NewEmployeeID bigint,  
  @ScheduleID bigint,  
  @EmployeeTimeSlotDetailIDs varchar(max),  
  @StartDate datetime,  
  @EndDate datetime,  
  @ScheduleStatusID bigint,  
  @SameDateWithTimeSlot bit,  
  @loggedInId bigint,  
  @SystemID varchar(100),  
  @ReferralTimeSlotDetailIDs nvarchar(max),  
  @IsRescheduleAction bit,  
  @CareTypeId bigint = 0,  
  @IsForceUpdate bit = 0,  
  @ReferralBillingAuthorizationID bigint = 0,  
  @IsVirtualVisit bit = 0  
AS  
BEGIN  
  
  /******Kundan Kumar Rai - 22-05-2020********      
  Adde new column ReferralBillingAuthorizationID to store the new mapping referral and service code      
  ******END********/  
  IF (@PayorID = 0  
    OR @PayorID IS NULL)  
    SET @PayorID = NULL  
  
  DECLARE @StartDateOnly date = CONVERT(date, @StartDate);  
  DECLARE @EndDateOnly date = CONVERT(date, @EndDate);  
  
  EXEC GenerateReferralTimeSlotDates @StartDateOnly,  
                                     @EndDateOnly,  
                                     @ReferralID  
  EXEC GenerateEmployeeTimeSlotDates @StartDateOnly,  
                                     @EndDateOnly,  
                                     @NewEmployeeID  
  
  DECLARE @TempTable TABLE (  
    NewGUID varchar(100),  
    ReferralID bigint,  
    StartDate datetime,  
    EndDate datetime,  
    ScheduleStatusID bigint,  
    CreatedBy bigint,  
    CreatedDate datetime,  
    UpdatedBy bigint,  
    UpdatedDate datetime,  
    SystemID varchar(max),  
    IsDeleted bit,  
    EmployeeID bigint,  
    EmployeeTSDateID bigint,  
    ReferralTSDateID bigint,  
    AnyTimeClockIn bit  
  )  
  
  DECLARE @PTOTable TABLE (  
    NewGUID varchar(100)  
  )  
  
  DECLARE @Output TABLE (  
    ScheduleID bigint,  
    StartDate datetime  
  )  

  DECLARE @CurScheduleID bigint;
  
  --DECLARE @DateTimeDiffrence bigint = 0,  
  --        @AllowedTime bigint = 0,  
  --        @RowCount bigint = 0;  
  --SELECT  
  --  @DateTimeDiffrence = SUM(ISNULL(DATEDIFF(MINUTE, ReferralTSStartTime, ReferralTSEndTime), 0)),  
  --  @RowCount = COUNT(*)  
  --FROM ReferralTimeSlotDates  
  --WHERE  
  --  ReferralID = 10  
  --  AND ReferralTSDate BETWEEN @StartDate AND @EndDate  
  --GROUP BY ReferralID  
  
  --Commented by Pallav Saxena to stop the calculation for the authorization hours during scheduling.       
  --select @AllowedTime = ISNULL((rba.AllowedTime * (case when rba.AllowedTimeType = 'Minutes' then 1 else 60 end)),0)        
  -- from [dbo].[ReferralBillingAuthorizations] AS rba          
  -- where rba.ReferralID = @ReferralID AND  (@StartDate BETWEEN rba.StartDate AND rba.EndDate ) AND (@EndDate BETWEEN rba.StartDate AND rba.EndDate )         
  
  --  -- Added @IsRescheduleAction condition to ignore force schedule on resechedule option      
  --  if((@AllowedTime * @RowCount) - @DateTimeDiffrence < 0 AND @IsForceUpdate = 0 AND @IsRescheduleAction = 0)        
  --  BEGIN          
  --   SELECT -4 RETURN;          
  --  END      
  
  -- UPDATED BY: KUNDAN ON: 29, JAN 2020      
  -- CHECK IF EMPLOYEE ALREADY ALLOTED TO OTHER PATIENT CONFLICTING WITH SAME DATE AND TIME       
  -- THIS FUNCTIONALITY NEEDS TO DEPECRATED IN FUTURE AS WE WOULD LIKE TO ALLOCATE A EMPLOYEE      
  -- TO MULTIPLE PATIENTS      
  DECLARE @retValue int;  
  SELECT  
    @retValue = COUNT(DISTINCT CONVERT(date, SM.StartDate))  
  FROM ScheduleMasters SM  
  WHERE  
    ((DATEADD(MINUTE, 1, @StartDate) BETWEEN SM.StartDate AND SM.EndDate)  
      OR (DATEADD(MINUTE, -1, @EndDate) BETWEEN SM.StartDate AND SM.EndDate)  -- IS EMPLOYEE TIME CONFLICTS INSIDE WITH EXISTING SCHEDULE      
    OR (DATEADD(MINUTE, 1, @StartDate) <= SM.StartDate  
        AND DATEADD(MINUTE, 1, @EndDate) >= SM.EndDate)) -- IS EMPLOYEE TIME OVERLAPPING AND CONFLICTS OUTSIDE WITH EXISTING SCHEDULE      
    AND SM.EmployeeID = @NewEmployeeID  
    AND SM.IsDeleted = 0  
  
  
  
  
  PRINT @retValue;  
  PRINT DATEDIFF(DAY, CONVERT(date, @StartDate), CONVERT(date, @EndDate))  
  IF (@retValue > 0  
    AND @retValue >= (DATEDIFF(DAY, CONVERT(date, @StartDate), CONVERT(date, @EndDate)) + 1))  
  BEGIN  
    SELECT  
      0  
    RETURN;  
  END  
  
  -- CHECK FOR EMPLOYEE BLOCK          
  IF EXISTS  
    (  
      SELECT  
        1  
      FROM ReferralBlockedEmployees RBE  
      WHERE  
        RBE.ReferralID = @ReferralID  
        AND RBE.EmployeeID = @NewEmployeeID  
        AND RBE.IsDeleted = 0  
    )  
  BEGIN  
    SELECT  
      -1  
    RETURN;  
  END  
  -- CHECK FOR EMPLOYEE BLOCK          
  IF (@ScheduleID = 0)  
  BEGIN  
    INSERT INTO @TempTable  
      SELECT  
        NEWID(),  
        @ReferralID,  
        RTD.ReferralTSStartTime,  
        RTD.ReferralTSEndTime,  
        @ScheduleStatusID,  
        @loggedInId,  
        GETDATE(),  
        1,  
        GETDATE(),  
        @SystemID,  
        0,  
        @NewEmployeeID,  
        ETD.EmployeeTSDateID,  
        RTD.ReferralTSDateID,  
        RTSD.AnyTimeClockIn  
      --ETD.*, RTD.*           
      FROM EmployeeTimeSlotDates ETD  
      --INNER JOIN EmployeeTimeSlotDetails ETSD ON ETSD.StartTime= CAST(ETD.EmployeeTSStartTime as time) AND DATEPART(dw,ETD.EmployeeTSDate)=ETSD.Day AND ETSD.IsDeleted=0          
      INNER JOIN EmployeeTimeSlotDetails ETSD  
        ON ETSD.IsDeleted = 0  
        AND ETSD.EmployeeTimeSlotDetailID = ETD.EmployeeTimeSlotDetailID  
        AND (  
        (@SameDateWithTimeSlot = 0  
        AND ETD.EmployeeTSDate BETWEEN @StartDate AND @EndDate)  
        OR (@SameDateWithTimeSlot = 1  
        AND @StartDate BETWEEN ETD.EmployeeTSStartTime AND ETD.EmployeeTSEndTime  
        AND @EndDate BETWEEN ETD.EmployeeTSStartTime AND ETD.EmployeeTSEndTime)  
        )  
        AND ETD.EmployeeID = @NewEmployeeID  
      INNER JOIN  
      (  
        SELECT  
          EmployeeTimeSlotDetailID = CONVERT(bigint, VAL)  
        FROM GetCSVTable(@EmployeeTimeSlotDetailIDs)  
      ) T  
        ON T.EmployeeTimeSlotDetailID = ETSD.EmployeeTimeSlotDetailID  
      INNER JOIN ReferralTimeSlotDates RTD  
        ON RTD.ReferralID = @ReferralID  
        AND RTD.ReferralTSStartTime BETWEEN ETD.EmployeeTSStartTime AND ETD.EmployeeTSEndTime  
        AND RTD.ReferralTSEndTime BETWEEN ETD.EmployeeTSStartTime AND ETD.EmployeeTSEndTime  
        AND (  
        (@SameDateWithTimeSlot = 0  
        AND RTD.ReferralTSDate BETWEEN @StartDate AND @EndDate)  
        OR (@SameDateWithTimeSlot = 1  
        AND RTD.ReferralTSStartTime BETWEEN @StartDate AND @EndDate  
        AND RTD.ReferralTSEndTime BETWEEN @StartDate AND @EndDate)  
        )  
      --INNER JOIN ReferralTimeSlotMaster RTSM ON RTSM.ReferralTimeSlotMasterID=RTD.ReferralTimeSlotMasterID          
      /*Updated by Pallav on 03/10/2020 to fix the no slot found issue and also creating the schedules with incorrect caretype*/  
      INNER JOIN ReferralTimeSlotMaster RTSM  
        ON RTSM.ReferralTimeSlotMasterID = RTD.ReferralTimeSlotMasterID  
      INNER JOIN ReferralTimeSlotDetails RTSD  
        ON RTSD.ReferralTimeSlotMasterID = RTSM.ReferralTimeSlotMasterID  
        AND RTSD.CareTypeId = @CareTypeId  
        AND RTSD.ReferralTimeSlotDetailID = RTD.ReferralTimeSlotDetailID  
      /*End of Pallav's changes*/  
      LEFT JOIN ScheduleMasters SM  
        ON SM.IsDeleted = 0  
  
        -- Updated by Kundan on 21-01-2020: Removed employee dependencies from condition to resolve issue of scheduling       
        -- with double employee on same time slot.      
        --AND (SM.ReferralID=@ReferralID       
        AND SM.EmployeeID = @NewEmployeeID  
        AND (SM.EmployeeTSDateID = ETD.EmployeeTSDateID)  
  
        --Updated by Kundan on 03 Feb 2020: removed to prevent create schedule for same employee with another patient in Case of bulk schedule.      
        AND SM.ReferralTSDateID = RTD.ReferralTSDateID  
        AND (RTD.ReferralTSStartTime BETWEEN DATEADD(MINUTE, 1, SM.StartDate) AND DATEADD(MINUTE, -1, SM.EndDate)  
        OR RTD.ReferralTSEndTime BETWEEN DATEADD(MINUTE, 1, SM.StartDate) AND DATEADD(MINUTE, -1, SM.EndDate))  
      WHERE  
        ETD.EmployeeTSDate BETWEEN CAST(@StartDate AS date) AND CAST(@EndDate AS date)  
        AND ETD.EmployeeID = @NewEmployeeID  
        AND 1 = 1  
        AND ((@IsRescheduleAction = 0  
            AND SM.ScheduleID IS NULL)  
          OR (@IsRescheduleAction = 1))  
        AND (LEN(@ReferralTimeSlotDetailIDs) = 0  
          OR RTD.ReferralTimeSlotDetailID IN  
        (  
          SELECT  
            CONVERT(bigint, VAL)  
          FROM GetCSVTable(@ReferralTimeSlotDetailIDs)  
        )  
        )  
        -- Pallav changes stop creating the ghost schedules multiple times and added startdate clause as some of the schedules were not getting created.      
        AND SM.ReferralTSDateID IS NULL  
        AND RTD.ReferralTSDateID NOT IN  
        (  
          SELECT  
            ReferralTSDateID  
          FROM ScheduleMasters  
          WHERE  
            EmployeeID = @NewEmployeeID  
            AND ReferralID = @ReferralID  
            AND IsDeleted = 0  
            AND startdate BETWEEN @startdate AND @enddate  
        )  
      ---End of Pallav's changes      
      ORDER BY ETD.EmployeeTSStartTime ASC  
    -- EMPLOYEE PTO CHECK          
    INSERT INTO @PTOTable  
      SELECT  
        T.NewGUID  
      FROM @TempTable T  
      LEFT JOIN EmployeeDayOffs EDO  
        ON EDO.EmployeeID = T.EmployeeID  
        AND EDO.DayOffStatus = 'Approved'  
        AND (  
        (T.StartDate BETWEEN EDO.StartTime AND EDO.EndTime  
        OR T.EndDate BETWEEN EDO.StartTime AND EDO.EndTime)  
        OR (EDO.StartTime BETWEEN T.StartDate AND T.EndDate  
        OR EDO.EndTime BETWEEN T.StartDate AND T.EndDate)  
        )  
      WHERE  
        EDO.IsDeleted = 0  
    -- EMPLOYEE PTO CHECK          
    -- DELETE FROM ScheduleMasters  WHERE ScheduleID > 78090           
    IF (  
      (  
        SELECT  
          COUNT(*)  
        FROM @TempTable  
      )  
      > 0  
      AND  
      (  
        SELECT  
          COUNT(*)  
        FROM @TempTable  
      )  
      =  
      (  
        SELECT  
          COUNT(*)  
        FROM @PTOTable  
      )  
      )  
    BEGIN  
      SELECT  
        -2  
      RETURN;  
    END  
    IF (@IsRescheduleAction = 0)  
    BEGIN  
      INSERT INTO ScheduleMasters  
      (  
        ReferralID,  
        StartDate,  
        EndDate,  
        ScheduleStatusID,  
        CreatedBy,  
        CreatedDate,  
        UpdatedBy,  
        UpdatedDate,  
        SystemID,  
        IsDeleted,  
        EmployeeID,  
        EmployeeTSDateID,  
        ReferralTSDateID,  
        PayorID,  
        CareTypeId,  
        ReferralBillingAuthorizationID,  
        AnyTimeClockIn,  
        IsVirtualVisit  
      )  
      OUTPUT inserted.ScheduleID,inserted.StartDate INTO @Output  
        SELECT  
          ReferralID,  
          StartDate,  
          EndDate,  
          ScheduleStatusID,  
          CreatedBy,  
          CreatedDate,  
          UpdatedBy,  
          UpdatedDate,  
          SystemID,  
          IsDeleted,  
          EmployeeID,  
          EmployeeTSDateID,  
          ReferralTSDateID,  
          @PayorID,  
          @CareTypeId,  
          @ReferralBillingAuthorizationID,  
          AnyTimeClockIn,  
          @IsVirtualVisit  
        FROM @TempTable T  
        WHERE  
          T.NewGUID NOT IN  
          (  
            SELECT  
              NewGUID  
            FROM @PTOTable  
          )  

        DECLARE eventCursor CURSOR FORWARD_ONLY FOR
            SELECT ScheduleID FROM @Output;
        OPEN eventCursor;
        FETCH NEXT FROM eventCursor INTO @CurScheduleID;
        WHILE @@FETCH_STATUS = 0 BEGIN
            EXEC [dbo].[ScheduleEventBroadcast] 'CreateSchedule', @CurScheduleID,'',''
            FETCH NEXT FROM eventCursor INTO @CurScheduleID;
        END;
        CLOSE eventCursor;
        DEALLOCATE eventCursor;

         
      SELECT TOP 1  
        ScheduleID  
      FROM @Output   
      ORDER BY StartDate;  
    END  
    ELSE  
    BEGIN  
      --                         
      DECLARE @I int = 0;  
      UPDATE SM  
      SET  
        SM.StartDate = T.StartDate,  
        SM.EndDate = T.EndDate,  
        SM.UpdatedBy = T.UpdatedBy,  
        SM.UpdatedDate = T.UpdatedDate,  
        SM.EmployeeID = T.EmployeeID,  
        SM.EmployeeTSDateID = T.EmployeeTSDateID,  
        SM.ReferralTSDateID = T.ReferralTSDateID,  
        SM.PayorID = @PayorID,  
        SM.CareTypeId = @CareTypeId,  
        SM.ReferralBillingAuthorizationID = @ReferralBillingAuthorizationID,  
        SM.IsVirtualVisit = @IsVirtualVisit,  
        SM.AnyTimeClockIn = T.AnyTimeClockIn 
      OUTPUT deleted.ScheduleID,inserted.StartDate INTO @Output   
      FROM ScheduleMasters SM  
      INNER JOIN @TempTable T  
        ON SM.ReferralTSDateID = T.ReferralTSDateID  
        AND SM.StartDate = T.StartDate  
        AND SM.EndDate = T.EndDate  
        AND SM.IsDeleted = 0  
      WHERE  
        T.NewGUID NOT IN  
        (  
          SELECT  
            NewGUID  
          FROM @PTOTable  
        )  
      --SELECT * FROM ScheduleMasters                        
      --SELECT T.ReferralID,T.StartDate,T.EndDate,T.ScheduleStatusID,T.CreatedBy,T.CreatedDate,T.UpdatedBy,T.UpdatedDate,T.SystemID,T.IsDeleted,T.EmployeeID,          
      --T.EmployeeTSDateID,T.ReferralTSDateID FROM @TempTable T                         
      --INNER JOIN ScheduleMasters SM ON SM.ReferralTSDateID=T.ReferralTSDateID AND SM.StartDate=T.StartDate AND SM.EndDate=T.EndDate                        
      --WHERE T.NewGUID NOT IN (SELECT NewGUID FROM @PTOTable)          
      SET @I = @I + @@ROWCOUNT;  

       DECLARE eventCursor CURSOR FORWARD_ONLY FOR
            SELECT ScheduleID FROM @Output;
        OPEN eventCursor;
        FETCH NEXT FROM eventCursor INTO @CurScheduleID;
        WHILE @@FETCH_STATUS = 0 BEGIN
            EXEC [dbo].[ScheduleEventBroadcast] 'EditSchedule', @CurScheduleID,'222','25'
            FETCH NEXT FROM eventCursor INTO @CurScheduleID;
        END;
        CLOSE eventCursor;
        DEALLOCATE eventCursor;  

        DELETE FROM @Output

      INSERT INTO ScheduleMasters  
      (  
        ReferralID,  
        StartDate,  
        EndDate,  
        ScheduleStatusID,  
        CreatedBy,  
        CreatedDate,  
        UpdatedBy,  
        UpdatedDate,  
        SystemID,  
        IsDeleted,  
        EmployeeID,  
        EmployeeTSDateID,  
        ReferralTSDateID,  
        PayorID,  
        CareTypeId,  
        ReferralBillingAuthorizationID,  
        AnyTimeClockIn,  
        IsVirtualVisit  
      )  
      OUTPUT inserted.ScheduleID,inserted.StartDate INTO @Output  
        SELECT  
          T.ReferralID,  
          T.StartDate,  
          T.EndDate,  
          T.ScheduleStatusID,  
          T.CreatedBy,  
          T.CreatedDate,  
          T.UpdatedBy,  
          T.UpdatedDate,  
          T.SystemID,  
          T.IsDeleted,  
          T.EmployeeID,  
          T.EmployeeTSDateID,  
          T.ReferralTSDateID,  
          @PayorID,  
          @CareTypeId,  
          @ReferralBillingAuthorizationID,  
          T.AnyTimeClockIn,  
          @IsVirtualVisit  
        FROM @TempTable T  
        LEFT JOIN ScheduleMasters SM  
          ON SM.ReferralTSDateID = T.ReferralTSDateID  
          AND SM.StartDate = T.StartDate  
          AND SM.EndDate = T.EndDate  
        WHERE  
          SM.ScheduleID IS NULL  
          AND T.NewGUID NOT IN  
          (  
            SELECT  
              NewGUID  
            FROM @PTOTable  
          )  
      SET @I = @I + @@ROWCOUNT;  

      
        DECLARE eventCursor CURSOR FORWARD_ONLY FOR
            SELECT ScheduleID FROM @Output;
        OPEN eventCursor;
        FETCH NEXT FROM eventCursor INTO @CurScheduleID;
        WHILE @@FETCH_STATUS = 0 BEGIN
            EXEC [dbo].[ScheduleEventBroadcast] 'CreateSchedule', @CurScheduleID,'',''
            FETCH NEXT FROM eventCursor INTO @CurScheduleID;
        END;
        CLOSE eventCursor;
        DEALLOCATE eventCursor;

      SELECT TOP 1  
        ScheduleID  
      FROM @Output   
      ORDER BY StartDate;  
    END  
  --SELECT @@ROWCOUNT;                        
  END  
  ELSE  
  BEGIN  
    DECLARE @Old_EmployeeTSDateID bigint = 0;  
    DECLARE @Old_ReferralTSDateID bigint = 0;  
    SELECT  
      @Old_EmployeeTSDateID = EmployeeTSDateID,  
      @Old_ReferralTSDateID = ReferralTSDateID  
    FROM ScheduleMasters  
    WHERE  
      ScheduleID = @ScheduleID  
    DECLARE @NEW_ReferralTSStartTime datetime = 0;  
    DECLARE @NEW_ReferralTSEndTime datetime = 0;  
    DECLARE @NEW_EmployeeTSDateID bigint = 0;  
    DECLARE @NEW_ReferralTSDateID bigint = 0;  
    DECLARE @ReferralTimeSlotDetailID bigint = 0;  
    SELECT  
      @NEW_ReferralTSDateID = ReferralTSDateID,  
      @ReferralTimeSlotDetailID = ReferralTimeSlotDetailID  
    FROM ReferralTimeSlotDates  
    WHERE  
      ReferralTSDateID = @Old_ReferralTSDateID  
    PRINT @Old_EmployeeTSDateID;  
    PRINT @Old_ReferralTSDateID;  
    PRINT @NEW_ReferralTSDateID;  
    PRINT @ReferralTimeSlotDetailID;  
    PRINT @NewEmployeeID;  
    SELECT TOP 1  
      @NEW_ReferralTSStartTime = RTD.ReferralTSStartTime,  
      @NEW_ReferralTSEndTime = RTD.ReferralTSEndTime,  
      @NEW_EmployeeTSDateID = ETD.EmployeeTSDateID,  
      @NEW_ReferralTSDateID = RTD.ReferralTSDateID  
    FROM EmployeeTimeSlotDates ETD  
    INNER JOIN EmployeeTimeSlotDetails ETSD  
      ON ETD.EmployeeID = @NewEmployeeID  
      AND ETSD.StartTime = CAST(ETD.EmployeeTSStartTime AS time)  
      AND DATEPART(dw, ETD.EmployeeTSDate) = ETSD.Day  
      AND ETSD.IsDeleted = 0  
  
      AND (  
      (@SameDateWithTimeSlot = 0  
      AND ETD.EmployeeTSDate BETWEEN @StartDate AND @EndDate)  
      OR (@SameDateWithTimeSlot = 1  
      AND @StartDate BETWEEN ETD.EmployeeTSStartTime AND ETD.EmployeeTSEndTime  
      AND @EndDate BETWEEN ETD.EmployeeTSStartTime AND ETD.EmployeeTSEndTime)  
      )  
  
    INNER JOIN  
    (  
      SELECT  
        EmployeeTimeSlotDetailID = CONVERT(bigint, VAL)  
      FROM GetCSVTable(@EmployeeTimeSlotDetailIDs)  
    ) T  
      ON T.EmployeeTimeSlotDetailID = ETSD.EmployeeTimeSlotDetailID  
    INNER JOIN ReferralTimeSlotDates RTD  
      ON RTD.ReferralID = @ReferralID  
      AND RTD.ReferralTSDateID = @New_ReferralTSDateID  
  
      AND RTD.ReferralTSStartTime BETWEEN ETD.EmployeeTSStartTime AND ETD.EmployeeTSEndTime  
      AND RTD.ReferralTSEndTime BETWEEN ETD.EmployeeTSStartTime AND ETD.EmployeeTSEndTime  
  
      AND (  
      (@SameDateWithTimeSlot = 0  
      AND RTD.ReferralTSDate BETWEEN @StartDate AND @EndDate)  
      OR (@SameDateWithTimeSlot = 1  
      AND (RTD.ReferralTSStartTime BETWEEN @StartDate AND @EndDate  
      AND RTD.ReferralTSEndTime BETWEEN @StartDate AND @EndDate)  
      OR (@StartDate BETWEEN RTD.ReferralTSStartTime AND RTD.ReferralTSEndTime  
      AND @EndDate BETWEEN RTD.ReferralTSStartTime AND RTD.ReferralTSEndTime)  
      )  
      )  
    --AND RTD.ReferralTimeSlotDetailID=@ReferralTimeSlotDetailID          
    LEFT JOIN ScheduleMasters SM  
      ON SM.IsDeleted = 0  
      AND SM.ScheduleID != @ScheduleID  
      AND (SM.ReferralID = @ReferralID  
      OR SM.EmployeeID = @NewEmployeeID)  
      AND (RTD.ReferralTSStartTime BETWEEN SM.StartDate AND SM.EndDate  
      OR RTD.ReferralTSEndTime BETWEEN SM.StartDate AND SM.EndDate)  
    --(SM.EmployeeTSDateID=ETD.EmployeeTSDateID AND SM.ReferralTSDateID=RTD.ReferralTSDateID)            
    WHERE  
      SM.ScheduleID IS NULL  
    ORDER BY ETD.EmployeeTSStartTime ASC  
    PRINT @NEW_EmployeeTSDateID;  
    PRINT @NEW_ReferralTSDateID; 
    DECLARE @Cnt int = 0;   
    IF (@NEW_EmployeeTSDateID > 0  
      AND @NEW_ReferralTSDateID > 0)  
    BEGIN  
      -- CHECK FOR EMPLOYEE PTO          
      IF EXISTS  
        (  
          SELECT TOP 1  
            1  
          FROM EmployeeDayOffs EDO  
          WHERE  
            EDO.EmployeeID = @NewEmployeeID  
            AND EDO.DayOffStatus = 'Approved'  
            AND EDO.EmployeeDayOffID IS NOT NULL  
            AND EDO.IsDeleted = 0  
            AND ((@NEW_ReferralTSStartTime BETWEEN EDO.StartTime AND EDO.EndTime  
                OR @NEW_ReferralTSEndTime BETWEEN EDO.StartTime AND EDO.EndTime)  
              OR (EDO.StartTime BETWEEN @NEW_ReferralTSStartTime AND @NEW_ReferralTSEndTime  
                OR EDO.EndTime BETWEEN @NEW_ReferralTSStartTime AND @NEW_ReferralTSEndTime)  
            )  
        )  
      BEGIN  
        SELECT  
          -2  
        RETURN;  
      END  
      -- CHECK FOR EMPLOYEE BLOCK          
      UPDATE ScheduleMasters  
      SET  
        StartDate = @NEW_ReferralTSStartTime,  
        EndDate = @NEW_ReferralTSEndTime,  
        ReferralID = @ReferralID,  
        EmployeeID = @NewEmployeeID,  
        UpdatedBy = @loggedInId,  
        UpdatedDate = GETUTCDATE(),  
        EmployeeTSDateID = @NEW_EmployeeTSDateID,  
        ReferralTSDateID = @NEW_ReferralTSDateID,  
        PayorID = @PayorID,  
        CareTypeId = @CareTypeId,  
        ReferralBillingAuthorizationID = @ReferralBillingAuthorizationID,  
        IsVirtualVisit = @IsVirtualVisit  
      WHERE  
        ScheduleID = @ScheduleID  
        SET @Cnt =  @Cnt + @@ROWCOUNT;

      EXEC [dbo].[ScheduleEventBroadcast] 'EditSchedule', @ScheduleID,'222','25'
    END  
    SELECT  
      @Cnt;  
  END  
END