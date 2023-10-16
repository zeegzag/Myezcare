CREATE PROCEDURE [dbo].[API_IVR_Bypass_ClockOut]  
 @EmployeeID bigint,  
 @StartDateBefore datetime,            
 @StartDateAfter datetime,            
 @ClockOutTime datetime            
AS              
BEGIN            
            
 -- SET NOCOUNT ON added to prevent extra result sets from              
 -- interfering with SELECT statements.              
 SET NOCOUNT ON;  
            
 DECLARE @tmpScheduleMasters TABLE (ScheduleID bigint,ClockOut datetime)            
       
       
 --Find all the schedules of logged in employee before and after 30 minutes of EndDate(Time)      
 INSERT INTO @tmpScheduleMasters (ScheduleID)            
 SELECT sm.ScheduleID          
 FROM dbo.ScheduleMasters sm             
 WHERE sm.EmployeeID=@EmployeeID AND sm.EndDate BETWEEN @StartDateBefore AND @StartDateAfter AND ISNULL(SM.OnHold, 0) = 0 AND sm.IsDeleted=0  
            
 IF EXISTS(SELECT 1 FROM @tmpScheduleMasters tsm)              
  BEGIN              
   IF NOT EXISTS (SELECT 1 FROM dbo.EmployeeVisits ev WHERE ev.ScheduleID IN (SELECT tsm.ScheduleID FROM @tmpScheduleMasters tsm))              
    BEGIN              
     INSERT INTO dbo.EmployeeVisits (ScheduleID,ClockOutTime,IsDeleted,CreatedDate,CreatedBy,SurveyCompleted,IVRClockOut)              
     SELECT tsm.ScheduleID,@ClockOutTime,0,@ClockOutTime,@EmployeeID,0,1 FROM @tmpScheduleMasters tsm            
    END              
   ELSE              
    BEGIN            
     DECLARE @COT datetime;          
     SELECT @COT=ClockOutTime FROM EmployeeVisits WHERE ScheduleID IN (SELECT tsm.ScheduleID FROM @tmpScheduleMasters tsm)          
     IF (@COT is null)          
         UPDATE ev            
         SET ClockOutTime = @ClockOutTime,IVRClockOut=1,UpdatedDate = @ClockOutTime,UpdatedBy = @EmployeeID            
         from dbo.EmployeeVisits ev            
         INNER JOIN @tmpScheduleMasters tsm ON ev.ScheduleID=tsm.ScheduleID            
     END   
    
    DECLARE @CurScheduleID bigint;
    DECLARE eventCursor CURSOR FORWARD_ONLY FOR
            SELECT ScheduleID FROM @tmpScheduleMasters;
        OPEN eventCursor;
        FETCH NEXT FROM eventCursor INTO @CurScheduleID;
        WHILE @@FETCH_STATUS = 0 BEGIN
            EXEC [dbo].[ScheduleEventBroadcast] 'EditSchedule', @CurScheduleID,'',''
            FETCH NEXT FROM eventCursor INTO @CurScheduleID;
        END;
        CLOSE eventCursor;
        DEALLOCATE eventCursor;           
   SELECT 1 AS TransactionResultId;              
  END              
 ELSE              
  BEGIN              
   SELECT 0 AS TransactionResultId;              
  END              
END