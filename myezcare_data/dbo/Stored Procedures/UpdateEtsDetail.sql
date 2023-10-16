-- EXEC UpdateRtsDetail @SortExpression = 'Day', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50', @ReferralTimeSlotDetailID = '30038', @ReferralTimeSlotMasterID = '1966', @StartTime = '06:00:00', @EndTime = '07:00:00', @UsedInScheduling = 'True', @Notes = '', @loggedInUserID = '1', @SystemID = '::1', @SelectedDays = '', @ListOfIdsInCsv = '30038', @IsShowList = 'True'  
  
CREATE PROCEDURE [dbo].[UpdateEtsDetail]       
 @EmployeeTimeSlotDetailID BIGINT,        
 @EmployeeTimeSlotMasterID BIGINT,
 @EmployeeID BIGINT,        
 @Day INT=0,        
 @StartTime TIME(7),        
 @EndTime TIME(7),  
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
 @ListOfIdsInCsv varchar(300)  
   
         
AS                  
BEGIN                  
DECLARE @TablePrimaryId bigint;
DECLARE @StartDate DATE;
DECLARE @EndDate DATE;
DECLARE @MaxDate DATE='2099-12-31';
DECLARE @EmployeeTimeSlotMasterIds VARCHAR(MAX);
              
 BEGIN TRANSACTION trans              
 BEGIN TRY              
  
  SELECT @StartDate=StartDate,@EndDate=EndDate FROM EmployeeTimeSlotMaster WHERE EmployeeTimeSlotMasterID=@EmployeeTimeSlotMasterID

   SELECT @EmployeeTimeSlotMasterIds = 
        STUFF((SELECT ', ' + CONVERT(VARCHAR(50),EmployeeTimeSlotMasterID)
           FROM EmployeeTimeSlotMaster
           WHERE EmployeeID=@EmployeeID AND ((StartDate>=@StartDate AND StartDate<=IsNull(@EndDate,@MaxDate)) OR  (EndDate>=@StartDate AND EndDate<=IsNull(@EndDate,@MaxDate)) OR (@StartDate>=StartDate AND @StartDate<=IsNull(EndDate,@MaxDate)) OR (@EndDate>=StartDate AND @EndDate<=IsNull(EndDate,@MaxDate)))
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

  DECLARE @ExistTimeSlotTable TABLE (EmployeeTimeSlotID BIGINT)

  INSERT INTO @ExistTimeSlotTable
	SELECT DISTINCT EmployeeTimeSlotMasterID
	FROM EmployeeTimeSlotDetails E       
	INNER JOIN @TempTable T ON E.Day=T.Day
	WHERE (
	(E.StartTime > @StartTime and E.StartTime< @EndTime)    
    or (E.EndTime > @StartTime and E.EndTime < @EndTime)    
    or (@StartTime > E.StartTime and @StartTime< E.EndTime) or (@EndTime > E.StartTime and @EndTime< E.EndTime)    
    or (@StartTime = E.StartTime and @EndTime = E.EndTime)    
	)     
	AND E.EmployeeTimeSlotMasterID in (SELECT val FROM GetCSVTable(@EmployeeTimeSlotMasterIds)) AND E.EmployeeTimeSlotDetailID!=@EmployeeTimeSlotDetailID 
	AND E.IsDeleted=0 AND E.Day=T.Day
  
  IF EXISTS(SELECT * FROM @ExistTimeSlotTable)
 BEGIN      
  
 UPDATE T  
 SET ExcludeFromInsert = 1  
 FROM EmployeeTimeSlotDetails E       
 INNER JOIN @TempTable T ON E.Day=T.Day  
 WHERE (      
   
                (E.StartTime > @StartTime and E.StartTime< @EndTime)      
    or (E.EndTime > @StartTime and E.EndTime < @EndTime)      
    or (@StartTime > E.StartTime and @StartTime< E.EndTime) or (@EndTime > E.StartTime and @EndTime< E.EndTime)
    or (@StartTime = E.StartTime and @EndTime = E.EndTime)             
   )       
      AND E.EmployeeTimeSlotMasterID in (SELECT val FROM GetCSVTable(@EmployeeTimeSlotMasterIds)) AND E.EmployeeTimeSlotDetailID!=@EmployeeTimeSlotDetailID AND E.IsDeleted=0  
  
    
END    
  
-- SELECT * FROm @TempTable  
  
-- EXEC UpdateRtsDetail @SortExpression = 'Day', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50', @ReferralTimeSlotDetailID = '50076', @ReferralTimeSlotMasterID = '1966', @StartTime = '04:00:00', @EndTime = '06:00:00', @UsedInScheduling = 'True', @Notes = '', @loggedInUserID = '1', @SystemID = '::1', @SelectedDays = '1', @ListOfIdsInCsv = '50076', @IsShowList = 'True'  
  
  
 SELECT @ExcludedCount = COUNT(*) FROM @TempTable WHERE ExcludeFromInsert = 1  
 SELECT @TotalCount = COUNT(*) FROM @TempTable   
   
     
  IF(@TotalCount > @ExcludedCount)  
  BEGIN  
   
          
   INSERT INTO EmployeeTimeSlotDetails                  
     (EmployeeTimeSlotMasterID,Day,StartTime,EndTime,Notes,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,SystemID)                  
     SELECT @EmployeeTimeSlotMasterID,T.Day,@StartTime,@EndTime,@Notes,@loggedInUserId,GETUTCDATE(),@loggedInUserId,GETUTCDATE(),@SystemID  
     FROM  @TempTable T   
     WHERE ExcludeFromInsert = 0  
  
     -- DELETE  
     Exec DeleteEtsDetail @EmployeeTimeSlotMasterID, NULL,NULL,-1, @SortExpression, @SortType, @FromIndex, @PageSize ,   
   @ListOfIdsInCsv, @IsShowList, @loggedInUserId

   --Create Slots
   INSERT INTO EmployeeTimeSlotDates   
		SELECT T.EmployeeID,T.EmployeeTimeSlotMasterID, T.EmployeeTSDate,T.EmployeeTSStartTime,T.EmployeeTSEndTime,T.DayNumber,T.EmployeeTimeSlotDetailID FROM (  
		SELECT E.EmployeeID,ETM.EmployeeTimeSlotMasterID, EmployeeTSDate=IndividualDate,ETMEndDate=ETM.EndDate,  
		EmployeeTSStartTime=CONVERT(DATETIME, CONVERT(CHAR(8), T1.IndividualDate, 112) + ' ' + CONVERT(CHAR(8), StartTime, 108)),  
		EmployeeTSEndTime=CONVERT(DATETIME, CONVERT(CHAR(8), T1.IndividualDate, 112) + ' ' + CONVERT(CHAR(8), EndTime, 108)),  
		DayNumber=T1.DayNameInt,ETSD.EmployeeTimeSlotDetailID  
		FROM DateRange(CASE WHEN @StartDate < @TodayDate THEN @TodayDate ELSE @StartDate END, @SlotEndDate) T1  
		INNER JOIN EmployeeTimeSlotDetails ETSD ON ETSD.Day=T1.DayNameInt AND ETSD.IsDeleted=0  
		INNER JOIN EmployeeTimeSlotMaster ETM ON ETM.EmployeeTimeSlotMasterID=ETSD.EmployeeTimeSlotMasterID  AND ETM.IsDeleted=0  
		INNER JOIN Employees E ON E.EmployeeID=ETM.EmployeeID  
		) AS T  
	LEFT JOIN EmployeeTimeSlotDates ETSDT ON ETSDT.EmployeeTSStartTime= T.EmployeeTSStartTime AND ETSDT.EmployeeTSEndTime= T.EmployeeTSEndTime   
	AND ETSDT.EmployeeID=T.EmployeeID  
	WHERE  ETSDT.EmployeeTSDateID IS NULL  AND T.EmployeeTSDate <= ISNULL(ETMEndDate,@SlotEndDate) 
	AND (@EmployeeID=0 OR T.EmployeeID=@EmployeeID) AND T.EmployeeTimeSlotMasterID=@EmployeeTimeSlotMasterID AND T.DayNumber IN (SELECT val FROM GetCSVTable(@SelectedDays))
	ORDER BY T.EmployeeID ASC, T.EmployeeTimeSlotMasterID ASC
	  
   SET @Result  = CASE WHEN @ExcludedCount = 0 THEN 1 ELSE 2 END  
  
    END  
    ELSE
	BEGIN
		IF((SELECT COUNT(*) FROM @ExistTimeSlotTable)>1) OR NOT EXISTS(SELECT * FROM @ExistTimeSlotTable WHERE EmployeeTimeSlotID=@EmployeeTimeSlotMasterID)     
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
