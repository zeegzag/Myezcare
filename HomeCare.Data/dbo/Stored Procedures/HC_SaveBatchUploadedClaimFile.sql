
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[HC_SaveBatchUploadedClaimFile]
@ClaimMD_FileID nvarchar(100),
@FileName nvarchar(100),
@Claims int,
@Amount nvarchar(100),
@BatchUploadedClaimID bigint,
@ClaimMD_ID nvarchar(100)
	
AS
BEGIN

	SET NOCOUNT ON;

	INSERT INTO BatchUploadedClaimFiles         
	(ClaimMD_FileID,[FileName],[Claims],[Date],Amount,BatchUploadedClaimID,ClaimMD_ID)        
	VALUES        
	(@ClaimMD_FileID,@FileName,@Claims,GETDATE(),@Amount,@BatchUploadedClaimID,@ClaimMD_ID)        
       
	SELECT * FROM BatchUploadedClaimFiles WHERE BatchUpClaimFileID=SCOPE_IDENTITY()  
  
END