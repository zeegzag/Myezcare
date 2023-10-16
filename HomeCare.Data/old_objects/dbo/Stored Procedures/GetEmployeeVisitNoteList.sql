-- EXEC GetEmployeeVisitList @EmployeeVisitID = '0', @Name = '', @PatientName = '', @SortExpression = 'EmployeeVisitID', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50'            
CREATE PROCEDURE [dbo].[GetEmployeeVisitNoteList]              
 @EmployeeVisitNoteID BIGINT = 0,            
 @EmployeeVisitID BIGINT = 0,          
 @Name NVARCHAR(100) = NULL,                
 @PatientName NVARCHAR(100) = NULL,                
 @VisitTaskDetail NVARCHAR(100) = NULL,            
 @Description NVARCHAR(100) = NULL,        
 @VisitTaskType NVARCHAR(30),        
 @ServiceTime BIGINT=0,          
 @StartDate DATE = NULL,                                    
 @EndDate DATE = NULL,                 
 @IsDeleted int=-1,                                  
 @SortExpression NVARCHAR(100),                                    
 @SortType NVARCHAR(10),                                  
 @FromIndex INT,                                  
 @PageSize INT                                   
AS                                  
BEGIN                                      
 ;WITH CTEEmployeeVisitNoteList AS                                  
 (                                   
  SELECT *,COUNT(t1.EmployeeVisitNoteID) OVER() AS Count FROM                                   
  (                                  
   SELECT ROW_NUMBER() OVER (ORDER BY                                   
                           
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'EmployeeVisitNoteID' THEN EmployeeVisitNoteID END END ASC,                                  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'EmployeeVisitNoteID' THEN EmployeeVisitNoteID END END DESC,                                  
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Name' THEN e.FirstName END END ASC,                                    
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Name' THEN e.FirstName END END DESC,                     
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'PatientName' THEN r.FirstName END END ASC,                                    
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'PatientName' THEN r.FirstName END END DESC,            
       CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'VisitTaskDetail' THEN v.VisitTaskDetail END END ASC,                                  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'VisitTaskDetail' THEN v.VisitTaskDetail END END DESC,            
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Description' THEN Description END END ASC,                                  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Description' THEN Description END END DESC,            
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ServiceTime' THEN ServiceTime END END ASC,                                  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ServiceTime' THEN ServiceTime END END DESC,          
 CASE WHEN @SortType = 'ASC' THEN                                    
      CASE                                     
      WHEN @SortExpression = 'StartDate' THEN sm.StartDate                                    
      END                                     
    END ASC,                                    
    CASE WHEN @SortType = 'DESC' THEN                                    
      CASE                                     
      WHEN @SortExpression = 'StartDate' THEN sm.StartDate                                    
      END                                    
    END DESC,                
 CASE WHEN @SortType = 'ASC' THEN                                    
      CASE                                     
      WHEN @SortExpression = 'EndDate' THEN sm.EndDate                
      END                                     
    END ASC,                                    
    CASE WHEN @SortType = 'DESC' THEN                                    
      CASE                    
      WHEN @SortExpression = 'EndDate' THEN sm.EndDate                
      END                         
    END DESC                                     
               
  ) AS Row,              
             
   ev.EmployeeVisitNoteID,Name=e.FirstName+ ' ' + e.LastName,PatientName=r.FirstName+ ' ' + r.LastName,sm.StartDate,sm.EndDate,rtm.ReferralTaskMappingID,    
   VisitTaskDetail = ISNULL(v.VisitTaskDetail,ev.Description),ev.Description,ev.ServiceTime,ev.IsDeleted            
   FROM  EmployeeVisitNotes ev            
   INNER JOIN EmployeeVisits evs on evs.EmployeeVisitID=ev.EmployeeVisitID      
   LEFT JOIN ReferralTaskMappings rtm on rtm.ReferralTaskMappingID=ev.ReferralTaskMappingID      
   LEFT JOIN VisitTasks v on v.VisitTaskID=rtm.VisitTaskID            
   INNER JOIN ScheduleMasters sm on sm.ScheduleID=evs.ScheduleID                
   LEFT JOIN Employees e on e.EmployeeID=sm.EmployeeID                
   INNER JOIN Referrals r on r.ReferralID=sm.ReferralID              
   WHERE ((CAST(@IsDeleted AS BIGINT)=-1) OR ev.IsDeleted=@IsDeleted)            
   AND (@EmployeeVisitID=0 OR ev.EmployeeVisitID=@EmployeeVisitID)          
   AND ((@StartDate IS NULL OR LEN(@StartDate)=0) OR CONVERT(VARCHAR(20),sm.StartDate,120) LIKE '%' + CONVERT(VARCHAR(20),@StartDate) + '%')                                    
   AND ((@EndDate IS NULL OR LEN(@EndDate)=0) OR CONVERT(VARCHAR(20),sm.EndDate,120) LIKE '%' + CONVERT(VARCHAR(20),@EndDate) + '%')            
   AND (@ServiceTime=0 OR ev.ServiceTime LIKE '%' + CONVERT(VARCHAR(30),@ServiceTime) + '%')          
   AND ((@VisitTaskDetail IS NULL OR LEN(@VisitTaskDetail)=0) OR v.VisitTaskDetail LIKE '%' + @VisitTaskDetail + '%')            
   AND ((@Description IS NULL OR LEN(@Description)=0) OR ev.Description LIKE '%' + @Description + '%')            
   AND               
   ((@Name IS NULL OR LEN(e.LastName)=0)                     
   OR (                    
       (e.FirstName LIKE '%'+@Name+'%' )OR                      
    (e.LastName  LIKE '%'+@Name+'%') OR                  
    (e.FirstName +' '+e.LastName like '%'+@Name+'%') OR                      
    (e.LastName +' '+e.FirstName like '%'+@Name+'%') OR                      
    (e.FirstName +', '+e.LastName like '%'+@Name+'%') OR                      
    (e.LastName +', '+e.FirstName like '%'+@Name+'%')))                
 AND                   
   ((@PatientName IS NULL OR LEN(r.LastName)=0)                     
   OR (                    
       (r.FirstName LIKE '%'+@PatientName+'%' )OR                      
    (r.LastName  LIKE '%'+@PatientName+'%') OR                      
    (r.FirstName +' '+e.LastName like '%'+@PatientName+'%') OR                      
    (r.LastName +' '+e.FirstName like '%'+@PatientName+'%') OR                      
    (r.FirstName +', '+e.LastName like '%'+@PatientName+'%') OR                      
    (r.LastName +', '+e.FirstName like '%'+@PatientName+'%')))            
    AND (v.VisitTaskType=@VisitTaskType OR (EV.ServiceTime>0 AND Description IS NOT NULL AND @VisitTaskType='Task'))  
                          
                                    
  ) AS t1  )                                  
                             
 SELECT * FROM CTEEmployeeVisitNoteList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                                   
                                  
END
