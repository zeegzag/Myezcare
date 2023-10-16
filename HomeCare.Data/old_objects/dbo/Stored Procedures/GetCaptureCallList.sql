--   EXEC GetCaptureCallList @IsDeleted = '0', @SortExpression = 'Id', @SortType = 'DESC', @FromIndex = '1', @PageSize = '50'        
                  
CREATE PROCEDURE [dbo].[GetCaptureCallList]                      
@IsDeleted BIT = NULL,              
@loggedInUserId BIGINT = NULL,              
@SortExpression NVARCHAR(100)='',              
@SortType NVARCHAR(10)='DESC',              
@FromIndex INT='1',              
@PageSize INT='50',          
@Email NVARCHAR(100) = NULL,          
@Name NVARCHAR(50) = NULL,          
@Address NVARCHAR(100) = NULL,          
@Contact NVARCHAR(15) = NULL          
                        
AS                                  
BEGIN              
   ;WITH CTECaptureCallList AS(                 
      SELECT *,COUNT(T1.Id) OVER() AS Count FROM(              
     SELECT ROW_NUMBER() OVER (ORDER BY              
     CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Id' THEN Id END END ASC,              
     CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Id' THEN Id END END DESC,              
     CASE WHEN @SortType = 'ASC' THEN              
       CASE              
       WHEN @SortExpression = 'Name' THEN Name              
       WHEN @SortExpression = 'Email' THEN Email              
       WHEN @SortExpression = 'Contact' THEN Contact              
       END                 
     END ASC,                
     CASE WHEN @SortType = 'DESC' THEN                
       CASE                       
       WHEN @SortExpression = 'Name' THEN Name                
       WHEN @SortExpression = 'Email' THEN Email                
       WHEN @SortExpression = 'Contact' THEN Contact                
       END                
     END DESC                
      ) AS Row,              
       * FROM (              
       SELECT CC.Id,CC.LastName + ', ' + CC.FirstName AS Name,CC.Contact,CC.Email,              
       CC.Address,CC.City,CC.ZipCode,CC.StateCode,CC.IsDeleted,CC.Notes,E.LastName+', '+E.FirstName as AssigneeName,CC.RoleIds,CC.EmployeesIDs,CC.CallType,CC.RelatedWithPatient,CC.InquiryDate,CC.FileName,CC.FilePath,CC.OrbeonID,Status--dm.Title as Status         
        
       FROM CaptureCall CC        
    LEFT JOIN Employees E ON E.EmployeeID=CC.CreatedBy   
 LEFT JOIN DDMaster dm on dm.DDMasterID=CC.Status           
       WHERE ((@Email IS NULL OR LEN(@Email)=0) OR CC.Email LIKE '%' + @Email + '%')     
    --AND (Isnull(CC.IsDeleted,1)=1)     
    AND ((@Name IS NULL OR LEN(CC.LastName)=0) OR (              
           (CC.FirstName LIKE '%' + @Name + '%') OR              
           (CC.LastName  LIKE '%' + @Name + '%') OR              
           (CC.FirstName +' '+CC.LastName like '%' + @Name + '%') OR              
           (CC.LastName +' '+CC.FirstName like '%' + @Name + '%')))              
     AND ((@Contact IS NULL OR LEN(@Contact)=0) OR CC.Contact LIKE '%' + @Contact + '%')              
     AND ((@Address IS NULL OR LEN(@Address)=0) OR              
         (CC.Address LIKE '%' + @Address+ '%') OR              
         (CC.City LIKE '%' + @Address+ '%') OR              
         (CC.ZipCode LIKE '%' + @Address+ '%') OR              
         (CC.StateCode LIKE '%' + @Address+ '%'))              
      ) AS T2) AS T1)              
              
   SELECT * FROM CTECaptureCallList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                 
  END 