  
CREATE procedure UpdatePayor  
@EmployeeVisitID INT=null,  
@ScheduleID INT=null,  
@PayorID INT=null,  
@ReferralID INT =null  
AS  
BEGIN  
-- Update PayorID in scheduleMaster table  
 set @ScheduleID = (select ScheduleID FROM EmployeeVisits where EmployeeVisitID = @EmployeeVisitID)   
UPDATE ScheduleMasters SET PayorID=@PayorID WHERE ScheduleID=@ScheduleID  
  
-- Update PayorID in ReferralPayorMappings table  
declare @PayorIDs int  
declare @ReferralPayorMappingIDs int  
 select @PayorIDs=rpm.PayorID,@ReferralPayorMappingIDs=rpm.ReferralPayorMappingID  
from ReferralPayorMappings rpm  
inner join Referrals r on r.ReferralID=rpm.ReferralID  
inner join ScheduleMasters sm on sm.ReferralID=r.ReferralID  
where sm.ScheduleID=@ScheduleID and r.ReferralID=@ReferralID and rpm.IsDeleted=0  
  
UPDATE ReferralPayorMappings SET PayorID=@PayorID WHERE ReferralPayorMappingID=@ReferralPayorMappingIDs  
  EXEC [dbo].[ScheduleEventBroadcast] 'EditSchedule', @ScheduleID,'222','25'
    
SELECT 1; RETURN;  
--SELECT -2; RETURN;  
END  