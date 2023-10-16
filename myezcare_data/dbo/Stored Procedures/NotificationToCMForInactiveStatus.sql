-- EXEC SendReceiptNotificationEmail @ReferralID = '2', @@EmailTemplateTypeID = '1'          
CREATE PROCEDURE [dbo].[NotificationToCMForInactiveStatus]          
@ReferralID BIGINT=0,          
@EmailTemplateTypeID VARCHAR(100)=null 
AS           
BEGIN           
 SELECT * FROM EmailTemplates where EmailTemplateTypeID=@EmailTemplateTypeID;          
            
 SELECT ref.AHCCCSID, ref.ReferralID,ref.LastName + ', ' + ref.FirstName as ClientName, cm.FirstName+' ' +cm.LastName as CaseManager, CM.Email as ToEmail,ref.ClientNickName,
 ref.ClosureDate,ref.ClosureReason, ref.RecordRequestEmail
 FROM Referrals ref           
 JOIN CaseManagers cm ON cm.CaseManagerID=ref.CaseManagerID
 WHERE ref.ReferralID=@ReferralID
END 
