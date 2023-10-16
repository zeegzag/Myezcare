-- Exec API_GetPCAConclusion @ReferralID=1953, @ScheduleID=88818, @VisitTaskType='Conclusion'        
        
CREATE PROCEDURE [dbo].[API_GetPCAConclusion]    
 @ReferralID BIGINT,                    
 @ScheduleID BIGINT,                    
 @VisitTaskType NVARCHAR(10)                        
AS                                    
BEGIN              
      
 --SELECT DISTINCT V.VisitTaskDetail,RM.ReferralTaskMappingID,V.MinimumTimeRequired,RM.IsRequired,        
 -- VG.VisitTaskCategoryName as CategoryName,VG.VisitTaskCategoryID as CategoryId,         
 -- VG1.VisitTaskCategoryName as SubCategoryName, VG1.VisitTaskCategoryID as SubCategoryId  --, evn.Description as Answer  
 -- FROM ReferralTaskMappings RM   
 -- INNER JOIN VisitTasks V ON V.VisitTaskID=RM.VisitTaskID   
 -- LEFT JOIN VisitTaskCategories VG ON VG.VisitTaskCategoryID=V.VisitTaskCategoryID        
 -- LEFT JOIN VisitTaskCategories VG1 ON VG1.VisitTaskCategoryID=V.VisitTaskSubCategoryID   
 -- LEFT JOIN ScheduleMasters SM ON SM.ReferralID=RM.ReferralID  AND SM.ReferralID= @ReferralID AND SM.ScheduleID=@ScheduleID  
 -- LEFT JOIN EmployeeVisits EV ON EV.ScheduleID=SM.ScheduleID    
 -- LEFT JOIN EmployeeVisitNotes EVN ON EVN.EmployeeVisitID=EV.EmployeeVisitID AND EVN.ReferralTaskMappingID=RM.ReferralTaskMappingID  
 -- WHERE RM.ReferralID= @ReferralID AND V.VisitTaskType=@VisitTaskType AND RM.IsDeleted=0   
 --  ORDER BY VG.VisitTaskCategoryName  
  
  
  
  
  SELECT DISTINCT V.VisitTaskDetail,RM.ReferralTaskMappingID,V.MinimumTimeRequired,RM.IsRequired,        
  VG.VisitTaskCategoryName as CategoryName,VG.VisitTaskCategoryID as CategoryId,         
  VG1.VisitTaskCategoryName as SubCategoryName, VG1.VisitTaskCategoryID as SubCategoryId , EVN.Description as Answer  
  FROM EmployeeVisits EV     
  LEFT JOIN EmployeeVisitNotes EVN ON EVN.EmployeeVisitID=EV.EmployeeVisitID AND EV.ScheduleID=@ScheduleID  
  LEFT JOIN ReferralTaskMappings RM ON RM.ReferralTaskMappingID=EVN.ReferralTaskMappingID AND RM.ReferralID= @ReferralID AND RM.IsDeleted=0   
  LEFT JOIN VisitTasks V ON V.VisitTaskID=RM.VisitTaskID   
  LEFT JOIN VisitTaskCategories VG ON VG.VisitTaskCategoryID=V.VisitTaskCategoryID        
  LEFT JOIN VisitTaskCategories VG1 ON VG1.VisitTaskCategoryID=V.VisitTaskSubCategoryID   
  WHERE RM.ReferralID= @ReferralID AND V.VisitTaskType=@VisitTaskType AND RM.IsDeleted=0 --AND EVN.Description IS NOT NULL  
  ORDER BY VG.VisitTaskCategoryName  
  
  
  --ReferralTaskMappings RM   
  --INNER JOIN VisitTasks V ON V.VisitTaskID=RM.VisitTaskID   
  --LEFT JOIN VisitTaskCategories VG ON VG.VisitTaskCategoryID=V.VisitTaskCategoryID        
  --LEFT JOIN VisitTaskCategories VG1 ON VG1.VisitTaskCategoryID=V.VisitTaskSubCategoryID   
  --LEFT JOIN ScheduleMasters SM ON SM.ReferralID=RM.ReferralID  AND SM.ReferralID= @ReferralID AND SM.ScheduleID=@ScheduleID  
  --LEFT JOIN EmployeeVisits EV ON EV.ScheduleID=SM.ScheduleID    
  --LEFT JOIN EmployeeVisitNotes EVN ON EVN.EmployeeVisitID=EV.EmployeeVisitID AND EVN.ReferralTaskMappingID=RM.ReferralTaskMappingID  
  --WHERE RM.ReferralID= @ReferralID AND V.VisitTaskType=@VisitTaskType AND RM.IsDeleted=0   
  -- ORDER BY VG.VisitTaskCategoryName  
  
  
  SELECT * FROM EmployeeVisits WHERE ScheduleID=@ScheduleID    
  
END
