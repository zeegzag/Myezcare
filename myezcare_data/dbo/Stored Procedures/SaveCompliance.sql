CREATE PROCEDURE [dbo].[SaveCompliance]          
@ComplianceID BIGINT,          
@UserType INT,        
@DocumentationType INT,        
@DocumentName NVARCHAR(MAX),        
@IsTimeBased BIT,      
@Type NVARCHAR(50),
@ParentID BIGINT,
@Value NVARCHAR(50),
@EBFormID NVARCHAR(MAX),    
@CurrentDate DATETIME,        
@loggedInUserId BIGINT,          
@SystemID VARCHAR(MAX)         
AS          
BEGIN          
IF EXISTS (SELECT TOP 1 ComplianceID FROM Compliances WHERE UserType=@UserType AND DocumentationType=@DocumentationType         
  AND DocumentName=@DocumentName  AND ComplianceID != @ComplianceID)          
BEGIN          
 SELECT -1 RETURN;          
END          
                   
IF(@ComplianceID=0)                              
BEGIN                              
 INSERT INTO Compliances(UserType,DocumentationType,DocumentName,IsTimeBased,EBFormID,ParentID,Type,Value,IsDeleted,CreatedBy,CreatedDate,UpdatedBy,    
 UpdatedDate,SystemID)            
 VALUES (@UserType,@DocumentationType,@DocumentName,@IsTimeBased,@EBFormID,@ParentID,@Type,@Value,0,@loggedInUserId,@CurrentDate,@loggedInUserId,    
 @CurrentDate,@SystemID)              
END                              
ELSE                              
BEGIN                              
 UPDATE Compliances                               
 SET                                    
 UserType=@UserType,                              
 DocumentationType=@DocumentationType,          
 DocumentName = @DocumentName,        
 IsTimeBased = @IsTimeBased,
 EBFormID=@EBFormID,
 ParentID=@ParentID,
 Type=@Type,
 Value=@Value,
 UpdatedBy=@loggedInUserId,                              
 UpdatedDate=@CurrentDate,                              
 SystemID=@SystemID            
 WHERE         
 ComplianceID=@ComplianceID;                              
END          
 SELECT 1; RETURN;                       
END
