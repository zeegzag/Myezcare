CREATE VIEW [dbo].[VW_ActiveEmployees]
	AS SELECT * FROM Employees where IsActive=1 and IsDeleted=0
