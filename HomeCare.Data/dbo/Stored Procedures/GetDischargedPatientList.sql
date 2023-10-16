--  EXEC GetDischargedPatientList @StartDate = '2021-05-25', @EndDate = '2021-05-26', @SortExpression = 'Patient', @SortType = 'ASC', @FromIndex = '1', @PageSize = '10'              
CREATE PROCEDURE [dbo].[GetDischargedPatientList]                  
@StartDate DATE =NULL,                                      
@EndDate  DATE=NULL,                   
@SortExpression NVARCHAR(100),                                      
@SortType NVARCHAR(10),                                    
@FromIndex INT,                                    
@PageSize INT                        
                         
AS                                    
BEGIN                                    
 DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()                                    
;WITH CTEGetDischargedPatientList AS                                    
 (                                     
  SELECT *,COUNT(T1.ReferralID) OVER() AS Count FROM                                     
  (                                    
   SELECT ROW_NUMBER() OVER (ORDER BY                                     
                                
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Patient' THEN t.LastName END END ASC,                              
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Patient' THEN t.LastName END END DESC,                               
                    
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CreatedDate' THEN  CONVERT(varchar, t.CreatedDate, 101) END END ASC,                                                    
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CreatedDate' THEN  CONVERT(varchar, t.CreatedDate, 101) END END DESC,                              
                    
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CreatedBy' THEN t.EmpFirstName END END ASC,                              
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CreatedBy' THEN t.EmpFirstName END END DESC                    
                            
   ) AS ROW,                                    
   t.*  FROM     (                          
                    
                    
SELECT DISTINCT R.ReferralID, R.FirstName, R.LastName,Patient = dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat),  
convert(varchar,R.CreatedDate,101) as CreatedDate, 
EmpFirstName=E.FirstName, EmpLastName=E.LastName, CreatedBy = dbo.GetGenericNameFormat(E.FirstName,E.MiddleName, E.LastName,@NameFormat)              
FROM Referrals R                            
INNER JOIN Employees E ON E.EmployeeID=R.CreatedBy                  
WHERE R.IsDeleted=0 AND R.ReferralStatusID=4 AND    
CONVERT(date, R.UpdatedDate, 111)>=@StartDate AND CONVERT(date, R.UpdatedDate, 111)<=@EndDate     
            
) AS T                    
                    
)  AS T1 )                        
                        
SELECT * FROM CTEGetDischargedPatientList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                                     
END  