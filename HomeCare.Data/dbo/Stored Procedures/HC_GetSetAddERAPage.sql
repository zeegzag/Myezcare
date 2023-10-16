CREATE PROCEDURE [dbo].[HC_GetSetAddERAPage]                  
@ClaimID BIGINT                            
AS                             
                           
BEGIN            
  
-- SELECT DISTINCT PayerID FROM LatestERAs LTE    
--SELECT * from Payors P WHERE  P.IsDeleted=0 AND P.IsBillingActive=1  AND P.PayorInvoiceType=1  
select * from OrganizationSettings
SELECT DisplayPayorName= P.PayorName +   
CASE WHEN LTE.PayerID IS NOT NULL AND LTE.PayerName IS NOT NULL THEN '   ('+ LTE.PayerID +' : '+ LTE.PayerName+')'  ELSE '' END  
, * from Payors P   
LEFT JOIN (SELECT DISTINCT PayerID,PayerName FROM LatestERAs) AS  LTE ON LTE.PayerID = P.EraPayorID  
WHERE P.IsDeleted=0 AND P.IsBillingActive=1  AND P.PayorInvoiceType=1 ORDER BY P.PayorName ASC;           
     
END  
GO