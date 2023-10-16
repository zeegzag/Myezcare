CREATE PROCEDURE [dbo].[API_VerifyPatientByMobile]      
 @MobileNumber VARCHAR(10)  
AS                                            
BEGIN                                
 SELECT STUFF((      
   SELECT ',' + CONVERT(NVARCHAR,r.ReferralID)      
   FROM dbo.Referrals r      
   INNER JOIN dbo.ContactMappings cm ON r.ReferralID = cm.ReferralID AND cm.ContactTypeID=1      
   INNER JOIN dbo.Contacts c ON cm.ContactID = c.ContactID      
   WHERE c.Phone1=@MobileNumber      
   ORDER BY r.ReferralID ASC      
   FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '') AS ReferralIds   
END
