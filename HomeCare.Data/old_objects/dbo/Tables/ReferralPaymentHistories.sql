CREATE TABLE [dbo].[ReferralPaymentHistories] (
    [ReferralPaymentHistoryID] BIGINT          IDENTITY (1, 1) NOT NULL,
    [ReferralInvoiceID]        BIGINT          NOT NULL,
    [PaymentDate]              DATE            NOT NULL,
    [PaidAmount]               DECIMAL (18, 2) NOT NULL,
    [IsDeleted]                BIT             CONSTRAINT [DF_ReferralPaymentHistories_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedDate]              DATETIME        NOT NULL,
    [CreatedBy]                BIGINT          NOT NULL,
    CONSTRAINT [PK_ReferralPaymentHistories] PRIMARY KEY CLUSTERED ([ReferralPaymentHistoryID] ASC)
);

