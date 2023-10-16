-- EXEC GetEtsDetailList @EmployeeTimeSlotMasterID =8, @SortExpression = 'Name', @SortType = 'DESC', @FromIndex = '1', @PageSize = '100'                
CREATE PROCEDURE [dbo].[GetEtsDetailList]                  
 @EmployeeTimeSlotMasterID BIGINT = 0,                
 @StartTime VARCHAR(20) = NULL,                
 @EndTime VARCHAR(20) = NULL,          
 @IsDeleted int=-1,                
 @SortExpression NVARCHAR(100),                  
 @SortType NVARCHAR(10),                
 @FromIndex INT,                
 @PageSize INT                 
AS                
BEGIN                    
 ;WITH CTEEtsDetailList AS                
 (                 
  SELECT *,COUNT(t1.EmployeeTimeSlotDetailID) OVER() AS Count FROM                 
  (                
   SELECT ROW_NUMBER() OVER (ORDER BY                 
            
         
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'EmployeeTimeSlotDetailID' THEN etd.EmployeeTimeSlotDetailID END END ASC,                
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'EmployeeTimeSlotDetailID' THEN etd.EmployeeTimeSlotDetailID END END DESC,                
             
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Name' THEN e.FirstName END END ASC,                
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Name' THEN e.FirstName END END DESC,                
                
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Notes' THEN etd.Notes END END ASC,                
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Notes' THEN etd.Notes END END DESC,  
  
    CASE WHEN @SortType = 'ASC' THEN                
      CASE                 
      WHEN @SortExpression = 'StartTime' THEN StartTime        
      END                 
    END ASC,                
    CASE WHEN @SortType = 'DESC' THEN                
      CASE                 
      WHEN @SortExpression = 'EndTime' THEN EndTime            
      END                
    END DESC, Day, StartTime                
  ) AS Row,                
   etd.EmployeeTimeSlotDetailID,etd.EmployeeTimeSlotMasterID, Name=e.FirstName+ ' ' + e.LastName,        
   CreatedBy=ec.FirstName+ ' ' + ec.LastName,UpdatedBy=eu.FirstName+ ' ' + eu.LastName , etd.CreatedDate,etd.UpdatedDate,        
   etd.IsDeleted,etd.StartTime,etd.EndTime,etd.Day,etd.Notes  
   FROM  EmployeeTimeSlotDetails etd        
   INNER JOIN EmployeeTimeSlotMaster ets on ets.EmployeeTimeSlotMasterID=etd.EmployeeTimeSlotMasterID        
   INNER JOIN Employees e on e.EmployeeID=ets.EmployeeID        
   INNER JOIN Employees ec on ec.EmployeeID=etd.CreatedBy        
   INNER JOIN Employees eu on eu.EmployeeID=etd.UpdatedBy        
        
   WHERE ((CAST(@IsDeleted AS BIGINT)=-1) OR etd.IsDeleted=@IsDeleted)                
   AND ((@EmployeeTimeSlotMasterID =0 OR LEN(@EmployeeTimeSlotMasterID)=0) OR etd.EmployeeTimeSlotMasterID=@EmployeeTimeSlotMasterID)        
   AND ((@StartTime IS NULL OR LEN(@StartTime)=0) OR etd.StartTime LIKE '%' + @StartTime + '%')                
   AND ((@EndTime IS NULL OR LEN(@EndTime)=0) OR etd.EndTime LIKE '%' + @EndTime + '%')      
   AND etd.IsDeleted=0    
  ) AS t1 )     
           
 SELECT * FROM CTEEtsDetailList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                 
                
END
