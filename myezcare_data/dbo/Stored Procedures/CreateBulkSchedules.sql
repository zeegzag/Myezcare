-- EXEC CreateBulkSchedules 1982,18,'1,6 ', '2018/03/05', '2018/03/18',2,1,'1'
CREATE PROCEDURE [dbo].[CreateBulkSchedules]         
@PayorID BIGINT=0,
@ReferralID BIGINT,
@NewEmployeeID BIGINT,
@ScheduleID BIGINT,
@EmployeeTimeSlotDetailIDs VARCHAR(MAX),
@StartDate DATETIME,
@EndDate DATETIME,
@ScheduleStatusID BIGINT,
@SameDateWithTimeSlot BIT,
@loggedInId BIGINT,
@SystemID VARCHAR(100),              
@ReferralTimeSlotDetailIDs NVARCHAR(MAX),              
@IsRescheduleAction BIT,    
@CareTypeId bigint =0              
AS
BEGIN
       
IF(@PayorID=0 OR @PayorID IS NULL)              
 SET @PayorID=NULL        
   
DECLARE @StartDateOnly DATE=CONVERT(DATE, @StartDate);
DECLARE @EndDateOnly DATE=CONVERT(DATE, @EndDate);

EXEC GenerateReferralTimeSlotDates @StartDateOnly, @EndDateOnly, @ReferralID
EXEC GenerateEmployeeTimeSlotDates @StartDateOnly,@EndDateOnly, @NewEmployeeID

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
   1=1 AND                
   ( (@IsRescheduleAction=0 AND SM.ScheduleID IS NULL)  OR (@IsRescheduleAction=1 )   )              
   AND (  LEN(@ReferralTimeSlotDetailIDs)=0 OR RTD.ReferralTimeSlotDetailID IN (SELECT CONVERT(BIGINT,VAL) FROM GetCSVTable(@ReferralTimeSlotDetailIDs))  )              
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
 IF(@IsRescheduleAction=0)              
 BEGIN              
   INSERT INTO ScheduleMasters  (ReferralID,StartDate,EndDate,ScheduleStatusID,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,SystemID,IsDeleted,EmployeeID,EmployeeTSDateID,ReferralTSDateID,PayorID,CareTypeId)  
  SELECT ReferralID,StartDate,EndDate,ScheduleStatusID,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,SystemID,IsDeleted,EmployeeID,
  EmployeeTSDateID,ReferralTSDateID,@PayorID,@CareTypeId FROM @TempTable T WHERE T.NewGUID NOT IN (SELECT NewGUID FROM @PTOTable)
  SELECT @@ROWCOUNT;              
 END              
 ELSE              
 BEGIN              
  --               
  DECLARE @I INT=0;              
  UPDATE SM SET               
  SM.StartDate=T.StartDate, SM.EndDate=T.EndDate, SM.UpdatedBy=T.UpdatedBy,SM.UpdatedDate=T.UpdatedDate,              
  SM.EmployeeID=T.EmployeeID, SM.EmployeeTSDateID=T.EmployeeTSDateID,SM.ReferralTSDateID=T.ReferralTSDateID,        
  SM.PayorID=@PayorID,SM.CareTypeId=@CareTypeId    
  FROM ScheduleMasters SM              
  INNER JOIN @TempTable T ON SM.ReferralTSDateID=T.ReferralTSDateID AND SM.StartDate=T.StartDate AND SM.EndDate=T.EndDate AND SM.IsDeleted=0              
  WHERE T.NewGUID NOT IN (SELECT NewGUID FROM @PTOTable)
  --SELECT * FROM ScheduleMasters              
  --SELECT T.ReferralID,T.StartDate,T.EndDate,T.ScheduleStatusID,T.CreatedBy,T.CreatedDate,T.UpdatedBy,T.UpdatedDate,T.SystemID,T.IsDeleted,T.EmployeeID,
  --T.EmployeeTSDateID,T.ReferralTSDateID FROM @TempTable T               
  --INNER JOIN ScheduleMasters SM ON SM.ReferralTSDateID=T.ReferralTSDateID AND SM.StartDate=T.StartDate AND SM.EndDate=T.EndDate              
  --WHERE T.NewGUID NOT IN (SELECT NewGUID FROM @PTOTable)
   SET  @I=  @I + @@ROWCOUNT;              
  INSERT INTO ScheduleMasters(ReferralID,StartDate,EndDate,ScheduleStatusID,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,SystemID,IsDeleted,EmployeeID,EmployeeTSDateID,ReferralTSDateID,PayorID,CareTypeId)  
  SELECT T.ReferralID,T.StartDate,T.EndDate,T.ScheduleStatusID,T.CreatedBy,T.CreatedDate,T.UpdatedBy,T.UpdatedDate,T.SystemID,T.IsDeleted,T.EmployeeID,
  T.EmployeeTSDateID,T.ReferralTSDateID,@PayorID,@CareTypeId FROM @TempTable T               
  LEFT JOIN ScheduleMasters SM ON SM.ReferralTSDateID=T.ReferralTSDateID AND SM.StartDate=T.StartDate AND SM.EndDate=T.EndDate              
  WHERE SM. ScheduleID IS NULL AND T.NewGUID NOT IN (SELECT NewGUID FROM @PTOTable)
  SET  @I=  @I + @@ROWCOUNT;              
  SELECT @I;            
 END               
 --SELECT @@ROWCOUNT;              
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
  EmployeeTSDateID=@NEW_EmployeeTSDateID,ReferralTSDateID=@NEW_ReferralTSDateID,        
  PayorID=@PayorID,CareTypeId=@CareTypeId               
  WHERE ScheduleID=@ScheduleID
  END
  SELECT @@ROWCOUNT;
 END
END
