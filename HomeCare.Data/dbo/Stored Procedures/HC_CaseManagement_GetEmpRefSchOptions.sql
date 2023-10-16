CREATE PROCEDURE [dbo].[HC_CaseManagement_GetEmpRefSchOptions]  
 @ReferralID BIGINT = 0,                                                     
 @ScheduleID BIGINT = 0,                                    
 @StartDate DATETIME,                  
 @EndDate DATETIME,                    
 @SameDateWithTimeSlot BIT        
AS                                    
BEGIN          
    
     DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()                
SELECT RH.*, 
CreatedBy =dbo.GetGenericNameFormat(EC.FirstName,EC.MiddleName, EC.LastName,@NameFormat),            
    UpdatedBy =dbo.GetGenericNameFormat(EU.FirstName,EU.MiddleName, EU.LastName,@NameFormat),
CurrentActiveGroup = CASE WHEN GETDATE() BETWEEN RH.StartDate AND RH.EndDate THEN 1 ELSE 0 END,                
OldActiveGroup = CASE WHEN GETDATE() > RH.EndDate THEN 1 ELSE 0 END                
FROM ReferralOnHoldDetails RH                
INNER JOIN Employees EC ON EC.EmployeeID=RH.CreatedBy                
INNER JOIN Employees EU ON EU.EmployeeID=RH.UpdatedBy                
WHERE RH.IsDeleted=0  AND RH.ReferralID=@ReferralID-- AND (GETDATE() BETWEEN RH.StartDate AND RH.EndDate OR GETDATE() < RH.StartDate)                
ORDER BY StartDate DESC                  
                
SELECT ReferralID, FirstName, LastName, dbo.GetGenericNameFormat(FirstName,MiddleName, LastName,@NameFormat) AS ReferralName FROM Referrals WHERE ReferralID=@ReferralID                                
                        
                                    
END  