--Exec AddEtsMaster @EmployeeTimeSlotMasterID=0,@EmployeeID=78,@StartDate='2017-12-12',@EndDate='2018-01-01',@loggedInUserId=1,@SystemID='::1'                    
CREATE PROCEDURE [dbo].[AddEtsMaster]                    
 @EmployeeTimeSlotMasterID BIGINT,                    
 @EmployeeID BIGINT,                    
 @StartDate DATE,                    
 @EndDate DATE=NULL,              
 @IsEndDateAvailable BIT,  
 @TodayDate DATE,  
 @SlotEndDate DATE,  
 @loggedInUserId BIGINT,                    
 @SystemID VARCHAR(100)                    
AS                              
BEGIN                              
DECLARE @TablePrimaryId bigint;  
DECLARE @EmployeeTimeSlotMasterIds VARCHAR(MAX);  

 DECLARE @Output TABLE (  
    ScheduleID bigint
  )  
                  
  BEGIN TRANSACTION trans                          
 BEGIN TRY            
             
 DECLARE @MaxDate date;              
 SET @MaxDate = '2099-12-31';                          
              
 IF(@EndDate='')              
   SET @EndDate=NULL;                     
      
      
----Code for check existing date range        
-- IF EXISTS(SELECT * FROM EmployeeTimeSlotMaster E WHERE ((@StartDate>=E.StartDate AND @StartDate<=COALESCE(E.EndDate,@MaxDate))          
-- OR (@EndDate>=E.StartDate AND @EndDate<=COALESCE(E.EndDate,@MaxDate))) AND E.EmployeeID in (@EmployeeID)         
-- AND E.EmployeeTimeSlotMasterID!=@EmployeeTimeSlotMasterID AND E.IsDeleted=0)                 
                  
