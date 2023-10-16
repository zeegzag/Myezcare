CREATE PROCEDURE [dbo].[HC_GenerateInvoicesService] @ReferralIDs NVARCHAR(max) = NULL,
  @CareTypeIDs NVARCHAR(max) = NULL,
  @StartDate DATE = NULL,
  @EndDate DATE = NULL,
  @InvoiceGenerationFrequency INT = 1
AS
BEGIN
  DECLARE @CurrentDateTime DATETIME = dbo.GetOrgCurrentDateTime();
  DECLARE @UnitTypeVisit INT = 2;
  DECLARE @InvoiceDueDays INT,
    @InvoiceTaxRate DECIMAL(5, 2);

  SELECT @InvoiceDueDays = InvoiceDueDays,
    @InvoiceTaxRate = InvoiceTaxRate
  FROM OrganizationSettings;

  DECLARE @TmpTable TABLE (
    ScheduleID BIGINT,
    StartDate DATETIME,
    PayorID BIGINT,
    Rate DECIMAL(18, 2),
    PerUnitQuantity DECIMAL(18, 2),
    UnitType INT,
    ServiceCodeID BIGINT,
    PayorInvoiceType INT,
    CareTypeID BIGINT,
    ReferralID BIGINT,
    EmployeeID BIGINT,
    ServiceTime INT,
    EmployeeVisitID BIGINT,
    Freq NVARCHAR(max),
    FreqStartDate DATE,
    FreqEndDate DATE
    );

  INSERT INTO @TmpTable
  SELECT SM.ScheduleID,
    SM.StartDate,
    P.PayorID,
    RBA.Rate,
    CASE 
      WHEN RBA.UnitType != @UnitTypeVisit
        AND ISNULL(RBA.PerUnitQuantity, 0) != 0
        THEN RBA.PerUnitQuantity
      END,
    RBA.UnitType,
    RBA.ServiceCodeID,
    P.PayorInvoiceType,
    SM.CareTypeId,
    SM.ReferralID,
    SM.EmployeeID,
    ServiceTime = ISNULL(DATEDIFF(MINUTE, EV.ClockInTime, EV.ClockOutTime), 0),
    EV.EmployeeVisitID,
    IGF.*
  FROM EmployeeVisits EV
  INNER JOIN ScheduleMasters SM
    ON SM.ScheduleID = EV.ScheduleID
      AND SM.PayorID IS NOT NULL
      AND SM.IsDeleted = 0
  CROSS APPLY (
    SELECT DATEDIFF(DAY, '1900-01-01', SM.StartDate) + 1 DayNum,
      DATEDIFF(WEEK, '1900-01-01', DATEADD(D, - 1, SM.StartDate)) + 1 WeekNum,
      DATEDIFF(MONTH, '1900-01-01', SM.StartDate) + 1 MonthNum
    ) DT
  CROSS APPLY (
    SELECT DATEADD(WEEK, DT.WeekNum - 1, '1900-01-01') WeekStart,
      DATEADD(MONTH, DT.MonthNum - 1, '1900-01-01') MonthStart
    ) DTS
  CROSS APPLY (
    SELECT DATEADD(DAY, 6, DTS.WeekStart) WeekEnd,
      EOMONTH(DTS.MonthStart) MonthEnd
    ) DTE
  CROSS APPLY (
    SELECT CASE @InvoiceGenerationFrequency
        WHEN 1
          THEN 'S' + CONVERT(VARCHAR(max), SM.ScheduleID)
        WHEN 2
          THEN 'W' + CONVERT(VARCHAR(max), DT.WeekNum)
        WHEN 3
          THEN 'M' + CONVERT(VARCHAR(max), DT.MonthNum)
        WHEN 4
          THEN 'D' + CONVERT(VARCHAR(max), DT.DayNum)
        END Freq,
      CASE @InvoiceGenerationFrequency
        WHEN 2
          THEN DTS.WeekStart
        WHEN 3
          THEN DTS.MonthStart
        ELSE SM.StartDate
        END FreqStartDate,
      CASE @InvoiceGenerationFrequency
        WHEN 2
          THEN DTE.WeekEnd
        WHEN 3
          THEN DTE.MonthEnd
        ELSE SM.StartDate
        END FreqEndDate
    ) IGF
  INNER JOIN Payors P
    ON P.PayorID = SM.PayorID
      AND P.PayorInvoiceType = 2
      AND P.IsDeleted = 0
  INNER JOIN ReferralBillingAuthorizations RBA
    ON RBA.ReferralID = SM.ReferralID
      AND RBA.PayorID = P.PayorID
      AND RBA.ReferralBillingAuthorizationID = SM.ReferralBillingAuthorizationID
  WHERE EV.IsDeleted = 0
    AND RBA.ServiceCodeID IS NOT NULL
    AND ISNULL(EV.IsInvoiceGenerated, 0) = 0
    AND (
      @ReferralIDs IS NULL
      OR SM.ReferralID IN (
        SELECT val
        FROM GetCSVTable(@ReferralIDs)
        )
      )
    AND (
      @CareTypeIDs IS NULL
      OR SM.CareTypeId IN (
        SELECT val
        FROM GetCSVTable(@CareTypeIDs)
        )
      )
    AND (
      @StartDate IS NULL
      OR CONVERT(DATE, SM.StartDate) >= @StartDate
      )
    AND (
      @EndDate IS NULL
      OR CONVERT(DATE, SM.StartDate) <= @EndDate
      );

  --SELECT * FROM @TmpTable tt   
  IF EXISTS (
      SELECT 1
      FROM @TmpTable
      )
  BEGIN
    DECLARE @TmpInvoiceTable TABLE (
      TempInvoiceID UNIQUEIDENTIFIER DEFAULT NEWID(),
      InvoiceDate DATETIME,
      InvoiceType INT,
      InvoiceStatus NVARCHAR(100),
      PayAmount DECIMAL(18, 2),
      PaidAmount DECIMAL(18, 2),
      PayorInvoiceType INT,
      CareTypeID BIGINT,
      ReferralID BIGINT,
      ScheduleIDs NVARCHAR(max),
      Freq NVARCHAR(max),
      ServiceStartDate DATE,
      ServiceEndDate DATE
      );
    DECLARE @TmpInvoiceTransactionTable TABLE (
      ReferralInvoiceTransactionID BIGINT IDENTITY(1, 1),
      InvoiceID BIGINT,
      ScheduleID BIGINT,
      EmployeeID BIGINT,
      EmployeeVisitID BIGINT,
      Rate DECIMAL(18, 2),
      PerUnitQuantity DECIMAL(18, 2),
      Amount DECIMAL(18, 2),
      ServiceDate DATE,
      ServiceTime BIGINT
      );

    BEGIN TRANSACTION trans;

    BEGIN TRY
      DECLARE @outputForSchedulewise TABLE (
        TempInvoiceID UNIQUEIDENTIFIER,
        ScheduleIDs NVARCHAR(max)
        );

      INSERT INTO @TmpInvoiceTable (
        InvoiceDate,
        InvoiceType,
        InvoiceStatus,
        PayAmount,
        PaidAmount,
        PayorInvoiceType,
        CareTypeID,
        ReferralID,
        ScheduleIDs,
        Freq,
        ServiceStartDate,
        ServiceEndDate
        )
      OUTPUT INSERTED.TempInvoiceID,
        INSERTED.ScheduleIDs
      INTO @outputForSchedulewise
      SELECT InvoiceDate = @CurrentDateTime,
        InvoiceType = @InvoiceGenerationFrequency,
        InvoiceStatus = 'NEW',
        PayAmount = SUM(CASE 
            WHEN tt.UnitType = @UnitTypeVisit
              OR tt.PerUnitQuantity IS NULL
              THEN tt.Rate
            ELSE CONVERT(DECIMAL(18, 2), CONVERT(DECIMAL(18, 2), ROUND(tt.ServiceTime 
                    / tt.PerUnitQuantity, 0, 0)) * tt.Rate)
            END),
        PaidAmount = 0,
        tt.PayorInvoiceType,
        tt.CareTypeID,
        ReferralID = tt.ReferralID,
        ScheduleIDs = STRING_AGG(tt.ScheduleID, ','),
        tt.Freq,
        MIN(FreqStartDate),
        MAX(FreqEndDate)
      FROM @TmpTable tt
      GROUP BY tt.PayorInvoiceType,
        tt.CareTypeID,
        tt.ReferralID,
        tt.Freq;

      DECLARE @outputReferralInvoices TABLE (
        ReferralInvoiceID BIGINT,
        TempInvoiceID UNIQUEIDENTIFIER
        );

      INSERT INTO ReferralInvoices (
        InvoiceDate,
        InvoiceType,
        InvoiceStatus,
        PayAmount,
        CareTypeID,
        ReferralID,
        IsDeleted,
        TempInvoiceID,
        InvoiceTaxRate,
        InvoiceDueDate,
        ServiceStartDate,
        ServiceEndDate
        )
      OUTPUT INSERTED.ReferralInvoiceID,
        INSERTED.TempInvoiceID
      INTO @outputReferralInvoices
      SELECT InvoiceDate,
        InvoiceType,
        1,
        PayAmount = PayAmount + ((PayAmount * @InvoiceTaxRate) / 100
          ),
        CareTypeID,
        ReferralID,
        0,
        TempInvoiceID,
        @InvoiceTaxRate,
        DATEADD(dd, @InvoiceDueDays, @CurrentDateTime),
        ServiceStartDate,
        ServiceEndDate
      FROM @TmpInvoiceTable;

      --SELECT * FROM ReferralInvoices
      INSERT INTO @TmpInvoiceTransactionTable (
        InvoiceID,
        ScheduleID,
        EmployeeID,
        EmployeeVisitID,
        Rate,
        PerUnitQuantity,
        Amount,
        ServiceDate,
        ServiceTime
        )
      SELECT ori.ReferralInvoiceID,
        tt.ScheduleID,
        EmployeeID,
        EmployeeVisitID,
        Rate,
        PerUnitQuantity,
        Amount = CASE 
          WHEN tt.UnitType = @UnitTypeVisit
            OR tt.PerUnitQuantity IS NULL
            THEN tt.Rate
          ELSE CONVERT(DECIMAL(18, 2), CONVERT(DECIMAL(18, 2), ROUND(tt.ServiceTime / 
                  tt.PerUnitQuantity, 0, 0)) * tt.Rate)
          END,
        CONVERT(DATE, tt.StartDate),
        ServiceTime
      FROM @TmpTable tt
      INNER JOIN @outputForSchedulewise O
        ON ',' + O.ScheduleIDs + ',' LIKE '%,' + CONVERT(NVARCHAR(max), tt.ScheduleID) + 
          ',%'
      INNER JOIN @outputReferralInvoices ori
        ON ori.TempInvoiceID = o.TempInvoiceID;

      INSERT INTO ReferralInvoiceTransactions (
        ReferralInvoiceID,
        ScheduleID,
        EmployeeID,
        EmployeeVisitID,
        Rate,
        PerUnitQuantity,
        Amount,
        ServiceDate,
        ServiceTime,
        IsDeleted
        )
      SELECT InvoiceID,
        ScheduleID,
        EmployeeID,
        EmployeeVisitID,
        Rate,
        PerUnitQuantity,
        Amount,
        ServiceDate,
        ServiceTime,
        0
      FROM @TmpInvoiceTransactionTable;

      --SELECT * FROM ReferralInvoiceTransactions
      UPDATE ev
      SET ev.IsInvoiceGenerated = 1
      FROM @TmpInvoiceTransactionTable T
      INNER JOIN EmployeeVisits ev
        ON ev.EmployeeVisitID = T.EmployeeVisitID;

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
  ELSE
  BEGIN
    SELECT 0 AS TransactionResultId;
  END
END
