          
CREATE PROCEDURE [dbo].[VehicleAddUpdate]                
(  
@VehicleID BIGINT = 0,              
@VIN_Number VARCHAR(100) = NULL,               
@SeatingCapacity BIGINT = 0,                 
@VehicleType VARCHAR(100) = NULL,               
@BrandName VARCHAR(100) = NULL,         
@Model VARCHAR(100) = NULL,         
@Color VARCHAR(100) = NULL,        
@Attendent VARCHAR(100) = NULL,        
@ContactID VARCHAR(100) = NULL,            
@IsDeleted bit =0,              
@loggedInUserID BIGINT = 0,               
@SystemID VARCHAR(100) = NULL,   
@note NVARCHAR(255) = NULL,  
@EmployeeID BIGINT = NULL  
)                           
AS                
 IF(@VehicleID=0)               
BEGIN              
              
INSERT INTO Vehicles (VIN_Number, SeatingCapacity, VehicleType, BrandName, Model, Color, Attendent, ContactID, IsDeleted, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy, SystemID, note, EmployeeID)               
VALUES (@VIN_Number, @SeatingCapacity, @VehicleType, @BrandName, @Model, @Color, @Attendent, @ContactID, 0, GETUTCDATE(), @loggedInUserID, GETUTCDATE(), @loggedInUserID, @SystemID, @note, @EmployeeID);              
              
 SELECT 1; RETURN;               
END              
              
 ELSE                                            
 BEGIN              
              
    UPDATE Vehicles                                             
   SET                
   VIN_Number=@VIN_Number,               
   SeatingCapacity=@SeatingCapacity,               
   VehicleType=@VehicleType,               
   BrandName=@BrandName,         
   Model=@Model,        
   Color=@Color,        
   Attendent=@Attendent,        
   ContactID=@ContactID,                  
   UpdatedDate=GETUTCDATE(),               
   UpdatedBy=@loggedInUserID,               
   SystemID=@SystemID ,  
   note = @note,  
   EmployeeID = @EmployeeID  
    WHERE VehicleID=@VehicleID;               
              
 SELECT 1; RETURN;               
 END   