-- EXEC GetFacilityScheduleDetails @FacilityID = 1, @StartDate = '2016-07-01', @EndDate = '2016-07-20'        
--EXEC GetFacilityScheduleDetails @FacilityID = '1', @StartDate = '2016-07-01', @EndDate = '2016-08-12'        
CREATE PROCEDURE [dbo].[GetFacilityScheduleDetails]        
 @FacilityID bigint,     
 @WeekMAsterID bigint     
 --@StartDate Date,        
 --@EndDate Date        
AS        
BEGIN         
  Declare @StartDate Date,@EndDate Date  
  SELECT @StartDate = StartDate,@EndDate=EndDate from WeekMasters where WeekMasterID=@WeekMAsterID  
 
 --Schecudle List For Perticular Facility--        
 SELECT
   SM.*, DropL.Location DropOffLocationName, PickL.Location PickUpLocationName,SS.ScheduleStatusName ,        
   R.FirstName,R.LastName, dbo.GetAge(R.Dob)as Age,R.Gender,R.NeedPrivateRoom , R.IsDeleted as IsRefrralDeleted,  R.PlacementRequirement, R.Dob,
   C.LastName+ ', '+ C.FirstName PrimaryContactName,C.Phone1,C.Phone2, C.Email,  
   
   (SELECT STUFF((SELECT '~' + RN.LastName + ' '+RN.FirstName+ ' ; ' + CONVERT (varchar,RN.ReferralID)            
   FROM Referrals RN         
   where ReferralID in (select (case when RM.ReferralID1 = R.ReferralID then RM.ReferralID2 else RM.ReferralID1 end)  as ReferralID          
  from ReferralSiblingMappings RM         
  where RM.ReferralID1= R.ReferralID  or RM.ReferralID2=R.ReferralID)         
   FOR XML PATH('')),1,1,'') ) ReferralSiblingMappingVlaue,
    
   F.DefaultScheduleDays    
 FROM ScheduleMasters SM        
  inner join Referrals R on R.ReferralID = SM.ReferralID    
  inner join ContactMappings CM on CM.ReferralID=R.ReferralID and CM.ContactTypeID=1-- (CM.IsPrimaryPlacementLegalGuardian =1 OR CM.IsDCSLegalGuardian=1)  
  inner join Contacts C on C.ContactID = CM.ContactID
  inner join TransportLocations DropL on DropL.TransportLocationID = SM.DropOffLocation        
  inner join TransportLocations PickL on PickL.TransportLocationID = SM.PickUpLocation        
  inner join ScheduleStatuses SS on SS.ScheduleStatusID= SM.ScheduleStatusID        
  inner join FrequencyCodes F on R.FrequencyCodeID = F.FrequencyCodeID            
 WHERE 
 (SM.IsDeleted = 0 )        
 and (SM.FacilityID=@FacilityID  )        
 and (SM.WeekMasterID=@WeekMAsterID)  
 --and (@EndDate is not null or SM.StartDate >= @StartDate or SM.EndDate >= @StartDate)         
 --and (@EndDate is null or ((SM.StartDate between @StartDate and @EndDate)        
 --       or(SM.EndDate between @StartDate and @EndDate )        
 --       or(@StartDate between SM.StartDate and SM.EndDate )) )        
                
                
 -- End Schecudle List For Perticular Facility--        
         
         
                
 -- Get Date wise Schedule and Confirmed Schedule List For Perticular Facility--        
 select         
  DT.Date, COUNT(SM.ScheduleID) TotalScheduleCount, SUM(Case when SM.ScheduleStatusID = 2 then 1 else 0 end) ConfirmedScheduleCount,        
  SUM(Case When R.NeedPrivateRoom=1 then 1 else 0 end) RequiredPrivateRoom,        
  SUM(Case When R.NeedPrivateRoom=1 and  SM.ScheduleStatusID = 2 then 1 else 0 end) ConfirmedPrivateRoom        
 from         
  (SELECT DATEADD(DAY,number+1,DATEADD(day,-1, @StartDate)) [Date]        
   FROM master..spt_values        
   WHERE type = 'P'        
   AND DATEADD(DAY,number+1,DATEADD(day,-1, @StartDate)) < DATEADD(day,1, @EndDate)) DT         
  left join ScheduleMasters SM on (DT.Date between SM.StartDate and SM.EndDate) and SM.FacilityID = @FacilityID        
  inner join Referrals R On R.ReferralID = SM.ReferralID        
  where SM.IsDeleted=0         
 group by DT.Date        
 --End Get Date wise Schedule and Confirmed Schedule List For Perticular Facility--        
         
         
          
 --Get Facility Details--         
 select         
  F.*,          
  (SELECT         
   STUFF((SELECT ', ' + p.PayorName        
   FROM FacilityApprovedPayors FAP         
   inner JOIN Payors P on FAP.PayorID=P.PayorID        
   WHERE FAP.FacilityID = F.FacilityID        
   ORDER BY FAP.PayorID        
   FOR XML PATH('')),1,1,'')) AS PayorApproved        
  from Facilities F        
 where F.FacilityID=@FacilityID        
 --End Get Facility Details--        
         
END 
