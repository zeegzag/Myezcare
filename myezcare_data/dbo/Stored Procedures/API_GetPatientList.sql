CREATE PROCEDURE [dbo].[API_GetPatientList]    
 @FromIndex INT,                                              
 @ToIndex INT,                                              
 @SortExpression NVARCHAR(100),                                              
 @SortType NVARCHAR(10),    
 @EmployeeID BIGINT,    
 @StartDate DATE=NULL,    
 @EndDate DATE=NULL,    
 @ServerCurrentDate NVARCHAR(100)    
AS                                              
BEGIN                    
                                            
 IF(@SortExpression IS NULL OR @SortExpression ='')                                              
 BEGIN                                              
  SET @SortExpression = 'FirstName'                                              
  SET @SortType='ASC'                                              
 END                                              
                                               
 ;WITH CTEPatientList AS                                                  
 (                                                       
  SELECT ROW_NUMBER() OVER                                               
      (ORDER BY                                              
       CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'FirstName' THEN T.FirstName END                                              
       END ASC,                                              
       CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'FirstName' THEN T.FirstName END                                              
       END DESC                                      
      ) AS Row,*,COUNT(T.ReferralID) OVER() AS Count                                              
  FROM                                              
  (                                              
   SELECT DISTINCT r.ReferralID,sm.EmployeeID,PatientName=dbo.GetGeneralNameFormat(r.FirstName,r.LastName),r.FirstName,  
 ImageURL=r.ProfileImagePath,c.Email,Phone=c.Phone1,FirstCharOfName=LEFT(R.FirstName, 1)  
 FROM dbo.ScheduleMasters sm                                                              
 INNER JOIN dbo.Referrals r ON sm.ReferralID = r.ReferralID AND r.IsDeleted=0   AND r.ReferralStatusID=1
 INNER JOIN ContactMappings cm ON cm.ReferralID=r.ReferralID AND cm.ContactTypeID=1  
 INNER JOIN Contacts c ON c.ContactID=cm.ContactID AND c.IsDeleted=0
 LEFT JOIN EmployeeVisits ev ON ev.ScheduleID=sm.ScheduleID                                    
 WHERE  
 sm.EmployeeID = @EmployeeId AND CONVERT(DATE,sm.StartDate)>=@ServerCurrentDate AND sm.ScheduleStatusID=2                                                     
 AND (ev.IsSigned=0 OR ev.IsSigned is null)    
 AND ((@StartDate IS NULL AND @EndDate IS NULL) OR (@StartDate IS NULL AND CONVERT(DATE,sm.EndDate)<=@EndDate) OR       
 (@EndDate IS NULL AND CONVERT(DATE,sm.StartDate)>=@StartDate) OR (CONVERT(DATE,sm.StartDate) >=@StartDate AND CONVERT(DATE,sm.EndDate) <= @EndDate ))    
 
 UNION
 SELECT DISTINCT r.ReferralID,e.EmployeeID,PatientName=dbo.GetGeneralNameFormat(r.FirstName,r.LastName),r.FirstName,  
 ImageURL=r.ProfileImagePath,c.Email,Phone=c.Phone1,FirstCharOfName=LEFT(R.FirstName, 1)  
 FROM dbo.Referrals r INNER JOIN employees e ON r.Assignee=e.EmployeeID AND r.IsDeleted=0  AND r.ReferralStatusID=1
 INNER JOIN ContactMappings cm ON cm.ReferralID=r.ReferralID AND cm.ContactTypeID=1  
 INNER JOIN Contacts c ON c.ContactID=cm.ContactID AND c.IsDeleted=0
 WHERE e.EmployeeID=@employeeid

 UNION
 SELECT DISTINCT r.ReferralID,e.EmployeeID,PatientName=dbo.GetGeneralNameFormat(r.FirstName,r.LastName),r.FirstName,  
 ImageURL=r.ProfileImagePath,c.Email,Phone=c.Phone1,FirstCharOfName=LEFT(R.FirstName, 1)  
 FROM referrals r INNER JOIN  dbo.ReferralCaseloads rc ON rc.ReferralID = r.ReferralID AND r.IsDeleted=0  AND r.ReferralStatusID=1
 INNER JOIN dbo.Employees e ON rc.EmployeeID = e.EmployeeID  
 INNER JOIN ContactMappings cm ON cm.ReferralID=r.ReferralID AND cm.ContactTypeID=1  
 INNER JOIN Contacts c ON c.ContactID=cm.ContactID AND c.IsDeleted=0
 WHERE e.EmployeeID=@employeeid
 
 
 
 ) T  
    
 )                                              
                                                 
  SELECT distinct * FROM CTEPatientList WHERE ROW BETWEEN @FromIndex AND @ToIndex                                              
                                                
                                                
END
