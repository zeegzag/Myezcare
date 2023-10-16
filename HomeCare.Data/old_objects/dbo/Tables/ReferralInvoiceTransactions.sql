CREATE TABLE [dbo].[ReferralInvoiceTransactions] (
    [ReferralInvoiceTransactionID] BIGINT          IDENTITY (1, 1) NOT NULL,
    [ReferralInvoiceID]            BIGINT          NULL,
    [ScheduleID]                   BIGINT          NULL,
    [EmployeeID]                   BIGINT          NULL,
    [EmployeeVisitID]              BIGINT          NULL,
    [EmployeeVisitNoteID]          BIGINT          NULL,
    [Rate]                         DECIMAL (18, 2) NULL,
    [PerUnitQuantity]              DECIMAL (18, 2) NULL,
    [Amount]                       DECIMAL (18, 2) NULL,
    [ServiceTime]                  BIGINT          NULL,
    [IsDeleted]                    BIT             NULL,
    CONSTRAINT [PK_ReferralInvoiceTransactions] PRIMARY KEY CLUSTERED ([ReferralInvoiceTransactionID] ASC)
);

