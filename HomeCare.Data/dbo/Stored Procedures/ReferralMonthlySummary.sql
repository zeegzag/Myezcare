--exec ReferralMonthlySummary
CREATE procedure [dbo].[ReferralMonthlySummary]
@ReferralID  bigint,
@ReferralMonthlySummariesID bigint=0,
@LoginID bigint
as                                                      
BEGIN
 
    if(@ReferralMonthlySummariesID > 0)
	BEGIN
		SELECT ROM.*,ES.SignaturePath as Signature,E.LastName + ', '+ E.FirstName As CompletedBy , R.ReferralID,
		R.LastName+', '+R.FirstName AS ClientName, R.AHCCCSID AS AHCCCSIdNumber, CM.LastName+', '+CM.FirstName AS CaseManager
		FROM ReferralMonthlySummaries ROM
		INNER JOIN Referrals R ON R.ReferralID=ROM.ReferralID
		INNER JOIN CaseManagers CM ON CM.CaseManagerID=R.CaseManagerID
		INNER JOIN Employees E on E.EmployeeID=ROM.UpdatedBy
		LEFT JOIN EmployeeSignatures ES on ES.EmployeeSignatureID=E.EmployeeSignatureID
		where ROM.ReferralMonthlySummariesID=@ReferralMonthlySummariesID
	END
   ELSE
	BEGIN
		declare @MonthlySummaryStartDate datetime
		declare @MonthlySummaryEndDate datetime
		SELECT  @MonthlySummaryStartDate=max(StartDate),@MonthlySummaryEndDate=EndDate from ScheduleMasters S Where S.ReferralID =@ReferralID AND S.ScheduleStatusID=2 AND S.IsDeleted=0 Group by StartDate,EndDate

		declare @ClientName varchar(max)
		declare @AHCCCSIdNumber varchar(max)
		declare @CaseManager varchar(max)

		SELECT  @ClientName=R.LastName+', '+R.FirstName,@AHCCCSIdNumber=R.AHCCCSID, @CaseManager=CM.LastName+', '+CM.FirstName
		FROM Referrals R INNER JOIN CaseManagers CM ON CM.CaseManagerID=R.CaseManagerID Where R.ReferralID =@ReferralID 

		-- R.LastName+', '+R.FirstName AS ClientName, R.AHCCCSID AS AHCCCSIdNumber, CM.LastName+', '+CM.FirstName AS CaseManager

		SELECT  ES.SignaturePath as Signature , E.LastName+', '+E.FirstName As CompletedBy,@MonthlySummaryStartDate  as MonthlySummaryStartDate,@MonthlySummaryEndDate as MonthlySummaryEndDate, 
		@ClientName AS ClientName, @AHCCCSIdNumber AS AHCCCSIdNumber,@CaseManager  AS CaseManager,@ReferralID AS ReferralID
		FROM Employees E 
		LEFT JOIN EmployeeSignatures ES on ES.EmployeeSignatureID=E.EmployeeSignatureID
		where  E.EmployeeID=@LoginID
	END

	SELECT 0; 

END