CREATE PROCEDURE [dbo].[SandataUpdateClientLastSent]
    @ReferralID BIGINT
AS
BEGIN
  UPDATE dbo.Referrals
  SET
    SandataLastSent = UpdatedDate
  WHERE
    ReferralID = @ReferralID
END