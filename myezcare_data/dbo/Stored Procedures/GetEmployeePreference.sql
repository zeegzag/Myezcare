CREATE PROCEDURE [dbo].[GetEmployeePreference]      
@EmployeeID BIGINT    
AS      
BEGIN      
      
 SELECT ep.*,p.PreferenceName FROM EmployeePreferences ep  
 INNER JOIN Preferences p ON ep.PreferenceID=p.PreferenceID  
 WHERE EmployeeID=@EmployeeID  
      
END
