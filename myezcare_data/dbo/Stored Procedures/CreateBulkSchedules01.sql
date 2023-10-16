CREATE PROCEDURE [dbo].[CreateBulkSchedules01]
@ReferralID BIGINT,    
@NewEmployeeID BIGINT,    
@ScheduleID BIGINT,    
@EmployeeTimeSlotDetailIDs VARCHAR(MAX),
@Days VARCHAR(MAX),
@StartTimes VARCHAR(MAX),
@EndTimes VARCHAR(MAX),
@StartDate DATETIME,    
@EndDate DATETIME,    
@ScheduleStatusID BIGINT,    
@SameDateWithTimeSlot BIT,    
@loggedInId BIGINT,    
@SystemID VARCHAR(100)    
    
AS    
BEGIN    

IF(@Days IS NOT NULL)
BEGIN
	DECLARE @TempEmployeeTimeSlotDetailIDs NVARCHAR(MAX)      
	SET @TempEmployeeTimeSlotDetailIDs = ''

	SELECT @TempEmployeeTimeSlotDetailIDs=(@TempEmployeeTimeSlotDetailIDs+CONVERT(VARCHAR(10),EmployeeTimeSlotDetailID)+',') FROM EmployeeTimeSlotDetails      
	WHERE EmployeeTimeSlotDetailID IN (SELECT CONVERT(BIGINT,VAL) FROM GetCSVTable(@EmployeeTimeSlotDetailIDs))
	AND Day IN (Select VAL FROM GetCSVTable(@Days))

	SET @EmployeeTimeSlotDetailIDs = @TempEmployeeTimeSlotDetailIDs
END

    
    
DECLARE @StartDateOnly DATE=CONVERT(DATE, @StartDate);    
DECLARE @EndDateOnly DATE=CONVERT(DATE, @EndDate);    
    
EXEC GenerateReferralTimeSlotDates01 @StartDateOnly, @EndDateOnly, @ReferralID, @StartTimes, @EndTimes
EXEC GenerateEmployeeTimeSlotDates01 @StartDateOnly,@EndDateOnly, @NewEmployeeID, @StartTimes, @EndTimes
    
DECLARE @TempTable Table(    
    NewGUID VARCHAR(100),    
 ReferralID BIGINT,    
 StartDate DATETIME,    
 EndDate DATETIME,    
 ScheduleStatusID BIGINT,    
 CreatedBy BIGINT,    
 CreatedDate DATETIME,    
 UpdatedBy BIGINT,    
 UpdatedDate DATETIME,    
 SystemID VARCHAR(MAX),    
 IsDeleted BIT,    
 EmployeeID BIGINT,    
 EmployeeTSDateID BIGINT,    
 ReferralTSDateID BIGINT    
)    
    
