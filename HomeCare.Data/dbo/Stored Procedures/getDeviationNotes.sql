
--UpdateBy:Akhilesh
--UpdatedDate:28/march/2020
--Description: for get DeviationType
-- exec getDeviationNotes  163 
-- exec getDeviationNotes  163    
CREATE procedure [dbo].[getDeviationNotes]      
@EmployeeVisitID bigint  
as      
begin      
  SELECT DM.Title AS DeviationType,sdn.DeviationID,sdn.DeviationNotes,sdn.DeviationNoteID,sdn.EmployeeVisitID,sdn.DeviationTime FROM SaveDeviationNote sdn    
  INNER JOIN EmployeeVisits ev ON ev.EmployeeVisitID=sdn.EmployeeVisitID    
  INNER JOIN DDMaster DM ON DM.DDMasterID=sdn.DeviationID    
  WHERE sdn.EmployeeVisitID=@EmployeeVisitID    
  
--SELECT WorkingHour=SUM(DATEDIFF(minute,ev.ClockInTime,EV.ClockOutTime)),ScheduleTime=SUM(DATEDIFF(minute,sm.StartDate,sm.EndDate))  
-- from EmployeeVisits ev  
-- inner join ScheduleMasters sm on ev.ScheduleID=sm.ScheduleID  
-- where ev.EmployeeVisitID=@EmployeeVisitID  
 select DDMasterID as DeviationID,Title as DeviationType 
 from DDMaster dm
 inner join lu_DDMasterTypes lu on lu.DDMasterTypeID=dm.ItemType
  where lu.Name='Deviation Type'
end      
      
-- select * from SaveDeviationNote      