-- =============================================
-- Author:		Kundan
-- Create date: 10/21/2020
-- Description:	To get relation list
-- =============================================
CREATE PROCEDURE [adc].[Daycare_GetRelationTypeList]
(
	@DDType INT
)
AS
BEGIN	
	SELECT DISTINCT
    Value = DD.DDMasterID,
    Name = DD.Title
	  FROM DDMaster DD
	  WHERE ItemType = @DDType AND DD.IsDeleted = 0
END