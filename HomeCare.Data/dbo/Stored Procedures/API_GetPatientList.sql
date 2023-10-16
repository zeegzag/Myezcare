--EXEC API_GetPatientList @FromIndex = N'1', @ToIndex = N'100', @SortType = N'string', @SortExpression = N'string', @ServerCurrentDate = N'2023-04-05', @EmployeeID = N'64'    
    
    
/* script */          
          
CREATE PROCEDURE [dbo].[API_GetPatientList]                
     
 @FromIndex INT,                                                          
 @ToIndex INT,                                                          
 @SortExpression NVARCHAR(100),                                                          
 @SortType NVARCHAR(10),                
 @EmployeeID BIGINT,                
 @StartDate DATE=NULL,                
 @EndDate DATE=NULL,                
 @ServerCurrentDate NVARCHAR(100)       
 as    
    
    
BEGIN     
DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat() 
 set @ToIndex=500                                                       
 IF(@SortExpression IS NULL OR @SortExpression ='')                                                          
 BEGIN                                                          
  SET @SortExpression = 'FirstName'                                                          
  SET @SortType='ASC'                                                          
 END                                                          
 Declare @Cnt int            
  Declare @Count int            
  Declare  @PatientGroupLeveCnt int     
          
SELECT @Count=Count(*)             
FROM            Permissions INNER JOIN            
                         RolePermissionMapping ON Permissions.PermissionID = RolePermissionMapping.PermissionID INNER JOIN            
                         Employees AS e ON RolePermissionMapping.RoleID = e.RoleID            
WHERE        (e.EmployeeID = @EmployeeID and Permissions.PermissionCode='Record_Access_All_Record' and  RolePermissionMapping.isDeleted=0)            
            
select @PatientGroupLeveCnt=count(*)             
FROM            Permissions INNER JOIN            
                         RolePermissionMapping ON Permissions.PermissionID = RolePermissionMapping.PermissionID INNER JOIN            
                         Employees AS e ON RolePermissionMapping.RoleID = e.RoleID  inner join roles on roles.RoleID=RolePermissionMapping.RoleID          
