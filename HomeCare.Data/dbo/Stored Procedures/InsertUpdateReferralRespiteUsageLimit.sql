CREATE procedure [dbo].[InsertUpdateReferralRespiteUsageLimit]         
@StartDate date,        
@EndDate date        
as        
insert into ReferralRespiteUsageLimit(StartDate,EndDate,ReferralID,IsActive,UsedRespiteHours)        
SELECT distinct @StartDate,@EndDate,ReferralID,1,UsedRespiteHours from ReferralRespiteUsageLimit where IsActive=1;        
update ReferralRespiteUsageLimit set IsActive=0 where  StartDate < @StartDate;