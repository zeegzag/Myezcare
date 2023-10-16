-- EXEC GetCareTypeDropDownByPayorId @PayorId = '1'
-- =============================================
-- Author:		Tapendra kumar sharma
-- Create date: 07/09/2019
-- Description:	For get care type drop down by payorid
-- =============================================
CREATE PROCEDURE [dbo].[GetCareTypeDropDownByPayorId]
 @PayorId bigint
AS
BEGIN
		SELECT DISTINCT DDMasterID = dm.DDMasterID , Title = dm.Title FROM PayorServiceCodeMapping   pscm
		INNER JOIN DDMaster dm 
		ON pscm.CareType  =  dm.DDMasterID 
		WHERE pscm.PayorID = @PayorId  AND pscm.POSEndDate > getdate()
END
GO