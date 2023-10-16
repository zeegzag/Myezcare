CREATE PROCEDURE [dbo].[SaveSchedule]          
@ScheduleID BIGINT,          
@ReferralID BIGINT,          
@EmployeeID BIGINT,          
-- @EmployeeTSDateID BIGINT,          
-- @ReferralTSDateID BIGINT,          
@DayView BIT,          
@StartDateTime DATETIME,          
@EndDateTime DATETIME,          
@ScheduleStatusID BIGINT,          
@loggedInId BIGINT,          
@SystemID VARCHAR(100)          
AS          
BEGIN          
          
 DECLARE @PayorID BIGINT=0;        
 IF(@ScheduleID=0)        
 BEGIN        
 SELECT @PayorID=RPM.PayorID FROM ReferralPayorMappings RPM        
 WHERE RPM.ReferralID=@ReferralID AND  RPM.Precedence=1 AND RPM.IsDeleted=0        
 AND  CONVERT(DATE,@StartDateTime) BETWEEN RPM.PayorEffectiveDate AND RPM.PayorEffectiveEndDate    
        
        
 IF(@PayorID=0 OR @PayorID IS NULL)              
  SET @PayorID=NULL        
        
 END        
        
        
        
          
 DECLARE @StartDate DATE = CONVERT(DATE, @StartDateTime)          
 DECLARE @EndDate DATE = CONVERT(DATE, @EndDateTime)          
          
          
 -- CHECK FOR EMPLOYEE BLOCK          
 IF EXISTS(          
  SELECT 1 FROM ReferralBlockedEmployees RBE           
  WHERE RBE.ReferralID=@ReferralID AND RBE.EmployeeID=@EmployeeID AND RBE.IsDeleted=0)          
  BEGIN          
   SELECT -1 RETURN;          
  END          
 -- CHECK FOR EMPLOYEE BLOCK          
          
  DECLARE @TempRTD TABLE (          
  ReferralTSDateID BIGINT,          
  ReferralID BIGINT,          
  ReferralTimeSlotMasterID BIGINT,          
  ReferralTSDate DATE,          
  ReferralTSStartTime DATETIME,          
  ReferralTSEndTime DATETIME,          
  UsedInScheduling BIT,          
  Notes  VARCHAR(MAX),          
  DayNumber INT,          
  ReferralTimeSlotDetailID BIGINT,        
  OnHold BIT,        
  ReferralOnHoldDetailID BIGINT,  
  IsDenied BIT  
 )          
          
          
 DECLARE @TempSM TABLE (          
  ReferralTSStartTime DATETIME,          
  ReferralTSEndTime DATETIME,          
  EmployeeTSDateID BIGINT,          
  ReferralTSDateID BIGINT,        
  EmployeeID BIGINT        
 )          
          
          
          
          
          
 IF(@ScheduleID=0)          
 BEGIN          
           
 IF(@DayView=0)           
 BEGIN          
  INSERT INTO @TempRTD          
  SELECT RTD.* FROM ReferralTimeSlotDates RTD          
  WHERE  RTD.ReferralID=@ReferralID          
  AND RTD.ReferralTSDate BETWEEN @StartDate AND @EndDate           
          
  IF NOT EXISTS (SELECT * FROM @TempRTD) BEGIN SELECT -3 RETURN END;          
 END          
 ELSE          
 BEGIN          
           
   INSERT INTO @TempRTD          
   SELECT RTD.* FROM ReferralTimeSlotDates RTD          
   LEFT JOIN ScheduleMasters SM ON  SM.IsDeleted=0 AND SM.ScheduleID!=@ScheduleID           
   AND SM.ReferralTSDateID = RTD.ReferralTSDateID          
   AND ( (@DayView=0) OR (@DayView=1 AND @StartDateTime BETWEEN SM.StartDate AND SM.EndDate OR @EndDateTime BETWEEN SM.StartDate AND SM.EndDate) )          
   WHERE SM.ScheduleID IS NULL AND RTD.ReferralID=@ReferralID          
   AND RTD.ReferralTSDate BETWEEN @StartDate AND @EndDate           
   AND ( (@DayView=0) OR (@DayView=1 AND @StartDateTime BETWEEN RTD.ReferralTSStartTime AND RTD.ReferralTSEndTime) )          
          
   IF NOT EXISTS (SELECT * FROM @TempRTD) BEGIN SELECT -3 RETURN END;          
 END          
            
           
          
 IF(@DayView=0)           
 BEGIN          
  INSERT INTO @TempSM          
  SELECT RTD.ReferralTSStartTime,RTD.ReferralTSEndTime,ETD.EmployeeTSDateID,RTD.ReferralTSDateID, ETD.EmployeeID         
  FROM @TempRTD RTD          
     INNER JOIN EmployeeTimeSlotDates ETD ON ETD.EmployeeTSDate=RTD.ReferralTSDate AND  ETD.EmployeeID=@EmployeeID AND RTD.ReferralTSStartTime BETWEEN ETD.EmployeeTSStartTime           
     AND  ETD.EmployeeTSEndTime AND RTD.ReferralTSEndTime BETWEEN ETD.EmployeeTSStartTime AND ETD.EmployeeTSEndTime        
  LEFT JOIN ScheduleMasters SM ON  SM.IsDeleted=0 AND SM.ScheduleID!=@ScheduleID AND ETD.EmployeeID=SM.EmployeeID           
  AND ( (RTD.ReferralTSStartTime BETWEEN SM.StartDate AND SM.EndDate OR RTD.ReferralTSEndTime BETWEEN SM.StartDate AND SM.EndDate)  OR         
       (SM.StartDate BETWEEN RTD.ReferralTSStartTime AND RTD.ReferralTSEndTime OR SM.EndDate BETWEEN RTD.ReferralTSStartTime AND RTD.ReferralTSEndTime )        
     )        
  WHERE SM.ScheduleID IS NULL        
          
  IF NOT EXISTS (SELECT * FROM @TempSM) BEGIN SELECT -4 RETURN END;          
 END          
        
        
        
 ELSE          
 BEGIN          
           
  INSERT INTO @TempSM          
  SELECT RTD.ReferralTSStartTime,RTD.ReferralTSEndTime,ETD.EmployeeTSDateID,RTD.ReferralTSDateID, ETD.EmployeeID FROM @TempRTD RTD          
     INNER JOIN EmployeeTimeSlotDates ETD ON ETD.EmployeeTSDate=RTD.ReferralTSDate AND  ETD.EmployeeID=@EmployeeID AND RTD.ReferralTSStartTime BETWEEN ETD.EmployeeTSStartTime           
     AND  ETD.EmployeeTSEndTime AND RTD.ReferralTSEndTime BETWEEN ETD.EmployeeTSStartTime AND ETD.EmployeeTSEndTime          
     LEFT JOIN ScheduleMasters SM ON  SM.IsDeleted=0 AND SM.ScheduleID!=@ScheduleID AND ETD.EmployeeID=SM.EmployeeID           
  AND ( (RTD.ReferralTSStartTime BETWEEN SM.StartDate AND SM.EndDate OR RTD.ReferralTSEndTime BETWEEN SM.StartDate AND SM.EndDate)  OR         
       (SM.StartDate BETWEEN RTD.ReferralTSStartTime AND RTD.ReferralTSEndTime OR SM.EndDate BETWEEN RTD.ReferralTSStartTime AND RTD.ReferralTSEndTime )        
     )        
  WHERE SM.ScheduleID IS NULL        
        
   IF NOT EXISTS (SELECT * FROM @TempSM) BEGIN SELECT -4 RETURN END;          
 END          
            
          
          
        
