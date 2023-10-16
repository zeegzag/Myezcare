--   EXEC HC_GetVehicleList @SortExpression = 'VehicleType', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50'  
CREATE PROCEDURE [dbo].[HC_GetVehicleList]                      
@VIN_Number VARCHAR(100) = NULL,                       
@TransportService VARCHAR(100) = NULL,  
@ContactID VARCHAR(10) = NULL,  
@Attendent VARCHAR(100) = NULL,                      
@Model VARCHAR(100) = NULL,                      
@BrandName VARCHAR(100) = NULL,                       
@Color VARCHAR(100) = NULL,                       
@IsDeleted BIGINT = -1,                        
@SortExpression VARCHAR(100),                          
@SortType VARCHAR(10),                        
@FromIndex INT,                        
@PageSize INT                                  
                                  
AS                                              
BEGIN                                              
                                              
;WITH List AS                                              
 (                                               
  SELECT *,COUNT(T1.VehicleID) OVER() AS Count FROM                                               
  (                                              
   SELECT ROW_NUMBER() OVER (ORDER BY                                               
                                          
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'VIN_Number' THEN t.VIN_Number END END ASC,                                        
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'VIN_Number' THEN t.VIN_Number END END DESC,                    
                     
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'TransportService' THEN t.TransportService END END ASC,                                        
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'TransportService' THEN t.TransportService END END DESC,    
   
  CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Attendent' THEN t.Attendent END END ASC,                                        
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Attendent' THEN t.Attendent END END DESC,    
                    
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Model' THEN t.Model END END ASC,                                        
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Model' THEN t.Model END END DESC,        
       
  CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'BrandName' THEN t.BrandName END END ASC,                                        
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'BrandName' THEN t.BrandName END END DESC,      
                     
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Color' THEN t.Color END END ASC,                                        
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Color' THEN t.Color END END DESC                     
                                      
   ) AS ROW,                                              
   t.*  FROM     (                                    
                          
SELECT v.VehicleID, v.VIN_Number, v.SeatingCapacity, v.VehicleType, v.BrandName, v.Model, v.Color, v.Attendent, v.ContactID,                 
  tm.Title AS ContactType, concat(tc.FirstName,' ',tc.LastName) AS TransportService, v.IsDeleted, v.CreatedDate                         
   FROM Vehicles v                  
   LEFT JOIN TransportContacts tc ON tc.ContactID = v.ContactID                  
   LEFT JOIN TransportMaster tm ON tm.TransportMasterID = tc.ContactType                  
   WHERE ((CAST(@IsDeleted AS BIGINT)=-1) OR v.IsDeleted=@IsDeleted)  
   AND ((@VIN_Number IS NULL OR LEN(v.VIN_Number)=0) OR ((v.VIN_Number LIKE '%'+@VIN_Number+'%' )))  
   AND ((@Attendent IS NULL OR LEN(v.Attendent)=0) OR ((v.Attendent LIKE '%'+@Attendent+'%' )))  
   AND ((@Model IS NULL OR LEN(v.Model)=0) OR ((v.Model LIKE '%'+@Model+'%' )))  
   AND ((@BrandName IS NULL OR LEN(v.BrandName)=0) OR ((v.BrandName LIKE '%'+@BrandName+'%' )))  
   AND ((@Color IS NULL OR LEN(v.Color)=0) OR ((v.Color LIKE '%'+@Color+'%' )))  
   AND (v.ContactID = @ContactID OR @ContactID IS NULL OR @ContactID = '')  
   AND ((@TransportService IS NULL OR LEN(tc.LastName)=0)                                 
   OR (                                
    (tc.FirstName LIKE '%'+@TransportService+'%' )OR                                  
    (tc.LastName  LIKE '%'+@TransportService+'%') OR                                  
    (tc.FirstName +' '+tc.LastName like '%'+@TransportService+'%') OR                                  
    (tc.LastName +' '+tc.FirstName like '%'+@TransportService+'%')))   
  
  ) t                        
                        
)  AS T1 )                                  
                                  
SELECT * FROM List WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                                               
END 