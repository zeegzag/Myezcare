CREATE PROCEDURE [dbo].[HC_GetPendingScheduleList]    
 @PatientName VARCHAR(100),                    
 @EmployeeID BIGINT=0,    
 @StartDate DATE=null,                    
 @EndDate DATE=null,       
 @IsDeleted BIGINT = -1,              
 @SortExpression VARCHAR(100),                      
 @SortType VARCHAR(10),                    
 @FromIndex INT,                    
 @PageSize INT    
AS                    
BEGIN          
  DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()                
 ;WITH CTEPendingScheduleList AS                    
 (                     
 SELECT *,COUNT(t1.PendingScheduleID) OVER() AS Count FROM                     
  (                    
   SELECT ROW_NUMBER() OVER (ORDER BY                    
                  
     CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'StartDate' THEN CONVERT(datetime, PS.ClockInTime, 103) END END ASC,                    
     CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'StartDate' THEN CONVERT(datetime, PS.ClockInTime, 103) END END DESC,                    
                    
     CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'EndDate' THEN CONVERT(datetime, PS.ClockOutTime, 103) END END ASC,                    
     CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'EndDate' THEN CONVERT(datetime, PS.ClockOutTime, 103) END END DESC,                    
    
  CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'AddedDate' THEN CONVERT(datetime, PS.CreatedDate, 103) END END ASC,                    
     CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'AddedDate' THEN CONVERT(datetime, PS.CreatedDate, 103) END END DESC,                    
                    
     CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'PatientName' THEN R.LastName END END ASC,                    
     CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'PatientName' THEN R.LastName END END DESC,                    
                  
     CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'EmplyeName' THEN E.LastName END END ASC,                    
     CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'EmplyeName' THEN E.LastName END END DESC,        
    
  CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'AddedBy' THEN CE.LastName END END ASC,                    
     CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'AddedBy' THEN CE.LastName END END DESC,        
        
  CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'PendingScheduleID' THEN  PS.PendingScheduleID  END END ASC,                
  CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'PendingScheduleID' THEN  PS.PendingScheduleID END END DESC                 
  ) AS Row,    
    
    PS.PendingScheduleID, PS.ClockInTime, PS.ClockOutTime, PS.CreatedDate,PS.IsDeleted, PS.ScheduleID,    
    PatientName=dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat), EmployeeName=dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat),    
 CreatedBy=dbo.GetGenericNameFormat(ce.FirstName,ce.MiddleName, ce.LastName,@NameFormat)    
                    
    FROM PendingSchedules PS                    
    INNER JOIN Referrals R ON R.ReferralID=PS.ReferralID                  
    INNER JOIN Employees E ON E.EmployeeID=PS.EmployeeID    
 INNER JOIN Employees CE ON CE.EmployeeID=PS.CreatedBy    
   WHERE                   
   ((CAST(@IsDeleted AS BIGINT)=-1) OR PS.IsDeleted=@IsDeleted)       
   AND (@EmployeeID=0 OR E.EmployeeID=@EmployeeID)                
   AND ((@PatientName IS NULL OR LEN(R.LastName)=0)                
   OR                
      (      
     (R.FirstName LIKE '%'+@PatientName+'%' )OR                  
     (R.LastName  LIKE '%'+@PatientName+'%') OR                  
     (R.FirstName +' '+R.LastName like '%'+@PatientName+'%') OR                  
     (R.LastName +' '+R.FirstName like '%'+@PatientName+'%') OR                  
     (R.FirstName +', '+R.LastName like '%'+@PatientName+'%') OR                  
     (R.LastName +', '+ R.FirstName like '%'+@PatientName+'%'))          
      )                    
   AND ((@StartDate is null OR Convert(Date,PS.ClockInTime) >= @StartDate) and   
   (@EndDate is null OR  Convert(Date,PS.ClockOutTime) <= @EndDate))                    
             
  ) AS t1                      
 )                     
 SELECT * FROM CTEPendingScheduleList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                     
                    
END  