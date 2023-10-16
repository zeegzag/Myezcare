-- EXEC GetScheduleEmail @Monthday = '30', @WeekDay = '7', @IsWeekMonthFromService = 'true'
CREATE Procedure [dbo].[GetScheduleEmail]                                    
 @Monthday int,                    
 @WeekDay int,                    
 @IsWeekMonthFromService bit=1,                    
 @ScheduleIds nvarchar(max) = ''                    
AS                                    
  
  SELECT
   SM.ScheduleID,ScheduleStatusID,R.FirstName,StartDate,EndDate,SM.WeekEmailDate,SM.WeekSMSDate,C.Email,                              
   DropL.MapImage as DropOffImage,PickL.MapImage as PickUpImage,                                              
   DropL.Location as DropOffLocation,PickL.Location as PickUPLocation,                                    
   DropL.Address as DropOffAddress,DropL.City as DropOffCity,DropL.Zip as DropOffZip,                                    
   PickL.Address as PickAddress,PickL.City as PickCity,PickL.Zip as PickZip,                                    
   --STD.StateName as DropoffStatename,STP.StateName as PickUpStatename,                                    
   PFM.MondayPickUp,PFM.TuesdayPickUp,PFM.WednesdayPickUp,PFM.ThursdayPickUp,PFM.FridayPickUp,PFM.SaturdayPickUp,PFM.SundayPickUp,                                    
   DFM.MondayDropOff,DFM.TuesdayDropOff,DFM.WednesdayDropOff,DFM.ThursdayDropOff,DFM.FridayDropOff,DFM.SaturdayDropOff,DFM.SundayDropOff,
   c.LastName AS ParentLastName,c.FirstName  AS ParentFirstName ,                                      
   R.PermissionForEmail,R.PermissionForSMS,R.PermissionForMail,C.Phone1,DropL.Phone as DropOffPhone ,PickL.Phone as PickUpPhone,c.Address as ParentAddress,c.City as ParentCity,   
   c.ZipCode as ParentZipCode,SC.StateName as ParenStateName,C.Email AS ParentEmail,C.Phone1 as ParentPhone,
   R.ClientNickName ,R.ReferralID,DropL.State as DropOffStateCode,PickL.State as PickUpStateCode,       
    CASE WHEN R.ClientNickName Is null then R.FirstName  ELSE R.ClientNickName END as ClientName,SM.DropOffLocation as DropTransportLocationID,      
   SM.PickUpLocation as PickupTransportLocationID,      
   
   --(SELECT  STUFF((SELECT  DISTINCT ',' +      
   -- CASE WHEN (R.ReferralID != RSM.ReferralID1) THEN convert(varchar, RSM.ReferralID1 )      
   --WHEN (R.ReferralID!=RSM.ReferralID2) THEN convert(varchar,RSM.ReferralID2)      
   --ELSE NULL END         
   --FROM ReferralSiblingMappings RSM where        
   --       (RSM.ReferralID1=R.ReferralID OR RSM.ReferralID2=R.ReferralID) OR (RSM.ReferralID2=R.ReferralID OR RSM.ReferralID1=R.ReferralID)      
   --FOR XML PATH('')),1,1,'')) SiblingIDs  ,


   (SELECT STUFF((
   SELECT ',' +
    (CASE WHEN RM1.ReferralID1 = R.ReferralID 
	    THEN CONVERT (VARCHAR(MAX),RL2.ReferralID)
	    ELSE  CONVERT (VARCHAR(MAX),RL1.ReferralID) END
	)  FROM ReferralSiblingMappings RM1
   INNER JOIN Referrals RL1 ON RL1.ReferralID=RM1.ReferralID1
   INNER JOIN Referrals RL2 ON RL2.ReferralID=RM1.ReferralID2
   WHERE RM1.ReferralID1= R.ReferralID  or RM1.ReferralID2=R.ReferralID         
   FOR XML PATH('')),1,1,'') ) SiblingIDs,   

   
   R.PCMEmail,R.PCMMail,R.PCMSMS,R.PCMVoiceMail,SM.EmailSent,SM.SMSSent,SM.NoticeSent
   from ScheduleMasters SM                                    
   INNER JOIN Referrals R on R.ReferralID = SM.ReferralID                                       
   INNER JOIN Facilities FF on FF.FacilityID=SM.FacilityID                                    
   INNER JOIN ContactMappings CM on CM.ReferralID=R.ReferralID and CM.ContactTypeID=1                  
   INNER JOIN Contacts C on C.ContactID=CM.ContactID                  
   LEFT JOIN States SC on SC.StateCode=C.State                  
   --inner join  States STD on STD.StateCode=DropL.State                                    
   --inner join  States STP on STP.StateCode=PickL.State                   
   LEFT JOIN FacilityTransportLocationMappings PFM on PFM.FacilityID=SM.FacilityID and PFM.TransportLocationID=SM.PickUpLocation
   LEFT JOIN FacilityTransportLocationMappings DFM on DFM.FacilityID=SM.FacilityID and DFM.TransportLocationID=SM.DropOffLocation  
   --left join TransportLocations DropL on DropL.TransportLocationID = DFM.TransportLocationID                               
   --left join TransportLocations PickL on PickL.TransportLocationID = PFM.TransportLocationID
   LEFT JOIN TransportLocations DropL on DropL.TransportLocationID = SM.DropOffLocation                                
   LEFT JOIN TransportLocations PickL on PickL.TransportLocationID = SM.PickUpLocation
 where                      
 (SM.IsDeleted =0)
  AND                               
    (
		 (@IsWeekMonthFromService=0 AND  SM.ScheduleStatusID NOT IN (6,9,7)) OR
		 ( @IsWeekMonthFromService=1 AND  SM.ScheduleStatusID=1 AND ((DATEDIFF(DAY,CONVERT(date, GETDATE()),SM.StartDate)  BETWEEN @WeekDay AND 9) 
		   AND (DATENAME(DW, SM.StartDate) IN  ('Friday','Saturday','Sunday')) AND DATENAME(DW, CONVERT(date, GETDATE())) IN  ('Friday')
		   AND (SM.WeekEmailDate IS NULL OR SM.WeekSMSDate IS NULL) ))
    ) 
 AND 
	(
		(@ScheduleIds = '') OR                    
		(SM.ScheduleID IN (SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@ScheduleIds)))
	)	
	 

-- EXEC GetScheduleEmail @Monthday = '30', @WeekDay = '7', @IsWeekMonthFromService = 'true'