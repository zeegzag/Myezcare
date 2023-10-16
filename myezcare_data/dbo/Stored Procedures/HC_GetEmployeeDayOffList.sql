--EXEC HC_GetEmployeeDayOffList @StartTime = '2018/04/10', @EndTime = '2018/04/10', @IsDeleted = '0', @SortExpression = 'EmployeeDayOffID', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50'  
  
CREATE PROCEDURE [dbo].[HC_GetEmployeeDayOffList]               
 @EmployeeID BIGINT = 0,              
 @StartTime DATE = NULL,              
 @EndTime DATE = NULL,      
 @SubmittedBy VARCHAR(100) = NULL,                
 @SubmittedDate DATE = NULL,      
 @IsDeleted int=-1,      
 @SortExpression NVARCHAR(100),                
 @SortType NVARCHAR(10),              
 @FromIndex INT,              
 @PageSize INT               
AS              
BEGIN                  
 ;WITH CTEEmployeeDayOffsList AS              
 (               
  SELECT *,COUNT(t1.EmployeeDayOffID) OVER() AS Count FROM               
  (              
   SELECT ROW_NUMBER() OVER (ORDER BY               
              
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'EmployeeDayOffID' THEN EmployeeDayOffID END END ASC,              
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'EmployeeDayOffID' THEN EmployeeDayOffID END END DESC,              
        
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Employee' THEN E.LastName END END ASC,              
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Employee' THEN E.LastName END END DESC,      
     
    
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'DayOffTypeID' THEN EDO.DayOffTypeID END END ASC,              
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'DayOffTypeID' THEN EDO.DayOffTypeID END END DESC,      
    
     
       
 CASE WHEN @SortType = 'ASC' THEN  CASE WHEN @SortExpression = 'StartTime' THEN EDO.StartTime END END ASC,                                    
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'StartTime' THEN EDO.StartTime END END DESC,      
      
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'EndTime' THEN EDO.EndTime END END ASC,                                    
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'EndTime' THEN EDO.EndTime END END DESC,      
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'SubmittedBy' THEN CB.FirstName END END ASC,              
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'SubmittedBy' THEN CB.FirstName END END DESC,      
 CASE WHEN @SortType = 'ASC' THEN  CASE WHEN @SortExpression = 'SubmittedDate' THEN EDO.CreatedDate END END ASC,                                    
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'SubmittedDate' THEN EDO.CreatedDate END END DESC,      
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Status' THEN EDO.DayOffStatus END END ASC,              
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Status' THEN EDO.DayOffStatus END END DESC,      
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ActionTakenByName' THEN ATB.FirstName END END ASC,              
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ActionTakenByName' THEN ATB.FirstName END END DESC,    
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Comment' THEN EDO.EmployeeComment END END ASC,              
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Comment' THEN EDO.EmployeeComment END END DESC      
              
  ) AS Row,              
   EDO.*, EmployeeName=dbo.GetGeneralNameFormat(E.FirstName,E.LastName), ActionTakenByName= dbo.GetGeneralNameFormat(ATB.FirstName,ATB.LastName),    
   SubmittedBy= dbo.GetGeneralNameFormat(CB.FirstName,CB.LastName)     
   FROM  EmployeeDayOffs EDO        
   INNER JOIN Employees E on E.EmployeeID=EDO.EmployeeID        
   INNER JOIN Employees CB on CB.EmployeeID=EDO.CreatedBy        
   LEFT JOIN Employees ATB on ATB.EmployeeID=EDO.ActionTakenBy        
   --INNER JOIN Employees UB on UB.UpdatedBy=EDO.EmployeeID             
        
   WHERE ((CAST(@IsDeleted AS BIGINT)=-1) OR EDO.IsDeleted=@IsDeleted)              
   AND         
   ((@EmployeeID IS NULL OR LEN(@EmployeeID)=0 OR @EmployeeID=0)           
   OR (E.EmployeeID=@EmployeeID ))
   --AND (((@StartTime is null OR EDO.StartTime >= @StartTime) and (@EndTime is null OR EDO.EndTime <= @EndTime))  
   -- OR(@StartTime is null OR CONVERT(DATE,EDO.StartTime)=@StartTime) OR(@EndTime is null OR CONVERT(DATE,EDO.EndTime)=@EndTime))  
   AND ((@StartTime IS NOT NULL AND @EndTime IS NULL AND CONVERT(DATE,EDO.StartTime) >= @StartTime) OR
   (@EndTime IS NOT NULL AND @StartTime IS NULL AND CONVERT(DATE,EDO.EndTime) <= @EndTime) OR
   (@StartTime IS NOT NULL AND @EndTime IS NOT NULL AND (CONVERT(DATE,EDO.StartTime) >= @StartTime AND CONVERT(DATE,EDO.EndTime) <= @EndTime)) OR
   (@StartTime IS NULL AND @EndTime IS NULL)
   )
   AND         
   ((@SubmittedBy IS NULL OR LEN(@SubmittedBy)=0)           
   OR (          
        (CB.FirstName LIKE '%'+@SubmittedBy+'%' )OR            
  (CB.LastName  LIKE '%'+@SubmittedBy+'%') OR            
  (CB.FirstName +' '+CB.LastName like '%'+@SubmittedBy+'%') OR            
  (CB.LastName +' '+CB.FirstName like '%'+@SubmittedBy+'%') OR            
  (CB.FirstName +', '+CB.LastName like '%'+@SubmittedBy+'%') OR            
  (CB.LastName +', '+CB.FirstName like '%'+@SubmittedBy+'%')))          
  AND ((@SubmittedDate IS NULL OR LEN(@SubmittedDate)=0) OR CONVERT(VARCHAR(20),EDO.CreatedDate,120) LIKE '%' + CONVERT(VARCHAR(20),@SubmittedDate) + '%')                                    
  ) AS t1  )              
         
 SELECT * FROM CTEEmployeeDayOffsList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)               
              
END
