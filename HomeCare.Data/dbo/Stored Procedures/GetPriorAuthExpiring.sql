CREATE PROCEDURE [dbo].[GetPriorAuthExpiring]      
@SortExpression NVARCHAR(100),                        
@SortType NVARCHAR(10),                      
@FromIndex INT,                      
@PageSize INT ,  
@IsExpired BIT=0  
          
AS                      
BEGIN    
DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()
 IF(@IsExpired=1)  
 BEGIN  
  
    
  
      ;WITH List AS                      
 (                       
  SELECT *,COUNT(T1.ReferralID) OVER() AS Count FROM                       
  (                      
   SELECT ROW_NUMBER() OVER (ORDER BY                       
                  
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Patient' THEN t.LastName END END ASC,                
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Patient' THEN t.LastName END END DESC    
              
   ) AS ROW,                      
   t.*  FROM     (            
      
     SELECT DISTINCT   
  sm.ReferralID, R.FirstName, R.LastName,  Patient = dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat), 					
  count(ev.EmployeeVisitID)as[TotalVisits],  
  rbz.AuthorizationCode,p.PayorName,  
  convert(varchar,rbz.EndDate,101) as[ExpireDate],  
  (count(sm.ScheduleID)-count(ev.EmployeeVisitID))as[RemainingVisit]  
 FROM EmployeeVisits ev  
  inner join ScheduleMasters sm on sm.ScheduleID=ev.ScheduleID  
  inner join Referrals r on r.ReferralID=sm.ReferralID  
  inner join ReferralBillingAuthorizations rbz on rbz.ReferralID=sm.ReferralID  
  inner join Payors p on p.PayorID=rbz.PayorID  
 WHERE  
  cast(rbz.EndDate as date) < [dbo].[GetOrgCurrentDateTime]() and rbz.IsDeleted=0  
  and rbz.ReferralID not in   
  (  
   select ReferralID from ReferralBillingAuthorizations where EndDate > [dbo].[GetOrgCurrentDateTime]()  
  )  
 GROUP BY  
  sm.ReferralID,rbz.AuthorizationCode,p.PayorName,rbz.EndDate,r.FirstName,r.MiddleName,r.LastName   
  
) AS T      
      
)  AS T1 )      
  
SELECT * FROM List WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)    
END   
ELSE  
BEGIN  
 ;WITH List AS                      
 (                       
  SELECT *,COUNT(T1.ReferralID) OVER() AS Count FROM                       
  (                      
   SELECT ROW_NUMBER() OVER (ORDER BY                       
                  
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Patient' THEN t.LastName END END ASC,                
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Patient' THEN t.LastName END END DESC    
              
   ) AS ROW,                      
   t.*  FROM     (            
      
     SELECT DISTINCT  
  sm.ReferralID, R.FirstName, R.LastName, Patient = dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat),   
  count(ev.EmployeeVisitID)as[TotalVisits],  
  rbz.AuthorizationCode,p.PayorName,  
  convert(varchar,rbz.EndDate,101) as[ExpireDate],  
  (count(sm.ScheduleID)-count(ev.EmployeeVisitID))as[RemainingVisit]  
 FROM EmployeeVisits ev  
  inner join ScheduleMasters sm on sm.ScheduleID=ev.ScheduleID  
  inner join Referrals r on r.ReferralID=sm.ReferralID  
  inner join ReferralBillingAuthorizations rbz on rbz.ReferralID=sm.ReferralID  
  inner join Payors p on p.PayorID=rbz.PayorID  
 WHERE  
  cast(rbz.EndDate as date) between [dbo].[GetOrgCurrentDateTime]() and DATEADD(week,1,[dbo].[GetOrgCurrentDateTime]()) and rbz.IsDeleted=0  
 GROUP BY  
  sm.ReferralID,rbz.AuthorizationCode,p.PayorName,rbz.EndDate,r.FirstName,r.MiddleName,r.LastName   
  
) AS T      
      
)  AS T1 )      
  
SELECT * FROM List WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)    
END  
END