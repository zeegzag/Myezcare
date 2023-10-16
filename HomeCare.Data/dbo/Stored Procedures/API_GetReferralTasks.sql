--EXEC API_GetReferralTasks @ReferralID = N'10241', @ScheduleID = N'123370', @VisitTaskType = N'Task'              
              
CREATE PROCEDURE [dbo].[API_GetReferralTasks]                              
 @ReferralID BIGINT,                                          
 @ScheduleID BIGINT,                                          
 @VisitTaskType NVARCHAR(10)                                              
AS                                                          
BEGIN         
    
 IF (@VisitTaskType='Task')                
 begin                
              
              
              
              
 Declare @isTaskAdded int,@Caretype int,@employeeVisitID int              
              
select @isTaskAdded= count(*) from EmployeeVisitNotes EVN inner join EmployeeVisits EV on EVN.EmployeeVisitID=ev.EmployeeVisitID where ScheduleID=@ScheduleID              
if @isTaskAdded>0               
begin              
Exec API_GetSelectedTaskList @scheduleID              
end              
else              
begin              
 declare @CaretypeId bigint                    
 select @CaretypeId=CaretypeId from ScheduleMasters where ScheduleID=@scheduleID                 
  IF (@CaretypeId IS NOT NULL and @CaretypeId <>0)                 
 begin                 
  SELECT V.VisitTaskDetail,RM.ReferralTaskMappingID,V.MinimumTimeRequired,RM.IsRequired,v.SendAlert,                            
   VG.VisitTaskCategoryName as CategoryName,VG.VisitTaskCategoryID as CategoryId,                             
   VG1.VisitTaskCategoryName as SubCategoryName, VG1.VisitTaskCategoryID as SubCategoryId ,0 isDone                     
   FROM ReferralTaskMappings RM                       
   INNER JOIN VisitTasks V ON V.VisitTaskID=RM.VisitTaskID                       
   LEFT JOIN VisitTaskCategories VG ON VG.VisitTaskCategoryID=V.VisitTaskCategoryID                            
   LEFT JOIN VisitTaskCategories VG1 ON VG1.VisitTaskCategoryID=V.VisitTaskSubCategoryID                       
   WHERE RM.ReferralID= @ReferralID AND V.VisitTaskType=@VisitTaskType AND RM.IsDeleted=0 AND v.CareType = @CaretypeId                
   ORDER BY VG.VisitTaskCategoryName                      
 end                
 else                 
 begin                
 SELECT V.VisitTaskDetail,RM.ReferralTaskMappingID,V.MinimumTimeRequired,RM.IsRequired,v.SendAlert,                            
  VG.VisitTaskCategoryName as CategoryName,VG.VisitTaskCategoryID as CategoryId,                             
  VG1.VisitTaskCategoryName as SubCategoryName, VG1.VisitTaskCategoryID as SubCategoryId                      
  FROM ReferralTaskMappings RM                       
  INNER JOIN VisitTasks V ON V.VisitTaskID=RM.VisitTaskID                       
  LEFT JOIN VisitTaskCategories VG ON VG.VisitTaskCategoryID=V.VisitTaskCategoryID                            
  LEFT JOIN VisitTaskCategories VG1 ON VG1.VisitTaskCategoryID=V.VisitTaskSubCategoryID                       
  WHERE RM.ReferralID= @ReferralID AND V.VisitTaskType=@VisitTaskType AND RM.IsDeleted=0                 
  ORDER BY VG.VisitTaskCategoryName                            
end                      
END               
end              
ELSE IF (@VisitTaskType='Conclusion')                
BEGIN          
Declare @ConclusionCount int        
SELECT @ConclusionCount=COUNT(*)        
FROM EmployeeVisits EV        
INNER JOIN EmployeeVisitNotes EVN ON EV.EmployeeVisitID=EVN.EmployeeVisitID        
INNER JOIN ReferralTaskMappings RTM ON RTM.ReferralTaskMappingID=EVN.ReferralTaskMappingID        
INNER JOIN VisitTasks VT ON VT.VisitTaskID=RTM.VisitTaskID        
WHERE VT.VisitTaskType='Conclusion' and EV.ScheduleID=@ScheduleID        
IF(@ConclusionCount=0)        
BEGIN        
SELECT V.VisitTaskDetail,RM.ReferralTaskMappingID,V.MinimumTimeRequired,RM.IsRequired,v.SendAlert,                            
  VG.VisitTaskCategoryName as CategoryName,VG.VisitTaskCategoryID as CategoryId,                             
  VG1.VisitTaskCategoryName as SubCategoryName, VG1.VisitTaskCategoryID as SubCategoryId                      
  FROM ReferralTaskMappings RM                       
  INNER JOIN VisitTasks V ON V.VisitTaskID=RM.VisitTaskID                       
  LEFT JOIN VisitTaskCategories VG ON VG.VisitTaskCategoryID=V.VisitTaskCategoryID                            
  LEFT JOIN VisitTaskCategories VG1 ON VG1.VisitTaskCategoryID=V.VisitTaskSubCategoryID                       
  WHERE RM.ReferralID= @ReferralID AND V.VisitTaskType=@VisitTaskType AND RM.IsDeleted=0                 
  ORDER BY VG.VisitTaskCategoryName         
