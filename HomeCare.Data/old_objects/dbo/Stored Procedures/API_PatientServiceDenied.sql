CREATE PROCEDURE [dbo].[API_PatientServiceDenied]        
@ScheduleID BIGINT,        
@EmployeeID BIGINT,        
@Note VARCHAR(1000)      
AS                              
BEGIN            
 DECLARE @ReferralTSDateID BIGINT        
 SET @ReferralTSDateID=(SELECT ReferralTSDateID FROM ScheduleMasters WHERE ScheduleID=@ScheduleID)        
        
 UPDATE ReferralTimeSlotDates SET IsDenied=1,Notes=@Note WHERE ReferralTSDateID=@ReferralTSDateID        
 UPDATE ScheduleMasters SET IsDeleted=1,UpdatedBy=@EmployeeID,UpdatedDate=GETDATE() WHERE ScheduleID=@ScheduleID  
 EXEC [dbo].[ScheduleEventBroadcast] 'DeleteSchedule', @ScheduleID,'',''      
END  