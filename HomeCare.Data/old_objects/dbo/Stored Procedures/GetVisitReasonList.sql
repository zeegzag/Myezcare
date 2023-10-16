CREATE PROCEDURE [dbo].[GetVisitReasonList]
  @Type nvarchar(max) = NULL,
  @CompanyName nvarchar(max) = NULL
AS
BEGIN
  SELECT
    *
  FROM MasterVisitReasons
  WHERE
    (@Type IS NULL
      OR LEN(@Type) = 0
      OR [Type] = @Type)
    AND (@CompanyName IS NULL
      OR LEN(@CompanyName) = 0
      OR [CompanyName] = @CompanyName)
END