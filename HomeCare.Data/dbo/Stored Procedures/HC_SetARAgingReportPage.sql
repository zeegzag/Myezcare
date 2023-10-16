-- EXEC HC_SetARAgingReportPage         
CREATE PROCEDURE HC_SetARAgingReportPage              
AS                                          
BEGIN                               
     
   SELECT Value=PayorID, Name=ShortName FROM Payors ORDER BY ShortName ASC  
   SELECT  Value=ReferralID, Name= LastName+', '+  FirstName  FROM Referrals  ORDER BY LastName ASC  
  
END 