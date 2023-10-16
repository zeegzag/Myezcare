--  exec HC_SetAddTransportAssignment 0                      
                      
CREATE PROCEDURE [dbo].[HC_SetAddTransportAssignmentGroup]                                                
@TransportID BIGINT = 0                                       
                                                 
AS                                                  
BEGIN              
             
 SELECT FacilityID,FacilityName  FROM [Live_Beta].[dbo].[Facilities] (nolock);                         
                
 SELECT dm.DDMasterID as TripDirectionId  ,dm.Title AS TripDirectionName                   
 FROM [Live_Beta].[dbo].ddmaster dm   (nolock)                 
 INNER JOIN [Live_Beta].[dbo].lu_DDMasterTypes lu (nolock) on lu.DDMasterTypeID = dm.ItemType                    
 WHERE lu.Name='Trip Direction'   and dm.IsDeleted=0                  
                
 SELECT VehicleID,VehicleName=CONCAT(VIN_Number,'-',Model,'-',BrandName)  FROM Vehicles   (nolock)             
            
 SELECT 0            
            
 SELECT 0            
        
 SELECT  RegionID,RegionName from  [Live_Beta].[dbo].Regions (nolock) ORDER BY RegionName ASC;        
       
 SELECT 0        
END 