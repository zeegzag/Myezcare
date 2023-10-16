-- EXEC UpdateRtsDetail @SortExpression = 'Day', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50', @ReferralTimeSlotDetailID = '30038', @ReferralTimeSlotMasterID = '1966', @StartTime = '06:00:00', @EndTime = '07:00:00', @UsedInScheduling = 'True', @Notes = '', @loggedInUserID = '1', @SystemID = '::1', @SelectedDays = '', @ListOfIdsInCsv = '30038', @IsShowList = 'True'        
CREATE PROCEDURE [dbo].[UpdateRtsDetail]             
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
 @ListOfIdsInCsv varchar(300),        
 @CareTypeId BIGINT,      
 @GeneratePatientSchedules BIT,      
 @AnyTimeClockIn  BIT=0,      
 @ReferralBillingAuthorizationID BIGINT=NULL
AS                        
BEGIN                        
DECLARE @TablePrimaryId bigint;                    
DECLARE @StartDate DATE;      
DECLARE @EndDate DATE;      
DECLARE @MaxDate DATE='2099-12-31';      
DECLARE @ReferralTimeSlotMasterIds VARCHAR(MAX);      
      
 BEGIN TRANSACTION trans                    
 BEGIN TRY      
      
 SELECT @StartDate=StartDate,@EndDate=EndDate FROM ReferralTimeSlotMaster (NOLOCK) WHERE ReferralTimeSlotMasterID=@ReferralTimeSlotMasterID      
      
   SELECT @ReferralTimeSlotMasterIds =       
        STUFF((SELECT ', ' + CONVERT(VARCHAR(50),ReferralTimeSlotMasterID)      
           FROM ReferralTimeSlotMaster     (NOLOCK)    
           WHERE     
    ReferralID=@ReferralID AND     
    (    
     (    
      StartDate>=@StartDate AND     
      StartDate<=IsNull(@EndDate,@MaxDate)    
     )     
     OR      
     (EndDate>=@StartDate AND EndDate<=IsNull(@EndDate,@MaxDate)) OR (@StartDate>=StartDate AND @StartDate<=IsNull(EndDate,@MaxDate))     
     OR (@EndDate >=StartDate AND @EndDate<=IsNull(EndDate,@MaxDate)))      
          FOR XML PATH('')), 1, 2, '')      
  Print 1     
      
-- CHECK FOR EXIST        
        
  DECLARE @TempTable TABLE          
  (          
    DayID BIGINT,           
    Day INT,        
 ExcludeFromInsert Bit DEFAULT 0        
  )          
        
  DECLARE @ExcludedCount INT = 0, @TotalCount INT = 0, @Result INT = 0;        
           
  Print 10    
    
  INSERT INTO @TempTable        
  SELECT ReturnId, RESULT, 0 FROM [dbo].[CSVtoTableWithIdentity](@SelectedDays, ',')        
        
Print 15    
    
  DECLARE @ExistTimeSlotTable TABLE (ReferralTimeSlotID BIGINT)      
      
  INSERT INTO @ExistTimeSlotTable      
 SELECT DISTINCT ReferralTimeSlotMasterID      
 FROM ReferralTimeSlotDetails E     (NOLOCK)         
 INNER JOIN @TempTable T ON E.Day=T.Day      
 WHERE (      
 (E.StartTime > @StartTime and E.StartTime< @EndTime)          
    or (E.EndTime > @StartTime and E.EndTime < @EndTime)          
    or (@StartTime > E.StartTime and @StartTime< E.EndTime) or (@EndTime > E.StartTime and @EndTime< E.EndTime)          
    or (@StartTime = E.StartTime and @EndTime = E.EndTime)          
 )           
 AND E.ReferralTimeSlotMasterID in (SELECT val FROM GetCSVTable(@ReferralTimeSlotMasterIds)) AND E.ReferralTimeSlotDetailID!=@ReferralTimeSlotDetailID    
 AND E.CareTypeId = @CareTypeId
 AND E.IsDeleted=0 AND E.Day=T.Day      
      
Print 20    
    
  IF EXISTS(SELECT * FROM @ExistTimeSlotTable)             
 BEGIN            
        
Print 22    
    
 UPDATE T        
 SET ExcludeFromInsert = 1        
 FROM ReferralTimeSlotDetails E             
 INNER JOIN @TempTable T ON E.Day=T.Day        
 WHERE (            
 (E.StartTime > @StartTime and E.StartTime< @EndTime)            
    or (E.EndTime > @StartTime and E.EndTime < @EndTime)            
    or (@StartTime > E.StartTime and @StartTime< E.EndTime) or (@EndTime > E.StartTime and @EndTime< E.EndTime)      
    or (@StartTime = E.StartTime and @EndTime = E.EndTime)                   
   ) AND E.ReferralTimeSlotMasterID in (SELECT val FROM GetCSVTable(@ReferralTimeSlotMasterIds)) AND E.ReferralTimeSlotDetailID!=@ReferralTimeSlotDetailID AND E.IsDeleted=0  
   AND E.CareTypeId = @CareTypeId
        
