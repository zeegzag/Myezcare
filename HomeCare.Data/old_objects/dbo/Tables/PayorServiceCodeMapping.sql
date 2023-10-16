CREATE TABLE [dbo].[PayorServiceCodeMapping] (
    [PayorServiceCodeMappingID] BIGINT          IDENTITY (1, 1) NOT NULL,
    [PayorID]                   BIGINT          NOT NULL,
    [ServiceCodeID]             BIGINT          NOT NULL,
    [ModifierID]                BIGINT          NULL,
    [PosID]                     BIGINT          NULL,
    [Rate]                      DECIMAL (10, 2) NULL,
    [POSStartDate]              DATE            NOT NULL,
    [POSEndDate]                DATE            NOT NULL,
    [IsDeleted]                 BIT             CONSTRAINT [DF_PayorServiceCodeMapping_IsDeleted] DEFAULT ((0)) NOT NULL,
    [BillingUnitLimit]          INT             NULL,
    [RevenueCode]               BIGINT          NULL,
    [UnitType]                  INT             NULL,
    [PerUnitQuantity]           DECIMAL (18)    NULL,
    [RoundUpUnit]               INT             NULL,
    [MaxUnit]                   INT             NULL,
    [DailyUnitLimit]            INT             NULL,
    [UPCRate]                   BIGINT          NULL,
    [NegRate]                   BIGINT          NULL,
    [UM]                        BIGINT          NULL,
    [CareType]                  BIGINT          NULL,
    CONSTRAINT [PK_PayorServiceCodeMapping] PRIMARY KEY CLUSTERED ([PayorServiceCodeMappingID] ASC),
    CONSTRAINT [FK_PayorServiceCodeMapping_ServiceCodes] FOREIGN KEY ([ServiceCodeID]) REFERENCES [dbo].[ServiceCodes] ([ServiceCodeID])
);


GO
CREATE NONCLUSTERED INDEX [IX_PayorServiceCodeMapping]
    ON [dbo].[PayorServiceCodeMapping]([ServiceCodeID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PayorServiceCodeMapping_PayorID]
    ON [dbo].[PayorServiceCodeMapping]([PayorID] ASC);


GO
CREATE TRIGGER [dbo].[tr_PayorServiceCodeMapping_Updated] ON [dbo].[PayorServiceCodeMapping]
FOR DELETE AS 

INSERT INTO JO_PayorServiceCodeMapping( 
PayorServiceCodeMappingID,
PayorID,
ServiceCodeID,
ModifierID,
PosID,
Rate,
POSStartDate,
POSEndDate,
IsDeleted,
Action,
ActionDate
)

SELECT  
PayorServiceCodeMappingID,
PayorID,
ServiceCodeID,
ModifierID,
PosID,
Rate,
POSStartDate,
POSEndDate,
IsDeleted,
'U',GETUTCDATE() FROM deleted

GO
DISABLE TRIGGER [dbo].[tr_PayorServiceCodeMapping_Updated]
    ON [dbo].[PayorServiceCodeMapping];


GO
create TRIGGER [dbo].[tr_PayorServiceCodeMapping_Deleted] ON [dbo].[PayorServiceCodeMapping]
FOR DELETE AS 

INSERT INTO JO_PayorServiceCodeMapping( 
PayorServiceCodeMappingID,
PayorID,
ServiceCodeID,
ModifierID,
PosID,
Rate,
POSStartDate,
POSEndDate,
IsDeleted,
Action,
ActionDate
)

SELECT  
PayorServiceCodeMappingID,
PayorID,
ServiceCodeID,
ModifierID,
PosID,
Rate,
POSStartDate,
POSEndDate,
IsDeleted,
'D',GETUTCDATE() FROM deleted

GO
DISABLE TRIGGER [dbo].[tr_PayorServiceCodeMapping_Deleted]
    ON [dbo].[PayorServiceCodeMapping];

