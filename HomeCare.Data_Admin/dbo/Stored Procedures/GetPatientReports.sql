CREATE PROCEDURE [dbo].[GetPatientReports]      
@ReportName NVARCHAR(max) = NULL,     
@ReportDescription NVARCHAR(max) = NULL,     
@SortExpression NVARCHAR(100),                                  
@SortType NVARCHAR(10)  
--@FromIndex INT,                                
--@PageSize INT                    
                    
AS                                
BEGIN                                
                                
--;WITH List AS                                
-- (                                 
  SELECT *,COUNT(T1.ReportID) OVER() AS Count FROM                                 
  (                                
   SELECT ROW_NUMBER() OVER (ORDER BY                                 
                            
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ReportName' THEN t.ReportName END END ASC,                          
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ReportName' THEN t.ReportName END END DESC,      
       
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ReportDescription' THEN t.ReportDescription END END ASC,                          
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ReportDescription' THEN t.ReportDescription END END DESC                
                        
   ) AS ROW,                                
   t.*  FROM     (                      
            
SELECT * from ReportMaster RM where RM.IsDeleted=0 AND RM.Category='Patient' AND RM.IsDisplay=1      
AND      
((@ReportName IS NULL OR LEN(RM.ReportName)=0) OR     
((RM.ReportName LIKE '%'+@ReportName+'%' )))     
AND      
((@ReportDescription IS NULL OR LEN(RM.ReportDescription)=0) OR     
((RM.ReportDescription LIKE '%'+@ReportDescription+'%' )))     
    
  ) t          
          
)  AS T1   
--)                                      
--SELECT * FROM List WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                                 
END