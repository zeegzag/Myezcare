--CreatedBy: Jemin                    
--CreatedDate: 10 June 2020                    
--Description: For Save CaptureC all in CaptureCall table                    
                    
CREATE PROCEDURE [dbo].[SaveCaptureCall]                                
 @Id BIGINT,                                                  
 @FirstName NVARCHAR(50),                                
 @LastName NVARCHAR(50),                     
 @Contact NVARCHAR(50),                     
 @Email NVARCHAR(50)=NULL,                                    
 @Address NVARCHAR(100),                                             
 @City NVARCHAR(50),                                
 @StateCode NVARCHAR(10),                                  
 @ZipCode NVARCHAR(15),                                            
 @loggedInUserId BIGINT,                  
 @Notes NVARCHAR(MAX),                
 @EmployeesIDs NVARCHAR(255),                
 @RoleIds NVARCHAR(255),              
 @CallType NVARCHAR(255),              
 @RelatedWithPatient NVARCHAR(255),              
 @InquiryDate Date=null,          
 @FileName NVARCHAR(MAX)=null,            
 @FilePath NVARCHAR(MAX)=null,            
 @GoogleFileId NVARCHAR(MAX)=null,      
 @Status NVARCHAR(MAX)=null,   
 @GroupIDs NVARCHAR(MAX)=null   
             
                                                   
AS                    
BEGIN                    
                
 IF(@Id=0)                                                  
  BEGIN  
   IF EXISTS (SELECT TOP 1 Id FROM CaptureCall WHERE FirstName=@FirstName AND LastName=@LastName AND Contact=@Contact)                            
 BEGIN                                        
 SELECT -1  RETURN;                                          
 END                      
    INSERT INTO CaptureCall                                                  
    (FirstName,LastName,Contact,Email,Flag,Address,City,StateCode,ZipCode,CreatedBy,CreatedDate,Notes,RoleIds,EmployeesIDs,CallType,RelatedWithPatient,InquiryDate,FileName,FilePath,OrbeonID,Status,GroupIDs)                                
    VALUES                                                  
    (@FirstName,@LastName,@Contact,@Email,1,@Address,@City,@StateCode,@ZipCode,@loggedInUserId,Getdate(),@Notes,@RoleIds,@EmployeesIDs,@CallType,@RelatedWithPatient,@InquiryDate,@FileName,@FilePath,@GoogleFileId,@Status,@GroupIDs);                        
        
              
              
  END                                                  
 ELSE                                                  
  BEGIN       
  IF(@GoogleFileId!=null)      
  BEGIN      
   Update CaptureCall SET                        
   FirstName=@FirstName,                        
   LastName=@LastName,                        
   Contact=@Contact,                        
   Email=@Email,                        
   Address=@Address,                        
   City=@City,                        
   StateCode=@StateCode,                        
   ZipCode=@ZipCode,                        
   UpdatedDate=GETUTCDATE(),                        
   UpdatedBy=@loggedInUserId,                  
   Notes=@Notes,                
   RoleIds=@RoleIds,                 
   EmployeesIDs=@EmployeesIDs,              
   CallType=@CallType,                 
   RelatedWithPatient=@RelatedWithPatient,              
   InquiryDate=@InquiryDate,       
   Status=@Status,   
   GroupIDs=@GroupIDs                  
   WHERE Id=@Id      
  END      
  ELSE       
  BEGIN                                                
   Update CaptureCall SET                        
   FirstName=@FirstName,                        
   LastName=@LastName,                        
   Contact=@Contact,                        
   Email=@Email,                        
   Address=@Address,                        
   City=@City,                        
   StateCode=@StateCode,                        
   ZipCode=@ZipCode,                        
   UpdatedDate=GETUTCDATE(),                        
   UpdatedBy=@loggedInUserId,                  
   Notes=@Notes,                
   RoleIds=@RoleIds,                 
   EmployeesIDs=@EmployeesIDs,              
   CallType=@CallType,                 
   RelatedWithPatient=@RelatedWithPatient,        
   InquiryDate=@InquiryDate,       
   Status=@Status ,      
   fileName=@FileName ,      
   FilePath=@FilePath ,      
   OrbeonID=@GoogleFileId,   
   GroupIDs=@GroupIDs                  
   WHERE Id=@Id                                                  
  END                                                  
                                              
  END                        
 SELECT 1; RETURN;                                              
                                              
END