WHERE        (e.EmployeeID = @EmployeeID and Permissions.PermissionCode='PatientRecord_Access_Group_And_Limited_Record' and  RolePermissionMapping.isDeleted=0)                  
                                                           
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
   SELECT DISTINCT r.ReferralID,sm.EmployeeID,PatientName=dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat),r.FirstName,              
 ImageURL=r.ProfileImagePath,c.Email,Phone=c.Phone1,FirstCharOfName=LEFT(R.FirstName, 1)              
 FROM dbo.ScheduleMasters sm                                                                          
 INNER JOIN dbo.Referrals r ON sm.ReferralID = r.ReferralID AND r.IsDeleted=0   AND r.ReferralStatusID=1            
 INNER JOIN ContactMappings cm ON cm.ReferralID=r.ReferralID AND cm.ContactTypeID=1              
 INNER JOIN Contacts c ON c.ContactID=cm.ContactID AND c.IsDeleted=0            
 LEFT JOIN EmployeeVisits ev ON ev.ScheduleID=sm.ScheduleID                                                
 WHERE              
 sm.EmployeeID = @EmployeeId AND CONVERT(DATE,sm.StartDate)>=@ServerCurrentDate AND sm.ScheduleStatusID=2  AND ISNULL(SM.OnHold, 0) = 0                                                                 
 AND (ev.IsSigned=0 OR ev.IsSigned is null)                
 AND ((@StartDate IS NULL AND @EndDate IS NULL) OR (@StartDate IS NULL AND CONVERT(DATE,sm.EndDate)<=@EndDate) OR                   
 (@EndDate IS NULL AND CONVERT(DATE,sm.StartDate)>=@StartDate) OR (CONVERT(DATE,sm.StartDate) >=@StartDate AND CONVERT(DATE,sm.EndDate) <= @EndDate ))                
             
 UNION            
 SELECT DISTINCT r.ReferralID,e.EmployeeID,PatientName=dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat),r.FirstName,              
 ImageURL=r.ProfileImagePath,c.Email,Phone=c.Phone1,FirstCharOfName=LEFT(R.FirstName, 1)              
 FROM dbo.Referrals r INNER JOIN employees e ON r.Assignee=e.EmployeeID AND r.IsDeleted=0  AND r.ReferralStatusID=1            
 INNER JOIN ContactMappings cm ON cm.ReferralID=r.ReferralID AND cm.ContactTypeID=1              
 INNER JOIN Contacts c ON c.ContactID=cm.ContactID AND c.IsDeleted=0            
 WHERE e.EmployeeID=@employeeid            
            
 UNION            
 SELECT DISTINCT r.ReferralID,e.EmployeeID,PatientName=dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat),r.FirstName,              
 ImageURL=r.ProfileImagePath,c.Email,Phone=c.Phone1,FirstCharOfName=LEFT(R.FirstName, 1)              
 FROM referrals r INNER JOIN  dbo.ReferralCaseloads rc ON rc.ReferralID = r.ReferralID AND r.IsDeleted=0  AND r.ReferralStatusID=1            
 INNER JOIN dbo.Employees e ON rc.EmployeeID = e.EmployeeID              
 INNER JOIN ContactMappings cm ON cm.ReferralID=r.ReferralID AND cm.ContactTypeID=1              
 INNER JOIN Contacts c ON c.ContactID=cm.ContactID AND c.IsDeleted=0            
 WHERE e.EmployeeID=@employeeid            
             
  UNION            
 SELECT DISTINCT r.ReferralID,e.EmployeeID,PatientName=dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat),r.FirstName,              
 ImageURL=r.ProfileImagePath,c.Email,Phone=c.Phone1,FirstCharOfName=LEFT(R.FirstName, 1)              
 FROM referrals r             
 INNER JOIN dbo.Employees e ON  e.EmployeeID  =r.casemanagerID            
 INNER JOIN ContactMappings cm ON cm.ReferralID=r.ReferralID AND cm.ContactTypeID=1              
 INNER JOIN Contacts c ON c.ContactID=cm.ContactID AND c.IsDeleted=0            
 WHERE e.EmployeeID=@EmployeeID AND r.IsDeleted=0  AND r.ReferralStatusID=1            
             
 UNION            
 SELECT Top (Case @Count when 0 then 0 Else 1000 End)              
 r.ReferralID, @EmployeeID EmployeeID,PatientName=dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat),r.FirstName,              
 ImageURL=r.ProfileImagePath,c.Email,Phone=c.Phone1,FirstCharOfName=LEFT(R.FirstName, 1)              
             
 FROM referrals r             
 INNER JOIN ContactMappings cm ON cm.ReferralID=r.ReferralID AND cm.ContactTypeID=1              
 INNER JOIN Contacts c ON c.ContactID=cm.ContactID AND c.IsDeleted=0            
 WHERE             
 r.IsDeleted=0  AND r.ReferralStatusID=1             
 UNION            
 SELECT DISTINCT r.ReferralID,e.EmployeeID,PatientName=dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat),r.FirstName,              
 ImageURL=r.ProfileImagePath,c.Email,Phone=c.Phone1,FirstCharOfName=LEFT(R.FirstName, 1)              
 FROM referrals r             
 inner join ReferralGroup RG on RG.ReferralID=r.ReferralID            
 inner join EmployeeGroup EG on RG.val=EG.val           
 INNER JOIN dbo.Employees e ON  e.EmployeeID  =eg.employeeID            
 INNER JOIN ContactMappings cm ON cm.ReferralID=r.ReferralID AND cm.ContactTypeID=1              
 INNER JOIN Contacts c ON c.ContactID=cm.ContactID AND c.IsDeleted=0            
 WHERE e.EmployeeID=@EmployeeID AND r.IsDeleted=0  AND r.ReferralStatusID=1        and @PatientGroupLeveCnt>0    
 ) T              
                
 )                                                          
             
  SELECT distinct * FROM CTEPatientList WHERE ROW BETWEEN @FromIndex AND @ToIndex   order by patientName                                                       
                                                            
                                                            
END 