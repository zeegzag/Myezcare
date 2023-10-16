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
 @GeneratePatientSchedules BIT
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
           WHERE ReferralID=@ReferralID AND ((StartDate>=@StartDate AND StartDate<=IsNull(@EndDate,@MaxDate)) OR  (EndDate>=@StartDate AND EndDate<=IsNull(@EndDate,@MaxDate)) OR (@StartDate>=StartDate AND @StartDate<=IsNull(EndDate,@MaxDate)) OR (@EndDate>=StartDate AND @EndDate<=IsNull(EndDate,@MaxDate)))
          FOR XML PATH('')), 1, 2, '')
  
-- CHECK FOR EXIST  
  
  DECLARE @TempTable TABLE    
  (    
    DayID BIGINT,     
    Day INT,  
 ExcludeFromInsert Bit DEFAULT 0  
  )    
  
  DECLARE @ExcludedCount INT = 0, @TotalCount INT = 0, @Result INT = 0;  
     
  INSERT INTO @TempTable  
  SELECT ReturnId, RESULT, 0 FROM [dbo].[CSVtoTableWithIdentity](@SelectedDays, ',')  
  
  DECLARE @ExistTimeSlotTable TABLE (ReferralTimeSlotID BIGINT)

  INSERT INTO @ExistTimeSlotTable
	SELECT DISTINCT ReferralTimeSlotMasterID
	FROM ReferralTimeSlotDetails E       
	INNER JOIN @TempTable T ON E.Day=T.Day
	WHERE (
	(E.StartTime > @StartTime and E.StartTime< @EndTime)    
    or (E.EndTime > @StartTime and E.EndTime < @EndTime)    
    or (@StartTime > E.StartTime and @StartTime< E.EndTime) or (@EndTime > E.StartTime and @EndTime< E.EndTime)    
    or (@StartTime = E.StartTime and @EndTime = E.EndTime)    
	)     
	AND E.ReferralTimeSlotMasterID in (SELECT val FROM GetCSVTable(@ReferralTimeSlotMasterIds)) AND E.ReferralTimeSlotDetailID!=@ReferralTimeSlotDetailID 
	AND E.IsDeleted=0 AND E.Day=T.Day

  IF EXISTS(SELECT * FROM @ExistTimeSlotTable)       
 BEGIN      
  
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
  
    
END    
  
-- SELECT * FROm @TempTable  
  
