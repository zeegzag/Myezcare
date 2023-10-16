-- =============================================
-- Author:		Kundan Kumar Rai
-- Create date: 15-04-2020
-- Description:	Add manage claim page
-- =============================================
CREATE PROCEDURE HC_AddManageClaim
AS
BEGIN
	SELECT p.PayorID,p.PayorName from Payors as p Where p.IsDeleted=0 ORDER BY p.PayorName ASC  
	SELECT ReferralID, ReferralName=dbo.GetGeneralNameFormat(FirstName,LastName), AHCCCSID  From Referrals Where IsDeleted=0 ORDER BY LastName ASC                 
END
GO