Print 22.1    
          
END          
        
-- SELECT * FROm @TempTable        
        
-- EXEC UpdateRtsDetail @SortExpression = 'Day', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50', @ReferralTimeSlotDetailID = '50076', @ReferralTimeSlotMasterID = '1966', @StartTime = '04:00:00', @EndTime = '06:00:00', @UsedInScheduling = 'True', @Notes = '', @loggedInUserID = '1', @SystemID = '::1', @SelectedDays = '1', @ListOfIdsInCsv = '50076', @IsShowList = 'True'        
        
        
 SELECT @ExcludedCount = COUNT(*) FROM @TempTable WHERE ExcludeFromInsert = 1        
 SELECT @TotalCount = COUNT(*) FROM @TempTable         
         
           
  IF(@TotalCount > @ExcludedCount)        
  BEGIN        
         
Print 30.0    
                
   INSERT INTO ReferralTimeSlotDetails                        
     (ReferralTimeSlotMasterID,Day,StartTime,EndTime,UsedInScheduling,Notes,AnyTimeClockIn,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,SystemID,CareTypeId, ReferralBillingAuthorizationID)                        
     SELECT @ReferralTimeSlotMasterID,T.Day,@StartTime,@EndTime,@UsedInScheduling,@Notes,@AnyTimeClockIn,@loggedInUserId,GETUTCDATE(),@loggedInUserId,GETUTCDATE(),@SystemID,@CareTypeId, @ReferralBillingAuthorizationID        
     FROM  @TempTable T         
     WHERE ExcludeFromInsert = 0        
        
Print 30.1    
        
     -- DELETE        
     Exec DeleteRtsDetail @ReferralTimeSlotMasterID, NULL,NULL,-1, @SortExpression, @SortType, @FromIndex, @PageSize ,  @ListOfIdsInCsv, @IsShowList, @loggedInUserId, @CareTypeId         
        
Print 30.2    
      
  --Create Slots      
   INSERT INTO ReferralTimeSlotDates         
  SELECT T.ReferralID,T.ReferralTimeSlotMasterID, T.ReferralTSDate,T.ReferralTSStartTime,T.ReferralTSEndTime,      
   T.UsedInScheduling,T.Notes,T.DayNumber,T.ReferralTimeSlotDetailID,0,NULL,0 FROM (        
   SELECT E.ReferralID,ETM.ReferralTimeSlotMasterID, ReferralTSDate=IndividualDate,ETMEndDate=ETM.EndDate,        
   ReferralTSStartTime=TS.TSStartTime,                  
   ReferralTSEndTime= CASE WHEN TS.TSStartTime <= TS.TSEndTime THEN TS.TSEndTime ELSE TS.TSEndTimeNextDay END,         
   ETSD.UsedInScheduling,ETSD.Notes,DayNumber=T1.DayNameInt,ETSD.ReferralTimeSlotDetailID,ETSD.CareTypeId        
  FROM DateRange(CASE WHEN @StartDate < @TodayDate THEN @TodayDate ELSE @StartDate END, @SlotEndDate) T1        
  INNER JOIN ReferralTimeSlotDetails ETSD  (NOLOCK) ON ETSD.Day=T1.DayNameInt AND ETSD.IsDeleted=0      
  CROSS APPLY (
		SELECT
		TSStartTime = CONVERT(datetime, CONVERT(char(8), T1.IndividualDate, 112) + ' ' + CONVERT(char(8), StartTime, 108)),  
		TSEndTime = CONVERT(datetime, CONVERT(char(8), T1.IndividualDate, 112) + ' ' + CONVERT(char(8), EndTime, 108)), 
		TSEndTimeNextDay = CONVERT(datetime, CONVERT(char(8), DATEADD(D, 1,T1.IndividualDate), 112) + ' ' + CONVERT(char(8), EndTime, 108))
	) TS
  INNER JOIN ReferralTimeSlotMaster ETM (NOLOCK) ON ETM.ReferralTimeSlotMasterID=ETSD.ReferralTimeSlotMasterID  AND ETM.IsDeleted=0        
  INNER JOIN Referrals E (NOLOCK) ON E.ReferralID=ETM.ReferralID        
  ) AS T        
 LEFT JOIN (SELECT SDT.ReferralID, SDT.ReferralTSDateID, SDT.ReferralTSStartTime, SDT.ReferralTSEndTime, SD.CareTypeId FROM ReferralTimeSlotDates SDT INNER JOIN ReferralTimeSlotDetails SD ON SDT.ReferralTimeSlotDetailID = SD.ReferralTimeSlotDetailID) ETSDT ON ETSDT.ReferralTSStartTime = T.ReferralTSStartTime AND ETSDT.ReferralTSEndTime= T.ReferralTSEndTime AND ETSDT.CareTypeId = T.CareTypeId         
 AND ETSDT.ReferralID=T.ReferralID        
 WHERE  ETSDT.ReferralTSDateID IS NULL  AND T.ReferralTSDate <= ISNULL(ETMEndDate,@SlotEndDate)       
 AND (@ReferralID=0 OR T.ReferralID=@ReferralID) AND T.ReferralTimeSlotMasterID=@ReferralTimeSlotMasterID AND T.DayNumber IN (SELECT val FROM GetCSVTable(@SelectedDays))      
 ORDER BY T.ReferralID ASC, T.ReferralTimeSlotMasterID ASC      
      
