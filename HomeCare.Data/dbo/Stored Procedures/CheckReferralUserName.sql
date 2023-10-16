
--EXEC CheckEmployeeUniqueID @EmployeeUniqueID = 'EMP000001', @EmployeeID = '167', @IsDeleted = '0'    
CREATE PROCEDURE [dbo].[CheckReferralUserName]              
 -- Add the parameters for the stored procedure here              
 @UserName nvarchar(100),    
 @ReferralID BIGINT,    
 @IsDeleted BIT                    
AS              
BEGIN              
       SELECT TOP 1 ReferralID FROM Referrals WHERE UserName=@UserName AND ReferralID != @ReferralID AND IsDeleted=@IsDeleted    
 --IF EXISTS (SELECT TOP 1 EmployeeID FROM Employees WHERE EmployeeUniqueID=@EmployeeUniqueID AND EmployeeID != @EmployeeID AND IsDeleted=@IsDeleted)    
 --BEGIN        
 --SELECT -1 RETURN;    
 --END    
    
END