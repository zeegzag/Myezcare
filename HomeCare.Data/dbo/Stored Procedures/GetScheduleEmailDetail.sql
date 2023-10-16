-- EXEC [GetScheduleEmailDetail] 17953
CREATE PROCEDURE [dbo].[GetScheduleEmailDetail]         
@ScheduleID bigint      
AS        
BEGIN        
select             
  SM.ScheduleID,ScheduleStatusID,R.FirstName,StartDate,EndDate,SM.WeekSMSDate,SM.WeekEmailDate,C.Email,            
  DropL.MapImage as DropOffImage,PickL.MapImage as PickUpImage,                            
  DropL.Location as DropOffLocation,PickL.Location as PickUPLocation,                  
  DropL.Address as DropOffAddress,DropL.City as DropOffCity,DropL.Zip as DropOffZip,                  
  PickL.Address as PickAddress,PickL.City as PickCity,PickL.Zip as PickZip,                  
  --STD.StateName as DropoffStatename,STP.StateName as PickUpStatename,                  
  PFM.MondayPickUp,PFM.TuesdayPickUp,PFM.WednesdayPickUp,PFM.ThursdayPickUp,PFM.FridayPickUp,PFM.SaturdayPickUp,PFM.SundayPickUp,                  
  DFM.MondayDropOff,DFM.TuesdayDropOff,DFM.WednesdayDropOff,DFM.ThursdayDropOff,DFM.FridayDropOff,DFM.SaturdayDropOff,DFM.SundayDropOff,
  c.LastName AS ParentLastName ,c.FirstName  AS ParentFirstName ,                    
  R.PermissionForEmail,R.PermissionForSMS,C.Phone1,DropL.Phone as DropOffPhone ,PickL.Phone as PickUpPhone,R.ClientNickName ,DropL.State as DropOffStateCode ,
  PickL.State as PickUpStateCode 
                      
  from ScheduleMasters SM                  
   inner join Referrals R on R.ReferralID = SM.ReferralID                     
   inner join Facilities FF on FF.FacilityID=SM.FacilityID                  
   inner join ContactMappings CM on CM.ReferralID=R.ReferralID and CM.ContactTypeID=1 --CM.IsPrimaryPlacementLegalGuardian =1                  
   inner join Contacts C on C.ContactID=CM.ContactID                  
   --inner join  States STD on STD.StateCode=DropL.State                  
   --inner join  States STP on STP.StateCode=PickL.State
   left join FacilityTransportLocationMappings PFM on PFM.FacilityID=SM.FacilityID and PFM.TransportLocationID=SM.PickUpLocation
   left join FacilityTransportLocationMappings DFM on DFM.FacilityID=SM.FacilityID and DFM.TransportLocationID=SM.DropOffLocation
   left join TransportLocations DropL on DropL.TransportLocationID = DFM.TransportLocationID
   left join TransportLocations PickL on PickL.TransportLocationID = PFM.TransportLocationID
 where              
 SM.IsDeleted =0 and SM.ScheduleID=@ScheduleID      
END