END        
ELSE IF(@ConclusionCount>0)        
              
 SELECT V.VisitTaskDetail,RM.ReferralTaskMappingID,V.MinimumTimeRequired,RM.IsRequired,v.SendAlert,                  
  VG.VisitTaskCategoryName as CategoryName,VG.VisitTaskCategoryID as CategoryId,evn.Description as Answer,                   
  VG1.VisitTaskCategoryName as SubCategoryName, VG1.VisitTaskCategoryID as SubCategoryId ,EVN.AlertComment            
  FROM ReferralTaskMappings RM             
  INNER JOIN VisitTasks V ON V.VisitTaskID=RM.VisitTaskID             
  LEFT JOIN VisitTaskCategories VG ON VG.VisitTaskCategoryID=V.VisitTaskCategoryID                  
  LEFT JOIN VisitTaskCategories VG1 ON VG1.VisitTaskCategoryID=V.VisitTaskSubCategoryID      
    INNER JOIN EmployeeVisitNotes EVN ON EVN.ReferralTaskMappingID=RM.ReferralTaskMappingID    
 INNER JOIN EmployeeVisits EV ON EV.EmployeeVisitID=EVN.EmployeeVisitID     
 INNER JOIN ScheduleMasters SM ON SM.ScheduleID=EV.ScheduleID AND SM.ScheduleID=@ScheduleID    
  WHERE RM.ReferralID= @ReferralID AND V.VisitTaskType='Conclusion' AND RM.IsDeleted=0       
      
  --UNION     
  --SELECT 'SurveyComment' VisitTaskDetail,EV.Employeevisitid ReferralTaskMappingID,'0' MinimumTimeRequired,'0' IsRequired,'0' SendAlert,                  
  --Null as CategoryName,Null as CategoryId,(cASE WHEN ev.SurveyComment  is null  then 'Yes' else 'No' end ) Answer,                   
  --Null as SubCategoryName, Null as SubCategoryId ,EV.SurveyComment AS AlertComment            
  --FROM  EmployeeVisits EV  INNER JOIN ScheduleMasters SM ON SM.ScheduleID=EV.ScheduleID AND SM.ScheduleID=@ScheduleID    
      
End                
                      
 --SELECT V.VisitTaskDetail,RM.ReferralTaskMappingID,V.MinimumTimeRequired,RM.IsRequired,                            
 --VG.VisitTaskCategoryName as CategoryName,VG.VisitTaskCategoryID as CategoryId,                             
 --VG1.VisitTaskCategoryName as SubCategoryName, VG1.VisitTaskCategoryID as SubCategoryId, evn.Description as Answer                      
 --FROM ReferralTaskMappings RM               
 --INNER JOIN VisitTasks V ON V.VisitTaskID=RM.VisitTaskID                       
 --LEFT JOIN VisitTaskCategories VG ON VG.VisitTaskCategoryID=V.VisitTaskCategoryID                            
 --LEFT JOIN VisitTaskCategories VG1 ON VG1.VisitTaskCategoryID=V.VisitTaskSubCategoryID                       
 --LEFT JOIN ScheduleMasters SM ON SM.ReferralID=RM.ReferralID  AND SM.ReferralID= @ReferralID AND SM.ScheduleID=@ScheduleID                      
 --LEFT JOIN EmployeeVisits EV ON EV.ScheduleID=SM.ScheduleID                   
 -- LEFT JOIN EmployeeVisitNotes EVN ON EVN.EmployeeVisitID=EV.EmployeeVisitID                      
 --WHERE RM.ReferralID= @ReferralID                      
                      
 SELECT EmployeeVisitID,SurveyComment FROM EmployeeVisits WHERE ScheduleID=@ScheduleID                            
                                
END
GO

