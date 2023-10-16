--CreatedBy  Akhilesh kamal       
--CreatedDate 8/Feb/2020       
--Description for saving ByPassReasonClockIn and Rejection resion without update date       
-- exec [HC_BypassActionTaken] 160,'rejection','apprkp'        
CREATE PROCEDURE [dbo].[HC_BypassActionTaken]          
@EmployeeVisitID BIGINT,          
@RejectReason NVARCHAR(1000),          
@ByPassReasonClockIn NVARCHAR(1000)=NULL,  
@ByPassReasonClockOut    NVARCHAR(1000)=NULL,     
@ActionTaken INT=0          
AS          
BEGIN         
      
IF(@ActionTaken=0)      
 UPDATE EmployeeVisits SET ByPassReasonClockIn=@ByPassReasonClockIn,ByPassReasonClockOut=@ByPassReasonClockOut,RejectReason=@RejectReason WHERE EmployeeVisitID=@EmployeeVisitID    
  ELSE      
BEGIN      
UPDATE EmployeeVisits SET  ActionTaken=@ActionTaken, RejectReason=@RejectReason,ByPassReasonClockIn=@ByPassReasonClockIn WHERE EmployeeVisitID=@EmployeeVisitID         
END   
DECLARE @ScheduleID bigint=(SELECT ScheduleID FROM EmployeeVisits WHERE EmployeeVisitID=@EmployeeVisitID  );
EXEC [dbo].[ScheduleEventBroadcast] 'EditSchedule', @ScheduleID,'222','25'
SELECT 1;     
END 