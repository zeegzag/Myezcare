-- [GetReleaseNoteList] null,null,null,null,-1,'Title','ASC',1,100        
CREATE PROCEDURE [dbo].[GetReleaseNoteList]                
@Title NVARCHAR(200)=null,        
@Description NVARCHAR(MAX)=null,        
@StartDate DATETIME=null,        
@EndDate DATETIME=null,             
@IsDeleted BIGINT = -1,                    
@SortExpression NVARCHAR(100),                                
@SortType NVARCHAR(10),                              
@FromIndex INT,                              
@PageSize INT                    
AS                            
BEGIN                            
 ;WITH CTEReleaseNoteList AS                                  
 (                                   
 SELECT *,COUNT(T1.ReleaseNoteID) OVER() AS Count FROM                                   
 (                                  
  SELECT ROW_NUMBER() OVER (ORDER BY                         
  CASE WHEN @SortType = 'ASC' THEN                                  
   CASE                                         
    WHEN @SortExpression = 'Title' THEN Title                    
    WHEN @SortExpression = 'Description' THEN DescriptionWithOutCode                      
   END                                   
  END ASC,                                  
  CASE WHEN @SortType = 'DESC' THEN                                  
   CASE                                         
    WHEN @SortExpression = 'Title' THEN Title                    
    WHEN @SortExpression = 'Description' THEN DescriptionWithOutCode                  
   END                                  
  END DESC,         
                
  CASE WHEN @SortType = 'ASC' THEN                                  
   CASE                                         
    WHEN @SortExpression = 'StartDate' THEN StartDate                
    WHEN @SortExpression = 'EndDate' THEN EndDate                
   END                                   
  END ASC,                                  
  CASE WHEN @SortType = 'DESC' THEN                                  
   CASE                                         
    WHEN @SortExpression = 'StartDate' THEN StartDate                
    WHEN @SortExpression = 'EndDate' THEN EndDate                
   END                                  
  END DESC         
  ) AS Row,      
  * FROM      
  (Select ReleaseNoteID,Title,Description,StartDate,EndDate,IsDeleted,IsActive,    
  dbo.RemoveHTML(Description) AS DescriptionWithOutCode    
  FROM  ReleaseNotes      
  ) AS RN      
  WHERE      
  ((CAST(@IsDeleted AS BIGINT)=-1) OR RN.IsDeleted=@IsDeleted)        
  AND         
  ((@Title IS NULL OR LEN(@Title)=0) OR RN.Title LIKE '%' + @Title + '%')                        
  AND         
  ((@Description IS NULL OR LEN(@Description)=0) OR RN.DescriptionWithOutCode LIKE '%' + @Description + '%')        
  AND         
  ((@StartDate is null OR RN.StartDate >= @StartDate) and (@EndDate is null OR RN.EndDate<= @EndDate))         
 )As T1      
 )                                  
 SELECT * FROM CTEReleaseNoteList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                                
END