CREATE PROCEDURE [dbo].[GetOrgFormTagList]  
@OrganizationFormID BIGINT  
AS          
BEGIN        
 SELECT OFT.*,FT.FormTagName FROM OrganizationFormTags OFT
 INNER JOIN FormTags FT ON FT.FormTagID=OFT.FormTagID
 WHERE OrganizationFormID=@OrganizationFormID  
END