                
CREATE PROCEDURE [dbo].[SaveTransportAssignment]                  
@TransportID BIGINT = 0,                
@FacilityID BIGINT = 0,                 
@StartDate DateTime = NULL,                 
@EndDate DateTime = NULL,                 
@Attendent VARCHAR(100) = NULL,                
@VehicleID BIGINT= 0,                 
@OrganizationID BIGINT = 0,                 
@loggedInUserID BIGINT = 0,                 
@SystemID VARCHAR(100) = NULL,  
@RouteCode BIGINT= 0  
                             
AS                  
 IF(@TransportID=0)                 
BEGIN                
                
INSERT INTO Transport (FacilityID,StartDate,EndDate,Attendent,VehicleID,OrganizationID,IsDeleted,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,RouteCode)                 
VALUES (@FacilityID, @StartDate, @EndDate, @Attendent, @VehicleID, @OrganizationID, 0, getdate(), @loggedInUserID, getdate(), @loggedInUserID,@RouteCode);                
                 
 SELECT 1; RETURN;                 
END                
                
 ELSE                                              
 BEGIN                
                
    UPDATE Transport                                               
   SET                  
   FacilityID=@FacilityID,                 
   StartDate=@StartDate,                 
   EndDate=@EndDate,                 
   Attendent=@Attendent,                 
   VehicleID=@VehicleID,                 
   OrganizationID=@OrganizationID,    
   UpdatedDate=GETUTCDATE(),                 
   UpdatedBy=@loggedInUserID,  
   RouteCode = @RouteCode  
   --SystemID=@SystemID                
    WHERE TransportID=@TransportID;                 
                
 SELECT 1; RETURN;                 
 END 