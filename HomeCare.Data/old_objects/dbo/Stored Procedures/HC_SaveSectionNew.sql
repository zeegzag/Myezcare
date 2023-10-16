CREATE PROCEDURE [dbo].[HC_SaveSectionNew]              
@Name NVARCHAR(MAX),                
@Value NVARCHAR(50),              
@Type NVARCHAR(50),              
@DocumentationType INT,              
@IsTimeBased BIT,              
@UserType INT,            
@EBFormID NVARCHAR(MAX),            
@CurrentDate DATETIME,                          
@LoggedInID BIGINT,                            
@SystemID VARCHAR(MAX),      
@SelectedRoles NVARCHAR(1000),  
@RoleID BIGINT                    
AS                            
BEGIN                            
IF EXISTS (SELECT TOP 1 ComplianceID FROM Compliances WHERE UserType=@UserType AND DocumentationType=@DocumentationType AND DocumentName=@Name)                
BEGIN                            
 SELECT -1 AS Result;                
 SELECT SectionID=C.ComplianceID,SectionName=DocumentName,Color=Value FROM Compliances C  
 INNER JOIN SectionPermissions SP ON SP.ComplianceID= C.ComplianceID AND SP.RoleID=@RoleID                       
 WHERE IsDeleted=0 AND ParentID=0;              
 RETURN;                
END                
              
 INSERT INTO Compliances(UserType,DocumentationType,DocumentName,IsTimeBased,Type,Value,ParentID,EBFormID,            
 IsDeleted,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID)              
 VALUES (@UserType,@DocumentationType,@Name,@IsTimeBased,@Type,@Value,0,@EBFormID,            
 0,@CurrentDate,@LoggedInID,@CurrentDate,@LoggedInID,@SystemID)   
        
      
 DECLARE @ComplianceID BIGINT=@@IDENTITY;      
      
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
 
 
 
 
              
 SELECT 1 AS Result;                
 SELECT C.ComplianceID,SectionName=DocumentName,Color=Value FROM Compliances   C           
 INNER JOIN SectionPermissions SP ON SP.ComplianceID= C.ComplianceID AND SP.RoleID=@RoleID                       
 WHERE IsDeleted=0 AND ParentID=0 AND UserType=@UserType;         
 
        
             
END 
