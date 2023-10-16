--EXEC API_GetPatientDetail @ReferralID = N'84', @EmployeeID = N'166', @ServerCurrentDate = N'2021-01-25 02:58:14 PM'    
CREATE PROCEDURE [dbo].[API_GetPatientDetail]            
 @ReferralID BIGINT,            
 @ServerCurrentDate DATETIME,      
 @EmployeeID BIGINT      
AS                              
BEGIN                              
             
 SET NOCOUNT ON;                              
                     
DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat() 
 SELECT  r.ReferralID, FullName=dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat),r.FirstName,r.LastName,c.Address, c.City, c.[State], c.ZipCode,                    
 ImageUrl=r.ProfileImagePath,Account=r.AHCCCSID,r.Gender,Language=l.Title,c.Email,Phone=c.Phone1,EmergencyPhone=ec.Phone1,sm.ScheduleID,sm.CareTypeTimeSlotID   
 FROM dbo.Referrals r      
 OUTER APPLY (  
    SELECT  
     MAX(SM.ScheduleID) ScheduleID  
 FROM ScheduleMasters sm   
 LEFT JOIN [dbo].[EmployeeVisits] ev ON ev.ScheduleID = sm.ScheduleID      
 WHERE sm.ReferralID=r.ReferralID       
 AND CONVERT(DATE,sm.StartDate)=CONVERT(DATE,@ServerCurrentDate)      
 AND sm.EmployeeID = @EmployeeID  
 AND ISNULL(SM.OnHold, 0) = 0 AND Sm.IsDeleted = 0  
 and (ISNULL(ev.IsPCACompleted, 0) <> 1 OR ev.ClockOutTime Is NULL OR sm.EndDate < @ServerCurrentDate)  
 GROUP BY CONVERT(DATE,sm.StartDate)  
 ) MS  
 LEFT JOIN ScheduleMasters sm ON MS.ScheduleID = SM.ScheduleID   
 LEFT JOIN DDMaster l ON l.DDMasterID=r.LanguageID              
 LEFT JOIN dbo.ContactMappings cm ON r.ReferralID = cm.ReferralID AND cm.ContactTypeID=1             
 LEFT JOIN dbo.ContactMappings ecm ON r.ReferralID = ecm.ReferralID  AND ecm.IsEmergencyContact=1          
 LEFT JOIN dbo.Contacts c ON cm.ContactID = c.ContactID AND c.IsDeleted = 0             
 LEFT JOIN dbo.Contacts ec ON ecm.ContactID = ec.ContactID AND ec.IsDeleted=0          
    
 WHERE r.IsDeleted = 0 AND r.ReferralID=@ReferralID    
END  