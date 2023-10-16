-- EXEC GetScheduleEmailForReminder @FromDate = '2017/09/02', @ToDate = '2017/09/04', @ScheduleStatus = '2'
CREATE PROCEDURE [dbo].[GetScheduleEmailForReminder]                                    
 @FromDate DATE,                    
 @ToDate DATE,                    
 @ScheduleStatus BIGINT=2
AS                                    
  
  SELECT
   R.ReferralID,SM.ScheduleID,ScheduleStatusID,StartDate,EndDate,SM.WeekEmailDate,SM.WeekSMSDate,C.Email,                              
   --PFM.MondayPickUp,PFM.TuesdayPickUp,PFM.WednesdayPickUp,PFM.ThursdayPickUp,PFM.FridayPickUp,PFM.SaturdayPickUp,PFM.SundayPickUp,                                    
   DFM.MondayDropOff,DFM.TuesdayDropOff,DFM.WednesdayDropOff,DFM.ThursdayDropOff,DFM.FridayDropOff,DFM.SaturdayDropOff,DFM.SundayDropOff,
   c.FirstName  AS ParentFirstName ,                                      
   CASE WHEN R.ClientNickName Is null then R.FirstName  ELSE R.ClientNickName END as ClientName,
   

   R.PermissionForEmail,R.PermissionForSMS,R.PermissionForMail,C.Phone1,C.Email AS ParentEmail,
   R.PCMEmail,R.PCMMail,R.PCMSMS,R.PCMVoiceMail,SM.EmailSent,SM.SMSSent,SM.NoticeSent
   from ScheduleMasters SM                                    
   INNER JOIN Referrals R on R.ReferralID = SM.ReferralID                                       
   INNER JOIN ContactMappings CM on CM.ReferralID=R.ReferralID and CM.ContactTypeID=1                  
   INNER JOIN Contacts C on C.ContactID=CM.ContactID                  
   --LEFT JOIN FacilityTransportLocationMappings PFM on PFM.FacilityID=SM.FacilityID and PFM.TransportLocationID=SM.PickUpLocation
   LEFT JOIN FacilityTransportLocationMappings DFM on DFM.FacilityID=SM.FacilityID and DFM.TransportLocationID=SM.DropOffLocation  
 where                      
 (SM.IsDeleted =0)
  AND                               
    (
	   (SM.ScheduleStatusID=2 AND ((DATEDIFF(DAY,CONVERT(DATE, GETDATE()),SM.StartDate)  BETWEEN 1 AND 3) 
		   AND (DATENAME(DW, SM.StartDate) IN  ('Friday','Saturday','Sunday')) AND DATENAME(DW, CONVERT(DATE, GETDATE())) IN  ('Thursday')
		   --AND (SM.WeekEmailDate IS NULL OR SM.WeekSMSDate IS NULL) ))
		   ))
    ) 
 
	 


