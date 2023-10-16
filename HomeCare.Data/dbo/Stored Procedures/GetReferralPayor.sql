CREATE procedure [dbo].[GetReferralPayor]                              
                                                                  
AS                                                    
BEGIN             
            
WITH CTEGetReferralPayor AS           
(            
SELECT *,COUNT(T1.PayorName) OVER() AS Count FROM                                                     
  (         
SELECT  P.PayorID, P.PayorName,COUNT(1) AS PayorCount   
FROM  Referrals R      
INNER JOIN ReferralPayorMappings RPM  ON R.ReferralID = RPM.ReferralID    AND RPM.IsActive = 1  AND RPM.IsDeleted = 0   AND RPM.Precedence = 1       
INNER JOIN Payors P ON P.PayorID = RPM.PayorID          
 WHERE R.IsDeleted=0 AND RPM.IsDeleted=0 AND P.IsDeleted=0 GROUP By  P.PayorID, P.PayorName) AS T1  )         
            
SELECT * FROM CTEGetReferralPayor                
 outer apply(select sum(ap.PayorCount) as TotalPayor from CTEGetReferralPayor ap) as TotalPayor                                                                     
            
END 