-- BEGIN                  
--SELECT -2 AS TransactionResultId                   
-- IF @@TRANCOUNT > 0                          
--     BEGIN                           
--      COMMIT TRANSACTION trans                           
--     END                    
--RETURN;                    
--END                  
                              
 IF(@EmployeeTimeSlotMasterID=0)                              
 BEGIN                              
     INSERT INTO EmployeeTimeSlotMaster                              
   (EmployeeID,StartDate,EndDate,IsEndDateAvailable,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,SystemID)                              
   VALUES                              
   (@EmployeeID,@StartDate,@EndDate,@IsEndDateAvailable,@loggedInUserId,GETUTCDATE(),@loggedInUserId,GETUTCDATE(),@SystemID);                               
                                 
 SET @TablePrimaryId = @@IDENTITY;                       
 END                              
 ELSE                              
 BEGIN                           
             
   DECLARE @LastEndDate DATE;            
   SELECT  @LastEndDate=EndDate FROM EmployeeTimeSlotMaster WHERE EmployeeTimeSlotMasterID=@EmployeeTimeSlotMasterID;                   
             
               
   UPDATE EmployeeTimeSlotMaster                               
   SET                                    
      EmployeeID=@EmployeeID,                              
      StartDate=@StartDate,                            
      EndDate=@EndDate,                       
      IsEndDateAvailable=@IsEndDateAvailable,              
      UpdatedBy=@loggedInUserId,                              
      UpdatedDate=GETUTCDATE(),                              
      SystemID=@SystemID                              
   WHERE EmployeeTimeSlotMasterID=@EmployeeTimeSlotMasterID;                              
            
            
   --IF(@LastEndDate IS NOT NULL AND @EndDate<@LastEndDate)            
   IF(@LastEndDate IS NULL OR @EndDate<@LastEndDate)            
 BEGIN            
  DECLARE @TempTable TABLE(EmployeeTSDateID BIGINT)            
  INSERT INTO @TempTable            
  SELECT EmployeeTSDateID FROM EmployeeTimeSlotDates ETD WHERE ETD.EmployeeTimeSlotMasterID=@EmployeeTimeSlotMasterID AND ETD.EmployeeTSDate > @EndDate            
        
        
  INSERT INTO @Output       
  SELECT ScheduleID FROM ScheduleMasters WHERE EmployeeTSDateID IN (SELECT EmployeeTSDateID FROM @TempTable) 

  DELETE FROM ScheduleMasters WHERE EmployeeTSDateID IN (SELECT EmployeeTSDateID FROM @TempTable)            
  DELETE FROM EmployeeTimeSlotDates  WHERE EmployeeTSDateID IN (SELECT EmployeeTSDateID FROM @TempTable)  
 END  
 --  ELSE  
 --BEGIN  
  --Start New Code  
  SELECT @EmployeeTimeSlotMasterIds =   
    STUFF((SELECT ', ' + CONVERT(VARCHAR(50),EmployeeTimeSlotMasterID)  
         FROM EmployeeTimeSlotMaster  
         WHERE EmployeeID=@EmployeeID AND ((StartDate>=@StartDate AND StartDate<=IsNull(@EndDate,@MaxDate)) OR  (EndDate>=@StartDate AND EndDate<=IsNull(@EndDate,@MaxDate)) OR (@StartDate>=StartDate AND @StartDate<=IsNull(EndDate,@MaxDate)) OR (@EndDate>
=StartDate AND @EndDate<=IsNull(EndDate,@MaxDate)))  
        FOR XML PATH('')), 1, 2, '')  
    
  DECLARE @ConflictedSlotDetail TABLE (RowNo int,EmployeeTimeSlotMasterID BIGINT,EmployeeTimeSlotDetailID BIGINT,Day INT,StartTime TIME,EndTime TIME)  
  DECLARE @ConflictedSlots TABLE (EmployeeTimeSlotMasterID BIGINT,EmployeeTimeSlotDetailID BIGINT,Day INT,StartTime TIME,EndTime TIME)  
  
  INSERT INTO @ConflictedSlotDetail  
  SELECT ROW_NUMBER() over (order by EmployeeTimeSlotMasterID,Day,StartTime,EndTime) as RowNo,EmployeeTimeSlotMasterID,EmployeeTimeSlotDetailID,Day,StartTime,EndTime  
  FROM EmployeeTimeSlotDetails  
  WHERE EmployeeTimeSlotMasterID IN (SELECT val FROM GetCSVTable(@EmployeeTimeSlotMasterIds)) AND IsDeleted=0  
  
  INSERT INTO @ConflictedSlots  
   SELECT t1.EmployeeTimeSlotMasterID,t1.EmployeeTimeSlotDetailID,t1.Day,t1.StartTime,t1.EndTime FROM @ConflictedSlotDetail t1  
   INNER JOIN @ConflictedSlotDetail t2  
   ON(t1.RowNo <> t2.RowNo AND  
   ((t2.StartTime > t1.StartTime AND t2.StartTime< t1.EndTime)      
   OR (t2.EndTime > t1.StartTime AND t2.EndTime < t1.EndTime)      
   OR (t1.StartTime > t2.StartTime AND t1.StartTime< t2.EndTime)   
   OR (t1.EndTime > t2.StartTime AND t1.EndTime< t2.EndTime)  
   OR (t1.StartTime = t2.StartTime AND t1.EndTime = t2.EndTime)) AND t1.Day=t2.Day)  
  
  --DELETE conflicting dates  
  DELETE FROM EmployeeTimeSlotDates WHERE EmployeeTimeSlotDetailID IN (SELECT EmployeeTimeSlotDetailID FROM @ConflictedSlots)   
  AND (EmployeeTSDate>=CASE WHEN @StartDate < @TodayDate THEN @TodayDate ELSE @StartDate END AND EmployeeTSDate<=ISNULL(@EndDate,@SlotEndDate))  
  
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
  AND (@EmployeeID=0 OR T.EmployeeID=@EmployeeID) AND T.EmployeeTimeSlotMasterID=@EmployeeTimeSlotMasterID --AND T.DayNumber IN (SELECT val FROM GetCSVTable(@SelectedDays))  
  ORDER BY T.EmployeeID ASC, T.EmployeeTimeSlotMasterID ASC    
  
  
  --End New Code  
 --END  
            
 END                              
                          
 SELECT 1 AS TransactionResultId,@TablePrimaryId AS TablePrimaryId;                        
      IF @@TRANCOUNT > 0                          
     BEGIN                           
      COMMIT TRANSACTION trans  
      DECLARE @CurScheduleID bigint;                         
      DECLARE eventCursor CURSOR FORWARD_ONLY FOR
            SELECT ScheduleID FROM @Output;
        OPEN eventCursor;
        FETCH NEXT FROM eventCursor INTO @CurScheduleID;
        WHILE @@FETCH_STATUS = 0 BEGIN
            EXEC [dbo].[ScheduleEventBroadcast] 'DeleteSchedule', @CurScheduleID,'',''
            FETCH NEXT FROM eventCursor INTO @CurScheduleID;
        END;
        CLOSE eventCursor;
        DEALLOCATE eventCursor;
     END                         END TRY                          
   BEGIN CATCH                          
    SELECT -1 AS TransactionResultId,ERROR_MESSAGE() AS ErrorMessage;                          
    IF @@TRANCOUNT > 0                          
     BEGIN                           
      ROLLBACK TRANSACTION trans                           
     END                          
  END CATCH                         
                          
END  