CREATE PROCEDURE [dbo].[GetTwilioNotifyModel]    
@EmployeeIDs VARCHAR(MAX)
AS    
BEGIN    
  
  
  SELECT binding_type='SMS',address=MobileNumber FROM Employees WHERE EmployeeID IN (SELECT VAL FROM GetCSVTable(@EmployeeIDs))
     
  
END
