--EXEC API_GetReferralDetail @ReferralID = N'24237', @ScheduleID = N'133553'
CREATE PROCEDURE [dbo].[API_GetReferralDetail]      
 @ReferralID bigint,            
 @ScheduleID bigint            
AS                
BEGIN                
 -- SET NOCOUNT ON added to prevent extra result sets from                
 -- interfering with SELECT statements.                
 SET NOCOUNT ON;                
                
 SELECT TOP 1 r.ReferralID, FullName=dbo.GetGeneralNameFormat(r.FirstName,r.LastName),r.FirstName,r.LastName,c.Address, c.City, c.[State], c.ZipCode,      
 sm.StartDate as ServiceDate,ImageUrl=r.ProfileImagePath,c.ContactID,Account=r.AHCCCSID,r.Gender,Language=l.Title,c.Email,Phone=c.Phone1,EmergencyPhone=ec.Phone1
 FROM dbo.Referrals r  
 INNER JOIN ScheduleMasters sm ON sm.ScheduleID=@ScheduleID  
 LEFT JOIN DDMaster l ON l.DDMasterID=r.LanguageID
 LEFT JOIN dbo.ContactMappings cm ON r.ReferralID = cm.ReferralID
 LEFT JOIN dbo.ContactMappings ecm ON r.ReferralID = ecm.ReferralID
 LEFT JOIN dbo.Contacts c ON cm.ContactID = c.ContactID
 LEFT JOIN dbo.Contacts ec ON ecm.ContactID = ec.ContactID
 WHERE r.IsDeleted = 0 AND c.IsDeleted = 0 AND ec.IsDeleted=0 AND (cm.ContactTypeID=1 OR ecm.IsEmergencyContact=1) AND r.ReferralID=@ReferralID
END
