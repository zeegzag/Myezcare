CREATE PROCEDURE [dbo].[AddRtsMaster]             
@ReferralTimeSlotMasterID BIGINT,                
  @ReferralID BIGINT,                
  @StartDate DATE,                
  @EndDate DATE = NULL,                
  @IsEndDateAvailable BIT,                
  @TodayDate DATE,                
  @SlotEndDate DATE,                
  @loggedInUserId BIGINT,                
  @SystemID VARCHAR(100),                
  @ReferralBillingAuthorizationID BIGINT = NULL,    
  @IsWithPriorAuth BIT=0,  
  @CareTypeID BIGINT=0  
AS          
          
BEGIN                
  DECLARE @TablePrimaryId BIGINT;                
  DECLARE @ReferralTimeSlotMasterIds VARCHAR(MAX);                
  DECLARE @Output TABLE (ScheduleID BIGINT)                
                
  BEGIN TRANSACTION trans                
                
  BEGIN TRY                
    IF (                
        @ReferralBillingAuthorizationID IS NOT NULL                
        AND @ReferralBillingAuthorizationID > 0                
        AND @ReferralTimeSlotMasterID = 0                
        )                
    BEGIN                
      -- User is trying to add time slot for the authorization (Case Management)                      
      --SET @ReferralTimeSlotMasterID = (                
      --    SELECT ReferralTimeSlotMasterID                
      --    FROM ReferralTimeSlotMaster                
      --    WHERE ReferralBillingAuthorizationID = @ReferralBillingAuthorizationID                
      --      AND ReferralID = @ReferralID                
      --    )              
                 
                   
          SELECT @ReferralTimeSlotMasterID=ReferralTimeSlotMasterID                
          FROM ReferralTimeSlotMaster                
          WHERE ReferralBillingAuthorizationID = @ReferralBillingAuthorizationID                
            AND ReferralID = @ReferralID                
            AND IsDeleted = 0            
    END                
                
    DECLARE @MaxDate DATE;                
                
    SET @MaxDate = '2099-12-31';                
                
    IF (@EndDate = '')                
      SET @EndDate = NULL;                
                
    --Code for check existing date range                      
     IF EXISTS(SELECT * FROM ReferralTimeSlotMaster E WHERE ((@StartDate>=E.StartDate                 
              AND @StartDate<=COALESCE(E.EndDate,@MaxDate)) OR (@EndDate>=E.StartDate AND @EndDate<=COALESCE(E.EndDate,@MaxDate)))                 
              AND E.ReferralID in (@ReferralID) AND E.ReferralTimeSlotMasterID!=@ReferralTimeSlotMasterID AND E.IsDeleted=0 AND E.ReferralBillingAuthorizationID=@ReferralBillingAuthorizationID)                          
     BEGIN                                  
  SELECT -2 AS TransactionResultId                                   
  -- IF @@TRANCOUNT > 0                                          
  --     BEGIN                                           
  --      COMMIT TRANSACTION trans                                           
  --     END                                    
  RETURN;                                    
    END                          
             
    IF (@ReferralTimeSlotMasterID = 0)                
    BEGIN                
      INSERT INTO ReferralTimeSlotMaster (                
        ReferralID,                
        StartDate,                
        EndDate,                
        IsEndDateAvailable,                
        CreatedBy,                
        CreatedDate,                
        UpdatedBy,                
        UpdatedDate,                
        SystemID,                
        ReferralBillingAuthorizationID,    
  IsWithPriorAuth,  
  CareTypeID  
        )                
      VALUES (                
        @ReferralID,                
        @StartDate,                
        @EndDate,                
        @IsEndDateAvailable,                
        @loggedInUserId,                
        GETUTCDATE(),                
        @loggedInUserId,                
  GETUTCDATE(),                
        @SystemID,                
        CASE                 
          WHEN @ReferralBillingAuthorizationID = 0                
            THEN NULL                
          ELSE @ReferralBillingAuthorizationID                
          END ,    
    @IsWithPriorAuth,  
 @CareTypeID  
        );                
                
      SET @TablePrimaryId = @@IDENTITY;                
    END                
    ELSE                
    BEGIN       
      DECLARE @NewStartDate DATE,                
        @NewEndDate DATE;                
                
      SELECT @NewStartDate = CASE                 
          WHEN @StartDate < @TodayDate                
            THEN @TodayDate             
          ELSE @StartDate                
          END,                
        @NewEndDate = ISNULL(@EndDate, @MaxDate)            
                    
      DECLARE @TempTable TABLE (ReferralTSDateID BIGINT)                
                
      INSERT INTO @TempTable                
      SELECT ReferralTSDateID                
      FROM ReferralTimeSlotDates ETD                
      WHERE ETD.ReferralTimeSlotMasterID = @ReferralTimeSlotMasterID                
        AND ETD.ReferralTSDate > @TodayDate                
        AND (                
          ETD.ReferralTSDate < @NewStartDate                
          OR ETD.ReferralTSDate > @NewEndDate                
          )               
            
         --Code for check existing date range                      
         IF EXISTS(SELECT SM.ScheduleID                
                      FROM ScheduleMasters SM             
                      INNER JOIN EmployeeVisits EV ON EV.ScheduleID = SM.ScheduleID AND EV.IsDeleted = 0            
                      WHERE SM.ReferralTSDateID IN (SELECT ReferralTSDateID FROM @TempTable))                          
         BEGIN                                  
      SELECT -3 AS TransactionResultId                                                       
      RETURN;                                    
        END              
                      
      INSERT INTO @Output                
      SELECT ScheduleID                
      FROM ScheduleMasters                
      WHERE ReferralTSDateID IN (SELECT ReferralTSDateID FROM @TempTable)               
                
      UPDATE ReferralTimeSlotMaster                
      SET ReferralID = @ReferralID,                
        StartDate = @StartDate,                
        EndDate = @EndDate,                
        IsEndDateAvailable = @IsEndDateAvailable,                
        UpdatedBy = @loggedInUserId,                
        UpdatedDate = GETUTCDATE(),                
        SystemID = @SystemID,                
        ReferralBillingAuthorizationID = @ReferralBillingAuthorizationID,    
  IsWithPriorAuth=@IsWithPriorAuth,  
  CareTypeID=@CareTypeID  
      WHERE ReferralTimeSlotMasterID = @ReferralTimeSlotMasterID;   
