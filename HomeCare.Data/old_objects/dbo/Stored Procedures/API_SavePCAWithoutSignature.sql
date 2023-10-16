CREATE PROCEDURE [dbo].[API_SavePCAWithoutSignature]  
 @EmployeeVisitID BIGINT,                    
 @ScheduleID BIGINT  
AS                              
BEGIN    
  Update EmployeeVisits      
  SET IsPCACompleted=1,UpdatedDate=GETUTCDATE()      
  WHERE EmployeeVisitID=@EmployeeVisitID AND ScheduleID=@ScheduleID        
END
