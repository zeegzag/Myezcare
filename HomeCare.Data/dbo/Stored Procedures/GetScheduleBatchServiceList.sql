CREATE PROCEDURE [dbo].[GetScheduleBatchServiceList]        
@ScheduleBatchServiceName varchar(100)=null,        
@ScheduleBatchServiceType varchar(100)=null,        
@ScheduleBatchServiceStatus varchar(100)=null,        
@AddedBy varchar(100)=null,        
@FilePath varchar(100)=null,        
@SORTEXPRESSION VARCHAR(100),                              
@SORTTYPE VARCHAR(10),                              
@FROMINDEX INT,                              
@PAGESIZE INT                              
AS                                                                        
BEGIN   
DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()
;WITH CTEScheduleBatchService AS                                                                         
 (                                                                        
  SELECT *,COUNT(P1.ScheduleBatchServiceID) OVER() AS COUNT FROM                                                                         
  (                                                                        
   SELECT ROW_NUMBER() OVER (ORDER BY                                                                        
                                                                        
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ScheduleBatchServiceID' THEN S.ScheduleBatchServiceID END END ASC,                                                  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ScheduleBatchServiceID' THEN S.ScheduleBatchServiceID  END END DESC,        
            
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ScheduleBatchServiceName' THEN S.ScheduleBatchServiceName END END ASC,                                                  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ScheduleBatchServiceName' THEN S.ScheduleBatchServiceName  END END DESC,        
            
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ScheduleBatchServiceType' THEN S.ScheduleBatchServiceType END END ASC,                                                  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ScheduleBatchServiceType' THEN S.ScheduleBatchServiceType  END END DESC,        
            
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ScheduleBatchServiceStatus' THEN S.ScheduleBatchServiceStatus END END ASC,                                                  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ScheduleBatchServiceStatus' THEN S.ScheduleBatchServiceStatus  END END DESC,        
            
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'AddedBy' THEN E.LastName END END ASC,                                                  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'AddedBy' THEN E.LastName  END END DESC        
            
    )AS ROW,                                                                        
   S.ScheduleBatchServiceID,S.ScheduleBatchServiceName,S.ScheduleBatchServiceType,S.ScheduleBatchServiceStatus,        
   S.CreatedDate,dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat) as AddedBy,S.FilePath         
  from          
  ScheduleBatchServices S        
  INNER JOIN Employees E on E.EmployeeID=S.CreatedBy        
  where           
   ((@ScheduleBatchServiceName IS NULL OR LEN(@ScheduleBatchServiceName)=0) OR S.ScheduleBatchServiceName LIKE '%' + @ScheduleBatchServiceName + '%')          
  AND ((@ScheduleBatchServiceType IS NULL OR LEN(@ScheduleBatchServiceType)=0) OR S.ScheduleBatchServiceType LIKE '%' + @ScheduleBatchServiceType+ '%')          
  AND ((@ScheduleBatchServiceStatus IS NULL OR LEN(@ScheduleBatchServiceStatus)=0) OR S.ScheduleBatchServiceStatus LIKE '%' + @ScheduleBatchServiceStatus+ '%')          
  AND ((@AddedBy IS NULL OR LEN(@AddedBy)=0) OR (( E.LastName  LIKE '%' + @AddedBy + '%')OR ( E.FirstName LIKE '%' + @AddedBy + '%')))          
  AND ((@FilePath IS NULL OR LEN(@FilePath)=0) OR S.FilePath LIKE '%' + @FilePath+ '%')       
           
          
  ) AS P1                              
 )                                                                                                  
 SELECT * FROM CTEScheduleBatchService  WHERE ROW BETWEEN ((@PAGESIZE *(@FROMINDEX-1))+1) AND (@PAGESIZE*@FROMINDEX)                                                                                                      
END  