-- EXEC SaveSchedule @ScheduleID = '0', @ReferralID = '1953', @EmployeeID = '18', @StartDateTime = '2018/04/05 09:00:00', @EndDateTime = '2018/04/05 09:00:00', @ScheduleStatusID = '2', @DayView = 'True', @loggedInId = '1', @SystemID = '::1'        
        
        
        
        
  -- CHECK FOR EMPLOYEE PTO        
  IF EXISTS(        
    SELECT TOP 1 1 FROM @TempSM T        
    INNER JOIN EmployeeDayOffs EDO ON EDO.EmployeeID=T.EmployeeID AND T.EmployeeID=@EmployeeID AND EDO.EmployeeID=@EmployeeID AND EDO.DayOffStatus='Approved'        
    WHERE EDO.EmployeeDayOffID IS NOT NULL AND  EDO.IsDeleted=0 AND         
    (        
    (T.ReferralTSStartTime BETWEEN EDO.StartTime AND EDO.EndTime OR T.ReferralTSEndTime BETWEEN EDO.StartTime AND EDO.EndTime)        
    OR        
    (EDO.StartTime BETWEEN T.ReferralTSStartTime AND T.ReferralTSEndTime OR EDO.EndTime BETWEEN T.ReferralTSStartTime AND T.ReferralTSEndTime)        
    )        
   )        
   BEGIN        
    SELECT -2 RETURN;        
   END        
