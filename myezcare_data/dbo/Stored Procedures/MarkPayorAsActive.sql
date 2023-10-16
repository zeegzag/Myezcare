CREATE PROCEDURE [dbo].[MarkPayorAsActive]
@ReferralID BIGINT,
@ReferralPayorMappingID BIGINT,
@LoggedIdID BIGINT
AS
BEGIN
 
 UPDATE ReferralPayorMappings SET IsActive=0,UpdatedBy=@LoggedIdID WHERE ReferralID=@ReferralID AND IsActive=1;
 UPDATE ReferralPayorMappings SET IsActive=0 WHERE ReferralID=@ReferralID;
 UPDATE ReferralPayorMappings SET IsActive=1,UpdatedBy=@LoggedIdID WHERE ReferralPayorMappingID=@ReferralPayorMappingID;

 
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
