  
  
-- =============================================  
-- Author:  Ashar A  
-- Create date: 27 Oct 2021  
-- Description: <Description,,>  
-- =============================================  
CREATE PROCEDURE [dbo].[API_SaveGroupTimesheetList]  
 @Remarks NVARCHAR(MAX) = NULL  
 ,@SystemID NVARCHAR(MAX) = NULL  
 ,@TimesheetDetails [dbo].[GroupTimesheet] READONLY  
 ,@TaskList [dbo].[GroupTask] READONLY  
AS  
BEGIN  
 DECLARE @Output TABLE (EmployeeVisitID BIGINT);  
  
 INSERT INTO EmployeeVisits (  
  ScheduleID  
  ,ClockInTime  
  ,ClockOutTime  
  ,SystemID  
  ,CreatedDate  
  ,UpdatedDate  
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
  ,GETUTCDATE()  
  ,@Remarks,  
  1,  
  1,  
  2  
 FROM @TimesheetDetails td  
  
 INSERT INTO EmployeeVisitNotes (  
  EmployeeVisitID  
  ,ServiceTime  
  ,CreatedDate  
  ,UpdatedDate  
  ,SystemID  
  ,ReferralTaskMappingID  
  ,AlertComment  
  )  
 SELECT EV.EmployeeVisitID  
  ,TL.ServiceTime  
  ,GETDATE()  
  ,GETDATE()  
  ,@SystemID  
  ,RTM.ReferralTaskMappingID  
  ,TL.Remarks  
 FROM @Output O  
 INNER JOIN EmployeeVisits EV ON EV.EmployeeVisitID = O.EmployeeVisitID  
 INNER JOIN ScheduleMasters SM ON EV.ScheduleID = SM.ScheduleID  
 INNER JOIN ReferralTaskMappings RTM ON RTM.ReferralID = SM.ReferralID AND RTM.IsDeleted=0     
 INNER JOIN @TaskList TL ON RTM.VisitTaskID = TL.VisitTaskID  
END