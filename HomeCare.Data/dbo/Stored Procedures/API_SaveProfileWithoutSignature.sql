CREATE PROCEDURE [dbo].[API_SaveProfileWithoutSignature]        
 @EmployeeID BIGINT,      
 @IsFingerPrintAuth BIT,      
 @FirstName VARCHAR(50),    
 @LastName VARCHAR(50),  
 @UserName VARCHAR(50),  
 @MobileNumber VARCHAR(20),  
 @IVRPin VARCHAR(4),  
 @Password VARCHAR(100)=NULL,  
 @PasswordSalt VARCHAR(MAX)=NULL  
AS                    
BEGIN  
  
DECLARE @OldPassword VARCHAR(100)  
DECLARE @OldPasswordSalt VARCHAR(MAX)  
  
SELECT @OldPassword=Password,@OldPasswordSalt=PasswordSalt FROM Employees WHERE EmployeeID=@EmployeeID  
 
 If EXISTS(SELECT EmployeeID FROM Employees WHERE MobileNumber=@MobileNumber AND EmployeeID!=@EmployeeID)
 BEGIN
	SELECT -1
 END
 ELSE If EXISTS(SELECT EmployeeID FROM Employees WHERE UserName=@UserName AND EmployeeID!=@EmployeeID)
 BEGIN
	SELECT -2
 END
 ELSE
 BEGIN
 UPDATE Employees SET FirstName=@FirstName, LastName=@LastName, IsFingerPrintAuth=@IsFingerPrintAuth, UpdatedBy=@EmployeeID, UpdatedDate=GETDATE(),  
 UserName=@UserName,MobileNumber=@MobileNumber,IVRPin=@IVRPin,Password=ISNULL(@Password,@OldPassword),PasswordSalt=ISNULL(@PasswordSalt,@OldPasswordSalt)  
 WHERE EmployeeID=@EmployeeID        
 SELECT 1
 END
END