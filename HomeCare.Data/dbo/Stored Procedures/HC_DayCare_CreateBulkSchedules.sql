  
-- EXEC CreateBulkSchedules 1982,18,'1,6 ', '2018/03/05', '2018/03/18',2,1,'1'              
CREATE PROCEDURE [dbo].[HC_DayCare_CreateBulkSchedules]     
@PayorID BIGINT=0,              
@ReferralID BIGINT,              
@FacilityID BIGINT,              
@ScheduleID BIGINT,              
@StartDate DATETIME,              
@EndDate DATETIME,              
@ScheduleStatusID BIGINT,              
@SameDateWithTimeSlot BIT,              
@loggedInId BIGINT,              
@SystemID VARCHAR(100),          
@ReferralTimeSlotDetailIDs NVARCHAR(MAX),          
@IsRescheduleAction BIT,  
@ReferralBillingAuthorizationID BIGINT = 0                
             
AS              
BEGIN          
  
DECLARE @EmployeeID BIGINT =0;  
SELECT @EmployeeID=R.Assignee From [dbo].[Referrals] R WHERE R.ReferralID = @ReferralID
  
DECLARE @CareTypeId BIGINT =0;  
SELECT @CareTypeId=CareType From ReferralBillingAuthorizations WHERE ReferralBillingAuthorizationID=@ReferralBillingAuthorizationID  
    
    
IF(@PayorID=0 OR @PayorID IS NULL)          
 SET @PayorID=NULL    
              
DECLARE @Output TABLE (  
    ScheduleID bigint,  
    StartDate datetime  
  )  

  DECLARE @CurScheduleID bigint;   

DECLARE @StartDateOnly DATE=CONVERT(DATE, @StartDate);              
DECLARE @EndDateOnly DATE=CONVERT(DATE, @EndDate);              
              
EXEC GenerateReferralTimeSlotDates @StartDateOnly, @EndDateOnly, @ReferralID              
    
              
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
 FacilityID BIGINT,              
 ReferralTSDateID BIGINT
)              
              
                
 IF(@ScheduleID=0)              
 BEGIN              
              
  INSERT INTO @TempTable              
  SELECT NEWID(),@ReferralID,RTD.ReferralTSStartTime,RTD.ReferralTSEndTime,@ScheduleStatusID,@loggedInId,GETDATE(),1,GETDATE(),@SystemID,0,@FacilityID,              
  RTD.ReferralTSDateID      
  FROM ReferralTimeSlotDates RTD    
  INNER JOIN ReferralTimeSlotDetails RTDS ON RTDS.ReferralTimeSlotDetailID=RTD.ReferralTimeSlotDetailID    
  AND RTD.ReferralID=@ReferralID               
  AND (              
   (@SameDateWithTimeSlot=0 AND RTD.ReferralTSDate  BETWEEN @StartDate AND @EndDate) OR              
   (@SameDateWithTimeSlot=1              
    AND RTD.ReferralTSStartTime BETWEEN @StartDate AND @EndDate              
       AND RTD.ReferralTSEndTime BETWEEN @StartDate AND @EndDate )              
  )              
      
  LEFT JOIN ScheduleMasters SM ON ISNULL(SM.OnHold, 0) = 0 AND SM.IsDeleted=0 AND (SM.ReferralID=@ReferralID) AND               
  (RTD.ReferralTSStartTime BETWEEN SM.StartDate AND SM.EndDate OR RTD.ReferralTSEndTime BETWEEN SM.StartDate AND SM.EndDate)              
  WHERE              
   1=1 AND            
   ( (@IsRescheduleAction=0 AND SM.ScheduleID IS NULL)  OR (@IsRescheduleAction=1 )   )          
   AND (  LEN(@ReferralTimeSlotDetailIDs)=0 OR RTD.ReferralTimeSlotDetailID IN (SELECT CONVERT(BIGINT,VAL) FROM GetCSVTable(@ReferralTimeSlotDetailIDs))  )          
  ORDER BY RTD.ReferralTSStartTime ASC              
                
       
 DECLARE @I INT=0;                     
              
 IF(@IsRescheduleAction=0)          
 BEGIN          
   
   INSERT INTO ScheduleMasters        
   (ReferralID,StartDate,EndDate,ScheduleStatusID,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,SystemID,IsDeleted,EmployeeID,FacilityID,ReferralTSDateID,PayorID,CareTypeId,ReferralBillingAuthorizationID)  
   OUTPUT inserted.ScheduleID, inserted.StartDate INTO @Output              
   SELECT ReferralID,StartDate,EndDate,ScheduleStatusID,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,SystemID,IsDeleted,@EmployeeID,@FacilityID,              
   ReferralTSDateID,@PayorID,@CareTypeId,@ReferralBillingAuthorizationID FROM @TempTable T               

   SET  @I=  @I + @@ROWCOUNT;  
              
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
    SELECT @I;              
 END          
 ELSE          
 BEGIN          
          
          
  --                  
  UPDATE SM SET           
  SM.StartDate=T.StartDate, SM.EndDate=T.EndDate, SM.UpdatedBy=T.UpdatedBy,SM.UpdatedDate=T.UpdatedDate,          
  SM.EmployeeID=@EmployeeID,SM.FacilityID=T.FacilityID,SM.ReferralTSDateID=T.ReferralTSDateID,    
  SM.PayorID=@PayorID  ,SM.CareTypeId=@CareTypeId,SM.ReferralBillingAuthorizationID=@ReferralBillingAuthorizationID   
  OUTPUT deleted.ScheduleID,inserted.StartDate INTO @Output    
  FROM ScheduleMasters SM          
  INNER JOIN @TempTable T ON SM.ReferralTSDateID=T.ReferralTSDateID AND SM.StartDate=T.StartDate AND SM.EndDate=T.EndDate AND SM.IsDeleted=0          
               
          
          
   SET  @I=  @I + @@ROWCOUNT;  
   
   DECLARE eventCursor CURSOR FORWARD_ONLY FOR
            SELECT ScheduleID FROM @Output;
        OPEN eventCursor;
        FETCH NEXT FROM eventCursor INTO @CurScheduleID;
        WHILE @@FETCH_STATUS = 0 BEGIN
            EXEC [dbo].[ScheduleEventBroadcast] 'EditSchedule', @CurScheduleID,'',''
            FETCH NEXT FROM eventCursor INTO @CurScheduleID;
        END;
        CLOSE eventCursor;
        DEALLOCATE eventCursor;           
         DELETE FROM @Output    
                 
          
  INSERT INTO ScheduleMasters(ReferralID,StartDate,EndDate,ScheduleStatusID,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,SystemID,IsDeleted,EmployeeID,FacilityID,ReferralTSDateID,PayorID,CareTypeId,ReferralBillingAuthorizationID)    
  OUTPUT inserted.ScheduleID,inserted.StartDate INTO @Output 
  SELECT T.ReferralID,T.StartDate,T.EndDate,T.ScheduleStatusID,T.CreatedBy,T  
    
