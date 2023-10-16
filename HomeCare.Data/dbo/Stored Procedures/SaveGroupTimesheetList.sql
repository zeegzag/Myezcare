CREATE PROCEDURE [dbo].[SaveGroupTimesheetList]  
 @Remarks NVARCHAR(MAX) = NULL  
 ,@SystemID NVARCHAR(MAX) = NULL  
 ,@LoggedInID BIGINT = NULL  
 ,@TimesheetDetails [dbo].[GroupTimesheet] READONLY  
 ,@TaskList  [dbo].[GroupTask] READONLY  
AS  
BEGIN  
 DECLARE @Output TABLE (EmployeeVisitID BIGINT);  
  
 INSERT INTO EmployeeVisits (  
  ScheduleID  
  ,ClockInTime  
  ,ClockOutTime  
  ,SystemID  
  ,CreatedDate  
  ,CreatedBy  
  ,UpdatedDate  
  ,UpdatedBy  
  ,SurveyComment,  
  SurveyCompleted,  
  IsPCACompleted,  
  ActionTaken  
  )  
 OUTPUT inserted.EmployeeVisitID INTO @Output(EmployeeVisitID)  
 SELECT td.ScheduleID  
  ,td.ClockInDateTime  
  ,td.ClockOutDateTime  
  ,@SystemID  
  ,GETUTCDATE()  
  ,@LoggedInID  
  ,GETUTCDATE()  
  ,@LoggedInID  
  ,@Remarks,  
  1,  
  1,  
  2  
 FROM @TimesheetDetails td  
  
 INSERT INTO EmployeeVisitNotes (  
  EmployeeVisitID  
  ,ServiceTime  
  ,CreatedDate  
  ,CreatedBy  
  ,UpdatedDate  
  ,UpdatedBy  
  ,SystemID  
  ,ReferralTaskMappingID  
  ,AlertComment
  ,ActivityStatus
  )  
 SELECT EV.EmployeeVisitID  
  ,TL.ServiceTime  
  ,GETDATE()  
  ,@LoggedInID  
  ,GETDATE()  
  ,@LoggedInID  
  ,@SystemID  
  ,RTM.ReferralTaskMappingID  
  ,TL.Remarks
  ,TL.TaskOption
 FROM @Output O  
 INNER JOIN EmployeeVisits EV ON EV.EmployeeVisitID = O.EmployeeVisitID  
 INNER JOIN ScheduleMasters SM ON EV.ScheduleID = SM.ScheduleID  
 INNER JOIN ReferralTaskMappings RTM ON RTM.ReferralID = SM.ReferralID AND RTM.IsDeleted=0     
 INNER JOIN @TaskList TL ON RTM.VisitTaskID = TL.VisitTaskID  
END
GO

