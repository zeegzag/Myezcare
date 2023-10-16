CREATE PROCEDURE [dbo].[HC_GetAddProcess270271Model]  
AS  
BEGIN  
  
SELECT Name=PayorName, Value=PayorID FROM Payors P WHERE  P.IsDeleted=0  
  
SELECT ReferralStatusID,Status FROM ReferralStatuses RS   
  
END
