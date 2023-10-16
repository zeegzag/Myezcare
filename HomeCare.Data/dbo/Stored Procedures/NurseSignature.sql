CREATE PROCEDURE [dbo].[NurseSignature]  
 @SystemID NVARCHAR(MAX) = NULL  
 ,@LoggedInID BIGINT = NULL  
 ,@Signature NVARCHAR(MAX) = NULL  
 ,@List  [dbo].[NurseSignatureVisit] READONLY  
AS  
BEGIN  
	
	 UPDATE EV
		SET 
		SignNote = L.[SignNote],
		UpdatedBy = @LoggedInID,
		UpdatedDate = GETUTCDATE(),
		[Signature] = @Signature,
		SignedBy = @LoggedInID,
		SignedDate = GETUTCDATE()
	 FROM EmployeeVisits EV 
	 INNER JOIN @List L ON EV.EmployeeVisitID = L.[EmployeeVisitID]
	 WHERE
		EV.IsDeleted = 0 AND ISNULL(EV.IsApproved, 1) = 1

END
GO

