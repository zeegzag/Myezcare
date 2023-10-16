CREATE TABLE [dbo].[JO_ReferralDXCodeMappings] (
    [JO_ReferralDXCodeMappingID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [ReferralDXCodeMappingID]    BIGINT        NOT NULL,
    [ReferralID]                 BIGINT        NOT NULL,
    [DXCodeID]                   VARCHAR (100) NOT NULL,
    [Precedence]                 INT           NULL,
    [StartDate]                  DATE          NULL,
    [EndDate]                    DATE          NULL,
    [CreatedDate]                DATETIME      NULL,
    [CreatedBy]                  BIGINT        NULL,
    [UpdatedDate]                DATETIME      NULL,
    [UpdatedBy]                  BIGINT        NULL,
    [SystemID]                   VARCHAR (100) NULL,
    [IsDeleted]                  BIT           NULL,
    [Action]                     CHAR (1)      NOT NULL,
    [ActionDate]                 DATETIME      NOT NULL,
    CONSTRAINT [PK_JO_ReferralDXCodeMappings] PRIMARY KEY CLUSTERED ([JO_ReferralDXCodeMappingID] ASC)
);

