--EXEC GetNurseSignatureList @LoggedInUserID = '1'      
    
    
      
CREATE PROCEDURE [dbo].[GetNurseSignatureList]       
  @EmployeeIDs NVARCHAR(500) = NULL                  
 ,@ReferralIDs NVARCHAR(500) = NULL                          
 ,@StartDate DATE = NULL                          
 ,@EndDate DATE = NULL                                    
 ,@CareTypeIDs NVARCHAR(500) = NULL    
  ,@StatusId INT = NULL    
 ,@SortExpression NVARCHAR(100)                          
 ,@SortType NVARCHAR(10)                          
 ,@FromIndex INT                          
 ,@PageSize INT ,    
  @LoggedInUserID bigint          
   as       
BEGIN          
          
        
  ;WITH CTEScheduleMasterList                          
 AS (                 
               
  SELECT *                          
   ,COUNT(t1.EmployeeVisitID) OVER () AS Count                          
  FROM (                          
   SELECT ROW_NUMBER() OVER (                          
     ORDER BY CASE                           
       WHEN @SortType = 'ASC'                          
        THEN CASE                           
          WHEN @SortExpression = 'StartDate' THEN CONVERT(NVARCHAR(MAX), sm.StartDate, 103)                      
    WHEN @SortExpression = 'EndDate' THEN CONVERT(NVARCHAR(MAX), sm.EndDate, 103)                  
    WHEN @SortExpression = 'CareType' THEN d.Title                  
    WHEN @SortExpression = 'Name' THEN dbo.GetGeneralNameFormat(E.FirstName, E.LastName)                  
    WHEN @SortExpression = 'PatientName' THEN dbo.GetGeneralNameFormat(r.FirstName, r.LastName)                  
    WHEN @SortExpression = 'EmployeeVisitID' THEN CONVERT(VARCHAR(500),EV.EmployeeVisitID)              
  END                        
       END ASC                  
      ,CASE                           
       WHEN @SortType = 'DESC'                          
        THEN CASE                           
          WHEN @SortExpression = 'StartDate' THEN CONVERT(NVARCHAR(MAX), sm.StartDate, 103)                   
    WHEN @SortExpression = 'EndDate' THEN CONVERT(NVARCHAR(MAX), sm.EndDate, 103)                  
    WHEN @SortExpression = 'CareType' THEN d.Title                  
    WHEN @SortExpression = 'Name' THEN dbo.GetGeneralNameFormat(E.FirstName, E.LastName)                  
    WHEN @SortExpression = 'PatientName' THEN dbo.GetGeneralNameFormat(r.FirstName, r.LastName)                  
    WHEN @SortExpression = 'EmployeeVisitID' THEN CONVERT(VARCHAR(500),EV.EmployeeVisitID)                
  END                     
       END DESC                   
    , CONVERT(NVARCHAR(MAX), sm.StartDate, 103) -- Addtional sort on start date                  
     ) AS Row ,                        
        
    EV.EmployeeVisitID,ev.ActionTaken,ev.IsApprovalRequired,ev.IsEarlyClockIn,ev.EarlyClockInComment,ev.EarlyClockOutComment,ev.IsSigned,  
 ev.IsByPassClockIn,ev.IsByPassClockOut,ev.ByPassReasonClockIn,ev.ByPassReasonClockOut,ev.SurveyCompleted,ev.SurveyComment,ev.IsPCACompleted,  
 E.EmployeeID,  TotalTask.TotalcreatedTask,CASE WHEN TotalTask.TotalcreatedTask >0 AND ev.SurveyCompleted = 1 AND ev.IsPCACompleted = 1 THEN 1 ELSE 0 END AnyActionMissing,  
 [dbo].[GetGeneralNameFormat](E.FirstName,E.LastName) [Name],        
 R.ReferralID,          
 [dbo].[GetGeneralNameFormat](R.FirstName,R.LastName) [PatientName],        
 SM.ScheduleID, SM.StartDate, SM.EndDate,        
 EV.ClockInTime, EV.ClockOutTime,        
 P.PayorID,P.PayorName,        
 D.DDMasterID as CareTypeID,D.Title As CareType,    EV.ApproveNote,    
 SM.ReferralBillingAuthorizationID, RBA.AuthorizationCode,ISNULL(EV.IsApproved, 0) IsApproved,
 [dbo].[GetGeneralNameFormat](ap.FirstName,ap.LastName) ApproveBy
        
  FROM [dbo].[EmployeeVisits] EV        
  INNER JOIN [dbo].[ScheduleMasters] SM ON EV.ScheduleID = SM.ScheduleID        
  INNER JOIN [dbo].[Employees] E ON E.EmployeeID = SM.EmployeeID        
  INNER JOIN [dbo].[Referrals] R ON R.ReferralID = SM.ReferralID        
  LEFT JOIN Payors P on P.PayorID = SM.PayorID          
  LEFT JOIN DDMaster D on D.DDMasterID = SM.CareTypeId   
    LEFT OUTER JOIN Employees ap on EV.UpdatedBy = ap.EmployeeID
  LEFT JOIN ReferralBillingAuthorizations RBA ON RBA.IsDeleted = 0     
 AND RBA.ReferralBillingAuthorizationID = SM.ReferralBillingAuthorizationID AND RBA.ReferralID = SM.ReferralID   
 outer apply(select count(*) TotalcreatedTask from EmployeeVisitNotes evn,ReferralTaskMappings rtm,VisitTasks v where rtm.ReferralTaskMappingID=evn.ReferralTaskMappingID                
and v.VisitTaskID=rtm.VisitTaskID and evn.IsDeleted=0 AND (v.VisitTaskType='Task' OR (evn.ServiceTime>0 AND Description IS NOT NULL))and evn.EmployeeVisitID=ev.EmployeeVisitID) as TotalTask                
  
  WHERE        
 --ISNULL(EV.IsApproved, 1) = 1 AND     
 EV.[Signature] IS NULL       
    
  AND (LEN(@EmployeeIDs) = 0 OR SM.EmployeeID IN ( SELECT EL.[val] FROM dbo.GetCSVTable(@EmployeeIDs) EL) )                    
    AND (LEN(@ReferralIDs) = 0 OR SM.ReferralID IN ( SELECT RL.[val] FROM dbo.GetCSVTable(@ReferralIDs) RL ))                         
    AND ( LEN(@CareTypeIDs) = 0 OR SM.CareTypeID IN (SELECT CL.[val]  FROM dbo.GetCSVTable(@CareTypeIDs) CL))     
 AND (ISNULL(@StatusId,-1) = -1 OR ISNULL(EV.IsApproved, 0) = @StatusId)     
    AND (( @StartDate IS NULL OR (CONVERT(DATE, SM.StartDate) >= @StartDate) )                          
     AND ( @EndDate IS NULL OR (CONVERT(DATE, SM.EndDate) <= @EndDate) ) )     
) AS t1                          
  )                  
               
 SELECT *                          
 FROM CTEScheduleMasterList                          
 WHERE ROW BETWEEN ((@PageSize * (@FromIndex - 1)) + 1) AND (@PageSize * @FromIndex)      
        
END