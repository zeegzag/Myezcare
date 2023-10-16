--EXEC API_ValidateReferralContactNumber '+266696687' 
CREATE PROCEDURE [dbo].[API_ValidateReferralContactNumber]  
@PhoneNo nvarchar(50) AS BEGIN  
-- SET NOCOUNT ON added to prevent extra result sets from  -- interfering with SELECT statements.  SET NOCOUNT ON;   
SELECT STUFF((  
   SELECT ',' + CONVERT(NVARCHAR,r.ReferralID)  
   FROM dbo.Referrals r  
   INNER JOIN dbo.ContactMappings cm ON r.ReferralID = cm.ReferralID AND cm.ContactTypeID=1  
   INNER JOIN dbo.Contacts c ON cm.ContactID = c.ContactID  
   WHERE c.Phone1=@PhoneNo  
   ORDER BY r.ReferralID ASC  
   FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '') AS ReferralIds END