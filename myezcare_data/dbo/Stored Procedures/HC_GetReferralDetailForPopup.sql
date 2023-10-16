CREATE PROCEDURE [dbo].[HC_GetReferralDetailForPopup]      
 @ReferralID bigint,
 @ContactTypeID bigint                          
AS    
BEGIN        
--Referral Details --   
 select   
 R.LastName + ', '+ R.FirstName ReferralName, R.AHCCCSID, R.CISNumber,R.PlacementRequirement, R.NeedPrivateRoom, R.LastAttendedDate,   
 C.LastName+ ', '+ C.FirstName PrimaryContactName,C.Phone1,C.Phone2, C.Email , F.Code FrequencyCode,  
 R.RespiteService,R.LifeSkillsService,R.CounselingService,R.ZSPCounsellingExpirationDate,R.ZSPLifeSkillsExpirationDate,R.ZSPRespiteExpirationDate,R.ReferralID  
 from Referrals R  
 inner join ContactMappings CM on CM.ReferralID=R.ReferralID AND CM.ContactTypeID =@ContactTypeID  -- (CM.IsPrimaryPlacementLegalGuardian =1 OR CM.IsDCSLegalGuardian=1)  
 inner join Contacts C on C.ContactID = CM.ContactID    
 inner join FrequencyCodes F on F.FrequencyCodeID = R.FrequencyCodeID  
 where R.ReferralID = @ReferralID   
--End Referral Details --      
  
  
 --Most Recent 4 Schedules--  
 select TOP 4 S.StartDate,S.EndDate,SS.ScheduleStatusName,EmployeeName = E.FirstName+' '+E.LastName from ScheduleMasters S    
 INNER JOIN ScheduleStatuses SS on S.ScheduleStatusID =SS.ScheduleStatusID    
 INNER JOIN Employees E ON E.EmployeeID=S.EmployeeID
 where S.IsDeleted=0  and S.ReferralID=@ReferralID  
 order by S.StartDate desc   
  
 --End Most Recent 4 Schedules--  
     
END 
