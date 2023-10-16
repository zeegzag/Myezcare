CREATE TABLE [dbo].[JO_PayorServiceCodeMapping] (
    [JO_PayorServiceCodeMappingID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [PayorServiceCodeMappingID]    BIGINT         NOT NULL,
    [PayorID]                      BIGINT         NOT NULL,
    [ServiceCodeID]                BIGINT         NOT NULL,
    [ModifierID]                   BIGINT         NULL,
    [PosID]                        BIGINT         NOT NULL,
    [Rate]                         DECIMAL (9, 2) NOT NULL,
    [POSStartDate]                 DATE           NOT NULL,
    [POSEndDate]                   DATE           NOT NULL,
    [IsDeleted]                    BIT            NOT NULL,
    [Action]                       CHAR (1)       NOT NULL,
    [ActionDate]                   DATETIME       NOT NULL
);

