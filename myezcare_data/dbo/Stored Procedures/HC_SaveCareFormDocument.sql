CREATE PROCEDURE [dbo].[HC_SaveCareFormDocument] 
@FileName nvarchar(max),  
@FilePath nvarchar(max),  
@KindOfDocument nvarchar(max),  
@UserType int,  
@CareFormID bigint,  
@CurrentDate datetime,  
@loggedInUserId bigint,  
@SystemID nvarchar(max), 
@UserID bigint=null 
AS 
BEGIN  
BEGIN TRANSACTION trans                                                                        
      
BEGIN TRY     
   
 SELECT @UserID=cf.ReferralID FROM dbo.CareForms cf WHERE cf.CareFormID=@CareFormID  --SELECT @UserID   
 INSERT INTO dbo.ReferralDocuments  
 (FileName,FilePath,KindOfDocument,DocumentTypeID,UserID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,UserType)  
 VALUES  
 (@FileName,@FilePath,@KindOfDocument,0,@UserID,@CurrentDate,@loggedInUserId,@CurrentDate,@loggedInUserId,@SystemID,@UserType)  
   SELECT 1 AS TransactionResultId;               
      
 IF @@TRANCOUNT > 0                                                                        
 BEGIN                                                                         
  COMMIT TRANSACTION trans                                                                
 END                                                                        
END TRY                                                         
BEGIN CATCH                                          
 SELECT -1 AS TransactionResultId,ERROR_MESSAGE() AS ErrorMessage;                                                                        
 IF @@TRANCOUNT > 0                                                                        
 BEGIN                                                                        
  ROLLBACK TRANSACTION trans                                                                         
 END                                                          
END CATCH                  
 END
