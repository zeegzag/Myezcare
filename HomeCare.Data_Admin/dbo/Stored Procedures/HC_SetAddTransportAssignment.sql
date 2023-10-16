--  exec HC_SetAddTransportAssignment 0        
        
CREATE PROCEDURE [dbo].[HC_SetAddTransportAssignment]                                    
@TransportID BIGINT = 0                         
                                   
AS                                    
BEGIN                      
                      
 SELECT *  FROM [DevHomecare].[dbo].[Transport] WHERE TransportID = @TransportID;            
           
 SELECT FacilityID,FacilityName  FROM [DevHomecare].[dbo].[Facilities];           
   SELECT OrganizationID, CompanyName AS OrganizationName FROM Organizations where IsDeleted=0          
    SELECT VehicleID,VehicleName=CONCAT(VIN_Number,'-',Model,'-',BrandName)  FROM Vehicles    
  
  select 0    
  
select dm.DDMasterID as RouteCode  ,dm.Title AS RouteName     
from [DevHomecare].[dbo].ddmaster dm      
inner join [DevHomecare].[dbo].lu_DDMasterTypes lu on lu.DDMasterTypeID = dm.ItemType      
where lu.Name='Route'   and dm.IsDeleted=0    
  
END 