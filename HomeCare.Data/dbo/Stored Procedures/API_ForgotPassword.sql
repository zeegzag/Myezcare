CREATE PROCEDURE [dbo].[API_ForgotPassword]    
 @Action INT,    
 @MobileNumber NVARCHAR(50),    
 @Password NVARCHAR(100),    
 @PasswordSalt VARCHAR(MAX),    
 @OTP NVARCHAR(10),    
 @ServerCurrentDateTime NVARCHAR(30),    
 @Type NVARCHAR(100)    
AS    
BEGIN    
 DECLARE @IsOTPNotFound BIT;    
 DECLARE @EmployeeID BIGINT=(SELECT EmployeeID FROM Employees WHERE MobileNumber=@MobileNumber);    
 DECLARE @CurrentDateTime DATETIME = CAST(@ServerCurrentDateTime AS DATETIME);    
 DECLARE @TablePrimaryId BIGINT;    
 BEGIN TRANSACTION trans    
 BEGIN TRY    
  IF (@EmployeeID IS NOT NULL)    
   BEGIN    
    IF (@Action = 1)    
     BEGIN    
      INSERT INTO UserOtps (EmployeeID,OTP,SentDate,IsSMSSent,Type)    
      VALUES (@EmployeeID,@OTP,@CurrentDateTime,1,@Type)    
      SET @TablePrimaryId =SCOPE_IDENTITY();    
     END    
    ELSE IF (@Action = 2)    
     BEGIN    
      UPDATE Employees SET Password=@Password,PasswordSalt=@PasswordSalt,IsActive=1,LoginFailedCount=0 WHERE MobileNumber=@MobileNumber    
     END    
    ELSE IF (@Action = 3)    
     BEGIN    
      IF EXISTS (SELECT 1 FROM UserOtps WHERE EmployeeID=@EmployeeID AND OTP=@OTP)    
       BEGIN    
        DELETE FROM UserOtps WHERE EmployeeID=@EmployeeID;    
        SET @IsOTPNotFound = 1    
       END    
      ELSE    
       BEGIN    
        SET @IsOTPNotFound = 0    
       END    
     END    
   END    
   IF @@TRANCOUNT > 0    
    BEGIN     
     COMMIT TRANSACTION trans    
    END    
   SELECT FirstName,LastName,MobileNumber,Password,Email FROM dbo.Employees e WHERE MobileNumber=@MobileNumber;    
   SELECT 1 AS TransactionResultId,@IsOTPNotFound AS IsOTPNotFound,@TablePrimaryId AS TablePrimaryId;       
 END TRY    
 BEGIN CATCH    
   SELECT -1 AS TransactionResultId,ERROR_MESSAGE() AS ErrorMessage;    
   IF @@TRANCOUNT > 0    
    BEGIN     
     ROLLBACK TRANSACTION trans     
    END    
 END CATCH    
END