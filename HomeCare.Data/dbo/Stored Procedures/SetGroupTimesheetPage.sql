CREATE PROCEDURE [dbo].[SetGroupTimesheetPage]
AS

SELECT EmployeeID
	,EmployeeName = dbo.GetGeneralNameFormat(FirstName, LastName)
FROM Employees
WHERE IsDeleted = 0
ORDER BY LastName ASC

SELECT FacilityID  
 ,FacilityName 
FROM Facilities  
WHERE IsDeleted = 0  
AND FacilityName IS NOT NULL
ORDER BY FacilityName ASC  

SELECT ReferralID
	,ReferralName = dbo.GetGeneralNameFormat(FirstName, LastName)
	,AHCCCSID
FROM Referrals
WHERE IsDeleted = 0
ORDER BY LastName ASC

SELECT p.PayorID
	,p.PayorName
FROM Payors AS p
WHERE p.IsDeleted = 0
ORDER BY p.PayorName ASC

SELECT dds.DDMasterID AS CareTypeID
	,dds.Title AS CareType
FROM DDMaster dds
INNER JOIN lu_DDMasterTypes AS luddm ON dds.ItemType = luddm.DDMasterTypeID
WHERE dds.IsDeleted = 0
	AND luddm.Name = 'Care Type'

RETURN
