CREATE PROCEDURE [dbo].[GetReferralAuthorizationsDetails]                            
    @ReferralIDs NVARCHAR(MAX)              
AS                          
BEGIN                              
 
    SELECT
        RBA.ReferralID,
        RBA.Type,
        RBA.AuthorizationCode,
        RBA.StartDate,
        RBA.EndDate,
        P.PayorID,
        P.PayorName
    FROM
        [dbo].[ReferralBillingAuthorizations] RBA
    INNER JOIN [dbo].[GetCSVTable](@ReferralIDs) I
        ON RBA.ReferralID = I.val
    LEFT JOIN [dbo].[Payors] P 
        ON RBA.PayorID = P.PayorID
     
END 