-- Update RTS Detals------
 DECLARE @TempRTDTable TABLE (ReferralTimeSlotMasterID BIGINT,ReferralTSDateID BIGINT) 

	  INSERT INTO @TempRTDTable 
select  RTM.ReferralTimeSlotMasterID,rtdl.ReferralTimeSlotDetailID
from ReferralTimeSlotMaster rtm
left join ReferralTimeSlotDetails rtdl on rtdl.ReferralTimeSlotMasterID=rtm.ReferralTimeSlotMasterID
left join ReferralTimeSlotDates rtd on rtd.ReferralTimeSlotMasterID=rtm.ReferralTimeSlotMasterID
where rtm.ReferralTimeSlotMasterID=@ReferralTimeSlotMasterID AND RTM.IsDeleted=0 AND RTDL.IsDeleted=0

update ReferralTimeSlotDetails set ReferralBillingAuthorizationID=@ReferralBillingAuthorizationID,CareTypeId=@CareTypeID where ReferralTimeSlotMasterID in (select ReferralTimeSlotMasterID from @TempRTDTable)
update ScheduleMasters set ReferralBillingAuthorizationID=@ReferralBillingAuthorizationID where cast(StartDate as date)>@StartDate and ReferralTSDateID in (select ReferralTSDateID from @TempRTDTable)


                
      UPDATE ScheduleMasters                
      SET IsDeleted = 1                
      WHERE ReferralTSDateID IN (                
          SELECT ReferralTSDateID                
          FROM @TempTable                
          )                
                
      DELETE                
      FROM ReferralTimeSlotDates                
      WHERE ReferralTSDateID IN (                
          SELECT ReferralTSDateID                
          FROM @TempTable                
          )                
                
      --Start New Code                      
      SELECT @ReferralTimeSlotMasterIds = STUFF((                
            SELECT ', ' + CONVERT(VARCHAR(50), ReferralTimeSlotMasterID)                
            FROM ReferralTimeSlotMaster                
            WHERE ReferralID = @ReferralID                
              AND IsDeleted = 0                
              AND (                
                (                
                  StartDate >= @StartDate                
                  AND StartDate <= IsNull(@EndDate, @MaxDate)                
                  )                
                OR (                
                  EndDate >= @StartDate                
                  AND EndDate <= IsNull(@EndDate, @MaxDate)                
                  )                
                OR (                
                  @StartDate >= StartDate                
                  AND @StartDate <= IsNull(EndDate, @MaxDate)         
                  )                
                OR (                
                  @EndDate >= StartDate                
                  AND @EndDate <= IsNull(EndDate, @MaxDate)                
                  )                
                )                
            FOR XML PATH('')                
            ), 1, 2, '')                
                
      DECLARE @ConflictedSlotDetail TABLE (                
        RowNo INT,                
        ReferralTimeSlotMasterID BIGINT,                
        ReferralTimeSlotDetailID BIGINT,                
        Day INT,                
        StartTime TIME,                
        EndTime TIME,                
        CareTypeId BIGINT                
        )                
      DECLARE @ConflictedSlots TABLE (                
        ReferralTimeSlotMasterID BIGINT,                
        ReferralTimeSlotDetailID BIGINT,                
        Day INT,                
        StartTime TIME,                
        EndTime TIME                
        )                
                
      INSERT INTO @ConflictedSlotDetail                
      SELECT ROW_NUMBER() OVER (                
          ORDER BY ReferralTimeSlotMasterID,                
            Day,                
            StartTime,                
            EndTime                
          ) AS RowNo,                
        ReferralTimeSlotMasterID,                
        ReferralTimeSlotDetailID,                
        Day,                
        StartTime,                
        EndTime,                
        CareTypeId                
      FROM ReferralTimeSlotDetails                
      WHERE ReferralTimeSlotMasterID IN (                
          SELECT val                
          FROM GetCSVTable(@ReferralTimeSlotMasterIds)                
          )                
        AND IsDeleted = 0                
                
      INSERT INTO @ConflictedSlots                
      SELECT t1.ReferralTimeSlotMasterID,                
        t1.ReferralTimeSlotDetailID,                
        t1.Day,                
        t1.StartTime,                
        t1.EndTime                
      FROM @ConflictedSlotDetail t1                
      INNER JOIN @ConflictedSlotDetail t2                
        ON (                
            t1.RowNo <> t2.RowNo                
            AND (                
              (                
                t2.StartTime > t1.StartTime                
                AND t2.StartTime < t1.EndTime                
                )                
              OR (                
                t2.EndTime > t1.StartTime                
                AND t2.EndTime < t1.EndTime                
                )                
              OR (                
                t1.StartTime > t2.StartTime                
                AND t1.StartTime < t2.EndTime                
                )                
              OR (                
                t1.EndTime > t2.StartTime                
                AND t1.EndTime < t2.EndTime                
                )                
              OR (                
                t1.StartTime = t2.StartTime                
                AND t1.EndTime = t2.EndTime                
          )                
              )                
            AND t1.Day = t2.Day                
            AND t1.CareTypeId = t2.CareTypeId                
            )                
                
      INSERT INTO @Output                
      SELECT ScheduleID                
      FROM ScheduleMasters                
      WHERE ReferralTSDateID IN (                
          SELECT ReferralTSDateID                
          FROM ReferralTimeSlotDates                
          WHERE ReferralTimeSlotDetailID IN (                
              SELECT ReferralTimeSlotDetailID                
              FROM @ConflictedSlots                
              )                
            AND (                
              ReferralTSDate >= @NewStartDate                
              AND ReferralTSDate <= ISNULL(@EndDate, @SlotEndDate)                
              )                
          )                
      
      UPDATE ScheduleMasters                
      SET IsDeleted = 1                
      WHERE ReferralTSDateID IN (                
          SELECT ReferralTSDateID                
          FROM ReferralTimeSlotDates                
          WHERE ReferralTimeSlotDetailID IN (                
              SELECT ReferralTimeSlotDetailID                
              FROM @ConflictedSlots                
              )                
            AND ReferralTSDate > @TodayDate                
            AND (                
              ReferralTSDate >= @NewStartDate                
              AND ReferralTSDate <= ISNULL(@EndDate, @SlotEndDate)                
              )                
          )                
                
      --DELETE conflicting dates                      
      DELETE                
      FROM ReferralTimeSlotDates                
      WHERE ReferralTimeSlotDetailID IN (                
          SELECT ReferralTimeSlotDetailID                
          FROM @ConflictedSlots                
          )                
        AND ReferralTSDate > @TodayDate                
        AND (                
          ReferralTSDate >= @NewStartDate                
          AND ReferralTSDate <= ISNULL(@EndDate, @SlotEndDate)                
          )                
                
      --Create Slots                      
      INSERT INTO ReferralTimeSlotDates                
      SELECT T.ReferralID,                
        T.ReferralTimeSlotMasterID,                
        T.ReferralTSDate,                
        T.ReferralTSStartTime,                
        T.ReferralTSEndTime,                
        T.UsedInScheduling,                
        T.Notes,                
        T.DayNumber,                
        T.ReferralTimeSlotDetailID,                
        0,                
        NULL,                
        0                
      FROM (                
        SELECT E.ReferralID,                
          ETM.ReferralTimeSlotMasterID,                
          ReferralTSDate = IndividualDate,                
          ETMEndDate = ETM.EndDate,                
          ReferralTSStartTime=TS.TSStartTime,                                  
    ReferralTSEndTime= CASE WHEN TS.TSStartTime <= TS.TSEndTime THEN TS.TSEndTime ELSE TS.TSEndTimeNextDay END,                 
          ETSD.UsedInScheduling,                
          ETSD.Notes,                
          DayNumber = T1.DayNameInt,                
          ETSD.ReferralTimeSlotDetailID,                
          ETSD.CareTypeId                
        FROM DateRange(@NewStartDate, @SlotEndDate) T1                
        INNER JOIN ReferralTimeSlotDetails ETSD                
          ON ETSD.Day = T1.DayNameInt                
            AND ETSD.IsDeleted = 0                
  CROSS APPLY (                
   SELECT                
   TSStartTime = CONVERT(datetime, CONVERT(char(8), T1.IndividualDate, 112) + ' ' + CONVERT(char(8), StartTime, 108)),                  
   TSEndTime = CONVERT(datetime, CONVERT(char(8), T1.IndividualDate, 112) + ' ' + CONVERT(char(8), EndTime, 108)),                 
   TSEndTimeNextDay = CONVERT(datetime, CONVERT(char(8), DATEADD(D, 1,T1.IndividualDate), 112) + ' ' + CONVERT(char(8), EndTime, 108))                
  ) TS                
        INNER JOIN ReferralTimeSlotMaster ETM                
          ON ETM.ReferralTimeSlotMasterID = ETSD.ReferralTimeSlotMasterID                
            AND ETM.IsDeleted = 0                
        INNER JOIN Referrals E                
          ON E.ReferralID = ETM.ReferralID                
        ) AS T                
      LEFT JOIN (                
        SELECT SDT.ReferralID,                
          SDT.ReferralTSDateID,                
          SDT.ReferralTSStartTime,                
          SDT.ReferralTSEndTime,                
          SD.CareTypeId                
        FROM ReferralTimeSlotDates SDT                
        INNER JOIN ReferralTimeSlotDetails SD                
          ON SDT.ReferralTimeSlotDetailID = SD.ReferralTimeSlotDetailID                
        ) ETSDT                
        ON ETSDT.ReferralTSStartTime = T.ReferralTSStartTime                
          AND ETSDT.ReferralTSEndTime = T.ReferralTSEndTime                
          AND ETSDT.CareTypeId = T.CareTypeId                
          AND ETSDT.ReferralID = T.ReferralID                
      WHERE ETSDT.ReferralTSDateID IS NULL                
        AND T.ReferralTSDate <= ISNULL(ETMEndDate, @SlotEndDate)                
        AND (                
          @ReferralID = 0                
          OR T.ReferralID = @ReferralID                
          )                
        AND T.ReferralTimeSlotMasterID = @ReferralTimeSlotMasterID                 
        --AND T.DayNumber IN (SELECT val FROM GetCSVTable(@SelectedDays))                      
      ORDER BY T.ReferralID ASC,                
        T.ReferralTimeSlotMasterID ASC                
        --End New Code                      
    END                
                
    SELECT 1 AS TransactionResultId,                
      @TablePrimaryId AS TablePrimaryId;                
                
    IF @@TRANCOUNT > 0                
    BEGIN                
      COMMIT TRANSACTION trans                
                
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