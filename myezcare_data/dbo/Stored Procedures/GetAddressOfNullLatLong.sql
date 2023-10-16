CREATE PROCEDURE [dbo].[GetAddressOfNullLatLong]    
     
AS                      
BEGIN                      
 SELECT EmployeeID, FullAddress=Address+','+City+','+s.StateName+','+ZipCode FROM Employees e    
 INNER JOIN States s ON s.StateCode=e.StateCode    
 WHERE Latitude IS NULL AND Longitude IS NULL    
    
 SELECT r.ReferralID,FullAddress=Address+','+City+','+s.StateName+','+ZipCode FROM Contacts c    
 INNER JOIN States s ON s.StateCode=c.State    
 INNER JOIN ContactMappings cm ON cm.ContactID=c.ContactID AND cm.ContactTypeID=1  
 INNER JOIN Referrals r ON r.ReferralID=cm.ReferralID    
 WHERE c.Latitude IS NULL AND c.Longitude IS NULL    
END
