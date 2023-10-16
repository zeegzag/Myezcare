-- =============================================  
-- Author:  <Sagar Thakkar>  
-- Create date: <22/08/2016>  
-- Description: <Description,,>  
-- =============================================  
CREATE PROCEDURE [dbo].[API_ValidateToken]  
 -- Add the parameters for the stored procedure here  
 @Token NVARCHAR(100),  
 @ServerCurrentDateTime NVARCHAR(30)  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
 DECLARE @CurrentDateTime DATETIME = CAST(@ServerCurrentDateTime AS DATETIME);   
    -- Insert statements for procedure here  
 SELECT UT.EmployeeID,UT.ExpireLogin   
 FROM usertokens UT   
 WHERE UT.Token= @Token  
 AND UT.ExpireLogin > (SELECT @CurrentDateTime);  
  
END