
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[HC_GetBatchUploadedClaimErrors]
	@BatchUploadedClaimID nvarchar(500)=0  
AS
BEGIN
	
	SELECT * FROM BatchUploadedClaimErrors WHERE BatchUploadedClaimID=@BatchUploadedClaimID
    
END