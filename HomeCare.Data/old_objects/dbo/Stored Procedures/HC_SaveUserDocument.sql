CREATE PROCEDURE [dbo].[HC_SaveUserDocument]    
@FileName VARCHAR(MAX),      
@FilePath VARCHAR(MAX),    
@KindOfDocument VARCHAR(20),
@UserID BIGINT,    
@UserType INT,    
@CurrentDate DATETIME,  
@LoggedInUserID BIGINT,      
@SystemID VARCHAR(30)    
AS      
BEGIN      
    
INSERT INTO ReferralDocuments   
(FileName,FilePath,KindOfDocument,DocumentTypeID,UserID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,UserType)  
VALUES  
(@FileName,@FilePath,@KindOfDocument,0,@UserID,@CurrentDate,@LoggedInUserID,@CurrentDate,@LoggedInUserID,@SystemID,@UserType)      
      
END
