  
  --CreatedBy              CreatedDate      UpdatedDate    description    
  
        --Akhilesh                20-6-2019            ""          Get DMAS FORMS   
  
    
  
-- exec DMAS_90Forms '','','4276','','','','','0','0','','ASC','1',50  
  
    
  
CREATE PROCEDURE [dbo].[DMAS_90Forms]      
  
  
 @EmployeeVisitID BIGINT = 0,                                      
 @EmployeeIDs NVARCHAR(500) = NULL,                                      
 @ReferralIDs NVARCHAR(500) = NULL,                                      
 @StartDate DATE = NULL,                                                          
 @EndDate DATE = NULL,                                
 @StartTime VARCHAR(20)=NULL,                                
 @EndTime VARCHAR(20)=NULL,                                
 @IsDeleted int=-1,          
 @ActionTaken int=0,          
 @SortExpression NVARCHAR(100),                                                            
 @SortType NVARCHAR(10),                                                          
 @FromIndex INT,                                                          
 @PageSize INT   
--@ReferralID int  
  
AS      
  
BEGIN     
;WITH CTEVisitTaskList AS  
 (                                                           
  SELECT *,COUNT(t1.VisitTaskID) OVER() AS Count FROM                                                           
  (  
  
   SELECT ROW_NUMBER() OVER (ORDER BY                                                           
                                                   
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'EmployeeVisitID' THEN EmployeeVisitID END END ASC,                                                          
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'EmployeeVisitID' THEN EmployeeVisitID END END DESC   
                                                                                      
  ) AS Row,    
  
 vt.VisitTaskID,vt.VisitTaskType,vt.VisitTaskDetail,vt.CreatedDate,PatientName=dbo.GetGeneralNameFormat(r.FirstName,r.LastName),DateName(WEEKDAY,vt.CreatedDate) as [CreatedDay],ev.EmployeeVisitID,e.EmployeeID,r.ReferralID,  
  
 CONVERT(TIME(3),ev.ClockInTime) AS ClockInTime,CONVERT(TIME(3),ev.ClockOutTime) AS ClockOutTime,sm.StartDate,sm.EndDate,ev.SurveyCompleted,ev.SurveyComment,ev.IsByPassClockIn,ev.IsByPassClockOut,ev.ByPassReasonClockIn,ev.ByPassReasonClockOut,  
  
  ev.PlaceOfService,ev.OtherActivity,ev.IsPCACompleted,ev.EmployeeSignatureID,ev.PatientSignature,ev.IVRClockIn,ev.IVRClockOut,ev.EarlyClockOutComment  
  
 from VisitTasks vt  
  
 INNER JOIN ReferralTaskMappings rtm ON vt.VisitTaskID=rtm.VisitTaskID  
  
 INNER JOIN Referrals r ON rtm.ReferralID=r.ReferralID  
  
 inner join ScheduleMasters sm on sm.ReferralID=r.ReferralID  
  
 inner join Employees e on e.EmployeeID=sm.EmployeeID  
  
 inner join EmployeeVisits ev on ev.ScheduleID=sm.ScheduleID  
  
where r.IsDeleted=0 and vt.IsDeleted=0 and e.IsDeleted=0 and sm.IsDeleted=0 and rtm.IsDeleted=0 and vt.VisitTaskType='task'  
  
 AND ((@ReferralIDs IS NULL OR LEN(@ReferralIDs)=0) OR (r.ReferralID in (SELECT CONVERT(BIGINT,VAL) FROM GetCSVTable(@ReferralIDs)))                            
   )  
  ) AS t1)          
  SELECT * FROM CTEVisitTaskList  WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)   
END