CREATE PROCEDURE [dbo].[UpdateLatLong]  
 @ID BIGINT,  
 @Latitude FLOAT,  
 @Longitude FLOAT,  
 @Type NVARCHAR(20),  
 @UpdatedDate DATETIME  
AS                    
BEGIN  
  
 DECLARE @ContactID BIGINT;  
  
 IF(@Type = 'Employee')  
 UPDATE Employees SET Latitude=@Latitude, Longitude=@Longitude, UpdatedDate=@UpdatedDate WHERE EmployeeID=@ID  
  
 IF(@Type = 'Referral')  
 BEGIN  
  SET @ContactID = (SELECT ContactID FROM ContactMappings WHERE ReferralID=@ID AND ContactTypeID=1)  
  
  UPDATE Contacts SET Latitude = @Latitude,Longitude=@Longitude,UpdatedDate=@UpdatedDate WHERE ContactID=@ContactID  
  
  --UPDATE c  
  --SET Latitude = @Latitude,Longitude=@Longitude,UpdatedDate=@UpdatedDate  
  -- from Contacts c  
  -- INNER JOIN ContactMappings cm  
  -- ON cm.ContactID = c.ContactID  
  --WHERE cm.ReferralID=@ID  
 END  
END