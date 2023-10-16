CREATE PROCEDURE [dbo].[AddRtsDetail]         
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
 @CareTypeId BIGINT,  
 @GeneratePatientSchedules BIT,
 @IsForcePatientSchedules  BIT = 0  
AS                    
BEGIN                    
DECLARE @TablePrimaryId bigint;  
DECLARE @StartDate DATE;  
DECLARE @EndDate DATE;  
DECLARE @MaxDate DATE='2099-12-31';  
DECLARE @ReferralTimeSlotMasterIds VARCHAR(MAX);  
Declare @PriorAuthorizationFrequencyType    varchar(100);
Declare @AllowedTime bigint;
Declare @IsLimitExceeded BIT = 0;           
 BEGIN TRANSACTION trans                
 BEGIN TRY  
   SELECT @StartDate=StartDate,@EndDate=EndDate FROM ReferralTimeSlotMaster WHERE ReferralTimeSlotMasterID=@ReferralTimeSlotMasterID  
  
	SELECT  @PriorAuthorizationFrequencyType = ddm.Title, @AllowedTime = rba.AllowedTime
		FROM [dbo].[ReferralBillingAuthorizations] AS rba
		LEFT JOIN [dbo].[DDMaster] ddm 
		ON ddm.DDMasterID = rba.PriorAuthorizationFrequencyType
		WHERE rba.ReferralID = @ReferralID and rba.StartDate <= @StartDate;
	
		SELECT  @IsLimitExceeded = (CASE 
		WHEN @PriorAuthorizationFrequencyType = 'Daily'  and (DATEDIFF(MINUTE, @StartTime, @EndTime) > @AllowedTime)	THEN  1
		WHEN @PriorAuthorizationFrequencyType = 'Weekly' and (7 * DATEDIFF(MINUTE, @StartTime, @EndTime) > @AllowedTime) THEN 1
		WHEN @PriorAuthorizationFrequencyType = 'Monthly' and (30 * DATEDIFF(MINUTE, @StartTime, @EndTime) > @AllowedTime) THEN 1
		WHEN @PriorAuthorizationFrequencyType = 'Monthly' and (365 * DATEDIFF(MINUTE, @StartTime, @EndTime) > @AllowedTime) THEN 1			
		ELSE 0
		END);

		print @AllowedTime;
		print @PriorAuthorizationFrequencyType;
IF((@IsForcePatientSchedules = 1 and @IsLimitExceeded = 1) or @IsLimitExceeded=0)
BEGIN 
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
    
  DECLARE @ExcludedCount INT = 0, @TotalCount INT = 0, @Result INT = 0;    
    
  INSERT INTO @TempTable    
  SELECT ReturnId, RESULT, 0    
  FROM [dbo].[CSVtoTableWithIdentity](@SelectedDays, ',')  
  
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
   )         
      AND E.ReferralTimeSlotMasterID in (SELECT val FROM GetCSVTable(@ReferralTimeSlotMasterIds)) AND E.ReferralTimeSlotDetailID!=@ReferralTimeSlotDetailID AND E.IsDeleted=0    
    
      
END      
    
  SELECT @ExcludedCount = COUNT(*)     
  FROM @TempTable     
  WHERE ExcludeFromInsert = 1    
    
  SELECT @TotalCount = COUNT(*)     
  FROM @TempTable     
      
  IF(@TotalCount > @ExcludedCount)    
  BEGIN    
     
   INSERT INTO ReferralTimeSlotDetails                    
     (ReferralTimeSlotMasterID,Day,StartTime,EndTime,UsedInScheduling,Notes,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,SystemID,CareTypeId)                    
     SELECT @ReferralTimeSlotMasterID,T.Day,@StartTime,@EndTime,@UsedInScheduling,@Notes,@loggedInUserId,GETUTCDATE(),@loggedInUserId,GETUTCDATE(),@SystemID,@CareTypeId    
     FROM  @TempTable T     
     WHERE ExcludeFromInsert = 0    
     SET @Result  = CASE WHEN @ExcludedCount = 0 THEN 1 ELSE 2 END  
  
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
 AND (@ReferralID=0 OR T.ReferralID=@ReferralID) AND T.ReferralTimeSlotMasterID=@ReferralTimeSlotMasterID  
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
    
 END    
 ELSE  
 BEGIN  
  IF((SELECT COUNT(*) FROM @ExistTimeSlotTable)>1) OR NOT EXISTS(SELECT * FROM @ExistTimeSlotTable WHERE ReferralTimeSlotID=@ReferralTimeSlotMasterID)       
   SET @Result= -3  
  ELSE  
   SET @Result = -2  
   END  
END 
ElSE    
	BEGIN  
		set @Result = -4
	END  
     
                    
 --IF(@ReferralTimeSlotDetailID=0)                    
 --BEGIN                    
 --  INSERT INTO ReferralTimeSlotDetails                    
 --  (ReferralTimeSlotMasterID,Day,StartTime,EndTime,UsedInScheduling,Notes,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,SystemID)                    
 --  VALUES                    
 --  (@ReferralTimeSlotMasterID,@Day,@StartTime,@EndTime,@UsedInScheduling,@Notes,@loggedInUserId,GETUTCDATE(),@loggedInUserId,GETUTCDATE(),@SystemID);                     
                       
 --SET @TablePrimaryId = @@IDENTITY;             
 --END                    
 --ELSE                    
 --BEGIN                    
 --  UPDATE ReferralTimeSlotDetails                     
 --  SET                          
 --     ReferralTimeSlotMasterID=@ReferralTimeSlotMasterID,          
 --  Day=@Day,                    
 --     StartTime=@StartTime,                  
 --  EndTime=@EndTime,      
 --  UsedInScheduling=@UsedInScheduling,    
 --  Notes=@Notes,      
 --     UpdatedBy=@loggedInUserId,                   
 --     UpdatedDate=GETUTCDATE(),                    
 --     SystemID=@SystemID                    
 --  WHERE ReferralTimeSlotDetailID=@ReferralTimeSlotDetailID;                    
 --END                    
                
     
     
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
