CREATE PROCEDURE [dbo].[GetServiceCodeList]      
@ServiceCode VARCHAR(MAX),        
@ModifierID  BIGINT=0,        
@ServiceName VARCHAR(MAX),        
@ServiceCodeType BIGINT=0,        
@UnitType BIGINT=0,        
@IsBillable INT=-1,        
@HasGroupOption INT=-1,            
@ServiceCodeStartDate VARCHAR(MAX),        
@ServiceCodeEndDate VARCHAR(MAX),        
@IsDeleted BIGINT = -1,                     
@SORTEXPRESSION NVARCHAR(100),              
@SORTTYPE NVARCHAR(10),            
@FROMINDEX INT,            
@PAGESIZE INT             
AS                      
BEGIN                        
;WITH CTEServiceCode AS                  
 (                       
  SELECT *,COUNT(P1.ServiceCodeID) OVER() AS Count FROM                  
  (                      
   SELECT ROW_NUMBER() OVER (ORDER BY                  
                  
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ServiceCodeID' THEN ServiceCodeID END END ASC,                  
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ServiceCodeID' THEN ServiceCodeID END END DESC,                  
        
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ServiceCode' THEN ServiceCode END END ASC,                  
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ServiceCode' THEN ServiceCode END END DESC,           
        
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ServiceName' THEN ServiceName END END ASC,                  
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ServiceName' THEN ServiceName END END DESC,           
        
        
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ModifierCode' THEN ModifierCode END END ASC,                  
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ModifierCode' THEN ModifierCode END END DESC,           
        
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ServiceCodeTypeName' THEN ServiceCodeTypeName END END ASC,                  
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ServiceCodeTypeName' THEN ServiceCodeTypeName END END DESC,           
        
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'UnitType' THEN UnitType END END ASC,                  
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'UnitType' THEN UnitType END END DESC,           
        
        
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'IsBillable' THEN IsBillable END END ASC,                  
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'IsBillable' THEN IsBillable END END DESC,           
        
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'HasGroupOption' THEN HasGroupOption END END ASC,                  
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'HasGroupOption' THEN HasGroupOption END END DESC,           
        
        
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ServiceCodeStartDate' THEN ServiceCodeStartDate END END ASC,                  
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ServiceCodeStartDate' THEN ServiceCodeStartDate END END DESC,        
         
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ServiceCodeEndDate' THEN ServiceCodeEndDate END END ASC,                  
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ServiceCodeEndDate' THEN ServiceCodeEndDate END END DESC      
      
 --CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Credentials' THEN Credentials END END ASC,                  
 --CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Credentials' THEN Credentials END END DESC      
        
                  
                  
    ) AS ROW,                      
   S.ServiceCodeID, S.ServiceCode,S.ServiceName,      
   Modifier=M.ModifierCode,      
   S.UnitType,        
   IsBillable=CASE WHEN S.IsBillable=1 THEN 'Yes' ELSE 'No' END        
   ,ServiceCodeType=ST.ServiceCodeTypeName,        
   S.ServiceCodeStartDate,S.ServiceCodeEndDate,        
   S.IsDeleted,            
   HasGroupOption=CASE WHEN S.HasGroupOption=1 THEN 'Yes' ELSE 'No' END            
    FROM  ServiceCodes S                  
    LEFT JOIN Modifiers M ON M.ModifierID=S.ModifierID            
    INNER JOIN ServiceCodeTypes ST ON ST.ServiceCodeTypeID=S.ServiceCodeType            
    WHERE         
 ((@IsDeleted = -1) OR S.IsDeleted=@IsDeleted)   AND            
    ((@ServiceCode IS NULL OR LEN(@ServiceCode)=0) OR S.ServiceCode LIKE '%' + @ServiceCode+ '%') AND                         
    ((@ServiceName IS NULL OR LEN(@ServiceName)=0) OR S.ServiceName LIKE '%' + @ServiceName+ '%')  AND                        
    ((@ModifierID=0 OR LEN(@ModifierID)=0) OR S.ModifierID = @ModifierID)   AND        
    ((@UnitType=0 OR LEN(@UnitType)=0) OR S.UnitType = @UnitType) AND        
 ((@IsBillable = -1) OR S.IsBillable =CONVERT(BIT, @IsBillable)) AND        
 ((@ServiceCodeType = 0) OR S.ServiceCodeType =CONVERT(BIT, @ServiceCodeType)) AND        
    ((@HasGroupOption = -1) OR S.HasGroupOption = CONVERT(BIT, @HasGroupOption))               
  ) AS P1   
 )                     
 SELECT * FROM CTEServiceCode WHERE ROW BETWEEN ((@PAGESIZE*(@FROMINDEX-1))+1) AND (@PAGESIZE*@FROMINDEX)                     
                    
END
