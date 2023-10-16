CREATE PROCEDURE [dbo].[GetEmployeeList]          
 @Name VARCHAR(100) = NULL,        
 @Email VARCHAR(50) = NULL,        
 @DepartmentID BIGINT = 0,        
 @RoleID BIGINT=0,        
 @IsDepartmentSupervisor INT = -1,        
 @Degree varchar(100)=null,      
 @CredentialID varchar(50)=null,  
 @MobileNumber varchar(10)=null,      
 @Address varchar(50)=null,      
 @IsDeleted int=-1,        
 @SortExpression NVARCHAR(100),          
 @SortType NVARCHAR(10),        
 @FromIndex INT,        
 @PageSize INT    ,
 @EmployeeId INT =null      
AS        
BEGIN            
 ;WITH CTEEmployeeList AS        
 (         
  SELECT *,COUNT(t1.EmployeeID) OVER() AS Count FROM         
  (        
   SELECT ROW_NUMBER() OVER (ORDER BY         
        
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'EmployeeID' THEN EmployeeID END END ASC,        
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'EmployeeID' THEN EmployeeID END END DESC,        
  
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'MobileNumber' THEN MobileNumber END END ASC,        
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'MobileNumber' THEN MobileNumber END END DESC,        
        
    CASE WHEN @SortType = 'ASC' THEN        
      CASE         
      WHEN @SortExpression = 'DepartmentName' THEN DepartmentName        
      WHEN @SortExpression = 'Name' THEN LastName        
      WHEN @SortExpression = 'Email' THEN Email        
      WHEN @SortExpression = 'RoleName' THEN RoleName        
      WHEN @SortExpression = 'CredentialName' THEN C.CredentialName        
      WHEN @SortExpression = 'Degree' THEN Degree   
   WHEN @SortExpression = 'Address' THEN E.Address        
      END         
    END ASC,        
    CASE WHEN @SortType = 'DESC' THEN        
      CASE         
      WHEN @SortExpression = 'DepartmentName' THEN DepartmentName        
      WHEN @SortExpression = 'Name' THEN LastName        
      WHEN @SortExpression = 'Email' THEN Email        
      WHEN @SortExpression = 'RoleName' THEN RoleName        
      WHEN @SortExpression = 'CredentialName' THEN C.CredentialName        
      WHEN @SortExpression = 'Degree' THEN Degree  
   WHEN @SortExpression = 'Address' THEN E.Address        
      END        
    END DESC,        
    CASE WHEN @SortType = 'ASC' THEN        
    CASE        
     WHEN @SortExpression = 'IsDepartmentSupervisor' THEN IsDepartmentSupervisor        
    END        
    END ASC,        
    CASE WHEN @SortType = 'DESC' THEN        
    CASE         
     WHEN @SortExpression = 'IsDepartmentSupervisor' THEN IsDepartmentSupervisor        
    END        
    END DESC        
  ) AS Row,        
   e.EmployeeID, dbo.GetGeneralNameFormat(e.FirstName ,e.LastName) as Name,e.UserName,e.Email,e.IsSecurityQuestionSubmitted,        
   e.IsDepartmentSupervisor, d.DepartmentName, R.RoleName,e.IsDeleted,C.CredentialName,E.Degree, E.EmployeeSignatureID, E.MobileNumber,E.Address,E.City,E.ZipCode,E.StateCode       
   FROM  employees e         
   LEFT JOIN Departments d on e.DepartmentID=d.DepartmentID        
   LEFT JOIN Roles R ON R.RoleID = E.RoleID        
   LEFT JOIN EmployeeCredentials C ON C.CredentialID=E.CredentialID      
   WHERE ((CAST(@IsDeleted AS BIGINT)=-1) OR e.IsDeleted=@IsDeleted)        
   AND ((@Email IS NULL OR LEN(@Email)=0) OR e.Email LIKE '%' + @Email + '%')        
    
   AND   
   ((@Name IS NULL OR LEN(e.LastName)=0)     
   OR (    
       (e.FirstName LIKE '%'+@Name+'%' )OR      
    (e.LastName  LIKE '%'+@Name+'%') OR      
    (e.FirstName +' '+e.LastName like '%'+@Name+'%') OR      
    (e.LastName +' '+e.FirstName like '%'+@Name+'%') OR      
    (e.FirstName +', '+e.LastName like '%'+@Name+'%') OR      
    (e.LastName +', '+e.FirstName like '%'+@Name+'%'))) 
-- ((@Name IS NULL OR LEN(@Name)=0) OR (e.FirstName+' '+e.LastName LIKE '%' + @Name + '%'))            
     
   AND ((@Degree IS NULL OR LEN(@Degree)=0) OR (E.Degree LIKE '%' + @Degree+ '%'))            
   AND (( CAST(@DepartmentID AS BIGINT)=0) OR d.DepartmentID = CAST(@DepartmentID AS BIGINT))        
   AND (( CAST(@RoleID AS BIGINT)=0) OR R.RoleID = CAST(@RoleID AS BIGINT))        
   AND ((@CredentialID IS NULL OR LEN(@CredentialID)=0) OR (C.CredentialID = @CredentialID))      
  
   AND ((@Address IS NULL OR LEN(@Address)=0) OR  
        (E.Address LIKE '%' + @Address+ '%')  OR  
  (E.City LIKE '%' + @Address+ '%')  OR  
  (E.ZipCode LIKE '%' + @Address+ '%')  OR  
  (E.StateCode LIKE '%' + @Address+ '%')   )      
   AND ((@MobileNumber IS NULL OR LEN(@MobileNumber)=0) OR (E.MobileNumber LIKE '%' + @MobileNumber+ '%'))      
         
   AND (( CAST(@IsDepartmentSupervisor AS INT)=-1) OR e.IsDepartmentSupervisor = CAST(@IsDepartmentSupervisor AS INT))     
  ) AS t1  )        
   
 SELECT * FROM CTEEmployeeList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)         
        
END