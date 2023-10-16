--  EXEC API_GetOnGoingVisitDetail @EmployeeID = N'52', @CurrentDateTime = NULL, @ScheduleID = N'3628'          
          
CREATE PROCEDURE [dbo].[API_GetOnGoingVisitDetail]                                    
 @EmployeeID BIGINT = 0,                            
 @ScheduleID BIGINT  ,                                
 @CurrentDateTime Datetime =null                                
AS                                                                          
BEGIN                                                           
 DECLARE @EmployeeVisitID BIGINT                                    
 DECLARE @ReferralID BIGINT                                    
 DECLARE @Count INT                                   
 DECLARE @AddTaskCount INT       
  DECLARE @MappedTaskCount INT  
  Declare @caretype int  
                                
 SET @EmployeeVisitID=(SELECT EmployeeVisitID FROM EmployeeVisits WHERE ScheduleID=@ScheduleID)                         
 SET @AddTaskCount=(SELECT COUNT(EmployeeVisitNoteID) FROM EmployeeVisitNotes evn            
  INNER JOIN ReferralTaskMappings RTM ON RTM.ReferralTaskMappingID=EVN.ReferralTaskMappingID             
 INNER JOIN VisitTasks  VT ON VT.VisitTaskID=RTM.VisitTaskID            
  WHERE EmployeeVisitID=@EmployeeVisitID AND VT.VisitTaskType='Task')   
    
      select @ReferralID=ReferralID,@caretype= CareTypeId from ScheduleMasters SM inner join EmployeeVisits EV on SM.ScheduleID=ev.ScheduleID where sm.ScheduleID=@ScheduleID  
 set @MappedTaskCount=  (select  count(*)     
                            from ReferralTaskMappings RTM       
                            inner join VisitTasks VT on VT.VisitTaskID=rtm.VisitTaskID       
                            LEFT JOIN VisitTaskCategories VG ON VG.VisitTaskCategoryID=VT.VisitTaskCategoryID                    
                            LEFT JOIN VisitTaskCategories VG1 ON VG1.VisitTaskCategoryID=VT.VisitTaskSubCategoryID   
                            where ReferralID=@ReferralID and CareType=@caretype and RTM.IsDeleted=0 and VT.IsDeleted=0      
                            and   VT.VisitTaskType='Task')  
--  set @MappedTaskCount=(SELECT COUNT(1)       
--FROM ReferralTaskMappings RTM                
-- INNER JOIN VisitTasks  VT ON VT.VisitTaskID=RTM.VisitTaskID            
--inner join ScheduleMasters sm on sm.ReferralID=rtm.ReferralID      
--where sm.ScheduleID=@ScheduleID and rtm.IsDeleted=0 and vt.IsDeleted=0 and vt.VisitTaskType='Task')      
                               
 --SELECT @ReferralID=ReferralID FROM ScheduleMasters WHERE ScheduleID=@ScheduleID                                    
                                    
 SET @Count=(SELECT COUNT(ReferralTaskMappingID) FROM ReferralTaskMappings RTM                                    
 INNER JOIN VisitTasks V ON V.VisitTaskID=RTM.VisitTaskID AND V.VisitTaskType='Conclusion'                                    
 WHERE RTM.ReferralID=@ReferralID AND RTM.IsDeleted=0)                                    
                                          
 IF(@EmployeeVisitID IS NOT NULL)                                          
  SELECT TOP 1 ClockInTime,ClockOutTime,SurveyCompleted,IsSigned,IsPCACompleted,EV.EmployeeVisitID,IVRClockOut,                                    
  --CASE WHEN UsedInScheduling=1 THEN 0 ELSE 1 END AS IsDenied,                                      
  IsDenied,Notes AS DeniedReason,CASE WHEN @Count>0 THEN 1 ELSE 0 END AS HasConclusion, @AddTaskCount AS AddTaskCount,@MappedTaskCount  as MappedTaskCount,dm.Title as CareType                                   
  FROM EmployeeVisits EV                                            
  INNER JOIN ScheduleMasters SM ON SM.ScheduleID=EV.ScheduleID                                            
  LEFT JOIN ReferralTimeSlotDates RTD ON RTD.ReferralTSDateID=SM.ReferralTSDateID --Join may be changed in future                           
  LEFT JOIN EmployeeVisitNotes EVN ON EVN.EmployeeVisitID = EV.EmployeeVisitID                  
  INNER JOIN DDMaster dm on dm.DDMasterID=sm.CareTypeId                          
  WHERE EV.EmployeeVisitID=@EmployeeVisitID --EV.ScheduleID=@ScheduleID                                            
 ELSE                                          
  SELECT --CASE WHEN UsedInScheduling=1 THEN 0 ELSE 1 END AS IsDenied,                                      
  IsDenied,Notes AS DeniedReason ,dm.Title as CareType                                          
  FROM ScheduleMasters SM                                          
  LEFT JOIN ReferralTimeSlotDates RTD ON RTD.ReferralTSDateID=SM.ReferralTSDateID                
  INNER JOIN DDMaster dm on dm.DDMasterID=sm.CareTypeId                                     
  --inner join EmployeeVisits EV on ev.ScheduleID=sm.ScheduleID                                
  WHERE sm.ScheduleID=@ScheduleID                                
                          
                                  
                          
END
GO

