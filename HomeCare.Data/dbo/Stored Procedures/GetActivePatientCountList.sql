CREATE PROCEDURE [dbo].[GetActivePatientCountList]    
        
           
AS                      
BEGIN                      
   SELECT DISTINCT count(*) as COUNT
FROM Referrals R          
WHERE R.IsDeleted=0 AND R.ReferralStatusID=1                    
END