Print 30.3    
    
 --Create Schedule For Daycare Type Organization      
 IF(@GeneratePatientSchedules=1 AND @ReferralID>0 AND @ReferralTimeSlotMasterID>0)        
  BEGIN      
        
Print 40.1    
     
    
    DECLARE @Output TABLE (  ScheduleID bigint   )    
    
   INSERT INTO ScheduleMasters(ReferralID,FacilityID,StartDate,EndDate,ScheduleStatusID,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,ReferralTSDateID,PayorID,        
   IsDeleted,IsPatientAttendedSchedule)          
       OUTPUT inserted.ScheduleID INTO @Output    
   SELECT R.ReferralID,R.DefaultFacilityID,RTD.ReferralTSStartTime,ReferralTSEndTime,2,@loggedInUserId,GETDATE(),@loggedInUserId,GETDATE(),RTD.ReferralTSDateID,RPM.PayorID,0,NULL         
   FROM ReferralTimeSlotDates RTD    (NOLOCK)     
   INNER JOIN Referrals R (NOLOCK) ON R.ReferralID=@ReferralID        
   LEFT JOIN ReferralPayorMappings RPM (NOLOCK) ON RPM.ReferralID=R.ReferralID AND RPM.Precedence=1 AND RPM.IsDeleted=0 AND IsActive=1        
   AND  RTD.ReferralTSDate BETWEEN PayorEffectiveDate AND ISNULL(PayorEffectiveEndDate,@MaxDate)        
   LEFT JOIN ScheduleMasters SM (NOLOCK) ON SM.ReferralTSDateID=RTD.ReferralTSDateID         
   WHERE RTD.ReferralID=@ReferralID AND RTD.ReferralTimeSlotMasterID=@ReferralTimeSlotMasterID         
   AND RTD.UsedInScheduling=1 AND RTD.OnHold=0 AND RTD.IsDenied=0 AND SM.ScheduleID IS NULL        
   AND RTD.ReferralTSDate BETWEEN @StartDate AND @EndDate        
        
Print 40.2    
     
     
  DECLARE @CurScheduleID bigint;    
    
   DECLARE eventCursor CURSOR FORWARD_ONLY FOR    
            SELECT ScheduleID FROM @Output;    
        OPEN eventCursor;    
        FETCH NEXT FROM eventCursor INTO @CurScheduleID;    
        WHILE @@FETCH_STATUS = 0 BEGIN    
        
Print 'begin ScheduleEventBroadcast '    
    
            EXEC [dbo].[ScheduleEventBroadcast] 'CreateSchedule', @CurScheduleID,'',''    
Print 'end [ScheduleEventBroadcast]'    
    
            FETCH NEXT FROM eventCursor INTO @CurScheduleID;    
        END;    
        CLOSE eventCursor;    
        DEALLOCATE eventCursor;    
    
  END       
      
 SET @Result  = CASE WHEN @ExcludedCount = 0 THEN 1 ELSE 2 END        
        
    END        
  ELSE        
   BEGIN      
    IF((SELECT COUNT(*) FROM @ExistTimeSlotTable)>1) OR NOT EXISTS(SELECT * FROM @ExistTimeSlotTable WHERE ReferralTimeSlotID=@ReferralTimeSlotMasterID)           
     SET @Result= -3      
    ELSE      
     SET @Result = -2      
   END      
            
          
        
        
Print 60    
        
         
   SELECT @Result AS TransactionResultId,1 AS TablePrimaryId;                    
   IF @@TRANCOUNT > 0          
    BEGIN                     
     COMMIT TRANSACTION trans                     
    END                    
  END TRY                    
  BEGIN CATCH                    
   SELECT -1 AS TransactionResultId,ERROR_MESSAGE() AS ErrorMessage;      
       
Print 70    
    
   IF @@TRANCOUNT > 0                    
   BEGIN                     
    ROLLBACK TRANSACTION trans                     
   END                    
  END CATCH                   
                    
END