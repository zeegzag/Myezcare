CREATE PROCEDURE [dbo].[API_GetEmpRefList]    
 @EmployeeID BIGINT                
AS                                    
BEGIN

DECLARE @RoleID BIGINT;
SELECT @RoleID=RoleID FROM Employees WHERE EmployeeID=@EmployeeID
	
  SELECT EmployeeID,EmployeeName=dbo.GetGeneralNameFormat(FirstName,LastName) FROM Employees WHERE IsDeleted=0
  
  SELECT DISTINCT sm.ReferralID,PatientName=dbo.GetGeneralNameFormat(r.FirstName,r.LastName) FROM ScheduleMasters sm    
 INNER JOIN Referrals r ON r.ReferralID=sm.ReferralID  
 WHERE r.IsDeleted=0
 --WHERE ((@RoleID!=1 AND EmployeeID=@EmployeeID) OR @RoleID=1) AND r.IsDeleted=0
END
