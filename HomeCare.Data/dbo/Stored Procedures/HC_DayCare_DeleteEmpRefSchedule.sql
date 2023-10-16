--EXEC DeleteEmpRefSchedule @ReferralTimeSlotDetailID = '50078', @ReferralTimeSlotMasterID = '1966', @Day = '1', @StartDate = '5/21/2018 12:00:00 AM', @EndDate = '5/21/2019 12:00:00 AM', @StartTime = '08:00:00', @EndTime = '10:00:00'      
--EXEC DeleteEmpRefSchedule @ReferralTimeSlotDetailID = '50078', @ReferralTimeSlotMasterID = '1966', @Day = '1', @StartDate = '5/21/2018 12:00:00 AM', @EndDate = '5/21/2019 12:00:00 AM', @StartTime = '08:00:00', @EndTime = '10:00:00'  
      
CREATE PROCEDURE [dbo].[HC_DayCare_DeleteEmpRefSchedule]  
 @ReferralTimeSlotDetailID BIGINT,      
 @ReferralTimeSlotMasterID BIGINT,      
 @Day INT,      
 @StartDate DATE,  
 @EndDate DATE,  
 @loggedInId BIGINT      
AS                                        
BEGIN                            
   
--DECLARE @TempTable TABLE(ReferralTSDateID BIGINT);  
--INSERT INTO  @TempTable  
--SELECT RSD.ReferralTSDateID FROM ReferralTimeSlotDates RSD  
--WHERE RSD.ReferralTimeSlotDetailID=@ReferralTimeSlotDetailID AND RSD.ReferralTimeSlotMasterID=@ReferralTimeSlotMasterID AND DayNumber=@Day   
--AND CONVERT(TIME,ReferralTSStartTime) = @StartTime AND CONVERT(TIME,ReferralTSEndTime) = @EndTime    
  
  
--UPDATE SM SET IsDeleted=1,UpdatedDate=GETDATE() FROM ScheduleMasters SM  
--INNER JOIN @TempTable RSD ON RSD.ReferralTSDateID=SM.ReferralTSDateID  
 
 DECLARE @Output TABLE (  
    ScheduleID bigint 
  )  

INSERT INTO @Output       
   SELECT ScheduleID FROM ScheduleMasters SM  
INNER JOIN ReferralTimeSlotDates RSD ON RSD.ReferralTSDateID=SM.ReferralTSDateID  
WHERE RSD.ReferralTimeSlotDetailID=@ReferralTimeSlotDetailID AND RSD.ReferralTimeSlotMasterID=@ReferralTimeSlotMasterID AND DayNumber=@Day  
AND RSD.ReferralTSDate BETWEEN @StartDate AND @EndDate    
  
UPDATE SM SET IsDeleted=1,UpdatedDate=GETDATE(), UpdatedBy=@loggedInId FROM ScheduleMasters SM  
INNER JOIN ReferralTimeSlotDates RSD ON RSD.ReferralTSDateID=SM.ReferralTSDateID  
WHERE RSD.ReferralTimeSlotDetailID=@ReferralTimeSlotDetailID AND RSD.ReferralTimeSlotMasterID=@ReferralTimeSlotMasterID AND DayNumber=@Day  
AND RSD.ReferralTSDate BETWEEN @StartDate AND @EndDate  
  
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
  
UPDATE N SET IsDeleted=1,UpdatedDate=GETDATE(), UpdatedBy=@loggedInId FROM ScheduleMasters SM  
INNER JOIN ReferralTimeSlotDates RSD ON RSD.ReferralTSDateID=SM.ReferralTSDateID  
LEFT JOIN Notes N ON N.ScheduleID=SM.ScheduleID  
WHERE RSD.ReferralTimeSlotDetailID=@ReferralTimeSlotDetailID AND RSD.ReferralTimeSlotMasterID=@ReferralTimeSlotMasterID AND DayNumber=@Day  
AND RSD.ReferralTSDate BETWEEN @StartDate AND @EndDate AND N.NoteID IS NOT NULL  
  
SELECT 1;  
      
END