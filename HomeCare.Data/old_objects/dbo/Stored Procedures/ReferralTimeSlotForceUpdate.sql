CREATE PROCEDURE [dbo].[ReferralTimeSlotForceUpdate]      
 @ReferralTimeSlotDetailID BIGINT,            
 @ReferralTimeSlotMasterID BIGINT,      
 @ReferralID BIGINT,      
 @Day INT=0,            
 @StartTime TIME(7),            
 @EndTime TIME(7),      
 @UsedInScheduling BIT,      
 @Notes NVARCHAR(1000),            
 @loggedInUserId BIGINT,            
 @SystemID VARCHAR(100),        
 @SelectedDays VARCHAR(20),      
 @TodayDate DATE,      
 @SlotEndDate DATE,      
 @SortExpression NVARCHAR(100),                    
 @SortType NVARCHAR(10),                  
 @FromIndex INT,                  
 @PageSize INT,                  
 @IsShowList bit,      
 @CareTypeId BIGINT,            
 @ListOfIdsInCsv varchar(300),      
 @IsEdit BIT,      
 @GeneratePatientSchedules BIT,  
 @AnyTimeClockIn  BIT=0     
AS                      
BEGIN                      
DECLARE @TablePrimaryId bigint;      
DECLARE @StartDate DATE;      
DECLARE @EndDate DATE;      
DECLARE @MaxDate DATE='2099-12-31';      
DECLARE @ReferralTimeSlotMasterIds VARCHAR(MAX);      
      
                  
BEGIN TRANSACTION trans                  
 BEGIN TRY                  
      
 SELECT @StartDate=StartDate,@EndDate=EndDate FROM ReferralTimeSlotMaster WHERE ReferralTimeSlotMasterID=@ReferralTimeSlotMasterID      
      
 SELECT @ReferralTimeSlotMasterIds =       
        STUFF((SELECT ', ' + CONVERT(VARCHAR(50),ReferralTimeSlotMasterID)      
           FROM ReferralTimeSlotMaster      
           WHERE ReferralID=@ReferralID AND ((StartDate>=@StartDate AND StartDate<=IsNull(@EndDate,@MaxDate)) OR  (EndDate>=@StartDate AND EndDate<=IsNull(@EndDate,@MaxDate)) OR (@StartDate>=StartDate AND @StartDate<=IsNull(EndDate,@MaxDate)) OR (@EndDate
  
    
>=StartDate AND @EndDate<=IsNull(EndDate,@MaxDate)))      
          FOR XML PATH('')), 1, 2, '')      
       
 DECLARE @TempTable TABLE (          
    DayID BIGINT,      
    Day INT,        
 ExcludeFromInsert Bit DEFAULT 0)          
        
  DECLARE @Result INT = 0;      
      
  INSERT INTO @TempTable        
  SELECT ReturnId, RESULT, 0        
  FROM [dbo].[CSVtoTableWithIdentity](@SelectedDays, ',')      
      
   --Insert Time Slot Details      
   INSERT INTO ReferralTimeSlotDetails                        
     (ReferralTimeSlotMasterID,Day,StartTime,EndTime,UsedInScheduling,Notes,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,SystemID,CareTypeId,AnyTimeClockIn)                        
     SELECT @ReferralTimeSlotMasterID,T.Day,@StartTime,@EndTime,@UsedInScheduling,@Notes,@loggedInUserId,GETUTCDATE(),@loggedInUserId,GETUTCDATE(),@SystemID,@CareTypeId,@AnyTimeClockIn        
     FROM  @TempTable T         
     WHERE ExcludeFromInsert = 0      
       
 --DELETE conflicting dates      
 DELETE FROM ReferralTimeSlotDates WHERE ReferralTimeSlotMasterID IN (SELECT val FROM GetCSVTable(@ReferralTimeSlotMasterIds))       
 AND (ReferralTSDate>=CASE WHEN @StartDate < @TodayDate THEN @TodayDate ELSE @StartDate END AND ReferralTSDate<=ISNULL(@EndDate,@SlotEndDate))       
 AND DayNumber IN (SELECT val FROM GetCSVTable(@SelectedDays))       
 AND ((CONVERT(TIME,ReferralTSStartTime) > @StartTime and CONVERT(TIME,ReferralTSStartTime)< @EndTime)          
    or (CONVERT(TIME,ReferralTSEndTime) > @StartTime and CONVERT(TIME,ReferralTSEndTime) < @EndTime)          
    or (@StartTime > CONVERT(TIME,ReferralTSStartTime) and @StartTime< CONVERT(TIME,ReferralTSEndTime)) or (@EndTime > CONVERT(TIME,ReferralTSStartTime) and @EndTime< CONVERT(TIME,ReferralTSEndTime))      
    or (@StartTime = CONVERT(TIME,ReferralTSStartTime) and @EndTime = CONVERT(TIME,ReferralTSEndTime)))      
      
 IF(@IsEdit=1)      
 Exec DeleteRtsDetail @ReferralTimeSlotMasterID, NULL,NULL,-1, @SortExpression, @SortType, @FromIndex, @PageSize,@ListOfIdsInCsv, @IsShowList, @loggedInUserId      
      
 --INSERT new date for new date range (Create Slots)      
 INSERT INTO ReferralTimeSlotDates         
  SELECT T.ReferralID,T.ReferralTimeSlotMasterID, T.ReferralTSDate,T.ReferralTSStartTime,T.ReferralTSEndTime,      
   T.UsedInScheduling,T.Notes,T.DayNumber,T.ReferralTimeSlotDetailID,0,NULL,0 FROM (        
   SELECT E.ReferralID,ETM.ReferralTimeSlotMasterID, ReferralTSDate=IndividualDate,ETMEndDate=ETM.EndDate,        
   ReferralTSStartTime=CONVERT(DATETIME, CONVERT(CHAR(8), T1.IndividualDate, 112) + ' ' + CONVERT(CHAR(8), StartTime, 108)),        
   ReferralTSEndTime=CONVERT(DATETIME, CONVERT(CHAR(8), T1.IndividualDate, 112) + ' ' + CONVERT(CHAR(8), EndTime, 108)),        
   ETSD.UsedInScheduling,ETSD.Notes,DayNumber=T1.DayNameInt,ETSD.ReferralTimeSlotDetailID,ETSD.CareTypeId      
  FROM DateRange(CASE WHEN @StartDate < @TodayDate THEN @TodayDate ELSE @StartDate END, @SlotEndDate) T1        
  INNER JOIN ReferralTimeSlotDetails ETSD ON ETSD.Day=T1.DayNameInt AND ETSD.IsDeleted=0        
  INNER JOIN ReferralTimeSlotMaster ETM ON ETM.ReferralTimeSlotMasterID=ETSD.ReferralTimeSlotMasterID  AND ETM.IsDeleted=0        
  INNER JOIN Referrals E ON E.ReferralID=ETM.ReferralID        
  ) AS T        
 LEFT JOIN (SELECT SDT.ReferralID, SDT.ReferralTSDateID, SDT.ReferralTSStartTime, SDT.ReferralTSEndTime, SD.CareTypeId FROM ReferralTimeSlotDates SDT INNER JOIN ReferralTimeSlotDetails SD ON SDT.ReferralTimeSlotDetailID = SD.ReferralTimeSlotDetailID) ETSDT ON ETSDT.ReferralTSStartTime = T.ReferralTSStartTime AND ETSDT.ReferralTSEndTime= T.ReferralTSEndTime AND ETSDT.CareTypeId = T.CareTypeId
 AND ETSDT.ReferralID=T.ReferralID        
 WHERE  ETSDT.ReferralTSDateID IS NULL  AND T.ReferralTSDate <= ISNULL(ETMEndDate,@SlotEndDate)       
 AND (@ReferralID=0 OR T.ReferralID=@ReferralID) AND T.ReferralTimeSlotMasterID=@ReferralTimeSlotMasterID AND T.DayNumber IN (SELECT val FROM GetCSVTable(@SelectedDays))      
 ORDER BY T.ReferralID ASC, T.ReferralTimeSlotMasterID ASC        
      
 --Create Schedule For Daycare Type Organization      
 IF(@GeneratePatientSchedules=1 AND @ReferralID>0 AND @ReferralTimeSlotMasterID>0)        
  BEGIN     
      
  DECLARE @Output TABLE (      
    ScheduleID bigint     
  )       
        
   INSERT INTO ScheduleMasters(ReferralID,FacilityID,StartDate,EndDate,ScheduleStatusID,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,ReferralTSDateID,PayorID,        
   IsDeleted,IsPatientAttendedSchedule)         
        OUTPUT inserted.ScheduleID INTO @Output    
   SELECT R.ReferralID,R.DefaultFacilityID,RTD.ReferralTSStartTime,ReferralTSEndTime,2,@loggedInUserId,GETDATE(),@loggedInUserId,GETDATE(),RTD.ReferralTSDateID,RPM.PayorID,0,NULL         
   FROM ReferralTimeSlotDates RTD        
   INNER JOIN Referrals R ON R.ReferralID=@ReferralID        
   LEFT JOIN ReferralPayorMappings RPM ON RPM.ReferralID=R.ReferralID AND RPM.Precedence=1 AND RPM.IsDeleted=0 AND IsActive=1        
   AND  RTD.ReferralTSDate BETWEEN PayorEffectiveDate AND ISNULL(PayorEffectiveEndDate,@MaxDate)        
   LEFT JOIN ScheduleMasters SM ON SM.ReferralTSDateID=RTD.ReferralTSDateID         
   WHERE RTD.ReferralID=@ReferralID AND RTD.ReferralTimeSlotMasterID=@ReferralTimeSlotMasterID         
   AND RTD.UsedInScheduling=1 AND RTD.OnHold=0 AND RTD.IsDenied=0 AND SM.ScheduleID IS NULL        
   AND RTD.ReferralTSDate BETWEEN @StartDate AND @EndDate        
        
    DECLARE @CurScheduleID bigint;    
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
  END       
      
      
  SELECT @Result=1      
                  
 SELECT @Result AS TransactionResultId,@TablePrimaryId AS TablePrimaryId;      
      IF @@TRANCOUNT > 0                  
     BEGIN                   
      COMMIT TRANSACTION trans                   
     END                  
   END TRY                  
   BEGIN CATCH                  
    SELECT -1 AS TransactionResultId,ERROR_MESSAGE() AS ErrorMessage;                  
    IF @@TRANCOUNT > 0                  
     BEGIN                   
      ROLLBACK TRANSACTION trans                   
     END                  
  END CATCH            
                  
END 