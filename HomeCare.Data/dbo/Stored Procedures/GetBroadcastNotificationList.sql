-- EXEC GetBroadcastNotificationList @Message = '', @SortExpression = 'Name', @SortType = 'DESC', @FromIndex = '1', @PageSize = '100'          
CREATE PROCEDURE [dbo].[GetBroadcastNotificationList]            
 @Message VARCHAR(100) = NULL,  
 @SortExpression NVARCHAR(100),  
 @SortType NVARCHAR(10),  
 @FromIndex INT,  
 @PageSize INT  
AS  
BEGIN  
DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()
 ;WITH CTEList AS  
 (  
  SELECT *,COUNT(t1.NotificationId) OVER() AS Count FROM  
  (  
   SELECT ROW_NUMBER() OVER (ORDER BY  
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'NotificationId' THEN mn.NotificationId END END ASC,  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'NotificationId' THEN mn.NotificationId END END DESC,  
  
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Title' THEN mn.Title END END ASC,  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Title' THEN mn.Title END END DESC,  
  
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'NotificationType' THEN mn.NotificationType END END ASC,  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'NotificationType' THEN mn.NotificationType END END DESC,  
  
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'InProgress' THEN mn.InProgress END END ASC,  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'InProgress' THEN mn.InProgress END END DESC,  
  
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CreatedDate' THEN CONVERT(datetime, mn.CreatedDate, 103) END END ASC,  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CreatedDate' THEN CONVERT(datetime, mn.CreatedDate, 103) END END DESC,  
  
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Employee' THEN E.FirstName END END ASC,  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Employee' THEN E.LastName END END DESC  
  
   ) AS Row,mn.NotificationId,mn.Title,mn.NotificationType, mn.InProgress, SentDate=mn.CreatedDate, EmpFirstName=E.FirstName, EmpLastName=E.LastName ,
   SentBy = dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat)
   FROM dbo.Mobile_Notifications mn  
   INNER JOIN Employees E ON E.EmployeeID=mn.CreatedBy  
   WHERE ((@Message IS NULL OR LEN(@Message)=0) OR mn.Title LIKE '%' + @Message + '%')  
  ) AS t1  
 )  
 SELECT * FROM CTEList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)  
END  