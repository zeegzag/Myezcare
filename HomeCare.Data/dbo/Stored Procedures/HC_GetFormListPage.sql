CREATE PROCEDURE [dbo].[HC_GetFormListPage]
AS
BEGIN

SELECT Name=dbo.GetGeneralNameFormat(R.FirstName,R.LastName), Value=R.ReferralID FROM Referrals R WHERE R.IsDeleted=0
SELECT Name=dbo.GetGeneralNameFormat(E.FirstName,E.LastName), Value=E.EmployeeID FROM Employees E WHERE E.IsDeleted=0

END