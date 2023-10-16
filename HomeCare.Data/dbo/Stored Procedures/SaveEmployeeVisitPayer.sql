CREATE PROCEDURE [dbo].[SaveEmployeeVisitPayer]          
@EmployeeVisitID BIGINT,                
 @ReferralPayorID BIGINT                           
         
AS                            
BEGIN          
 DECLARE @ScheduleID BIGINT;     
 DECLARE @Payor BIGINT;         
 SET @ScheduleID = (SELECT ScheduleID FROM EmployeeVisits WHERE EmployeeVisitID=@EmployeeVisitID)          
 SET @Payor =(select sm.PayorID from ScheduleMasters sm where sm.ScheduleID=@ScheduleID)          
        
          
 IF(@Payor is null)          
 BEGIN          
 UPDATE  ScheduleMasters SET    
 PayorID=@ReferralPayorID,    
 UpdatedDate=GETUTCDATE()    
 where ScheduleID=@ScheduleID   
 
  EXEC [dbo].[ScheduleEventBroadcast] 'EditSchedule', @ScheduleID,'',''
 END       
    
  SELECT r.ReferralID,STRING_AGG(CONCAT('', '', p.PayorID), ',')PayorID ,STRING_AGG(CONCAT('', '', p.PayorName), ',')PayorName from Referrals r       
     inner join [ReferralPayorMappings] as rpm  on rpm.ReferralID=r.ReferralID      
     inner join Payors p on p.PayorID=rpm.PayorID      
     group  by r.ReferralID      
END