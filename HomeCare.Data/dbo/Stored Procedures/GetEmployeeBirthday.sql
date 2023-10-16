    
--  EXEC GetEmployeeBirthday '2021-04-10', '2021-07-10', 'Birthday', 'ASC', 1, 10      
      
CREATE PROCEDURE [dbo].[GetEmployeeBirthday]           
@StartDate DATE = NULL,                            
@EndDate  DATE = NULL,       
@SortExpression NVARCHAR(100),                              
@SortType NVARCHAR(10),                            
@FromIndex INT,                            
@PageSize INT                
                
AS                            
BEGIN                            
 DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()                           
;WITH List AS                            
 (                             
  SELECT *,COUNT(T1.EmployeeID) OVER() AS Count FROM                             
  (                            
   SELECT ROW_NUMBER() OVER (ORDER BY                             
                        
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Employee' THEN t.LastName END END ASC,                      
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Employee' THEN t.LastName END END DESC,         
     
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Birthday' THEN t.DateOfBirth END END ASC,                      
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Birthday' THEN t.DateOfBirth END END DESC       
                    
   ) AS ROW,                            
   t.*  FROM     (                  
            
  SELECT        
  E.EmployeeID, E.FirstName, E.LastName, EmployeeName = dbo.GetGenericNameFormat(E.FirstName,E.MiddleName, E.LastName,@NameFormat),              
  FORMAT (E.DateOfBirth, 'dd, MMMM') as DateOfBirth,    
  ISNULL(E.MobileNumber, 'NA') AS [Phone]      
   FROM (      
SELECT  CAST(CONCAT(RIGHT('0' + RTRIM(MONTH(DateOfBirth)), 2),'-',RIGHT('0' + RTRIM(DAY(DateOfBirth)), 2)) as varchar) as cdob, * FROM Employees where DateOfBirth is not null       
) E      
       
 WHERE      
 e.cdob >= CAST((SELECT CONCAT(RIGHT('0' + RTRIM(MONTH(@StartDate)), 2),'-',RIGHT('0' + RTRIM(DAY(@StartDate)), 2))) as varchar)       
 AND e.cdob  <=      
  CAST((SELECT CONCAT(RIGHT('0' + RTRIM(MONTH(@EndDate)), 2),'-',RIGHT('0' + RTRIM(DAY(@EndDate)), 2))) as varchar)      
  AND e.IsDeleted = 0      
     GROUP BY E.EmployeeID, E.FirstName,E.MiddleName, E.LastName, E.DateOfBirth, E.MobileNumber      
        
) AS t            
            
)  AS T1 )                
                
SELECT * FROM List WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                             
END 