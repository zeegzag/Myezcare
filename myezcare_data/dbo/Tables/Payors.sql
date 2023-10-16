CREATE TABLE [dbo].[Payors] (
    [PayorID]                   BIGINT         IDENTITY (1, 1) NOT NULL,
    [PayorName]                 VARCHAR (100)  NOT NULL,
    [ShortName]                 VARCHAR (50)   NOT NULL,
    [PayorSubmissionName]       VARCHAR (100)  NULL,
    [PayorIdentificationNumber] VARCHAR (50)   NULL,
    [Address]                   VARCHAR (MAX)  NOT NULL,
    [City]                      VARCHAR (50)   NOT NULL,
    [StateCode]                 VARCHAR (10)   NOT NULL,
    [ZipCode]                   VARCHAR (15)   NOT NULL,
    [PayorTypeID]               BIGINT         NULL,
    [IsDeleted]                 BIT            CONSTRAINT [DF_Payors_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedDate]               DATETIME       NOT NULL,
    [CreatedBy]                 BIGINT         NOT NULL,
    [UpdatedDate]               DATETIME       NOT NULL,
    [UpdatedBy]                 BIGINT         NOT NULL,
    [SystemID]                  VARCHAR (100)  NOT NULL,
    [BillingProviderID]         BIGINT         NULL,
    [RenderingProviderID]       BIGINT         NULL,
    [IsBillingActive]           BIT            CONSTRAINT [DF__Payors__IsBillin__2630A1B7] DEFAULT ((1)) NOT NULL,
    [AgencyNPID]                NVARCHAR (50)  NULL,
    [AgencyTaxNumber]           NVARCHAR (100) NULL,
    [NPIOption]                 BIGINT         NULL,
    [Taxenomy]                  NVARCHAR (100) NULL,
    [PayerGroup]                BIGINT         NULL,
    [BussinessLine]             BIGINT         NULL,
    [PayorBillingType]          NVARCHAR (100) NULL,
    [PayorInvoiceType]          INT            CONSTRAINT [DF__Payors__PayorInv__7251D655] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Payors] PRIMARY KEY CLUSTERED ([PayorID] ASC)
);


GO
create TRIGGER [dbo].[tr_Payors_Deleted] ON [dbo].[Payors]
FOR DELETE AS 

INSERT INTO JO_Payors( 
PayorID,
PayorName,
ShortName,
PayorSubmissionName,
PayorIdentificationNumber,
Address,
City,
StateCode,
ZipCode,
PayorTypeID,
IsDeleted,
CreatedDate,
CreatedBy,
UpdatedDate,
UpdatedBy,
SystemID,
Action,
ActionDate
)

SELECT  
PayorID,
PayorName,
ShortName,
PayorSubmissionName,
PayorIdentificationNumber,
Address,
City,
StateCode,
ZipCode,
PayorTypeID,
IsDeleted,
CreatedDate,
CreatedBy,
UpdatedDate,
UpdatedBy,
SystemID,
'D',GETUTCDATE() FROM deleted

GO
DISABLE TRIGGER [dbo].[tr_Payors_Deleted]
    ON [dbo].[Payors];


GO
create TRIGGER [dbo].[tr_Payors_Updated] ON [dbo].[Payors]
FOR DELETE AS 

INSERT INTO JO_Payors( 
PayorID,
PayorName,
ShortName,
PayorSubmissionName,
PayorIdentificationNumber,
Address,
City,
StateCode,
ZipCode,
PayorTypeID,
IsDeleted,
CreatedDate,
CreatedBy,
UpdatedDate,
UpdatedBy,
SystemID,
Action,
ActionDate
)

SELECT  
PayorID,
PayorName,
ShortName,
PayorSubmissionName,
PayorIdentificationNumber,
Address,
City,
StateCode,
ZipCode,
PayorTypeID,
IsDeleted,
CreatedDate,
CreatedBy,
UpdatedDate,
UpdatedBy,
SystemID,
'U',GETUTCDATE() FROM deleted

GO
DISABLE TRIGGER [dbo].[tr_Payors_Updated]
    ON [dbo].[Payors];

