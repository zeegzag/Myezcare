CREATE procedure rpt.getTaskListbyVisit (@employeevisitID int)
as
select distinct evn.EmployeeVisitID,evn.EmployeeVisitNoteID,dd.Title [Care Type],vt.TaskCode,vt.VisitTaskDetail TaskName,vtc.VisitTaskCategoryName,evn.IsSimpleTask,evn.AlertComment,evn.ServiceTime
	, dbo.Fn_GetDDTitlebyID(vt.VisitType) VisitType from employeevisitnotes evn 
inner join EmployeeVisits ev on evn.EmployeeVisitID=ev.EmployeeVisitID
inner join ReferralTaskMappings rtm on evn.ReferralTaskMappingID=rtm.ReferralTaskMappingID
inner join VisitTasks vt on rtm.VisitTaskID=vt.VisitTaskID
inner join VisitTaskCategories vtc on vtc.VisitTaskCategoryID=vt.VisitTaskCategoryID 
inner join DDMaster dd on dd.DDMasterID=vt.CareType
where evn.EmployeeVisitID=@employeevisitID