.CreatedDate,T.UpdatedBy,T.UpdatedDate,T.SystemID,T.IsDeleted,@EmployeeID,T.FacilityID,              
  T.ReferralTSDateID,@PayorID,@CareTypeId,@ReferralBillingAuthorizationID FROM @TempTable T           
  LEFT JOIN ScheduleMasters SM ON SM.ReferralTSDateID=T.ReferralTSDateID AND SM.StartDate=T.StartDate AND SM.EndDate=T.EndDate          
  WHERE SM. ScheduleID IS NULL     
          
  SET  @I=  @I + @@ROWCOUNT;   
  
  
  
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
  
         
  SELECT @I;        
 END           
                    
 END              
 ELSE               
 BEGIN              
                
      
  DECLARE @Old_ReferralTSDateID BIGINT=0;              
  SELECT @Old_ReferralTSDateID=ReferralTSDateID FROM ScheduleMasters WHERE ScheduleID=@ScheduleID              
      
  DECLARE @NEW_ReferralTSDateID BIGINT=0;                      
  DECLARE @NEW_ReferralTSStartTime DATETIME=0;              
  DECLARE @NEW_ReferralTSEndTime DATETIME=0;              
      
  SELECT @NEW_ReferralTSDateID=ReferralTSDateID,@NEW_ReferralTSStartTime=ReferralTSStartTime,@NEW_ReferralTSEndTime=ReferralTSEndTime    
   FROM ReferralTimeSlotDates WHERE ReferralTSDateID=@Old_ReferralTSDateID              
      
       DECLARE @Cnt int = 0;          
  IF(@NEW_ReferralTSDateID>0)              
  BEGIN              
              
   UPDATE ScheduleMasters SET StartDate=@NEW_ReferralTSStartTime, EndDate=@NEW_ReferralTSEndTime, ReferralID=@ReferralID, FacilityID=@FacilityID,              
   UpdatedBy=@loggedInId,UpdatedDate=GETUTCDATE(),              
   ReferralTSDateID=@NEW_ReferralTSDateID,    
   PayorID=@PayorID,CareTypeId=@CareTypeId,ReferralBillingAuthorizationID=@ReferralBillingAuthorizationID                 
   WHERE ScheduleID=@ScheduleID              
            SET @Cnt =  @Cnt + @@ROWCOUNT;
            EXEC [dbo].[ScheduleEventBroadcast] 'EditSchedule', @ScheduleID,'',''    
  END              
              
  SELECT @Cnt;              
 END              
              
          
       
       
    
    
--EXEC HC_DayCare_CreateBillingNotes @ReferralIDs=@ReferralID,@ScheduleDate = @StartDateOnly ,@LoggedInID=@loggedInId,@SystemID =@SystemID    
           
END