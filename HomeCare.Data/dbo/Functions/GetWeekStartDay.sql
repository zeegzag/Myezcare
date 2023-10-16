CREATE FUNCTION [dbo].[GetWeekStartDay] ()
RETURNS INT
AS
BEGIN
  RETURN (
      SELECT CAST(OP.WeekStartDay AS INT) - 1
      FROM [Admin_Myezcare_Live].dbo.OrganizationPreference OP
      WHERE OP.OrganizationID = [dbo].[GetOrgId]()
      )
END
