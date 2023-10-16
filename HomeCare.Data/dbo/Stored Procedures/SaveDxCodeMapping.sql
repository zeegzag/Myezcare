-- =============================================
-- Author:		<Sagar Thakkar>
-- Create date: <09/07/2016>
-- Description:	<This sp will save the Dxcode mapping lsit>
-- =============================================
CREATE PROCEDURE [dbo].[SaveDxCodeMapping]
	-- Add the parameters for the stored procedure here
	@DxCodelist VARCHAR(MAX),
	@ReferralID BIGINT,
	@SystemID VARCHAR(100),
	@LoggedInUserId BIGINT,
	@ServerDateTime DATETIME

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
	INSERT INTO ReferralDXCodeMappings (ReferralID,DXCodeID,CreatedDate,CreatedBy,UpdatedBy,UpdatedDate,SystemID) 
				SELECT @ReferralID,val ,@ServerDateTime,@LoggedInUserId,@LoggedInUserId,@ServerDateTime,@SystemID
				FROM [dbo].[GetCSVTable](@DxCodelist)
				WHERE 
				val NOT IN 
						(	SELECT DXCodeID FROM ReferralDXCodeMappings 
							WHERE 
								ReferralID=@ReferralID
						)	


END