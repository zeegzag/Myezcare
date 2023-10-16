CREATE PROCEDURE [dbo].[UpdateReferralTimeSlotAssignee]            
@ScheduleID INT = 0,          
@EmployeeID INT = 0    
            
AS              
BEGIN          
         
   IF(@ScheduleID>0)
   BEGIN
		UPDATE ScheduleMasters SET EmployeeID = @EmployeeID WHERE ScheduleID = @ScheduleID
   END
   
END
GO