-- CHECK FOR EMPLOYEE BLOCK        
        
   
 DECLARE @Output TABLE (  
    ScheduleID bigint 
  )       
          
 INSERT INTO ScheduleMasters    (ReferralID,StartDate,EndDate,ScheduleStatusID,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,SystemID,IsDeleted,EmployeeID,EmployeeTSDateID,ReferralTSDateID,PayorID)          
           OUTPUT inserted.ScheduleID INTO @Output
 SELECT TOP 1 @ReferralID,RTD.ReferralTSStartTime,RTD.ReferralTSEndTime,@ScheduleStatusID,@loggedInId,GETUTCDATE(),1,GETUTCDATE(),@SystemID,0,          
 @EmployeeID,RTD.EmployeeTSDateID,RTD.ReferralTSDateID,@PayorID          
 FROM @TempSM RTD          
 LEFT JOIN ScheduleMasters SM ON SM.IsDeleted=0           
 AND (SM.ReferralID=@ReferralID OR SM.EmployeeID=@EmployeeID) AND           
 ( (RTD.ReferralTSStartTime BETWEEN SM.StartDate AND SM.EndDate OR RTD.ReferralTSEndTime BETWEEN SM.StartDate AND SM.EndDate)  OR         
   (SM.StartDate BETWEEN RTD.ReferralTSStartTime AND RTD.ReferralTSEndTime OR SM.EndDate BETWEEN RTD.ReferralTSStartTime AND RTD.ReferralTSEndTime )        
  )        
 WHERE SM.ScheduleID IS NULL          
          
 SELECT @@ROWCOUNT;   
 
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
          
          
          
          
          
 ELSE          
          
          
          
 BEGIN          
            
            
          
            
          
  INSERT INTO @TempRTD          
  SELECT RTD.* FROM ReferralTimeSlotDates RTD          
  LEFT JOIN ScheduleMasters SM ON  SM.IsDeleted=0 AND SM.ScheduleID!=@ScheduleID AND SM.EmployeeID=@EmployeeID          
  --AND SM.ReferralTSDateID = RTD.ReferralTSDateID          
  AND (@StartDateTime BETWEEN SM.StartDate AND SM.EndDate  OR @EndDateTime BETWEEN SM.StartDate AND SM.EndDate)           
  WHERE   RTD.ReferralID=@ReferralID          
  AND RTD.ReferralTSDate BETWEEN @StartDate AND @EndDate           
  AND (@StartDateTime BETWEEN RTD.ReferralTSStartTime AND RTD.ReferralTSEndTime AND @EndDateTime BETWEEN RTD.ReferralTSStartTime AND RTD.ReferralTSEndTime)          
               
            
  IF NOT EXISTS (SELECT * FROM @TempRTD) BEGIN SELECT -3 RETURN END;          
          
          
          
  INSERT INTO @TempSM          
  SELECT RTD.ReferralTSStartTime,RTD.ReferralTSEndTime,ETD.EmployeeTSDateID,RTD.ReferralTSDateID , ETD.EmployeeID          
  FROM @TempRTD RTD          
     INNER JOIN EmployeeTimeSlotDates ETD ON ETD.EmployeeTSDate=RTD.ReferralTSDate AND  ETD.EmployeeID=@EmployeeID           
  AND RTD.ReferralTSStartTime BETWEEN ETD.EmployeeTSStartTime AND  ETD.EmployeeTSEndTime           
  AND RTD.ReferralTSEndTime BETWEEN ETD.EmployeeTSStartTime AND ETD.EmployeeTSEndTime          
  LEFT JOIN ScheduleMasters SM ON  SM.IsDeleted=0 AND SM.ScheduleID!=@ScheduleID AND SM.EmployeeID=@EmployeeID          
  --AND SM.EmployeeTSDateID = ETD.EmployeeTSDateID          
  AND        
  (        
    ( @StartDateTime BETWEEN SM.StartDate AND SM.EndDate OR @EndDateTime BETWEEN SM.StartDate AND SM.EndDate )          
 OR         
  ( SM.StartDate BETWEEN @StartDateTime AND @EndDateTime OR SM.EndDate BETWEEN @StartDateTime AND @EndDateTime  )          
  )        
  WHERE SM.ScheduleID IS NULL          
             
   IF NOT EXISTS (SELECT * FROM @TempSM) BEGIN SELECT -4 RETURN END;          
          
          
          
  DECLARE @EmployeeTSDateID BIGINT;          
  DECLARE @ReferralTSDateID BIGINT;          
          
  SELECT TOP 1 @EmployeeTSDateID=EmployeeTSDateID,@ReferralTSDateID=ReferralTSDateID FROM @TempSM          
          
  IF(@EmployeeTSDateID>0 AND @ReferralTSDateID>0)         
  BEGIN        
          
  -- CHECK FOR EMPLOYEE PTO        
  IF EXISTS(        
   SELECT TOP 1 1 FROM @TempSM T        
   INNER JOIN EmployeeDayOffs EDO ON EDO.EmployeeID=T.EmployeeID AND T.EmployeeID=@EmployeeID AND EDO.EmployeeID=@EmployeeID AND EDO.DayOffStatus='Approved'        
   WHERE EDO.EmployeeDayOffID IS NOT NULL AND EDO.IsDeleted=0 AND         
   ((@StartDateTime BETWEEN EDO.StartTime AND EDO.EndTime OR @ENDDateTime BETWEEN EDO.StartTime AND EDO.EndTime)        
   OR        
   (EDO.StartTime  BETWEEN @StartDateTime  AND @ENDDateTime OR EDO.EndTime BETWEEN @StartDateTime  AND @ENDDateTime)        
   )        
   )        
   BEGIN        
    SELECT -2 RETURN;        
   END        
  -- CHECK FOR EMPLOYEE BLOCK        
          
           
        
  UPDATE ScheduleMasters SET StartDate=@StartDateTime, EndDate=@ENDDateTime, ReferralID=@ReferralID, EmployeeID=@EmployeeID, UpdatedBy=@loggedInId,UpdatedDate=GETUTCDATE(),          
  EmployeeTSDateID=@EmployeeTSDateID,ReferralTSDateID=@ReferralTSDateID           
  WHERE ScheduleID=@ScheduleID          
        
        EXEC [dbo].[ScheduleEventBroadcast] 'EditSchedule', @ScheduleID,'',''
  END        
           
  SELECT @@ROWCOUNT;          
           
          
          
          
          
          
          
 END          
          
          
          
          
END