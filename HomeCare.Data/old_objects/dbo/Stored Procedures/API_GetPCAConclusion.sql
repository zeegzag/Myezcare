-- Exec API_GetPCAConclusion @ReferralID=39, @ScheduleID=26215, @VisitTaskType='Conclusion'          
          
CREATE PROCEDURE [dbo].[API_GetPCAConclusion]      
 @ReferralID BIGINT,                      
 @ScheduleID BIGINT,                      
 @VisitTaskType NVARCHAR(10)                          
AS                                      
BEGIN                
        

    
    
    
  SELECT  V.VisitTaskDetail,RM.ReferralTaskMappingID,V.MinimumTimeRequired,RM.IsRequired,          
  VG.VisitTaskCategoryName as CategoryName,VG.VisitTaskCategoryID as CategoryId,           
  VG1.VisitTaskCategoryName as SubCategoryName, VG1.VisitTaskCategoryID as SubCategoryId , EVN.Description as Answer    
  FROM 		ReferralTaskMappings RM 
  INNER JOIN VisitTasks V ON V.VisitTaskID=RM.VisitTaskID   AND V.VisitTaskType=@VisitTaskType
  Left Join EmployeeVisits EV        ON  ev.ScheduleID=@ScheduleID and ev.IsDeleted=0
  LEFT JOIN EmployeeVisitNotes EVN ON EVN.EmployeeVisitID=EV.EmployeeVisitID AND RM.ReferralTaskMappingID=EVN.ReferralTaskMappingID --AND EV.ScheduleID=26215     
  LEFT JOIN VisitTaskCategories VG ON VG.VisitTaskCategoryID=V.VisitTaskCategoryID          
  LEFT JOIN VisitTaskCategories VG1 ON VG1.VisitTaskCategoryID=V.VisitTaskSubCategoryID     
  WHERE RM.ReferralID= @ReferralID  AND RM.IsDeleted=0    --AND EVN.Description IS NOT NULL    
  ORDER BY VG.VisitTaskCategoryName, evn.ReferralTaskMappingID    
    
     
    
  SELECT * FROM EmployeeVisits WHERE ScheduleID=@ScheduleID      
    
END  