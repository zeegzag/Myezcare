-- exec [HC_GetSetAddBatchPage] 144  
CREATE PROCEDURE [dbo].[HC_GetSetAddBatchPage]
  @BatchID bigint
AS

BEGIN

  SELECT
    *
  FROM Batches
  WHERE
    BatchID = @BatchID;

  SELECT
    *
  FROM BatchTypes
  WHERE
    IsDeleted = 0;

  SELECT
    *
  FROM Payors
  WHERE
    IsDeleted = 0
    AND IsBillingActive = 1
    AND PayorInvoiceType = 1
  ORDER BY PayorName;

  SELECT
    ServiceCodeID,
    ServiceCode =
    Sc.servicename + ' - ' +
    SC.ServiceCode +
    CASE
      WHEN LEN(M.Code) = 0 THEN ''
      ELSE ' - ' + M.Code
    END

  FROM ServiceCodes SC
  OUTER APPLY
  (
    SELECT
      ISNULL(STRING_AGG(M.ModifierCode, ', '), '') Code
    FROM Modifiers M
    WHERE
      M.ModifierID IN
      (
        SELECT
          val
        FROM GetCSVTable(SC.ModifierID)
      )
  ) M
  WHERE
    sc.ServiceCode IS NOT NULL
    AND SC.IsDeleted = 0 --and SC.ModifierID is not null
END