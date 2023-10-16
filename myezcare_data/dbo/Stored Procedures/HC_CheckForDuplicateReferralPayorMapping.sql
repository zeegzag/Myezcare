--  [HC_CheckForDuplicateReferralPayorMapping] 0,14232,1,1,'2018-07-01','2018-07-31'  
CREATE Procedure [dbo].[HC_CheckForDuplicateReferralPayorMapping]              
@ReferralPayorMappingID bigint=0,            
@ReferralID bigint,            
@PayorID bigint,            
@Precedence bigint,            
@StartDate date,            
@EndDate date             
as                
 select CountValue=Count(*) from   
 ReferralPayorMappings  
 where   
 ReferralID=@ReferralID AND   
 --PayorID=@PayorID  AND  
 Precedence =@Precedence AND   
 ReferralPayorMappingID!=@ReferralPayorMappingID            
 and            
 (            
  (@StartDate >= PayorEffectiveDate AND @StartDate <= PayorEffectiveEndDate)             
  OR (@EndDate >= PayorEffectiveDate AND @EndDate <= PayorEffectiveEndDate)             
  OR (@StartDate < PayorEffectiveDate AND @EndDate > PayorEffectiveEndDate)             
 )
