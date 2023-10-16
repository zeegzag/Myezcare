--Exec API_GetSelectedTaskList '0'  
  
CREATE Procedure API_GetSelectedTaskList(@ScheduleID int)  
AS  
Begin  
Declare @caretype int  
Declare @ReferralID int,  
@SelectedTask int,  
@EmployeeVisitID int  
  
  
  
select @ReferralID=ReferralID,@caretype= CareTypeId ,@EmployeeVisitID=ev.EmployeeVisitID from ScheduleMasters SM inner join EmployeeVisits EV on SM.ScheduleID=ev.ScheduleID where sm.ScheduleID=@ScheduleID  
select   
VT.VisitTaskDetail,RTM.ReferralTaskMappingID,VT.MinimumTimeRequired,RTM.IsRequired,vT.SendAlert,                
   VG.VisitTaskCategoryName as CategoryName,VG.VisitTaskCategoryID as CategoryId,                 
   VG1.VisitTaskCategoryName as SubCategoryName, VG1.VisitTaskCategoryID as SubCategoryId   
 , IsDone = Case When EVN.EmployeeVisitNoteID IS NULL Then 0 Else 1 END  
from ReferralTaskMappings RTM   
inner join VisitTasks VT on VT.VisitTaskID=rtm.VisitTaskID   
LEFT JOIN VisitTaskCategories VG ON VG.VisitTaskCategoryID=VT.VisitTaskCategoryID                
   LEFT JOIN VisitTaskCategories VG1 ON VG1.VisitTaskCategoryID=VT.VisitTaskSubCategoryID           
left join EmployeeVisitNotes EVN on RTM.ReferralTaskMappingID = EVN.ReferralTaskMappingID and evn.EmployeeVisitID=@EmployeeVisitID   
  
where ReferralID=@ReferralID and CareType=@caretype and RTM.IsDeleted=0 and VT.IsDeleted=0  
and   VT.VisitTaskType='Task'
  
order by VT.VisitTaskDetail  
  
  
end