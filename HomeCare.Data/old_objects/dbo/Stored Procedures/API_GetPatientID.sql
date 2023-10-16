CREATE PROCEDURE [dbo].[API_GetPatientID]
@AccountNumber VARCHAR(100)
AS
BEGIN

SELECT ReferralID FROM Referrals WHERE AHCCCSID=@AccountNumber

END
