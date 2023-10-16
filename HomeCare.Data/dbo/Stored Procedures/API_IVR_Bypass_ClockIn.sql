﻿CREATE PROCEDURE [dbo].[API_IVR_Bypass_ClockIn]          
 @EmployeeID bigint,          
 --@ReferralIds nvarchar(max),        
 @StartDateBefore datetime,        
 @StartDateAfter datetime,        
 @ClockInTime datetime          
AS          
BEGIN          
 -- SET NOCOUNT ON added to prevent extra result sets from          
 -- interfering with SELECT statements.          
 SET NOCOUNT ON;        
   
 DECLARE @tmpScheduleMasters TABLE (ScheduleID bigint)        
        
 INSERT INTO @tmpScheduleMasters (ScheduleID)        
 SELECT sm.ScheduleID        
 FROM dbo.ScheduleMasters sm         
 WHERE sm.EmployeeID=@EmployeeID AND sm.StartDate BETWEEN @StartDateBefore AND @StartDateAfter AND ISNULL(SM.OnHold, 0) = 0 AND sm.IsDeleted=0    
       
    IF EXISTS(SELECT 1 FROM @tmpScheduleMasters tsm)        
  BEGIN          
   IF NOT EXISTS (SELECT 1 FROM dbo.EmployeeVisits ev WHERE ev.ScheduleID IN (SELECT tsm.ScheduleID FROM @tmpScheduleMasters tsm))          
    BEGIN          
     INSERT INTO dbo.EmployeeVisits (ScheduleID,ClockInTime,IsDeleted,CreatedDate,CreatedBy,SurveyCompleted)        
     SELECT tsm.ScheduleID,@ClockInTime,0,@ClockInTime,@EmployeeID,0 FROM @tmpScheduleMasters tsm        

     
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
    END          
   SELECT 1 AS TransactionResultId;          
  END          
 ELSE          
  BEGIN          
   SELECT 0 AS TransactionResultId;          
  END          
END