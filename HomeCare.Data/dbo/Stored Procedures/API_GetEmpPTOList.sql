--EXEC API_GetEmpPTOList @FromIndex = N'1', @ToIndex = N'10', @SortType = NULL, @SortExpression = NULL, @EmployeeID = N'1328', @DayOffTypeID = N'0', @DayOffStatus = NULL, @StartDate='2018-12-24', @EndDate='2018-12-28'  
CREATE PROCEDURE [dbo].[API_GetEmpPTOList]                
 @FromIndex INT,                    
 @ToIndex INT,                    
 @SortType NVARCHAR(10),          
 @SortExpression NVARCHAR(100),                            
 @EmployeeID BIGINT,            
 @DayOffTypeID INT,    
 @DayOffStatus VARCHAR(30),  
 @StartDate DATE=NULL,  
 @EndDate DATE=NULL  
AS                    
BEGIN                    
 DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()                  
IF(@SortExpression IS NULL OR @SortExpression ='')                    
BEGIN                    
 SET @SortExpression = 'CreatedDate'                    
 SET @SortType='DESC'                    
END                    
         
          
;WITH CTENoteList AS                        
 (                             
  SELECT ROW_NUMBER() OVER                     
    (ORDER BY                    
       CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CreatedDate' THEN T.CreatedDate END                    
       END ASC,                    
       CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CreatedDate' THEN T.CreatedDate END                    
       END DESC                  
    ) AS Row,*,      
 COUNT(T.EmployeeDayOffID) OVER() AS Count                    
  FROM                    
  (                    
   SELECT EmployeeDayOffID,DayOffTypeID,DayOffStatus,StartTime,EndTime,EmployeeComment,ApproverComment,ActionTakenDate,    
 ActionTakenBy=dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat),EDO.CreatedDate    
 FROM EmployeeDayOffs EDO    
 INNER JOIN Employees E ON EDO.EmployeeID = E.EmployeeID    
 WHERE EDO.EmployeeID=@EmployeeID AND EDO.IsDeleted=0    
 AND (@DayOffStatus IS NULL OR LEN(@DayOffStatus)=0 OR EDO.DayOffStatus=@DayOffStatus)    
 AND (@DayOffTypeID IS NULL OR @DayOffTypeID=0 OR EDO.DayOffTypeID=@DayOffTypeID)  
 AND ((@StartDate IS NULL AND @EndDate IS NULL) OR (@StartDate IS NULL AND CONVERT(DATE,EDO.EndTime)<=@EndDate) OR   
 (@EndDate IS NULL AND CONVERT(DATE,EDO.StartTime)>=@StartDate) OR (CONVERT(DATE,EDO.StartTime) >=@StartDate AND CONVERT(DATE,EDO.EndTime) <= @EndDate ))  
  ) AS T                    
 )                    
                       
  SELECT * FROM CTENoteList WHERE ROW BETWEEN @FromIndex AND @ToIndex                    
                      
END  