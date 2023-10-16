-- EXEC GetVisitTaskList @ServiceCodeID = '0', @IsDeleted = '0', @SortExpression = 'VisitTaskType', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50'
CREATE PROCEDURE [dbo].[GetBlockEmpList]
 @ReferralID NVARCHAR(100), 
 @SortExpression NVARCHAR(100),          
 @SortType NVARCHAR(10),        
 @FromIndex INT,        
 @PageSize INT        
AS        
BEGIN        
 ;WITH CTEGetBlockEmpList AS        
 (         
  SELECT *,COUNT(T1.ReferralBlockedEmployeeID) OVER() AS Count FROM         
  (        
   SELECT ROW_NUMBER() OVER (ORDER BY         
   CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ReferralBlockedEmployeeID' THEN ReferralBlockedEmployeeID END END ASC,        
   CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ReferralBlockedEmployeeID' THEN ReferralBlockedEmployeeID END END DESC,   
   
   
   CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Employee' THEN Employee END END ASC,        
   CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Employee' THEN Employee END END DESC,   

   CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'BlockingReason' THEN BlockingReason END END ASC,        
   CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'BlockingReason' THEN BlockingReason END END DESC,   

   CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'BlockingRequestedBY' THEN BlockingRequestedBY END END ASC,        
   CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'BlockingRequestedBY' THEN BlockingRequestedBY END END DESC,   

   CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CreatedDate' THEN CONVERT(DateTime, CreatedDate,101) END END ASC,        
   CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CreatedDate' THEN CONVERT(DateTime, CreatedDate,101) END END DESC  
            
        
  ) AS Row, * FROM (
         
   SELECT Employee=E.FirstName +' ' + E.LastName, RB.*
   FROM  ReferralBlockedEmployees RB
   INNER JOIN Employees E ON E.EmployeeID=RB.EmployeeID
   WHERE RB.IsDeleted=0
   AND  RB.ReferralID=@ReferralID

   ) AS TEMP
  ) AS T1          
 )        
         
 SELECT * FROM CTEGetBlockEmpList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)         
END 
