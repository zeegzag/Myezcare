CREATE PROCEDURE [dbo].[API_UpdatePatientLatLong]    
 @ContactID BIGINT,
 @EmployeeID BIGINT,
 @Latitude DECIMAL(10,7),              
 @Longitude DECIMAL(10,7),
 @UpdatedDate DateTime
AS          
BEGIN        
 DECLARE @OldLatitude DECIMAL(10,7)
 DECLARE @OldLongitude DECIMAL(10,7)

 SELECT @OldLatitude=Latitude,@OldLongitude=Longitude FROM Contacts WHERE ContactID=@ContactID

 UPDATE Contacts SET Latitude=@Latitude,Longitude=@Longitude,OldLatitude=@OldLatitude,OldLongitude=@OldLongitude,
 UpdatedBy=@EmployeeID,UpdatedDate=@UpdatedDate
 WHERE ContactID=@ContactID
     
 SELECT 1;    
      
END