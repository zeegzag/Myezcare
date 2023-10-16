  
  
-- EXEC [rpt].[VehicleList]  
  
CREATE PROCEDURE [rpt].[FacilityList]   
  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
      
 WITH CTE AS (  
 --SELECT 0 AS VehicleID,'All' AS VehicleName  
 --UNION ALL  
 SELECT FacilityID,FacilityName,[Address]  FROM [Facilities] (nolock) where IsNull(IsDeleted,0)=0   
 --SELECT VehicleID,VehicleName=CONCAT(VIN_Number,'-',Model,'-',BrandName)  FROM [DEVAdmin].[dbo].Vehicles   (nolock)           
 --where IsDeleted=0   
 )  
 SELECT * FROM CTE  
 WHERE FacilityName IS NOT NULL  
 ORDER BY CASE WHEN FacilityName = 'All' THEN '0'  
  ELSE FacilityName END ASC;  
END
GO

