CREATE PROCEDURE [dbo].[DeleteFormTag]
@OrganizationFormTagID BIGINT
AS  
BEGIN   
  
DELETE FROM OrganizationFormTags WHERE OrganizationFormTagID=@OrganizationFormTagID
  
END