DECLARE @PTOTable Table(    
  NewGUID VARCHAR(100)    
)    
    
     
 -- CHECK FOR EMPLOYEE BLOCK    
 IF EXISTS(    
  SELECT 1 FROM ReferralBlockedEmployees RBE     
  WHERE RBE.ReferralID=@ReferralID AND RBE.EmployeeID=@NewEmployeeID AND RBE.IsDeleted=0)    
  BEGIN    
   SELECT -1 RETURN;    
  END    
 -- CHECK FOR EMPLOYEE BLOCK    
    
    
 IF(@ScheduleID=0)    
 BEGIN    
    
   INSERT INTO @TempTable    
  SELECT NEWID(),@ReferralID,RTD.ReferralTSStartTime,RTD.ReferralTSEndTime,@ScheduleStatusID,@loggedInId,GETDATE(),1,GETDATE(),@SystemID,0,@NewEmployeeID,    
  ETD.EmployeeTSDateID,RTD.ReferralTSDateID    
  --ETD.*, RTD.*     
  FROM EmployeeTimeSlotDates ETD     
  --INNER JOIN EmployeeTimeSlotDetails ETSD ON ETSD.StartTime= CAST(ETD.EmployeeTSStartTime as time) AND DATEPART(dw,ETD.EmployeeTSDate)=ETSD.Day AND ETSD.IsDeleted=0    
  INNER JOIN EmployeeTimeSlotDetails ETSD ON ETSD.IsDeleted=0 AND ETSD.EmployeeTimeSlotDetailID= ETD.EmployeeTimeSlotDetailID    
    
  AND (    
      (@SameDateWithTimeSlot=0 AND ETD.EmployeeTSDate  BETWEEN @StartDate AND @EndDate) OR    
   (@SameDateWithTimeSlot=1    
   AND @StartDate  BETWEEN ETD.EmployeeTSStartTime AND ETD.EmployeeTSEndTime      
   AND @EndDate  BETWEEN ETD.EmployeeTSStartTime AND ETD.EmployeeTSEndTime )    
  )    
    
  AND ETD.EmployeeID=@NewEmployeeID    
  INNER JOIN (SELECT EmployeeTimeSlotDetailID=CONVERT(BIGINT,VAL) FROM GetCSVTable(@EmployeeTimeSlotDetailIDs)) T ON T.EmployeeTimeSlotDetailID=ETSD.EmployeeTimeSlotDetailID    
      
  INNER JOIN ReferralTimeSlotDates RTD ON RTD.ReferralID=@ReferralID     
  AND RTD.ReferralTSStartTime BETWEEN ETD.EmployeeTSStartTime AND ETD.EmployeeTSEndTime     
  AND RTD.ReferralTSEndTime BETWEEN ETD.EmployeeTSStartTime AND ETD.EmployeeTSEndTime    
    
  AND (    
      (@SameDateWithTimeSlot=0 AND RTD.ReferralTSDate  BETWEEN @StartDate AND @EndDate) OR    
   (@SameDateWithTimeSlot=1    
    AND RTD.ReferralTSStartTime BETWEEN @StartDate AND @EndDate    
       AND RTD.ReferralTSEndTime BETWEEN @StartDate AND @EndDate )    
  )    
    
      
  --INNER JOIN ReferralTimeSlotMaster RTSM ON RTSM.ReferralTimeSlotMasterID=RTD.ReferralTimeSlotMasterID    
  LEFT JOIN ScheduleMasters SM ON SM.IsDeleted=0 AND (SM.ReferralID=@ReferralID OR SM.EmployeeID=@NewEmployeeID) AND     
  -- (SM.EmployeeTSDateID=ETD.EmployeeTSDateID AND SM.ReferralTSDateID=RTD.ReferralTSDateID)    
      
   (RTD.ReferralTSStartTime BETWEEN SM.StartDate AND SM.EndDate OR RTD.ReferralTSEndTime BETWEEN SM.StartDate AND SM.EndDate)    
      
       
    
  WHERE --ETD.EmployeeTSDate  BETWEEN @StartDate AND @EndDate AND ETD.EmployeeID=@NewEmployeeID AND    
   1=1 AND SM.ScheduleID IS NULL    
  ORDER BY ETD.EmployeeTSStartTime ASC    
      
  -- EMPLOYEE PTO CHECK    
  INSERT INTO @PTOTable    
  SELECT T.NewGUID FROM @TempTable T    
  LEFT JOIN  EmployeeDayOffs EDO ON EDO.EmployeeID=T.EmployeeID AND EDO.DayOffStatus='Approved'    
  AND     
  (    
   (T.StartDate BETWEEN EDO.StartTime AND EDO.EndTime OR T.EndDate  BETWEEN EDO.StartTime AND EDO.EndTime)    
   OR    
   (EDO.StartTime BETWEEN T.StartDate  AND T.EndDate OR EDO.EndTime BETWEEN T.StartDate  AND T.EndDate)    
   )    
  WHERE EDO.IsDeleted=0     
  -- EMPLOYEE PTO CHECK    
       
    
    
    
   -- DELETE FROM ScheduleMasters  WHERE ScheduleID > 78090     
       
    
 IF((SELECT COUNT(*) FROM @TempTable)>0 AND (SELECT COUNT(*) FROM @TempTable)=(SELECT COUNT(*) FROM @PTOTable))    
 BEGIN    
 SELECT -2 RETURN;    
 END    
    
  INSERT INTO ScheduleMasters(ReferralID,StartDate,EndDate,ScheduleStatusID,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,SystemID,IsDeleted,EmployeeID,EmployeeTSDateID,ReferralTSDateID)      
  SELECT ReferralID,StartDate,EndDate,ScheduleStatusID,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,SystemID,IsDeleted,EmployeeID,    
  EmployeeTSDateID,ReferralTSDateID FROM @TempTable T WHERE T.NewGUID NOT IN (SELECT NewGUID FROM @PTOTable)    
      
    
  SELECT @@ROWCOUNT;
    
 END    
 ELSE     
 BEGIN    
      
  DECLARE @Old_EmployeeTSDateID BIGINT=0;    
  DECLARE @Old_ReferralTSDateID BIGINT=0;    
  SELECT @Old_EmployeeTSDateID=EmployeeTSDateID,@Old_ReferralTSDateID=ReferralTSDateID FROM ScheduleMasters WHERE ScheduleID=@ScheduleID    
    
      
       
    
  DECLARE @NEW_ReferralTSStartTime DATETIME=0;    
  DECLARE @NEW_ReferralTSEndTime DATETIME=0;    
  DECLARE @NEW_EmployeeTSDateID BIGINT=0;    
      
  DECLARE @NEW_ReferralTSDateID BIGINT=0;    
  DECLARE @ReferralTimeSlotDetailID BIGINT=0;    
    
    
  SELECT @NEW_ReferralTSDateID=ReferralTSDateID,@ReferralTimeSlotDetailID=ReferralTimeSlotDetailID FROM ReferralTimeSlotDates WHERE ReferralTSDateID=@Old_ReferralTSDateID    
    
    
  PRINT @Old_EmployeeTSDateID;    
  PRINT @Old_ReferralTSDateID;    
    
  PRINT @NEW_ReferralTSDateID;    
  PRINT @ReferralTimeSlotDetailID;    
      
  PRINT @NewEmployeeID;    
    
  -- EXEC CreateBulkSchedules @ReferralID = '3993', @NewEmployeeID = '8',@ScheduleID = '29600', @EmployeeTimeSlotDetailIDs = '94,104,114,124', @StartDate = '2018/04/10 13:00:00', @EndDate = '2018/04/10 14:00:00', @ScheduleStatusID = '2', @loggedInId = '1', @SystemID = '::1', @SameDateWithTimeSlot = 'True'  
  
  
    
  SELECT TOP 1 @NEW_ReferralTSStartTime=RTD.ReferralTSStartTime,@NEW_ReferralTSEndTime=RTD.ReferralTSEndTime,    
   @NEW_EmployeeTSDateID=ETD.EmployeeTSDateID,@NEW_ReferralTSDateID=RTD.ReferralTSDateID    
  FROM EmployeeTimeSlotDates ETD     
  INNER JOIN EmployeeTimeSlotDetails ETSD ON ETD.EmployeeID= @NewEmployeeID      
   AND ETSD.StartTime= CAST(ETD.EmployeeTSStartTime as time) AND DATEPART(dw,ETD.EmployeeTSDate)=ETSD.Day AND ETSD.IsDeleted=0    
    
   AND (    
      (@SameDateWithTimeSlot=0 AND ETD.EmployeeTSDate  BETWEEN @StartDate AND @EndDate) OR    
   (@SameDateWithTimeSlot=1    
   AND @StartDate  BETWEEN ETD.EmployeeTSStartTime AND ETD.EmployeeTSEndTime      
   AND @EndDate  BETWEEN ETD.EmployeeTSStartTime AND ETD.EmployeeTSEndTime )    
  )    
    
  INNER JOIN (SELECT EmployeeTimeSlotDetailID=CONVERT(BIGINT,VAL) FROM GetCSVTable(@EmployeeTimeSlotDetailIDs)) T ON T.EmployeeTimeSlotDetailID=ETSD.EmployeeTimeSlotDetailID    
  INNER JOIN ReferralTimeSlotDates RTD ON RTD.ReferralID=@ReferralID  AND RTD.ReferralTSDateID=@New_ReferralTSDateID     
      
  AND RTD.ReferralTSStartTime BETWEEN ETD.EmployeeTSStartTime AND ETD.EmployeeTSEndTime     
     AND RTD.ReferralTSEndTime BETWEEN ETD.EmployeeTSStartTime AND ETD.EmployeeTSEndTime     
    
  AND (    
      (@SameDateWithTimeSlot=0 AND RTD.ReferralTSDate  BETWEEN @StartDate AND @EndDate) OR    
   (@SameDateWithTimeSlot=1    
    AND (RTD.ReferralTSStartTime BETWEEN @StartDate AND @EndDate  AND RTD.ReferralTSEndTime BETWEEN @StartDate AND @EndDate)  
 OR  (@StartDate BETWEEN RTD.ReferralTSStartTime AND RTD.ReferralTSEndTime  AND @EndDate BETWEEN RTD.ReferralTSStartTime AND RTD.ReferralTSEndTime)  
  )      
  )    
    
    
      
      
      
    --AND RTD.ReferralTimeSlotDetailID=@ReferralTimeSlotDetailID    
  LEFT JOIN ScheduleMasters SM ON  SM.IsDeleted=0 AND SM.ScheduleID!=@ScheduleID AND (SM.ReferralID=@ReferralID OR SM.EmployeeID=@NewEmployeeID) AND     
   (RTD.ReferralTSStartTime BETWEEN SM.StartDate AND SM.EndDate OR RTD.ReferralTSEndTime BETWEEN SM.StartDate AND SM.EndDate)    
      
   --(SM.EmployeeTSDateID=ETD.EmployeeTSDateID AND SM.ReferralTSDateID=RTD.ReferralTSDateID)      
  WHERE SM.ScheduleID IS NULL    
  ORDER BY ETD.EmployeeTSStartTime ASC    
    
    
    
    
     
  PRINT @NEW_EmployeeTSDateID;    
  PRINT @NEW_ReferralTSDateID;    
    
  IF(@NEW_EmployeeTSDateID>0 AND @NEW_ReferralTSDateID>0)    
  BEGIN    
    
    
      
      -- CHECK FOR EMPLOYEE PTO    
  IF EXISTS(    
   SELECT TOP 1 1 FROM EmployeeDayOffs EDO     
   WHERE EDO.EmployeeID=@NewEmployeeID AND EDO.DayOffStatus='Approved'    
   AND EDO.EmployeeDayOffID IS NOT NULL AND EDO.IsDeleted=0 AND     
   ((@NEW_ReferralTSStartTime BETWEEN EDO.StartTime AND EDO.EndTime OR @NEW_ReferralTSEndTime BETWEEN EDO.StartTime AND EDO.EndTime)    
   OR    
   (EDO.StartTime  BETWEEN @NEW_ReferralTSStartTime  AND @NEW_ReferralTSEndTime OR EDO.EndTime BETWEEN @NEW_ReferralTSStartTime  AND @NEW_ReferralTSEndTime)    
   )    
   )    
   BEGIN    
    SELECT -2 RETURN;    
   END    
  -- CHECK FOR EMPLOYEE BLOCK    
    
    
    
    
    
    
    
    
    
    
    
  UPDATE ScheduleMasters SET StartDate=@NEW_ReferralTSStartTime, EndDate=@NEW_ReferralTSEndTime, ReferralID=@ReferralID, EmployeeID=@NewEmployeeID,    
  UpdatedBy=@loggedInId,UpdatedDate=GETUTCDATE(),    
  EmployeeTSDateID=@NEW_EmployeeTSDateID,ReferralTSDateID=@NEW_ReferralTSDateID     
  WHERE ScheduleID=@ScheduleID    
    
  END    
    
  SELECT @@ROWCOUNT;    
 END    
    
    
