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
@SystemID VARCHAR(MAX),
@SelectedRoles NVARCHAR(1000)         
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
  SET @ComplianceID =@@IDENTITY;            
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

IF(LEN(@SelectedRoles)=0)      
 BEGIN      
 SELECT @SelectedRoles=SUBSTRING(      
  (SELECT ',' + CONVERT(NVARCHAR(100),R.RoleID)      
  FROM Roles R      
  ORDER BY R.RoleID      
  FOR XML PATH('')),2,200000)       
 END      
           
 PRINT @ComplianceID      
 PRINT @SelectedRoles      
        
 INSERT INTO SectionPermissions(ComplianceID,RoleID)       
 SELECT @ComplianceID, T.VAL      
 FROM dbo.GetCSVTable(@SelectedRoles) T      
 LEFT JOIN SectionPermissions  SP ON SP.RoleID=T.Val AND SP.ComplianceID=@ComplianceID      
 WHERE SP.ComplianceID IS NULL      
       
 DELETE SP      
 FROM SectionPermissions SP      
 LEFT JOIN dbo.GetCSVTable(@SelectedRoles) T ON T.Val=SP.RoleID AND SP.ComplianceID=@ComplianceID      
 WHERE T.VAL IS NULL AND SP.ComplianceID=@ComplianceID 
      
 SELECT 1; RETURN;                       
END
