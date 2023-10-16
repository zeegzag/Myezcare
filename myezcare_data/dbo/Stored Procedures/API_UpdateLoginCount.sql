CREATE PROCEDURE [dbo].[API_UpdateLoginCount]
 @EmployeeId BIGINT,
 @LoginFailedCount INT,
 @IsActive BIT
AS        
BEGIN        
UPDATE dbo.Employees SET LoginFailedCount=@LoginFailedCount,IsActive=@IsActive
 WHERE EmployeeID=@EmployeeId
END 
