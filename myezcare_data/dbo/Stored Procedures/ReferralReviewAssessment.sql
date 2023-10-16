-- EXEC ReferralReviewAssessment @ReferralID = '2892', @ReferralAssessmentID = '13', @LoginID = '1'
-- EXEC ReferralReviewAssessment @ReferralID = '2892', @ReferralAssessmentID = '13', @LoginID = '10080'
CREATE procedure [dbo].[ReferralReviewAssessment]                                  
@ReferralID  bigint,
@ReferralAssessmentID bigint,
@LoginID bigint
as                                                      
  SET NOCOUNT ON;                

BEGIN
  
  if(@ReferralAssessmentID > 0)
	BEGIN
		select top 1 RAR.*  from ReferralAssessmentReview RAR
		INNER JOIN Referrals R on R.ReferralID=RAR.ReferralID
		INNER JOIN Employees E on E.EmployeeID=@LoginID
		LEFT  JOIN EmployeeSignatures ES on ES.EmployeeSignatureID=E.EmployeeSignatureID
		where RAR.ReferralID=@ReferralID AND RAR.ReferralAssessmentID!=@ReferralAssessmentID AND RAR.ReferralAssessmentID < @ReferralAssessmentID
	    order by RAR.ReferralAssessmentID desc

		SELECT RAR.*,ES.SignaturePath as Signature,E.LastName+', '+E.FirstName As CompletedBy 
		FROM ReferralAssessmentReview RAR
		LEFT join Employees E on ( (RAR.SignatureBy IS NOT NULL AND E.EmployeeID=RAR.SignatureBy )  OR ( RAR.SignatureBy IS  NULL AND E.EmployeeID=@LoginID) )
		LEFT join EmployeeSignatures ES on ES.EmployeeSignatureID=E.EmployeeSignatureID
		where ReferralAssessmentID=@ReferralAssessmentID
	END
  ELSE
	BEGIN
		select top 1 RAR.* from ReferralAssessmentReview RAR
		inner join Referrals R on R.ReferralID=RAR.ReferralID
		inner join Employees E on E.EmployeeID=@LoginID
		left join EmployeeSignatures ES on ES.EmployeeSignatureID=E.EmployeeSignatureID
		where RAR.ReferralID=@ReferralID order by RAR.ReferralAssessmentID desc

		SELECT  ES.SignaturePath as Signature , E.LastName+', '+E.FirstName As CompletedBy, Assignee=@LoginID  FROM Employees E 
		LEFT JOIN EmployeeSignatures ES on ES.EmployeeSignatureID=E.EmployeeSignatureID
		where  E.EmployeeID=@LoginID
	END


	SELECT AHCCCSID FROM Referrals R WHERE R.ReferralID=@ReferralID

END
