CREATE PROCEDURE [dbo].[API_GetPatientDetail]      
 @ReferralID BIGINT,      
 @ServerCurrentDate DATE      
AS                        
BEGIN                        
       
 SET NOCOUNT ON;                        
                        
 SELECT TOP 1 r.ReferralID, FullName=dbo.GetGeneralNameFormat(r.FirstName,r.LastName),r.FirstName,r.LastName,c.Address, c.City, c.[State], c.ZipCode,              
 ImageUrl=r.ProfileImagePath,Account=r.AHCCCSID,r.Gender,Language=l.Title,c.Email,Phone=c.Phone1,EmergencyPhone=ec.Phone1,sm.ScheduleID,sm.CareTypeTimeSlotID    
 FROM dbo.Referrals r          
 LEFT JOIN ScheduleMasters sm ON sm.ReferralID=r.ReferralID AND CONVERT(DATE,sm.StartDate)=@ServerCurrentDate      
 LEFT JOIN DDMaster l ON l.DDMasterID=r.LanguageID        
 LEFT JOIN dbo.ContactMappings cm ON r.ReferralID = cm.ReferralID        
 LEFT JOIN dbo.ContactMappings ecm ON r.ReferralID = ecm.ReferralID        
 LEFT JOIN dbo.Contacts c ON cm.ContactID = c.ContactID        
 LEFT JOIN dbo.Contacts ec ON ecm.ContactID = ec.ContactID        
 WHERE r.IsDeleted = 0 AND c.IsDeleted = 0 AND ec.IsDeleted=0 AND (cm.ContactTypeID=1 OR ecm.IsEmergencyContact=1) AND r.ReferralID=@ReferralID
 ORDER BY sm.ScheduleID DESC
END
