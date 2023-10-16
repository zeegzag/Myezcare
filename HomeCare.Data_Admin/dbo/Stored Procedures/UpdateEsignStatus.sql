CREATE PROCEDURE [dbo].[UpdateEsignStatus]
@OrganizationEsignID BIGINT,
@OrganizationID BIGINT,
@OrganizationStatusEsignEmailSent INT,
@LoggedInUserId BIGINT,
@SystemID VARCHAR(20)
AS    
BEGIN
	
	UPDATE
		Organizations
	SET
		OrganizationStatusID = @OrganizationStatusEsignEmailSent,
		UpdatedDate = GETDATE(),
		UpdatedBy = @LoggedInUserId
	WHERE
		OrganizationID = @OrganizationID
		
	UPDATE
		OrganizationEsigns
	SET
		EsignSentDate = GETDATE(),
		UpdatedDate = GETDATE(),
		UpdatedBy = @LoggedInUserId
	WHERE
		OrganizationEsignID = @OrganizationEsignID
		
	SELECT 
		1 AS TransactionResultId,
		@OrganizationEsignID AS TablePrimaryId
END