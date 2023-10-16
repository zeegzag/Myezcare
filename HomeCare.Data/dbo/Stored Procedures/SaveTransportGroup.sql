--TransportGroup      
create PROCEDURE [dbo].[SaveTransportGroup]            
(          
@TransportGroupID BIGINT = 0,       
@Name    NVARCHAR(100) =NULL,      
@FacilityID   BIGINT  = 0,      
@TripDirection  BIGINT  = 0,      
@StartDate   DATETIME  = NULL,      
@EndDate   DATETIME  = NULL,      
@VehicleID   BIGINT  = 0,      
@RouteDesc   NVARCHAR(200)  = NULL,      
@loggedInUserID  BIGINT = 0 --,        
      
)          
AS                            
 IF(@TransportGroupID=0)                           
BEGIN                          
                          
  INSERT INTO [fleet].TransportGroup           
  (          
    [Name]      
  , FacilityID      
  , TripDirection      
  , StartDate      
  , EndDate      
  , VehicleID      
  , RouteDesc      
  --, IsDeleted      
  , CreatedDate      
  , CreatedBy      
  , UpdatedDate      
  , UpdatedBy      
  )                           
  VALUES           
  (          
   @Name   ,          
   @FacilityID  ,       
   @TripDirection ,      
   @Startdate   ,          
   @EndDate   ,          
   @VehicleID    ,         
   @RouteDesc,      
   GETDATE()   ,          
   @loggedInUserID  ,          
   GETDATE()   ,          
   @loggedInUserID       
  );                          
                             
   SELECT 1; RETURN;                           
END      
ELSE                                                        
BEGIN                                              
 UPDATE [fleet].TransportGroup                                                         
 SET                            
  Name   = @Name   ,      
  FacilityID  = @FacilityID  ,      
  TripDirection = @TripDirection ,      
  StartDate  = @StartDate  ,      
  EndDate   = @EndDate  ,      
  VehicleID  = @VehicleID  ,      
  RouteDesc  = @RouteDesc  ,      
  UpdatedDate = getdate()   ,          
  UpdatedBy = @loggedInUserID       
 WHERE       
  TransportGroupID=@TransportGroupID;                           
                          
 SELECT 1; RETURN;                           
 END 