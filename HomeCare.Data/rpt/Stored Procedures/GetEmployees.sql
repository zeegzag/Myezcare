  
CREATE PROCEDURE [rpt].[GetEmployees]   
--@orgID int =null, @dbname varchar(4000)=null,@domainName varchar(4000)=null  
AS  
BEGIN  
DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat() 
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
 --select e.EmployeeID,e.LastName+', '+E.FirstName from Employees E  
 WITH CTE AS (  
 SELECT 0 AS EmployeeID,'All' AS EmployeeName  
 UNION ALL  
 SELECT DISTINCT EmployeeID,dbo.GetGenericNameFormat(FirstName,MiddleName, LastName,@NameFormat) AS EmployeeName   
 FROM dbo.Employees e where IsDeleted=0)  
 SELECT * FROM CTE  
 WHERE EmployeeName IS NOT NULL  
 ORDER BY CASE WHEN EmployeeName = 'All' THEN '0'  
  ELSE EmployeeName END ASC;  
END  