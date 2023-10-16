CREATE TABLE [dbo].[JO_ReferralInternalMessages] (
    [JO_ReferralInternalMessageID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [ReferralInternalMessageID]    BIGINT        NOT NULL,
    [Note]                         VARCHAR (MAX) NOT NULL,
    [ReferralID]                   BIGINT        NOT NULL,
    [Assignee]                     BIGINT        NOT NULL,
    [IsResolved]                   BIT           NOT NULL,
    [CreatedDate]                  DATETIME      NOT NULL,
    [CreatedBy]                    BIGINT        NOT NULL,
    [UpdatedDate]                  DATETIME      NOT NULL,
    [UpdatedBy]                    BIGINT        NOT NULL,
    [SystemID]                     VARCHAR (100) NOT NULL,
    [IsDeleted]                    BIT           NOT NULL,
    [Action]                       CHAR (1)      NOT NULL,
    [ActionDate]                   DATETIME      NOT NULL,
    CONSTRAINT [PK_JO_ReferralInternalMessages] PRIMARY KEY CLUSTERED ([JO_ReferralInternalMessageID] ASC)
);

