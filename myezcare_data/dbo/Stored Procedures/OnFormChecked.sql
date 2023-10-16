CREATE PROCEDURE [dbo].[OnFormChecked]    
@TaskFormMappingID BIGINT,
@IsRequired BIT,
@CurrentDateTime DATETIME,
@LoggedIn BIGINT    
AS    
    
BEGIN    
     
 UPDATE TaskFormMappings SET IsRequired=@IsRequired, UpdatedBy=@LoggedIn, UpdatedDate=@CurrentDateTime
 WHERE TaskFormMappingID=@TaskFormMappingID
END
