-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DeleteDxCodeMapping]
	-- Add the parameters for the stored procedure here
	@ReferralDXCodeMappingID BIGINT,
	@ReferralId BIGINT,
	@IsSoftDelte BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   IF(@IsSoftDelte=1)
   BEGIN
			UPDATE ReferralDXCodeMappings SET IsDeleted= (IsDeleted ^ 1)
			WHERE ReferralDXCodeMappingID=@ReferralDXCodeMappingID;
   END

   ELSE
   BEGIN
			DELETE FROM ReferralDXCodeMappings
			WHERE ReferralDXCodeMappingID=@ReferralDXCodeMappingID;
   END

   SELECT RD.ReferralDXCodeMappingID,D.DXCodeID,D.DXCodeName,RD.Precedence,RD.StartDate,RD.EndDate,D.Description,D.EffectiveFrom,D.EffectiveTo,RD.IsDeleted
	FROM ReferralDXCodeMappings RD
	INNER JOIN DxCodes D ON D.DXCodeID=RD.DXCodeID
	WHERE RD.ReferralID = @ReferralId;

END

