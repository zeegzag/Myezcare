CREATE PROCEDURE [dbo].[DeleteInvoices] @ReferralInvoiceIDs NVARCHAR(max) = NULL
AS
BEGIN
  BEGIN TRANSACTION trans;

  BEGIN TRY
    UPDATE ReferralInvoices
    SET IsDeleted = 1
    WHERE ReferralInvoiceID IN (
        SELECT Val
        FROM GetCSVTable(@ReferralInvoiceIDs)
        )

    UPDATE ReferralInvoiceTransactions
    SET IsDeleted = 1
    WHERE ReferralInvoiceID IN (
        SELECT Val
        FROM GetCSVTable(@ReferralInvoiceIDs)
        )

    UPDATE ReferralPaymentHistories
    SET IsDeleted = 1
    WHERE ReferralInvoiceID IN (
        SELECT Val
        FROM GetCSVTable(@ReferralInvoiceIDs)
        )

    UPDATE EV
    SET IsInvoiceGenerated = 0
    FROM EmployeeVisits EV
    INNER JOIN ReferralInvoiceTransactions RIT
      ON RIT.EmployeeVisitID = EV.EmployeeVisitID
    WHERE RIT.ReferralInvoiceID IN (
        SELECT Val
        FROM GetCSVTable(@ReferralInvoiceIDs)
        )
      AND EV.IsDeleted = 0

    SELECT 1 AS TransactionResultId;

    IF @@TRANCOUNT > 0
    BEGIN
      COMMIT TRANSACTION trans;
    END
  END TRY

  BEGIN CATCH
    SELECT - 1 AS TransactionResultId,
      ERROR_MESSAGE() AS ErrorMessage;

    IF @@TRANCOUNT > 0
    BEGIN
      ROLLBACK TRANSACTION trans;
    END
  END CATCH
END