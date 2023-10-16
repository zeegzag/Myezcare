CREATE PROCEDURE [dbo].[GetPatientResignatureFlag]  
AS  
BEGIN  
 SELECT TOP 1 PatientResignatureNeeded FROM OrganizationSettings
END
