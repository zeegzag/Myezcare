CREATE PROCEDURE [dbo].[API_GetCareTypes]
 @DDType_CareType INT
AS                  
BEGIN
	SELECT CareTypeID=DDMasterID,CareTypeName=Title FROM DDMaster WHERE ItemType=@DDType_CareType AND IsDeleted=0
END