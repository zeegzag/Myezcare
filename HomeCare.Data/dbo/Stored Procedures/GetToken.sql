CREATE PROCEDURE [dbo].[GetToken]  
  -- Add the parameters for the stored procedure here      
  @EmployeeId bigint,  
  @ExpireLoginDuration int,  
  @Token nvarchar(100),  
  @IsMobileToken bit,  
  @ServerCurrentDateTime nvarchar(30)  
AS  
BEGIN  
  -- SET NOCOUNT ON added to prevent extra result sets from      
  -- interfering with SELECT statements.      
  SET NOCOUNT ON;  
  DECLARE @CurrentDateTime datetime = CAST(@ServerCurrentDateTime AS datetime);  
  -- Insert statements for procedure here      
  
  IF NOT EXISTS  
    (  
      SELECT  
        1  
      FROM dbo.UserTokens  
      WHERE  
        EmployeeID = @EmployeeId  
    )  
  BEGIN  
  
    INSERT INTO UserTokens  
    (  
      EmployeeId,  
      ExpireLogin,  
      Token,  
      IsMobileToken  
    )  
    VALUES  
    (  
      @EmployeeId,  
      DATEADD(mi, @ExpireLoginDuration, @CurrentDateTime),  
      @Token,  
      @IsMobileToken  
    );  
  END  
  
  
  SELECT TOP 1  
    UT.EmployeeId,  
    UT.ExpireLogin,  
    UT.Token  
  FROM UserTokens UT  
  WHERE  
    UT.EmployeeId = @EmployeeId  
  
END