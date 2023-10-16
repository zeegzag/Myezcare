 --  EXEC GetReferralStatus          
CREATE procedure [dbo].[GetReferralStatus]  
AS                                                  
BEGIN   
  
WITH CTEList AS         
(          
SELECT *,COUNT(T1.Status) OVER() AS Count FROM                                                   
  (       
                                               
SELECT RS.ReferralStatusID, RS.Status,COUNT(1) AS StatusCount FROM Referrals R   
INNER JOIN ReferralStatuses RS ON RS.ReferralStatusID = R.ReferralStatusID           
WHERE R.IsDeleted=0 GROUP By  RS.ReferralStatusID, RS.Status) AS T1  )       
          
SELECT * FROM CTEList              
 outer apply(select sum(ap.StatusCount) as TotalStatus from CTEList ap) as TotalStatus  
  
 END 