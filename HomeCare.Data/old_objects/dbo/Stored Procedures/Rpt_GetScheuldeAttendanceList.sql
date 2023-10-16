CREATE Procedure [dbo].[Rpt_GetScheuldeAttendanceList]                  
--declare                  
 @RegionID bigint,                  
 @EndDate date =null,                  
 @StartDate date=null                  
 as                  
select R.FirstName,R.LastName,R.AHCCCSID,dbo.GetAge(r.Dob) as Age,R.Gender,                
(CONVERT(VARCHAR(10),CONVERT(datetime,R.Dob ,1),101)) AS Dob,                  
 CN.FirstName +' ' +CN.LastName as ParentName,CN.Phone1 as ParentPhone1,CN.Phone2 as ParentPhone2,                  
 CN.Address as ParentAddress,CN.Email as ParentEmail,CN.ZipCode As ParentZipCode,CN.City as ParentCity,                   
 CN.State as ParentState,R.PlacementRequirement,R.BehavioralIssue,DropL.Location DropOffLocationName,SM.StartDate,SM.EndDate,                  
 PickL.Location PickUpLocationName,F.FacilityName as FacilityHouseName,RG.RegionName,A.NickName as Agency,P.ShortName AS PayorName,RG.RegionName,                  
	PickL.LocationCode as PickUpLocationCode,DropL.LocationCode as DropOffLocationCode,                   
 (SELECT  STUFF((SELECT ',' + D.DXCodeName  FROM  DXCodes D inner join                     
   ReferralDXCodeMappings F ON D.DXCodeID=F.DXCodeID where f.ReferralID=R.ReferralID order by D.DXCodeName  ASC                             
   FOR XML PATH('')),1,1,'')) DXCodeName,(SELECT  STUFF((SELECT ',' + CT.FirstName +' ' +CT.LastName + ' '+ CT.Phone1                  
    FROM  Contacts CT inner join  ContactMappings CMS ON CT.ContactID=CMS.ContactID                  
    where CMS.ReferralID=R.ReferralID AND  CMS.IsEmergencyContact=1 FOR XML PATH('')),1,1,'')) EmergencyContact ,SS.ScheduleStatusName,SS.ScheduleStatusID  ,                
    CM.FirstName +' ' + CM.LastName as  Facilitator ,sm.StartDate,sM.EndDate,r.PermissionForVoiceMail, R.NeedPrivateRoom,SM.Comments,SM.CancelReason
 from ScheduleMasters SM            
 inner join ScheduleStatuses SS on SS.ScheduleStatusID=Sm.ScheduleStatusID                  
 inner join Referrals r on r.ReferralID=SM.ReferralID                      
 inner join Regions RG on RG.RegionID=R.RegionID                  
 inner join ContactMappings C on C.ReferralID=R.ReferralID and C.ContactTypeID=1                  
 inner join Contacts CN on Cn.ContactID=C.ContactID                   
 inner join TransportLocations DropL on DropL.TransportLocationID = SM.DropOffLocation                        
 inner join TransportLocations PickL on PickL.TransportLocationID = SM.PickUpLocation                        
 inner join ReferralPayorMappings RPM on RPM.ReferralID = R.ReferralID and RPM.IsActive=1 and RPM.IsDeleted=0                   
 inner join Payors P on P.PayorID = RPM.PayorID            
 inner join CaseManagers CM on CM.CaseManagerID=R.CaseManagerID            
 left join Agencies A ON a.AgencyID=R.AgencyID                  
 left join AttendanceMaster AM  on sm.ScheduleID=am.ScheduleMasterID            
 left join Facilities F on F.FacilityID=SM.FacilityID                  
 where(SM.IsDeleted = 0) AND R.RegionID=@RegionID               
 AND ((@StartDate is null OR sm.StartDate >= @StartDate) and (@EndDate is null OR SM.EndDate<= @EndDate))            
 order by F.FacilityName ASC

