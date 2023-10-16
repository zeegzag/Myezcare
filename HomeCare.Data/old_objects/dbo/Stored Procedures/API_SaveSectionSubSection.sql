CREATE PROCEDURE [dbo].[API_SaveSectionSubSection]        
@ComplianceID BIGINT,        
@Name NVARCHAR(MAX),                    
@Value NVARCHAR(50),                  
@Type NVARCHAR(50),                  
@DocumentationType INT,                  
@IsTimeBased BIT,                  
@UserType INT,                
@EBFormID NVARCHAR(MAX),      
@ParentID BIGINT,                
@ServerCurrentDateTime DATETIME,                              
@LoggedInID BIGINT,                                
@SystemID VARCHAR(MAX)                               
AS                                
BEGIN                                
IF EXISTS (SELECT TOP 1 ComplianceID FROM Compliances WHERE UserType=@UserType AND DocumentationType=@DocumentationType AND DocumentName=@Name AND ParentID=@ParentID    
AND ComplianceID!=@ComplianceID)                    
BEGIN                                
 SELECT -1 AS Result;                    
 SELECT SectionID=ComplianceID,SectionName=DocumentName,Color=Value FROM Compliances                  
 WHERE IsDeleted=0 AND ParentID=0;                  
 RETURN;                    
END                    
 IF(@ComplianceID=0)        
 BEGIN        
  INSERT INTO Compliances(UserType,DocumentationType,DocumentName,IsTimeBased,Type,Value,ParentID,EBFormID,                
  IsDeleted,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID)                  
  VALUES (@UserType,@DocumentationType,@Name,@IsTimeBased,@Type,@Value,@ParentID,@EBFormID,                
  0,@ServerCurrentDateTime,@LoggedInID,@ServerCurrentDateTime,@LoggedInID,@SystemID)   
  SET @ComplianceID = @@IDENTITY; 
 END        
 ELSE        
 BEGIN        
  UPDATE Compliances SET DocumentationType=@DocumentationType,DocumentName=@Name,IsTimeBased=@IsTimeBased,Value=@Value,EBFormID=@EBFormID,        
  UpdatedBy=@LoggedInID,UpdatedDate=@ServerCurrentDateTime,SystemID=@SystemID      
  WHERE ComplianceID=@ComplianceID        
 END    
 
 DECLARE @SelectedRoles NVARCHAR(MAX)
 SELECT @SelectedRoles=SUBSTRING(        
  (SELECT ',' + CONVERT(NVARCHAR(100),R.RoleID)        
  FROM Roles R        
  ORDER BY R.RoleID        
  FOR XML PATH('')),2,200000)         
 
          
 INSERT INTO SectionPermissions(ComplianceID,RoleID)         
 SELECT @ComplianceID, T.VAL        
 FROM dbo.GetCSVTable(@SelectedRoles) T        
 LEFT JOIN SectionPermissions  SP ON SP.RoleID=T.Val AND SP.ComplianceID=@ComplianceID        
 WHERE SP.ComplianceID IS NULL     
        
 SELECT @ComplianceID AS Result;                    
  SELECT ComplianceID,SectionName=DocumentName,Color=Value FROM Compliances                  
  WHERE IsDeleted=0 AND ParentID=0 AND UserType=@UserType;        
END  