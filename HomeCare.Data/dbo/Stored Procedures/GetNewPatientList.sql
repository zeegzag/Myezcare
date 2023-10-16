CREATE PROCEDURE [dbo].[GetNewPatientList]      
@StartDate DATE =NULL,                          
@EndDate  DATE=NULL,       
@SortExpression NVARCHAR(100),                          
@SortType NVARCHAR(10),                        
@FromIndex INT,                        
@PageSize INT            
             
AS                        
BEGIN                        
  DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()       
;WITH CTEGetEmpClockInOutList AS                        
 (                         
  SELECT *,COUNT(T1.ReferralID) OVER() AS Count FROM                         
  (                        
   SELECT ROW_NUMBER() OVER (ORDER BY                         
                    
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Patient' THEN t.LastName END END ASC,                  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Patient' THEN t.LastName END END DESC,                   
        
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CreatedDate' THEN  CONVERT(date, t.CreatedDate, 105) END END ASC,                                        
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CreatedDate' THEN  CONVERT(date, t.CreatedDate, 105) END END DESC,                  
        
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CreatedBy' THEN t.EmpFirstName END END ASC,                  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CreatedBy' THEN t.EmpFirstName END END DESC        
                
   ) AS ROW,                        
   t.*  FROM     (              
        
        
SELECT DISTINCT R.ReferralID, R.FirstName, R.LastName,Patient= dbo.GetGenericNameFormat(R.FirstName,R.MiddleName, R.LastName,@NameFormat),
R.CreatedDate, EmpFirstName=E.FirstName, EmpLastName=E.LastName ,CreatedBy=dbo.GetGenericNameFormat(E.FirstName,E.MiddleName, E.LastName,@NameFormat)     
FROM Referrals R         
INNER JOIN Employees E ON E.EmployeeID=R.CreatedBy        
LEFT JOIN ScheduleMasters SM ON SM.ReferralID=R.ReferralID AND ISNULL(SM.OnHold, 0) = 0 AND SM.IsDeleted=0         
LEFT JOIN ReferralOnHoldDetails ROH ON ROH.ReferralID=R.ReferralID      
WHERE R.IsDeleted=0 AND R.ReferralStatusID=1 AND SM.ScheduleID IS NULL  AND ROH.ReferralOnHoldDetailID IS NULL AND    
R.CreatedDate>=@StartDate AND R.CreatedDate<=@EndDate    
        
) AS T        
        
)  AS T1 )            
            
SELECT * FROM CTEGetEmpClockInOutList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                         
END  