CREATE PROCEDURE [dbo].[SetOrganizationListModel]  
AS  
BEGIN  
  
SELECT NAME=OrganizationTypeName, Value=OrganizationTypeID FROM OrganizationTypes  

SELECT NAME=OrganizationStatusName, Value=OrganizationStatusID FROM OrganizationStatuses  
  
END