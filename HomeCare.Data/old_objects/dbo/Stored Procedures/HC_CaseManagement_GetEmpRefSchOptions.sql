CREATE PROCEDURE [dbo].[HC_CaseManagement_GetEmpRefSchOptions]
 @ReferralID BIGINT = 0,                                                   
 @ScheduleID BIGINT = 0,                                  
 @StartDate DATETIME,                
 @EndDate DATETIME,                  
 @SameDateWithTimeSlot BIT      
AS                                  
BEGIN        
  
                  
SELECT RH.*, CreatedBy=dbo.GetGeneralNameFormat(EC.FirstName,EC.LastName), UpdatedBy=dbo.GetGeneralNameFormat(EU.FirstName,EU.LastName),               
CurrentActiveGroup = CASE WHEN GETDATE() BETWEEN RH.StartDate AND RH.EndDate THEN 1 ELSE 0 END,              
OldActiveGroup = CASE WHEN GETDATE() > RH.EndDate THEN 1 ELSE 0 END              
FROM ReferralOnHoldDetails RH              
INNER JOIN Employees EC ON EC.EmployeeID=RH.CreatedBy              
INNER JOIN Employees EU ON EU.EmployeeID=RH.UpdatedBy              
WHERE RH.IsDeleted=0  AND RH.ReferralID=@ReferralID-- AND (GETDATE() BETWEEN RH.StartDate AND RH.EndDate OR GETDATE() < RH.StartDate)              
ORDER BY StartDate DESC                
              
SELECT ReferralID, FirstName, LastName FROM Referrals WHERE ReferralID=@ReferralID                              
                      
                                  
END