END    
    
    
-- EXEC CreateBulkSchedules @ReferralID = '1951', @NewEmployeeID = '18', @ScheduleID = '57837', @EmployeeTimeSlotDetailIDs = '1', @StartDate = '2018/03/19', @EndDate = '2018/03/19', @ScheduleStatusID = '2', @loggedInId = '1', @SystemID = '::1'    
    
-- EXEC CreateBulkSchedules @ReferralID = '1951', @EmployeeID = '18', @EmployeeTimeSlotDetailIDs = '1,6', @StartDate = '2018/03/05', @EndDate = '2018/03/25', @ScheduleStatusID = '1', @loggedInId = '1', @SystemID = '::1'    
    
-- SELECT TOP 5 * FROM ScheduleMasters ORDER BY ScheduleID DESC    
    
-- DELETE FROM ScheduleMasters WHERE ScheduleID > 57792    
-- SELECT * FROM Referrals WHERE ReferralID=1951    
--SELECT * FROM ReferralTimeSlotDates WHERE ReferralID=1951 ORDER BY ReferralTSStartTime ASC    
-- SELECT * FROM ScheduleMasters ORDER BY 1 DESC    
    
    
--DELETE FROM EmployeeDayOffs    
-- SELECT * FROM ScheduleMasters WHERE ScheduleID > 78090  ORDER BY 1 DESC    
-- DELETE FROM ScheduleMasters  WHERE ScheduleID > 78090     
    
-- DELETE FROM EmployeeTimeSlotDates WHERE EmployeeID=18     
-- SELECT * FROM EmployeeTimeSlotDates WHERE EmployeeID=18 ORDER BY EmployeeTSDate ASC
