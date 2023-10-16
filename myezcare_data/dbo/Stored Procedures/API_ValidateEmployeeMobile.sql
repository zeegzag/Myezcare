CREATE PROCEDURE [dbo].[API_ValidateEmployeeMobile]        
 @MobileNumber VARCHAR(20)  
AS        
BEGIN        
 -- SET NOCOUNT ON added to prevent extra result sets from        
 -- interfering with SELECT statements.  
    SELECT e.EmployeeID  
 FROM dbo.Employees e  
 WHERE e.MobileNumber=@MobileNumber  
END
