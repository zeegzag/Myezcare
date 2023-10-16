            
CREATE PROCEDURE [dbo].[TransportContactAddUpdate]              
@ContactID BIGINT = 0,            
@FirstName VARCHAR(100) = NULL,             
@LastName VARCHAR(100) = NULL,             
@Email VARCHAR(100) = NULL,             
@Phone VARCHAR(100) = NULL,            
@MobileNumber VARCHAR(100) = NULL,             
@Fax VARCHAR(100) = NULL,             
@ApartmentNo VARCHAR(100) = NULL,             
@Address VARCHAR(100) = NULL,             
@City VARCHAR(100) = NULL,             
@State VARCHAR(100) = NULL,             
@ZipCode VARCHAR(100),             
@ContactType VARCHAR(100) = NULL,        
@OrganizationID VARCHAR(100) = NULL,   
@IsDeleted bit =0,            
@loggedInUserID BIGINT = 0,             
@SystemID VARCHAR(100) = NULL            
                         
AS              
 IF(@ContactID=0)             
BEGIN            
            
INSERT INTO TransportContacts (FirstName, LastName, Email, Phone, MobileNumber, Fax, ApartmentNo, Address, City, State, ZipCode, ContactType, OrganizationID, IsDeleted, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy, SystemID)             
VALUES (@FirstName, @LastName, @Email, @Phone, @MobileNumber, @Fax, @ApartmentNo, @Address, @City, @State, @ZipCode, @ContactType, @OrganizationID, 0, GETUTCDATE(), @loggedInUserID, GETUTCDATE(), @loggedInUserID, @SystemID);            
             
 SELECT 1; RETURN;             
END            
            
 ELSE                                          
 BEGIN            
            
    UPDATE TransportContacts                                           
   SET              
   FirstName=@FirstName,             
   LastName=@LastName,             
   Email=@Email,             
   Phone=@Phone,             
   MobileNumber=@MobileNumber,             
   Fax=@Fax,             
   ApartmentNo=@ApartmentNo,             
   Address=@Address,             
   City=@City,             
   State=@State,             
   ZipCode=@ZipCode,             
   ContactType=@ContactType,   
   OrganizationID=@OrganizationID,  
   UpdatedDate=GETUTCDATE(),             
   UpdatedBy=@loggedInUserID,             
   SystemID=@SystemID            
    WHERE ContactID=@ContactID;             
            
 SELECT 1; RETURN;             
 END 