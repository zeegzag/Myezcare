    
--EXEC ADC.HC_DayCare_GetSchedulePatientTasks @ScheduleID = '0', @ReferralID = '10038'    
CREATE PROCEDURE [adc].[HC_DayCare_GetSchedulePatientTasks]      
@ReferralID BIGINT,      
@ScheduleID BIGINT =0     
AS  
BEGIN                  
      
IF(@ScheduleID>0)      
BEGIN      
      
SELECT distinct RTM.ReferralTaskMappingID,VT.VisitTaskID, VT.VisitTaskDetail,     
IsSelected = CONVERT(BIT,CASE WHEN EVN.EmployeeVisitNoteID > 0 THEN 1 ELSE 0 END)    
--IsSelected = 0    
,RTM.ReferralID FROM ReferralTaskMappings RTM      
INNER JOIN VisitTasks VT ON VT.VisitTaskID=RTM.VisitTaskID      
INNER JOIN EmployeeVisitNotes EVN ON EVN.ReferralTaskMappingID=RTM.ReferralTaskMappingID      
INNER JOIN EmployeeVisits EV ON EV.EmployeeVisitID=EVN.EmployeeVisitID AND EV.ScheduleID=@ScheduleID      
WHERE VT.IsDeleted=0 AND RTM.IsDeleted=0 AND RTM.ReferralID=@ReferralID      
ORDER BY RTM.ReferralID ASC      
      
END      
      
ELSE       
BEGIN      
    
 SELECT distinct RTM.ReferralTaskMappingID,VT.VisitTaskID, VT.VisitTaskDetail, IsSelected = CONVERT(BIT,0),RTM.ReferralID FROM ReferralTaskMappings RTM      
 INNER JOIN VisitTasks VT ON VT.VisitTaskID=RTM.VisitTaskID      
 WHERE VT.IsDeleted=0 AND RTM.IsDeleted=0 AND RTM.ReferralID=@ReferralID      
 ORDER BY RTM.ReferralID ASC      
END      
    
  exec [adc].[GetVisitTaskOptionList]  @ReferralID
   
END