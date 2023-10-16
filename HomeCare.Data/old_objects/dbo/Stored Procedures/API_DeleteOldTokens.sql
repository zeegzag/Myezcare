CREATE PROCEDURE [dbo].[API_DeleteOldTokens]  
 @EmployeeId BIGINT  
AS      
BEGIN      
 SELECT * FROM  dbo.UserTokens WHERE EmployeeID=@EmployeeId ;     
   
 DELETE FROM  dbo.UserTokens WHERE EmployeeID=@EmployeeId;      
      
END
