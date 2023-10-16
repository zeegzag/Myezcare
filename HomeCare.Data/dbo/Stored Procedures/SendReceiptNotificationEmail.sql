
-- EXEC SendReceiptNotificationEmail @ReferralID = '2', @@EmailTemplateTypeID = '1'          
CREATE  procedure [dbo].[SendReceiptNotificationEmail]          
@ReferralID BIGINT=0,          
@EmailTemplateTypeID VARCHAR(100)=null 
AS           
BEGIN           
 SELECT * FROM EmailTemplates where EmailTemplateTypeID=@EmailTemplateTypeID;          
            
 SELECT ref.ReferralID,ref.LastName + ', ' + ref.FirstName as ClientName, cm.FirstName+' ' +cm.LastName as CaseManager, CM.Email as ToEmail,ref.ClientNickName          
,C.LastName +' ' + C.FirstName as ParentName,Phone1 as ParentPhone 
 FROM Referrals ref           
 JOIN CaseManagers cm ON cm.CaseManagerID=ref.CaseManagerID
 inner join ContactMappings CMS on CMS.ReferralID=ref.ReferralID and CMS.ContactTypeID=1                    
 inner join Contacts C on C.ContactID=CMS.ContactID     
 WHERE ref.ReferralID=@ReferralID
END