create PROCEDURE GetReferralPayorDetail
@PayorID BIGINT=0,
@ReferralID BIGINT =0
AS
BEGIN
SELECT DISTINCT P.PayorID,P.PayorInvoiceType
FROM Payors P
left JOIN  ReferralPayorMappings RPM ON RPM.PayorID=P.PayorID
WHERE P.PayorID=@PayorID AND RPM.IsDeleted=0 AND RPM.ReferralID=ReferralID
END

