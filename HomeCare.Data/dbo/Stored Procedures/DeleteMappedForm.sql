CREATE PROCEDURE [dbo].[DeleteMappedForm]  
@TaskFormMappingID BIGINT  
AS  
BEGIN  
 UPDATE TaskFormMappings SET IsDeleted=1 WHERE TaskFormMappingID=@TaskFormMappingID
 SELECT 1;
END