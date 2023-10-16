-- [HC_GetServiceCodeList] null,null,null,1,'ServiceCodeID','ASC',1,100      
CREATE PROCEDURE [dbo].[HC_GetServiceCodeList]                            
@ServiceCode VARCHAR(MAX)=null,                        
@ModifierName  VARCHAR(100)=NULL,          
@ServiceName VARCHAR(MAX)=NULL,
@AccountCode VARCHAR(MAX)=NULL,
@IsBillable INT=-1,                                           
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
                        
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Modifier' THEN Modifier END END ASC,                                  
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Modifier' THEN Modifier END END DESC,                                      
                        
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'IsBillable' THEN IsBillable END END ASC,                                  
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'IsBillable' THEN IsBillable END END DESC,
 
  CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'AccountCode' THEN AccountCode END END ASC,                                  
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'AccountCode' THEN AccountCode END END DESC
                                  
    ) AS ROW,                                      
 * From      
  (SELECT      
  S.ServiceCodeID, S.ServiceCode,S.ServiceName, S.IsDeleted, S.AccountCode, 
  STUFF(          
  (SELECT ', ' + convert(varchar(100), M.ModifierCode, 120)          
  FROM Modifiers M  where M.ModifierID IN (SELECT val FROM GetCSVTable(S.ModifierID)) AND M.IsDeleted=0         
  FOR XML PATH ('')) , 1, 1, '')  AS Modifier,                  
  IsBillable=CASE WHEN S.IsBillable=1 THEN 'Yes' ELSE 'No' END ,      
  IsBillable AS IB                   
  FROM  ServiceCodes S                              
  )As T      
  WHERE                         
  ((@ServiceCode IS NULL OR LEN(@ServiceCode)=0) OR ServiceCode LIKE '%' + @ServiceCode+ '%') AND                                     
  ((@ServiceName IS NULL OR LEN(@ServiceName)=0) OR ServiceName LIKE '%' + @ServiceName+ '%')  AND 
   ((@AccountCode IS NULL OR LEN(@AccountCode)=0) OR AccountCode LIKE '%' + @AccountCode+ '%')  AND
  ((@ModifierName IS NULL OR LEN(@ModifierName)=0) OR Modifier LIKE '%' + @ModifierName+ '%')  AND        
  ((@IsBillable = -1) OR IB =CONVERT(BIT, @IsBillable)) AND IsDeleted = 0  
  ) AS P1                            
 )                                      
       
 SELECT * FROM CTEServiceCode WHERE ROW BETWEEN ((@PAGESIZE*(@FROMINDEX-1))+1) AND (@PAGESIZE*@FROMINDEX)                                     
                                    
END  