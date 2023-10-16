CREATE TABLE [dbo].[BillingInformation] (
    [ProfileNumber]             BIGINT         IDENTITY (1, 1) NOT NULL,
    [OrganizationId]            BIGINT         NULL,
    [CardNumber]                VARCHAR (100)  NULL,
    [ExpirationDate]            DATETIME       NULL,
    [AccountNumber]             VARCHAR (100)  NULL,
    [RoutingNumber]             VARCHAR (100)  NULL,
    [NameOnAccount]             VARCHAR (100)  NULL,
    [BankName]                  VARCHAR (100)  NULL,
    [customerProfileId]         BIGINT         NULL,
    [customerPaymentProfileId]  BIGINT         NULL,
    [customerShippingAddressId] BIGINT         NULL,
    [CreatedDate]               DATETIME       NULL,
    [UpdatedDate]               DATETIME       NULL,
    [Statuscode]                NVARCHAR (MAX) NULL,
    [ErrorCode]                 NVARCHAR (MAX) NULL,
    [ErrorText]                 NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_BillingInformation] PRIMARY KEY CLUSTERED ([ProfileNumber] ASC)
);

