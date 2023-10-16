CREATE PROCEDURE [dbo].[DeleteEmployeePreference]
@EmployeePreferenceID BIGINT
AS
BEGIN 

DELETE FROM EmployeePreferences WHERE EmployeePreferenceID=@EmployeePreferenceID

END