CREATE procedure [dbo].[GetAlreadyExistReferralRespiteUsageLimit]   
@ReferralID bigint  
as  
select * from ReferralRespiteUsageLimit where  
 ReferralID=CAST(@ReferralID AS BIGINT)  AND StartDate <= GETUTCDATE() AND  IsActive=1