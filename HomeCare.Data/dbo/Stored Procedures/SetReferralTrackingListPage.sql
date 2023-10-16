
CREATE PROCEDURE [dbo].[SetReferralTrackingListPage]   
   
AS  
BEGIN  
 SELECT PayorID,PayorName from Payors order by PayorName ASC  
 SELECT * FROM ReferralStatuses  
 SELECT CaseManagerID,LastName+', '+FirstName as Name From CaseManagers order by LastName ASC --where IsDeleted=0  
 select AgencyID,NickName from Agencies order by NickName ASC  
 SELECT 0;  
 SELECT 0;  
END