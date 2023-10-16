--Exec AddEtsDetail @EmployeeTimeSlotMasterID=6,@EmployeeID=56,@StartDate='2007-03-01',@EndDate='2022-12-31',@loggedInUserId=1,@SystemID='::1'      
CREATE PROCEDURE [dbo].[EmployeeTimeSlotForceUpdate]      
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
 @ListOfIdsInCsv varchar(300),
 @IsEdit BIT

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
 
 DECLARE @TempTable TABLE (    
    DayID BIGINT,
    Day INT,  
	ExcludeFromInsert Bit DEFAULT 0)    
  
  DECLARE @Result INT = 0;

  INSERT INTO @TempTable  
  SELECT ReturnId, RESULT, 0  
  FROM [dbo].[CSVtoTableWithIdentity](@SelectedDays, ',')

   --Insert Time Slot Details
   INSERT INTO EmployeeTimeSlotDetails                  
     (EmployeeTimeSlotMasterID,Day,StartTime,EndTime,Notes,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,SystemID)                     
     SELECT @EmployeeTimeSlotMasterID,T.Day,@StartTime,@EndTime,@Notes,@loggedInUserId,GETUTCDATE(),@loggedInUserId,GETUTCDATE(),@SystemID  
     FROM  @TempTable T   
     WHERE ExcludeFromInsert = 0
	
	--DELETE conflicting dates
	DELETE FROM EmployeeTimeSlotDates WHERE EmployeeTimeSlotMasterID IN (SELECT val FROM GetCSVTable(@EmployeeTimeSlotMasterIds)) 
	AND EmployeeTSDate > @TodayDate
	AND (EmployeeTSDate>=CASE WHEN @StartDate < @TodayDate THEN @TodayDate ELSE @StartDate END AND EmployeeTSDate<=ISNULL(@EndDate,@SlotEndDate)) 
	AND DayNumber IN (SELECT val FROM GetCSVTable(@SelectedDays)) 
	AND ((CONVERT(TIME,EmployeeTSStartTime) > @StartTime and CONVERT(TIME,EmployeeTSStartTime)< @EndTime)    
    or (CONVERT(TIME,EmployeeTSEndTime) > @StartTime and CONVERT(TIME,EmployeeTSEndTime) < @EndTime)    
    or (@StartTime > CONVERT(TIME,EmployeeTSStartTime) and @StartTime< CONVERT(TIME,EmployeeTSEndTime)) or (@EndTime > CONVERT(TIME,EmployeeTSStartTime) and @EndTime< CONVERT(TIME,EmployeeTSEndTime))
    or (@StartTime = CONVERT(TIME,EmployeeTSStartTime) and @EndTime = CONVERT(TIME,EmployeeTSEndTime)))

	IF(@IsEdit=1)
	Exec DeleteEtsDetail @EmployeeTimeSlotMasterID, NULL,NULL,-1, @SortExpression, @SortType, @FromIndex, @PageSize,@ListOfIdsInCsv, @IsShowList, @loggedInUserId

	--INSERT new date for new date range (Create Slots)
	INSERT INTO EmployeeTimeSlotDates   
		SELECT T.EmployeeID,T.EmployeeTimeSlotMasterID, T.EmployeeTSDate,T.EmployeeTSStartTime,T.EmployeeTSEndTime,T.DayNumber,T.EmployeeTimeSlotDetailID FROM (  
		SELECT E.EmployeeID,ETM.EmployeeTimeSlotMasterID, EmployeeTSDate=IndividualDate,ETMEndDate=ETM.EndDate,  
		EmployeeTSStartTime = TS.TSStartTime,  
        EmployeeTSEndTime = CASE WHEN TS.TSStartTime <= TS.TSEndTime AND ISNULL(ETSD.Is24Hrs, 0) = 0 THEN TS.TSEndTime ELSE TS.TSEndTimeNextDay END, 
		DayNumber=T1.DayNameInt,ETSD.EmployeeTimeSlotDetailID  
		FROM DateRange(CASE WHEN @StartDate < @TodayDate THEN @TodayDate ELSE @StartDate END, @SlotEndDate) T1  
		INNER JOIN EmployeeTimeSlotDetails ETSD ON ETSD.Day=T1.DayNameInt AND ETSD.IsDeleted=0  
		CROSS APPLY (
		  SELECT
				TSStartTime = CONVERT(datetime, CONVERT(char(8), T1.IndividualDate, 112) + ' ' + ISNULL(CONVERT(char(8), StartTime, 108), '00:00:00')),  
				TSEndTime = CONVERT(datetime, CONVERT(char(8), T1.IndividualDate, 112) + ' ' + ISNULL(CONVERT(char(8), EndTime, 108), '23:59:59')), 
				TSEndTimeNextDay = CONVERT(datetime, CONVERT(char(8), DATEADD(D, 1,T1.IndividualDate), 112) + ' ' + ISNULL(CONVERT(char(8), EndTime, 108), '11:59:59'))
		) TS
		INNER JOIN EmployeeTimeSlotMaster ETM ON ETM.EmployeeTimeSlotMasterID=ETSD.EmployeeTimeSlotMasterID  AND ETM.IsDeleted=0  
		INNER JOIN Employees E ON E.EmployeeID=ETM.EmployeeID  
		) AS T  
	LEFT JOIN EmployeeTimeSlotDates ETSDT ON ETSDT.EmployeeTSStartTime= T.EmployeeTSStartTime AND ETSDT.EmployeeTSEndTime= T.EmployeeTSEndTime   
	AND ETSDT.EmployeeID=T.EmployeeID  
	WHERE  ETSDT.EmployeeTSDateID IS NULL  AND T.EmployeeTSDate <= ISNULL(ETMEndDate,@SlotEndDate) 
	AND (@EmployeeID=0 OR T.EmployeeID=@EmployeeID) AND T.EmployeeTimeSlotMasterID=@EmployeeTimeSlotMasterID AND T.DayNumber IN (SELECT val FROM GetCSVTable(@SelectedDays))
	ORDER BY T.EmployeeID ASC, T.EmployeeTimeSlotMasterID ASC  

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