-- EXEC UpdateRtsDetail @SortExpression = 'Day', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50', @ReferralTimeSlotDetailID = '50076', @ReferralTimeSlotMasterID = '1966', @StartTime = '04:00:00', @EndTime = '06:00:00', @UsedInScheduling = 'True', @Notes = '', @loggedInUserID = '1', @SystemID = '::1', @SelectedDays = '1', @ListOfIdsInCsv = '50076', @IsShowList = 'True'  
  
  
 SELECT @ExcludedCount = COUNT(*) FROM @TempTable WHERE ExcludeFromInsert = 1  
 SELECT @TotalCount = COUNT(*) FROM @TempTable   
   
     
  IF(@TotalCount > @ExcludedCount)  
  BEGIN  
   
          
   INSERT INTO ReferralTimeSlotDetails                  
     (ReferralTimeSlotMasterID,Day,StartTime,EndTime,UsedInScheduling,Notes,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,SystemID,CareTypeId)                  
     SELECT @ReferralTimeSlotMasterID,T.Day,@StartTime,@EndTime,@UsedInScheduling,@Notes,@loggedInUserId,GETUTCDATE(),@loggedInUserId,GETUTCDATE(),@SystemID,@CareTypeId  
     FROM  @TempTable T   
     WHERE ExcludeFromInsert = 0  
  
  
     -- DELETE  
     Exec DeleteRtsDetail @ReferralTimeSlotMasterID, NULL,NULL,-1, @SortExpression, @SortType, @FromIndex, @PageSize ,   
   @ListOfIdsInCsv, @IsShowList, @loggedInUserId  
  

  --Create Slots
   INSERT INTO ReferralTimeSlotDates   
		SELECT T.ReferralID,T.ReferralTimeSlotMasterID, T.ReferralTSDate,T.ReferralTSStartTime,T.ReferralTSEndTime,
			T.UsedInScheduling,T.Notes,T.DayNumber,T.ReferralTimeSlotDetailID,0,NULL,0 FROM (  
			SELECT E.ReferralID,ETM.ReferralTimeSlotMasterID, ReferralTSDate=IndividualDate,ETMEndDate=ETM.EndDate,  
			ReferralTSStartTime=CONVERT(DATETIME, CONVERT(CHAR(8), T1.IndividualDate, 112) + ' ' + CONVERT(CHAR(8), StartTime, 108)),  
			ReferralTSEndTime=CONVERT(DATETIME, CONVERT(CHAR(8), T1.IndividualDate, 112) + ' ' + CONVERT(CHAR(8), EndTime, 108)),  
			ETSD.UsedInScheduling,ETSD.Notes,DayNumber=T1.DayNameInt,ETSD.ReferralTimeSlotDetailID  
		FROM DateRange(CASE WHEN @StartDate < @TodayDate THEN @TodayDate ELSE @StartDate END, @SlotEndDate) T1  
		INNER JOIN ReferralTimeSlotDetails ETSD ON ETSD.Day=T1.DayNameInt AND ETSD.IsDeleted=0  
		INNER JOIN ReferralTimeSlotMaster ETM ON ETM.ReferralTimeSlotMasterID=ETSD.ReferralTimeSlotMasterID  AND ETM.IsDeleted=0  
		INNER JOIN Referrals E ON E.ReferralID=ETM.ReferralID  
		) AS T  
	LEFT JOIN ReferralTimeSlotDates ETSDT ON ETSDT.ReferralTSStartTime= T.ReferralTSStartTime AND ETSDT.ReferralTSEndTime= T.ReferralTSEndTime   
	AND ETSDT.ReferralID=T.ReferralID  
	WHERE  ETSDT.ReferralTSDateID IS NULL  AND T.ReferralTSDate <= ISNULL(ETMEndDate,@SlotEndDate) 
	AND (@ReferralID=0 OR T.ReferralID=@ReferralID) AND T.ReferralTimeSlotMasterID=@ReferralTimeSlotMasterID AND T.DayNumber IN (SELECT val FROM GetCSVTable(@SelectedDays))
	ORDER BY T.ReferralID ASC, T.ReferralTimeSlotMasterID ASC

	--Create Schedule For Daycare Type Organization
	IF(@GeneratePatientSchedules=1 AND @ReferralID>0 AND @ReferralTimeSlotMasterID>0)  
		BEGIN
  
			INSERT INTO ScheduleMasters(ReferralID,FacilityID,StartDate,EndDate,ScheduleStatusID,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,ReferralTSDateID,PayorID,  
			IsDeleted,IsPatientAttendedSchedule)        
			SELECT R.ReferralID,R.DefaultFacilityID,RTD.ReferralTSStartTime,ReferralTSEndTime,2,@loggedInUserId,GETDATE(),@loggedInUserId,GETDATE(),RTD.ReferralTSDateID,RPM.PayorID,0,NULL		 
			FROM ReferralTimeSlotDates RTD  
			INNER JOIN Referrals R ON R.ReferralID=@ReferralID  
			LEFT JOIN ReferralPayorMappings RPM ON RPM.ReferralID=R.ReferralID AND RPM.Precedence=1 AND RPM.IsDeleted=0 AND IsActive=1  
			AND  RTD.ReferralTSDate BETWEEN PayorEffectiveDate AND ISNULL(PayorEffectiveEndDate,@MaxDate)  
			LEFT JOIN ScheduleMasters SM ON SM.ReferralTSDateID=RTD.ReferralTSDateID   
			WHERE RTD.ReferralID=@ReferralID AND RTD.ReferralTimeSlotMasterID=@ReferralTimeSlotMasterID   
			AND RTD.UsedInScheduling=1 AND RTD.OnHold=0 AND RTD.IsDenied=0 AND SM.ScheduleID IS NULL  
			AND RTD.ReferralTSDate BETWEEN @StartDate AND @EndDate  
  
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
      
    
  
  
  
   
 SELECT @Result AS TransactionResultId,1 AS TablePrimaryId;              
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
