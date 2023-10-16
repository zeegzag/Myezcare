CREATE PROCEDURE [dbo].[VisitTaskFormEditCompliance]    
@TaskFormMappingID BIGINT,
@ComplianceID INT,
@CurrentDateTime DATETIME,
@LoggedIn BIGINT    
AS    
    
BEGIN    
     
 UPDATE TaskFormMappings SET ComplianceID=@ComplianceID, UpdatedBy=@LoggedIn, UpdatedDate=@CurrentDateTime
 WHERE TaskFormMappingID=@TaskFormMappingID
END
