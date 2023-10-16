CREATE PROCEDURE [dbo].[API_VerifyPatient]  
 @AccountNumber VARCHAR(10)
AS                                        
BEGIN                            
	SELECT ReferralID FROM Referrals WHERE AHCCCSID=@AccountNumber
END