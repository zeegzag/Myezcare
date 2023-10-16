-- Exec API_GetReferralTasks @ReferralID=1, @ScheduleID=372, @VisitTaskType='Task'              
--SELECT * FROM schedulemasters WHERE referralid=2 

--SELECT * FROM dbo.VisitTasks vt WHERE vt.CareType=0
      
CREATE PROCEDURE [dbo].[API_GetReferralTasks]              
 @ReferralID BIGINT,                          
 @ScheduleID BIGINT,                          
 @VisitTaskType NVARCHAR(10)                              
AS                                          
BEGIN                    
 --EXEC API_GetReferralTasks @ReferralID = N'4007', @ScheduleID = N'218144', @VisitTaskType = N'Task'           
 declare @CaretypeId bigint    
 select @CaretypeId=CaretypeId from ScheduleMasters where ScheduleID=@scheduleID    
  IF (@CaretypeId	IS NOT NULL and @CaretypeId	<>0) 
 begin 
		SELECT V.VisitTaskDetail,RM.ReferralTaskMappingID,V.MinimumTimeRequired,RM.IsRequired,v.SendAlert,            
		 VG.VisitTaskCategoryName as CategoryName,VG.VisitTaskCategoryID as CategoryId,             
		 VG1.VisitTaskCategoryName as SubCategoryName, VG1.VisitTaskCategoryID as SubCategoryId      
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
      
 SELECT * FROM EmployeeVisits WHERE ScheduleID=@ScheduleID            
                
END
