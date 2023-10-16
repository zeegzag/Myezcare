
--  EXEC ADC.Daycare_GetRelationTypeList @DDType = '33'
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
	--SELECT DISTINCT
 --   Value = DD.DDMasterID,
 --   Name = DD.Title
	--  FROM DDMaster DD
	--  WHERE ItemType = @DDType AND DD.IsDeleted = 0

	-- =============================================
-- Author:		Akhilesh
-- Create date: 10/29/2020
-- Description:	To get relation list
-- =============================================
	  SELECT DISTINCT
    Value = DD.DDMasterID,
    Name = DD.Title
	  FROM DDMaster DD
	  inner join lu_DDMasterTypes lu on lu.DDMasterTypeID = DD.ItemType  
where lu.Name='Relation Type'  AND DD.IsDeleted = 0

  
END