--EXEC GetEmployeeVisitList @EmployeeVisitID = '0',              
  --@EmployeeIDs = null, @ReferralIDs = null, @StartTime = '14:15:00', @EndTime = '', --@StartDate = '', @EndDate = '',               
 -- @IsDeleted = '0', @SortExpression = 'EmployeeVisitID', @SortType = 'ASC', @FromIndex = '1', @PageSize = '100'                      
CREATE PROCEDURE [dbo].[GetEmployeeVisitList]                    
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
                      
AS                                                      
BEGIN                                                          
 ;WITH CTEEmployeeVisitList AS                                                      
 (                                                       
  SELECT *,COUNT(t1.EmployeeVisitID) OVER() AS Count FROM                                                       
  (                                                      
   SELECT ROW_NUMBER() OVER (ORDER BY                                                       
                                               
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'EmployeeVisitID' THEN EmployeeVisitID END END ASC,                                                      
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'EmployeeVisitID' THEN EmployeeVisitID END END DESC,                                                      
                                                   
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Name' THEN e.LastName END END ASC,                                                      
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Name' THEN e.LastName END END DESC,                                       
        
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'PCACompletedQ' THEN ev.IsPCACompleted END END ASC,                                                      
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'PCACompletedQ' THEN ev.IsPCACompleted END END DESC,                                       
        
        
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'PatientName' THEN r.LastName END END ASC,                                                      
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'PatientName' THEN r.LastName END END DESC,                                  
                                       
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ClockInTime' THEN ClockInTime END END ASC,                                                      
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ClockInTime' THEN ClockInTime END END DESC,                                                       
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ClockOutTime' THEN ClockOutTime END END ASC,                                                      
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ClockOutTime' THEN ClockOutTime END END DESC,                                                       
                                                      
    CASE WHEN @SortType = 'ASC' THEN                                                      
      CASE                     
      WHEN @SortExpression = 'StartDate' THEN sm.StartDate                                         
      END         
    END ASC,                                                      
CASE WHEN @SortType = 'DESC' THEN                                                      
      CASE                                                       
      WHEN @SortExpression = 'StartDate' THEN sm.StartDate                                                      
      END                                                      
    END DESC,                                  
 CASE WHEN @SortType = 'ASC' THEN                  
      CASE                                                       
      WHEN @SortExpression = 'EndDate' THEN sm.EndDate                                  
      END                                                       
    END ASC,                                                      
    CASE WHEN @SortType = 'DESC' THEN                                    
      CASE                                                       
      WHEN @SortExpression = 'EndDate' THEN sm.EndDate                                  
      END                                                      
    END DESC                         
  ) AS Row,                         
                                   
   ev.EmployeeVisitID, e.EmployeeID,Name=dbo.GetGeneralNameFormat(e.FirstName,e.LastName) ,r.ReferralID,                        
   PatientName=dbo.GetGeneralNameFormat(r.FirstName,r.LastName) ,CONVERT(TIME(3),ev.ClockInTime) AS ClockInTime,CONVERT(TIME(3),ev.ClockOutTime) AS ClockOutTime,ev.IsDeleted,sm.StartDate,sm.EndDate,ev.IsByPassClockIn,ev.IsByPassClockOut,ev.ByPassReasonClockIn,ev.ByPassReasonClockOut,ev.ActionTaken,ev.RejectReason, ev.IsApprovalRequired,ev.SurveyCompleted ,ev.IsPCACompleted,ev.IVRClockIn,ev.IVRClockOut  
   FROM  EmployeeVisits ev                                                    
   INNER JOIN ScheduleMasters sm on sm.ScheduleID=ev.ScheduleID                                  
   LEFT JOIN Employees e on e.EmployeeID=sm.EmployeeID                                  
   INNER JOIN Referrals r on r.ReferralID=sm.ReferralID                                  
   WHERE ((CAST(@IsDeleted AS BIGINT)=-1) OR ev.IsDeleted=@IsDeleted)      
   AND ((@ActionTaken=0) OR ev.ActionTaken=@ActionTaken)
   and (ev.ClockInTime is not null or ev.ClockInTime = '')                
   --Search By Time                  
   --AND ((@StartTime IS NULL OR LEN(@StartTime)=0) OR FORMAT(ev.ClockInTime,'HH:mm:00') LIKE '%' + @StartTime + '%')                      
   --AND ((@EndTime IS NULL OR LEN(@EndTime)=0) OR FORMAT(ev.ClockOutTime,'HH:mm:00') LIKE '%' + @EndTime + '%')                  
               
  AND ((@StartTime IS NULL OR LEN(@StartTime)=0) OR format(ev.ClockInTime,'HH:mm:00') >=  format(CONVERT(datetime,@StartTime),'HH:mm:00'))                  
   AND ((@EndTime IS NULL OR LEN(@EndTime)=0) OR format(ev.ClockOutTime,'HH:mm:00')  <= format(CONVERT(datetime,@EndTime),'HH:mm:00'))                  
                 
   AND ((@StartDate IS NULL OR LEN(@StartDate)=0) OR CONVERT(DATE,sm.StartDate) >= @StartDate)                  
   AND ((@EndDate IS NULL OR LEN(@EndDate)=0) OR CONVERT(DATE,sm.EndDate) <= @EndDate)                  
 --AND ((@StartDate IS NULL OR LEN(@StartDate)=0) OR CONVERT(VARCHAR(20),sm.StartDate,120) LIKE '%' + CONVERT(VARCHAR(20),@StartDate) + '%')                  
 --  AND ((@EndDate IS NULL OR LEN(@EndDate)=0) OR CONVERT(VARCHAR(20),sm.EndDate,120) LIKE '%' + CONVERT(VARCHAR(20),@EndDate) + '%')                  
  -- AND ((@StartDate IS NULL OR LEN(@StartDate)=0) OR (@EndDate IS NULL OR LEN(@EndDate)=0) OR                      
  --(CONVERT(Date,sm.StartDate) > CONVERT(DATE,@StartDate)) AND (CONVERT(Date,sm.EndDate) < CONVERT(DATE,@EndDate)))                      
   AND ((@EmployeeIDs IS NULL OR LEN(@EmployeeIDs)=0) OR (e.EmployeeID in (SELECT CONVERT(BIGINT,VAL) FROM GetCSVTable(@EmployeeIDs))))                        
   AND ((@ReferralIDs IS NULL OR LEN(@ReferralIDs)=0) OR (r.ReferralID in (SELECT CONVERT(BIGINT,VAL) FROM GetCSVTable(@ReferralIDs)))                   
                        
   )                        
                                 
  ) AS t1  )                    
                                                 
 SELECT * FROM CTEEmployeeVisitList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                                
                                                      
END
