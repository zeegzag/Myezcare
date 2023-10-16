-- EXEC Rpt_GetRequestedDocsForAttendanceList @RegionID = '2', @StartDate = '2017/02/01'
CREATE Procedure [dbo].[Rpt_GetRequestedDocsForAttendanceList]
 @RegionID bigint,                  
 @EndDate date =null,                  
 @StartDate date=null                  
 as                  
select R.LastName+', '+R.FirstName  AS ClientName, CASE WHEN R.CareConsent=1 THEN 'Yes' ELSE 'No' END AS CareConsent,
dbo.GetAge(r.Dob) as Age, R.AHCCCSID, R.Gender,  (CONVERT(VARCHAR(10),CONVERT(datetime,R.Dob ,1),101)) AS Dob,

(CONVERT(VARCHAR(10),CONVERT(datetime,R.ZSPRespiteExpirationDate ,1),101)) AS ZSPRespiteExpirationDate,
(CONVERT(VARCHAR(10),CONVERT(datetime,R.ZSPLifeSkillsExpirationDate ,1),101)) AS ZSPLifeSkillsExpirationDate,
(CONVERT(VARCHAR(10),CONVERT(datetime,R.ZSPCounsellingExpirationDate ,1),101)) AS ZSPCounsellingExpirationDate,
(CONVERT(VARCHAR(10),CONVERT(datetime,R.ZSPConnectingFamiliesExpirationDate ,1),101)) AS ZSPConnectingFamiliesExpirationDate,

--(CONVERT(VARCHAR(10),CONVERT(datetime,SM.StartDate ,1),101)) AS 
			SM.StartDate as  StartDate,
--(CONVERT(VARCHAR(10),CONVERT(datetime,SM.EndDate ,1),101)) AS
		SM.EndDate as EndDate,	

 CN.FirstName +' ' +CN.LastName as ParentName,CN.Phone1 as ParentPhone1,CN.Phone2 as ParentPhone2,                  
 CN.Address as ParentAddress,CN.Email as ParentEmail,CN.ZipCode As ParentZipCode,CN.City as ParentCity,                   
 CN.State as ParentState,R.PlacementRequirement,R.BehavioralIssue,DropL.Location DropOffLocationName,
 PickL.Location PickUpLocationName,F.FacilityName as FacilityHouseName,RG.RegionName,A.NickName as Agency,P.ShortName AS PayorName,RG.RegionName,CM.FirstName+' '+ CM.LastName AS Facilitator,                  
 (SELECT  STUFF((SELECT ',' + CT.FirstName +' ' +CT.LastName + ' '+ CT.Phone1                  
    FROM  Contacts CT inner join  ContactMappings CMS ON CT.ContactID=CMS.ContactID                  
    where CMS.ReferralID=R.ReferralID AND  CMS.IsEmergencyContact=1 FOR XML PATH('')),1,1,'')) EmergencyContact ,SS.ScheduleStatusName
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
 where(ISNULL(SM.OnHold, 0) = 0 AND SM.IsDeleted = 0) AND R.RegionID=@RegionID               
 AND ((@StartDate is null OR sm.StartDate >= @StartDate) and (@EndDate is null OR SM.EndDate<= @EndDate))            
 order by R.LastName ASC