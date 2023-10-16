-- EXEC GetVisitTaskList @ServiceCodeID = '0', @IsDeleted = '0', @SortExpression = 'VisitTaskType', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50'
CREATE PROCEDURE [dbo].[GetPreferenceList]        
 @PreferenceType VARCHAR(100) = NULL,        
 @PreferenceName VARCHAR(1000) = NULL,
 @IsDeleted BIGINT = -1,        
 @SortExpression NVARCHAR(100),          
 @SortType NVARCHAR(10),        
 @FromIndex INT,        
 @PageSize INT        
AS        
BEGIN        
 ;WITH CTEPreferenceList AS        
 (         
  SELECT *,COUNT(T1.PreferenceID) OVER() AS Count FROM         
  (        
   SELECT ROW_NUMBER() OVER (ORDER BY         
   CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'PreferenceID' THEN PreferenceID END END ASC,        
   CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'PreferenceID' THEN PreferenceID END END DESC,   
   
   
   CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'PreferenceName' THEN PreferenceName END END ASC,        
   CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'PreferenceName' THEN PreferenceName END END DESC,   

   CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'KeyType' THEN KeyType END END ASC,        
   CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'KeyType' THEN KeyType END END DESC
        
  ) AS Row, * FROM (
         
   SELECT p.*
   FROM  Preferences P
   WHERE ((CAST(@IsDeleted AS BIGINT)=-1) OR P.IsDeleted=@IsDeleted)        
   AND ((@PreferenceType IS NULL OR LEN(@PreferenceType)=0) OR (KeyType LIKE '%' + @PreferenceType + '%'))         
   AND ((@PreferenceName IS NULL OR LEN(@PreferenceName)=0) OR (PreferenceName LIKE '%' + @PreferenceName + '%'))           
   ) AS TEMP
  ) AS T1          
 )        
         
 SELECT * FROM CTEPreferenceList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)         
END