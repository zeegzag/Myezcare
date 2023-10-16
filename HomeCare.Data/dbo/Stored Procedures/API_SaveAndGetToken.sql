CREATE PROCEDURE [dbo].[API_SaveAndGetToken]  
 -- Add the parameters for the stored procedure here  
 @EmployeeId BIGINT,  
 @ExpireLoginDuration INT,  
 @Token NVARCHAR(100),  
 @IsMobileToken BIT,  
 @ServerCurrentDateTime NVARCHAR(30)  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
 DECLARE @CurrentDateTime DATETIME = CAST(@ServerCurrentDateTime AS DATETIME);   
    -- Insert statements for procedure here  
   
 DELETE FROM dbo.UserTokens WHERE EmployeeID=@EmployeeId  
  
 INSERT INTO UserTokens(EmployeeId, ExpireLogin , Token, IsMobileToken)  
 VALUES(@EmployeeId,DATEADD(mi,@ExpireLoginDuration,@CurrentDateTime),@Token,@IsMobileToken);   
   
 --UPDATE Employees SET LastLoginDate = @CurrentDateTime WHERE EmployeeId = @EmployeeId;  
   
 SELECT UT.EmployeeId,UT.ExpireLogin   
 FROM UserTokens UT  
 WHERE UT.Token=@Token  
  
END