-- EXEC GetVisitTaskListForReferral 'Task','',3,'42','VisitTaskID','DESC',1,50    
-- EXEC GetVisitTaskListForReferral @ReferralID = '24236', @SortExpression = 'VisitTaskID', @SortType = 'DESC', @FromIndex = '1', @PageSize = '50'            
CREATE PROCEDURE [dbo].[GetVisitTaskListForReferral]                    
 @VisitTaskType VARCHAR(100) = NULL,                            
 @VisitTaskDetail VARCHAR(100) = NULL,                    
 @ReferralID NVARCHAR(100),         
 @CareTypeID NVARCHAR(100),                
 @SortExpression NVARCHAR(100),                              
 @SortType NVARCHAR(10),                            
 @FromIndex INT,                            
 @PageSize INT                            
AS                            
BEGIN                
-- Declare @temp Table(                  
-- ParentCategoryId bigint,                  
-- ParentCategoryName nvarchar(200)                  
--)                  
                  
--Insert Into @temp Select VisitTaskCategoryID,VisitTaskCategoryName From VisitTaskCategories Where ParentCategoryLevel is null                  
                
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
                       
                            
    CASE WHEN @SortType = 'ASC' THEN                            
      CASE                                   
      WHEN @SortExpression = 'TaskType' THEN VisitTaskType                             
      WHEN @SortExpression = 'TaskDetail' THEN VisitTaskDetail                       
      WHEN @SortExpression = 'ServiceCode' THEN ServiceCode    
      WHEN @SortExpression = 'CareType' THEN CareType                         
                                
      END                             
    END ASC,                            
    CASE WHEN @SortType = 'DESC' THEN                            
      CASE                                   
      WHEN @SortExpression = 'TaskType' THEN VisitTaskType                          
      WHEN @SortExpression = 'TaskDetail' THEN VisitTaskDetail                         
      WHEN @SortExpression = 'ServiceCode' THEN ServiceCode      
      WHEN @SortExpression = 'CareType' THEN CareType                    
      END                            
    END DESC                            
                                
                            
  ) AS Row, * FROM (                    
                             
   SELECT DISTINCT V.VisitTaskID,V.CareType As CareTypeID,DM.Title As CareType,VisitTaskType,VisitTaskDetail,V.IsDeleted, V.ServiceCodeID,ServiceCode = S.ServiceCode + '-' + S.ServiceName ,V.IsRequired, MinimumTimeRequired, VG1.VisitTaskCategoryName,VG.VisitTaskCategoryName as ParentCategoryName         
      
        
           
   FROM  VisitTasks V          
   --INNER JOIN Referrals R ON R.ReferralID=@ReferralID          
   --INNER JOIN ReferralPayorMappings RPM ON RPM.ReferralID=@ReferralID          
   --INNER JOIN PayorServiceCodeMapping PSM ON PSM.ServiceCodeID=V.ServiceCodeID AND PSM.PayorID=RPM.PayorID          
   LEFT JOIN ReferralTaskMappings RT ON RT.VisitTaskID= V.VisitTaskID AND RT.IsDeleted=0 AND RT.ReferralID=@ReferralID                  
   LEFT JOIN VisitTaskCategories VG ON V.VisitTaskCategoryID=VG.VisitTaskCategoryID            
   LEFT JOIN VisitTaskCategories VG1 ON V.VisitTaskSubCategoryID=VG1.VisitTaskCategoryID            
   --LEFT JOIN @temp as t on VG.ParentCategoryLevel = t.ParentCategoryId                
   LEFT JOIN ServiceCodes S ON S.ServiceCodeID= V.ServiceCodeID         
   LEFT JOIN DDMaster DM on DDMasterID = V.CareType   
   INNER JOIN referrals R  on ((ISNULL(v.CareType,0) = 0 OR v.CareType in (select val from dbo.GetCSVTable(R.CareTypeIds))) and R.ReferralID=@ReferralID)  
   WHERE V.IsDeleted=0                    
   AND ((@VisitTaskType IS NULL OR LEN(@VisitTaskType)=0) OR (VisitTaskType LIKE '%' + @VisitTaskType + '%'))                             
   AND ((@VisitTaskDetail IS NULL OR LEN(@VisitTaskDetail)=0) OR (VisitTaskDetail LIKE '%' + @VisitTaskDetail + '%'))       
   AND ((@CareTypeID IS NULL OR @CareTypeID=0) OR (V.CareType =@CareTypeID ) )                                
   AND  RT.ReferralTaskMappingID IS NULL                    
   ) AS TEMP                    
  ) AS T1                              
 )                            
                             
 SELECT * FROM CTEVisitTaskList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                
                  
END