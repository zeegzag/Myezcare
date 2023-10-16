-- EXEC GetEtsMasterList @EmployeeID =135, @SortExpression = 'Name', @SortType = 'DESC', @FromIndex = '1', @PageSize = '100'                          
CREATE PROCEDURE [dbo].[GetEtsMasterList]      
 @EmployeeID BIGINT = 0,                          
 @StartDate DATE = NULL,                          
 @EndDate DATE = NULL,                    
 @TodayDate DATE,    
 @IsDeleted int=-1,                          
 @SortExpression NVARCHAR(100),                            
 @SortType NVARCHAR(10),                          
 @FromIndex INT,                          
 @PageSize INT                           
AS                          
BEGIN 
 DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()
DECLARE @ActiveEmployeeTimeSlotMasterID BIGINT    
    
---------OldCode for get upcoming slot master    
--SELECT TOP 1 @ActiveEmployeeTimeSlotMasterID=EmployeeTimeSlotMasterID FROM EmployeeTimeSlotDates WHERE EmployeeTimeSlotMasterID IN (SELECT EmployeeTimeSlotMasterID FROM EmployeeTimeSlotMaster WHERE EmployeeID=@EmployeeID)    
--AND EmployeeTSDate <= @TodayDate ORDER BY EmployeeTSDate DESC    
    
--IF (@ActiveEmployeeTimeSlotMasterID IS NULL)    
 --SELECT TOP 1 @ActiveEmployeeTimeSlotMasterID=EmployeeTimeSlotMasterID FROM EmployeeTimeSlotDates WHERE EmployeeTimeSlotMasterID IN (SELECT EmployeeTimeSlotMasterID FROM EmployeeTimeSlotMaster WHERE EmployeeID=@EmployeeID)    
 --AND EmployeeTSDate >= CONVERT(DATE,GETDATE()) ORDER BY EmployeeTSDate    
---------OldCode End    
    
 SELECT TOP 1 @ActiveEmployeeTimeSlotMasterID=etsdate.EmployeeTimeSlotMasterID FROM EmployeeTimeSlotDetails etsd    
 INNER JOIN EmployeeTimeSlotDates etsdate ON etsdate.EmployeeTimeSlotDetailID=etsd.EmployeeTimeSlotDetailID    
 WHERE etsd.EmployeeTimeSlotMasterID IN (SELECT EmployeeTimeSlotMasterID FROM EmployeeTimeSlotMaster WHERE EmployeeID=@EmployeeID) AND IsDeleted=0    
 AND EmployeeTSDate >= CONVERT(DATE,GETDATE())    
 ORDER BY EmployeeTSDate    
    
    
 ;WITH CTEEtsMasterList AS                          
 (                           
  SELECT *,COUNT(t1.EmployeeTimeSlotMasterID) OVER() AS Count FROM                           
  (                          
   SELECT ROW_NUMBER() OVER (ORDER BY                           
                      
                   
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'EmployeeTimeSlotMasterID' THEN TBL1.EmployeeTimeSlotMasterID END END ASC,                          
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'EmployeeTimeSlotMasterID' THEN TBL1.EmployeeTimeSlotMasterID END END DESC,                          
                       
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Name' THEN TBL1.Name END END ASC,                          
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Name' THEN TBL1.Name END END DESC,           
           
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'TotalETSDetailCount' THEN TBL1.TotalETSDetailCount END END ASC,                          
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'TotalETSDetailCount' THEN TBL1.TotalETSDetailCount END END DESC,                           
                          
    CASE WHEN @SortType = 'ASC' THEN                          
      CASE                           
      WHEN @SortExpression = 'StartDate' THEN TBL1.StartDate                          
      END                           
    END ASC,                          
    CASE WHEN @SortType = 'DESC' THEN                          
      CASE                           
      WHEN @SortExpression = 'EndDate' THEN TBL1.EndDate                          
      END                          
    END DESC        
  ) AS Row,  * FROM (                
                  
   SELECT DISTINCT ets.EmployeeTimeSlotMasterID,ets.EmployeeID, Name = dbo.GetGenericNameFormat(E.FirstName,E.MiddleName, E.LastName,@NameFormat),    
   CreatedBy= dbo.GetGenericNameFormat(ec.FirstName,ec.MiddleName, ec.LastName,@NameFormat),UpdatedBy=dbo.GetGenericNameFormat(eu.FirstName,eu.MiddleName, eu.LastName,@NameFormat),
   ets.CreatedDate,ets.UpdatedDate,ets.IsDeleted,    
   ets.StartDate,ets.EndDate,ets.IsEndDateAvailable,TotalETSDetailCount= COUNT(ed.EmployeeTimeSlotMasterID) OVER(PARTITION BY ed.EmployeeTimeSlotMasterID),    
   ActiveStat=CASE WHEN ets.EmployeeTimeSlotMasterID=@ActiveEmployeeTimeSlotMasterID THEN 1 ELSE 0 END    
   FROM  EmployeeTimeSlotMaster ets                         
   INNER JOIN Employees e on e.EmployeeID=ets.EmployeeID                  
   INNER JOIN Employees ec on ec.EmployeeID=ets.CreatedBy                  
   INNER JOIN Employees eu on eu.EmployeeID=ets.UpdatedBy                  
   LEFT JOIN EmployeeTimeSlotDetails ED ON ets.EmployeeTimeSlotMasterID=ED.EmployeeTimeSlotMasterID AND ED.IsDeleted=0               
   WHERE ((CAST(@IsDeleted AS BIGINT)=-1) OR ets.IsDeleted=@IsDeleted)                          
   AND ((@EmployeeID =0 OR LEN(@EmployeeID)=0) OR ets.EmployeeID=@EmployeeID)                  
   AND ((@StartDate IS NULL OR LEN(@StartDate)=0) OR ets.StartDate LIKE '%' + CONVERT(VARCHAR(20),@StartDate) + '%')                          
   AND ((@EndDate IS NULL OR LEN(@EndDate)=0) OR ets.EndDate LIKE '%' + CONVERT(VARCHAR(20),@EndDate) + '%')                  
   AND (ets.IsEndDateAvailable = 0 OR (ets.EndDate >= GETDATE()))             
  )   AS TBL1    
                            
  ) AS t1  )                          
                     
 SELECT * FROM CTEEtsMasterList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                           
                          
END  