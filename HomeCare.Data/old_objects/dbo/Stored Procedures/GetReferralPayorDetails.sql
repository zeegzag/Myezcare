CREATE PROCEDURE [dbo].[GetReferralPayorDetails]
@ReferralID BIGINT
AS
BEGIN
 
 SELECT RPM.*, RPM.PayorID AS TempPayorID          
 FROM ReferralPayorMappings RPM          
 WHERE RPM.IsDeleted = 0 AND RPM.IsActive =1  AND RPM.ReferralID =@ReferralID    
 
 
 SELECT RPM.ReferralPayorMappingID,RPM.PayorEffectiveDate,RPM.PayorEffectiveEndDate, RPM.IsActive ,P.PayorName,E.LastName +', ' + E.FirstName AS AddedByName,E1.LastName +' ' + E1.FirstName AS UpdatedByName,          
 RPM.CreatedDate,RPM.UpdatedDate          
 FROM ReferralPayorMappings RPM           
 INNER JOIN Payors P ON P.PayorID = RPM.PayorID          
 INNER JOIN Employees E ON E.EmployeeID = RPM.CreatedBy          
 INNER JOIN Employees E1 ON E1.EmployeeID = RPM.UpdatedBy          
 AND RPM.IsDeleted = 0 AND RPM.ReferralID = @ReferralID           
 ORDER BY RPM.UpdatedDate DESC 


 




END
