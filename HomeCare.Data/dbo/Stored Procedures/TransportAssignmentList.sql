--   EXEC HC_GetVehicleList @SortExpression = 'VehicleType', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50'              
CREATE PROCEDURE [dbo].[TransportAssignmentList]                                  
@FacilityID VARCHAR(100) = NULL,                                   
@VehicleID VARCHAR(100) = NULL,             
@RouteCode VARCHAR(100) = NULL,            
@StartDate VARCHAR(10) = NULL,              
@EndDate VARCHAR(100) = NULL,                                  
@Attendent VARCHAR(100) = NULL,                                                        
@IsDeleted BIGINT = -1,                                    
@SortExpression VARCHAR(100) = '',                                      
@SortType VARCHAR(10) = 'DESC',                                    
@FromIndex INT=1,                                    
@PageSize INT  =10                                            
                                              
AS                                                          
BEGIN                                                            
;WITH List AS                                                          
 (                                                           
  SELECT *,COUNT(T1.TransportID) OVER() AS Count FROM                                                           
  (                                                          
   SELECT ROW_NUMBER() OVER (ORDER BY                                                           
                    
     CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'TransportID' THEN t.TransportID END END ASC,                                                    
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'TransportID' THEN t.TransportID END END DESC,         
      
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN  @SortExpression = '' or @SortExpression = 'FacilityName' THEN t.FacilityName END END ASC,                                                    
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = '' or @SortExpression = 'FacilityName' THEN t.FacilityName END END DESC,                                
                                 
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'VehicleName' THEN t.VehicleName END END ASC,                                                    
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'VehicleName' THEN t.VehicleName END END DESC,                
               
  CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Attendent' THEN t.Attendent END END ASC,                                                    
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Attendent' THEN t.Attendent END END DESC,                
                                
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'StartDate' THEN t.StartDate END END ASC,                                                    
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'StartDate' THEN t.StartDate END END DESC,                    
                   
  CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'EndDate' THEN t.EndDate END END ASC,                                                    
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'EndDate' THEN t.EndDate END END DESC   ,          
                   
  CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'RouteCode' THEN t.RouteCode END END ASC,                                                    
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'RouteCode' THEN t.RouteCode END END DESC             
                                                    
                                                  
   ) AS ROW,                                                          
   t.*  FROM     (                                                
                                      
select             
Tr.TransportID,Tr.FacilityID,F.FacilityName, V.VehicleID,          
VehicleName=CONCAT(V.VIN_Number,'-',V.Model,'-',V.BrandName),Tr.StartDate,Tr.EndDate,Tr.Attendent,Tr.OrganizationID,          
Tr.IsDeleted,Tr.CreatedDate,Tr.CreatedBy,Tr.UpdatedDate,Tr.UpdatedBy  ,          
Tr.RouteCode, DDMaster.Title as RouteName          
from Transport Tr  (NOLOCK)           
INNER JOIN Facilities F  (NOLOCK) ON F.FacilityID=Tr.FacilityID            
INNER JOIN [Admin_Myezcare_Live].DBO.Vehicles V  (NOLOCK) ON V.VehicleID=Tr.VehicleID             
INNER JOIN DDMaster  (NOLOCK) on DDMaster.DDMasterID = Tr.RouteCode          
  -- WHERE ((CAST(@IsDeleted AS BIGINT)=-1) OR T.IsDeleted=@IsDeleted)              
where           
 ((CAST(@IsDeleted AS BIGINT)=-1) OR Tr.IsDeleted=@IsDeleted) and             
 (Tr.FacilityID=@FacilityID  or Isnull(@FacilityID,'') ='') and            
 (Tr.VehicleID=@VehicleID  or Isnull(@VehicleID,'') ='') and            
 (Tr.Attendent like @Attendent +'%' or Isnull(@Attendent,'') ='') and          
 (Tr.StartDate >= @StartDate or Isnull(@StartDate,'') ='') and           
    (Tr.EndDate <= @EndDate or Isnull(@EndDate ,'') ='') AND          
 (Tr.RouteCode = @RouteCode or Isnull(@RouteCode ,'') ='')          
  ) t                                    
                                    
)  AS T1 )                                              
                                              
SELECT * FROM List WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                                                           
END 