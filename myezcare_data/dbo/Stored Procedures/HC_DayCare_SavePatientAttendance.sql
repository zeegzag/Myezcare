CREATE PROCEDURE [dbo].[HC_DayCare_SavePatientAttendance]
@ScheduleID BIGINT,    
@ReferralID BIGINT,    
@IsPatientAttendedSchedule BIT,    
@PateintAbsentReason NVARCHAR(MAX),    
@loggedInId BIGINT,    
@SystemID NVARCHAR(MAX)    
AS    
BEGIN    
    
DECLARE @InfinateDate DATE='2099-12-31';    

IF(@IsPatientAttendedSchedule=1 OR LEN(@PateintAbsentReason)=0)
SET @PateintAbsentReason=NULL;

Update ScheduleMasters SET IsPatientAttendedSchedule=@IsPatientAttendedSchedule, AbsentReason=@PateintAbsentReason,UpdatedBy=@loggedInId, UpdatedDate=GETUTCDATE(),
SystemID=@SystemID  WHERE ScheduleID=@ScheduleID
    
    
END
