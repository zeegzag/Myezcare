 -- EXEC GetReferralAssessmentReview 123
CREATE PROCEDURE [dbo].[DeleteReferralAssessmentReview]  
 @ReferralAssessmentID bigint,
 @ReferralID bigint,
 @StartDate Date =null,
 @EndDate DAte=null
AS  
BEGIN  
	UPDATE ReferralAssessmentReview SET IsDeleted=1 WHERE ReferralAssessmentID=@ReferralAssessmentID
	EXEC [GetReferralAssessmentReview]  @ReferralID,@StartDate,@EndDate
END