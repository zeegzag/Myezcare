
CREATE PROCEDURE [dbo].[GetReferralAuthorizationsDetails]                            
    @ReferralIDs NVARCHAR(MAX)              
AS                          
BEGIN                              
 
    SELECT
		RBA.[Type],
        RBA.ReferralID,
		RBA.PayorID,
        STRING_AGG(RBA.AuthorizationCode, ', '),
        MIN(RBA.StartDate) StartDate,
        MAX(RBA.EndDate) EndDate,
        MIN(P.PayorID) PayorID,
        MIN(P.PayorName) PayorName
    FROM
        [dbo].[ReferralBillingAuthorizations] RBA
    INNER JOIN [dbo].[GetCSVTable](@ReferralIDs) I
        ON RBA.ReferralID = I.val
	LEFT JOIN [dbo].[Payors] P 
        ON RBA.PayorID = P.PayorID
    GROUP BY
		RBA.[Type], RBA.ReferralID, RBA.PayorID
     
END