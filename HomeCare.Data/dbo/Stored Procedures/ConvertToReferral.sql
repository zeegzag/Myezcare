        
 -- [ConvertToReferral] '3','er','er'                      
                        
CREATE PROCEDURE [dbo].[ConvertToReferral]                                    
 @Id BIGINT,                                                      
 @FirstName NVARCHAR(MAX),                                    
 @LastName NVARCHAR(MAX),                          
 @Email NVARCHAR(MAX)=NULL,                                        
 @Address NVARCHAR(MAX)=NULL,                                                 
 @City NVARCHAR(MAX)=NULL,                                    
 @StateCode NVARCHAR(MAX)=NULL,                                      
 @ZipCode BIGINT=0,                                                
 @loggedInUserId BIGINT =0,      
 @Phone BIGINT=0,
 @Status  NVARCHAR(MAX)=NULL,
 @EmployeesIDs   NVARCHAR(MAX)=NULL,              
 @GroupIDs   NVARCHAR(MAX)=NULL
                                                       
AS                        
BEGIN                        
     declare  @ReferralID bigint       
  declare  @ContactID bigint       
  declare  @ClientID bigint                                               
    
IF(@Id>0)
 IF EXISTS (SELECT TOP 1 ReferralID FROM referrals WHERE FirstName=@FirstName AND LastName=@LastName)                          
 BEGIN                                      
 SELECT -1 as ReferralID RETURN;                                        
 END 
BEGIN
   INSERT INTO Clients (FirstName,LastName,Dob,Gender,ClientNumber,AHCCCSID,CISNumber,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,IsDeleted)      
  VALUES(@FirstName,@LastName,getdate(),'M',0,0,0,getdate(),@loggedInUserId,getdate(),@loggedInUserId,0,0)      
  SELECT @ClientID=ClientID FROM Clients WHERE FirstName=@FirstName and LastName= @LastName     
-----------------------------------------------------------------------------------------------                            
    INSERT INTO referrals                                                      
    (FirstName,LastName,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,ReferralTrackingID,Gender,ClientID,Assignee,GroupIDs)                                    
    VALUES                                                      
    (@FirstName,@LastName,getdate(),@loggedInUserId,getdate(),1,0,@Id,'M',@ClientID,@EmployeesIDs,@GroupIDs);       
 SELECT @ReferralID=ReferralID from referrals where ReferralTrackingID= @Id      
 --------------------------------------------------------------------------------------------------------------      
 INSERT INTO Contacts (FirstName,LastName,Email,Address,City,State,ZipCode,Phone1,LanguageID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,IsDeleted)       
 VALUES(@FirstName,@LastName,@Email,@Address,@City,@StateCode,@ZipCode,@Phone,'',getdate(),@loggedInUserId,getdate(),@loggedInUserId,0,0);      
 SELECT @ContactID=ContactID FROM Contacts WHERE FirstName=@FirstName and LastName= @LastName       
 ---------------------------------------------------------------------------------------------------------------      
 --INSERT INTO Clients (FirstName,LastName,Dob,Gender,ClientNumber,AHCCCSID,CISNumber,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,IsDeleted)      
 -- VALUES(@FirstName,@LastName,getdate(),'M',0,0,0,getdate(),@loggedInUserId,getdate(),@loggedInUserId,0,0)      
 -- SELECT @ClientID=ClientID FROM Clients WHERE FirstName=@FirstName and LastName= @LastName      
 ---------------------------------------------------------------------------------------------------------------       
 INSERT INTO ContactMappings(ContactID,ReferralID,ClientID,IsEmergencyContact,ContactTypeID,Relation,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID)      
  VALUES(@ContactID,@ReferralID,@ClientID,0,1,'',getdate(),@loggedInUserId,getdate(),@loggedInUserId,0)    
  END  
 ---------------------------------------------------------------------------------------------------------------    
 IF(@ReferralID>0)  
 BEGIN
 UPDATE CaptureCall SET Status=@Status   WHERE Id=@Id
 END 
--declare  @ReferralID bigint                                     
    SELECT top 1  ReferralID from referrals where ReferralTrackingID= @Id  and FirstName=@FirstName  return;                          
END 