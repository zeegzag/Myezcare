-- EXEC GetEmployeeList @Name = '', @SortExpression = 'Name', @SortType = 'DESC', @FromIndex = '1', @PageSize = '100'          
CREATE PROCEDURE [dbo].[GetSentSMSList]            
 @Message VARCHAR(100) = NULL,          
 @NotificationSID VARCHAR(100) = NULL,  
 @SortExpression NVARCHAR(100),            
 @SortType NVARCHAR(10),          
 @FromIndex INT,          
 @PageSize INT      
AS          
BEGIN 
DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()
 ;WITH CTGroupMessageList AS          
 (           
  SELECT *,COUNT(t1.GroupMessageLogID) OVER() AS Count FROM           
  (          
   SELECT ROW_NUMBER() OVER (ORDER BY           
          
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'GroupMessageLogID' THEN GroupMessageLogID END END ASC,          
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'GroupMessageLogID' THEN GroupMessageLogID END END DESC,  
   
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Message' THEN Message END END ASC,          
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Message' THEN Message END END DESC,  
   
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'NotificationSid' THEN NotificationSid END END ASC,          
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'NotificationSid' THEN NotificationSid END END DESC,  
   
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'SentBy' THEN E.LastName END END ASC,          
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'SentBy' THEN E.LastName END END DESC,  
   
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'SentBy' THEN E.LastName END END ASC,          
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'SentBy' THEN E.LastName END END DESC,  
   
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'SentDate' THEN CONVERT(datetime, GM.CreatedDate, 103) END END ASC,  
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'SentDate' THEN CONVERT(datetime, GM.CreatedDate, 103) END END DESC  
        
          
  ) AS Row,          
    
   GM.GroupMessageLogID,GM.EmployeeIDs,GM.Message, GM.NotificationSID, SentDate=GM.CreatedDate, EmpFirstName=E.FirstName, EmpLastName=E.LastName ,
   SentBy=dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat)
  
   FROM  GroupMessageLogs GM           
   INNER JOIN Employees E ON E.EmployeeID=GM.CreatedBy  
   WHERE  
   ((@Message IS NULL OR LEN(@Message)=0) OR GM.Message LIKE '%' + @Message + '%') AND  
   ((@NotificationSID IS NULL OR LEN(@NotificationSID)=0) OR GM.NotificationSID LIKE '%' + @NotificationSID + '%')          
      
  ) AS t1  )          
     
 SELECT * FROM CTGroupMessageList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)           
          
END  
