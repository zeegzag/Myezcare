CREATE PROCEDURE [dbo].[SavePhysician]        
 -- Add the parameters for the stored procedure here                          
 @PhysicianID BIGINT,                          
 @FirstName NVARCHAR(50),        
 @MiddleName NVARCHAR(50),                
 @LastName NVARCHAR(50),            
 @Address NVARCHAR(100),                     
 @City NVARCHAR(50),        
 @StateCode NVARCHAR(10),          
 @ZipCode NVARCHAR(15),              
 @Email NVARCHAR(50)=NULL,                     
 @Phone NVARCHAR(20),                     
 @Mobile NVARCHAR(20),      
 @NPINumber NVARCHAR(20)=NULL,      
 @loggedInUserId BIGINT,                    
 @SystemID VARCHAR(100)                          
AS                          
BEGIN                          
                       
					   
 --IF EXISTS (SELECT TOP 1 PhysicianID FROM Physicians WHERE FirstName=@FirstName AND LastName=@LastName AND PhysicianID != @PhysicianID)                            

	 IF EXISTS (SELECT TOP 1 PhysicianID FROM Physicians WHERE ((  ( @NPINumber IS NOT NULL AND LEN(@NPINumber)>0 AND NPINumber=@NPINumber) 
	 OR ( @Email IS NOT NULL AND LEN(@Email)>0 AND Email=@Email)) AND PhysicianID != @PhysicianID))    
	 BEGIN                    
	 SELECT -1 RETURN;                      
	 END                    

                        
        
 IF(@PhysicianID=0)                          
 BEGIN                          
   INSERT INTO Physicians                          
   (FirstName,MiddleName,LastName,Address,City,StateCode,ZipCode,Email,Phone,Mobile,NPINumber,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,SystemID)        
   VALUES                          
   (@FirstName,@MiddleName,@LastName,@Address,@City,@StateCode,@ZipCode,@Email,@Phone,@Mobile,@NPINumber,        
   @loggedInUserId,GETUTCDATE(),@loggedInUserId,GETUTCDATE(),@SystemID);        
              
 END                          
 ELSE                          
 BEGIN                          
 UPDATE Physicians                           
 SET                                
 FirstName=@FirstName,                          
 MiddleName=@MiddleName,                
 LastName=@LastName,            
 Address=@Address,            
 City=@City,        
 StateCode=@StateCode,        
 ZipCode=@ZipCode,        
 Email=@Email,        
 Phone=@Phone,        
 Mobile=@Mobile,       
 NPINumber=@NPINumber,      
 UpdatedBy=@loggedInUserId,                          
 UpdatedDate=GETUTCDATE(),                          
 SystemID=@SystemID        
 WHERE PhysicianID=@PhysicianID;                          
 END                          
                      
                  
 SELECT 1; RETURN;                      
                      
                      
END
