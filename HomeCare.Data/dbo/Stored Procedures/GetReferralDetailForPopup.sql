--       
--EXEC GetReferralDetailForPopup 23      
CREATE PROCEDURE [dbo].[GetReferralDetailForPopup]        
 @ReferralID bigint,  
 @ContactTypeID bigint                            
AS      
BEGIN    
DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat() 
--Referral Details --     
 select     
 dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat) ReferralName, R.AHCCCSID, R.CISNumber,R.PlacementRequirement, R.NeedPrivateRoom, R.LastAttendedDate,     
 dbo.GetGenericNameFormat(C.FirstName,'', C.LastName,@NameFormat) PrimaryContactName,C.Phone1,C.Phone2, C.Email , F.Code FrequencyCode,    
 R.RespiteService,R.LifeSkillsService,R.CounselingService,R.ZSPCounsellingExpirationDate,R.ZSPLifeSkillsExpirationDate,R.ZSPRespiteExpirationDate,R.ReferralID    
 from Referrals R    
 inner join ContactMappings CM on CM.ReferralID=R.ReferralID AND CM.ContactTypeID =@ContactTypeID  -- (CM.IsPrimaryPlacementLegalGuardian =1 OR CM.IsDCSLegalGuardian=1)    
 inner join Contacts C on C.ContactID = CM.ContactID      
 inner join FrequencyCodes F on F.FrequencyCodeID = R.FrequencyCodeID    
 where R.ReferralID = @ReferralID     
--End Referral Details --        
    
    
 --Most Recent 4 Schedules--    
 select top 4 S.StartDate,S.EndDate,SS.ScheduleStatusName,F.FacilityName from ScheduleMasters S      
  inner join ScheduleStatuses SS on S.ScheduleStatusID =SS.ScheduleStatusID      
  left join Facilities F on F.FacilityID =S.FacilityID      
 where S.IsDeleted=0  and S.ReferralID=@ReferralID    
 order by S.StartDate desc     
    
 --End Most Recent 4 Schedules--    
       
END   