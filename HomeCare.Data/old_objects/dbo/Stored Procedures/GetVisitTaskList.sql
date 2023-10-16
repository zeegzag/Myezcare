
-- EXEC GetVisitTaskList @ServiceCodeID = '0', @IsDeleted = '0', @SortExpression = 'VisitTaskType', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50'          
-- EXEC GetVisitTaskList @ServiceCode = 'h0', @IsDeleted = '0', @SortExpression = 'VisitTaskType', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50'          
CREATE PROCEDURE [dbo].[GetVisitTaskList]                  
 @VisitTaskType VARCHAR(100) = NULL,                  
 @VisitTaskDetail VARCHAR(100) = NULL,          
 --@ServiceCodeID BIGINT = 0,        
 @ServiceCode VARCHAR(100) = null,        
 @VisitTaskCategoryID BIGINT=0,                   
 @VisitTaskVisitTypeID BIGINT=0,                   
 @VisitTaskCareTypeID BIGINT=0,                   
 @IsDeleted BIGINT = -1,                  
 @SortExpression NVARCHAR(100),                    
 @SortType NVARCHAR(10),                  
 @FromIndex INT,                  
 @PageSize INT                  
AS                  
BEGIN                  
 ;WITH CTEVisitTaskList AS                  
 (                   
  SELECT *,COUNT(T1.VisitTaskID) OVER() AS Count FROM                   
  (                  
   SELECT ROW_NUMBER() OVER (ORDER BY                   
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'VisitTaskID' THEN VisitTaskID END END ASC,                  
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'VisitTaskID' THEN VisitTaskID END END DESC,   
             
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'MinimumTimeRequired' THEN MinimumTimeRequired END END ASC,                  
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'MinimumTimeRequired' THEN MinimumTimeRequired END END DESC,     
           
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'IsRequired' THEN IsRequired END END ASC,                  
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'IsRequired' THEN IsRequired END END DESC,  
         
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'IsDefault' THEN IsDefault END END ASC,                  
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'IsDefault' THEN IsDefault END END DESC,    
     
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Category' THEN VisitTaskCategoryName END END ASC,                  
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Category' THEN VisitTaskCategoryName END END DESC,             
             
 CASE WHEN @SortType = 'ASC' THEN                  
  CASE                         
   WHEN @SortExpression = 'TaskType' THEN VisitTaskType                   
   WHEN @SortExpression = 'TaskDetail' THEN VisitTaskDetail             
   WHEN @SortExpression = 'ServiceCode' THEN ServiceCode             
   WHEN @SortExpression = 'CareType' THEN CareType             
   WHEN @SortExpression = 'VisitType' THEN VisitType             
  END                   
 END ASC,                  
 CASE WHEN @SortType = 'DESC' THEN                  
  CASE                         
   WHEN @SortExpression = 'TaskType' THEN VisitTaskType                
   WHEN @SortExpression = 'TaskDetail' THEN VisitTaskDetail               
   WHEN @SortExpression = 'ServiceCode' THEN ServiceCode           
   WHEN @SortExpression = 'CareType' THEN CareType           
   WHEN @SortExpression = 'VisitType' THEN VisitType           
  END                  
 END DESC                  
  ) AS Row, * FROM (          
                   
 SELECT VisitTaskID,VisitTaskType,VisitTaskDetail,V.IsDeleted, V.ServiceCodeID,DM_CT.Title AS CareType,DM_VT.Title AS VisitType,    
 --ServiceCode = S.ServiceCode + '-' + S.ServiceName ,    
 ServiceCode =     
 --CASE WHEN DM.Title IS NOT NULL THEN DM.Title + ' - ' ELSE '' END + S.ServiceCode +                  
 S.ServiceCode +  
 CASE WHEN (S.ModifierID IS NULL OR S.ModifierID = '') THEN '' ELSE ' -' +                  
 STUFF((SELECT ', ' + convert(varchar(100),M.ModifierCode, 120)                      
 FROM Modifiers M WHERE M.ModifierID IN (SELECT val FROM GetCSVTable(S.ModifierID))            
 FOR XML PATH ('')), 1, 1, '')                  
 END,    
 IsRequired,IsDefault, MinimumTimeRequired,VG.VisitTaskCategoryName               
 FROM  VisitTasks V          
 LEFT JOIN ServiceCodes S ON S.ServiceCodeID = V.ServiceCodeID        
 LEFT JOIN DDMaster DM_CT ON DM_CT.DDMasterID = V.CareType    
 LEFT JOIN DDMaster DM_VT ON DM_VT.DDMasterID = V.VisitType  
 LEFT JOIN VisitTaskCategories VG ON VG.VisitTaskCategoryID=V.VisitTaskCategoryID        
 WHERE ((CAST(@IsDeleted AS BIGINT)=-1) OR V.IsDeleted=@IsDeleted)                  
 AND ((@VisitTaskType IS NULL OR LEN(@VisitTaskType)=0) OR (VisitTaskType LIKE '%' + @VisitTaskType + '%'))                   
 AND ((@VisitTaskDetail IS NULL OR LEN(@VisitTaskDetail)=0) OR (VisitTaskDetail LIKE '%' + @VisitTaskDetail + '%'))     
 AND ((@ServiceCode IS NULL OR LEN(@ServiceCode)=0) OR (S.ServiceCode LIKE '%' + @ServiceCode + '%' ))        
 --AND ((@ServiceCodeID IS NULL OR LEN(@ServiceCodeID)=0) OR @ServiceCodeID=0 OR (V.ServiceCodeID=@ServiceCodeID))        
 AND ((@VisitTaskCategoryID IS NULL OR LEN(@VisitTaskCategoryID)=0) OR @VisitTaskCategoryID=0 OR (V.VisitTaskCategoryID=@VisitTaskCategoryID))        
 AND ((@VisitTaskVisitTypeID IS NULL OR LEN(@VisitTaskVisitTypeID)=0) OR @VisitTaskVisitTypeID=0 OR (V.VisitType=@VisitTaskVisitTypeID))        
 AND ((@VisitTaskCareTypeID IS NULL OR LEN(@VisitTaskCareTypeID)=0) OR @VisitTaskCareTypeID=0 OR (V.CareType=@VisitTaskCareTypeID))        
 ) AS TEMP          
  ) AS T1                    
 )                  
                   
 SELECT * FROM CTEVisitTaskList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                
END 
