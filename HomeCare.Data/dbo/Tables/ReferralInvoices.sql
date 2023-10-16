CREATE TABLE [dbo].[ReferralInvoices] (
    [ReferralInvoiceID] BIGINT           IDENTITY (1, 1) NOT NULL,
    [InvoiceDate]       DATETIME         NULL,
    [InvoiceType]       INT              NULL,
    [InvoiceStatus]     INT              NULL,
    [PayAmount]         DECIMAL (18, 2)  NULL,
    [PaidAmount]        DECIMAL (18, 2)  NULL,
    [ReferralID]        BIGINT           NULL,
    [TransactionID]     VARCHAR (MAX)    NULL,
    [RequestObject]     VARCHAR (MAX)    NULL,
    [ResponseObject]    VARCHAR (MAX)    NULL,
    [IsDeleted]         BIT              NULL,
    [TempInvoiceID]     UNIQUEIDENTIFIER NULL,
    [InvoiceTaxRate]    DECIMAL (5, 2)   CONSTRAINT [DF__ReferralI__Invoi__5E15D37E] DEFAULT ((0)) NOT NULL,
    [InvoiceDueDate]    DATETIME         NULL,
    [CareTypeID]        BIGINT           NULL,
    [ServiceStartDate]  DATE             NULL,
    [ServiceEndDate]    DATE             NULL,
    CONSTRAINT [PK_ReferralInvoices] PRIMARY KEY CLUSTERED ([ReferralInvoiceID] ASC)
);

