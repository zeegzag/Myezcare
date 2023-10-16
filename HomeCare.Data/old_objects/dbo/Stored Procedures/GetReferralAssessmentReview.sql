 -- EXEC GetReferralAssessmentReview 123
CREATE PROCEDURE [dbo].[GetReferralAssessmentReview]  
 @ReferralID bigint,
 @StartDate Date =null,
 @EndDate DAte=null
AS  
BEGIN  
 
 	SELECT * FROM ReferralAssessmentReview RAR
 	WHERE
 		(RAR.ReferralID=@ReferralID) AND IsDeleted=0 AND
 		((@StartDate is null) OR ( RAR.AssessmentDate >=@StartDate)) AND
 		((@EndDate is null) OR ( RAR.AssessmentDate <=@EndDate))
 	ORDER BY  RAR.AssessmentDate ASC
 	
END
