CREATE TABLE [dbo].[ReferralInternalMessages] (
    [ReferralInternalMessageID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [Note]                      VARCHAR (MAX) NOT NULL,
    [ReferralID]                BIGINT        NOT NULL,
    [Assignee]                  BIGINT        NOT NULL,
    [IsResolved]                BIT           NOT NULL,
    [CreatedDate]               DATETIME      NOT NULL,
    [CreatedBy]                 BIGINT        NOT NULL,
    [UpdatedDate]               DATETIME      NOT NULL,
    [UpdatedBy]                 BIGINT        NOT NULL,
    [SystemID]                  VARCHAR (100) NOT NULL,
    [IsDeleted]                 BIT           NOT NULL,
    [ResolveDate]               DATETIME      NULL,
    [MarkAsResolvedRead]        BIT           DEFAULT ((0)) NOT NULL,
    [ResolvedComment]           VARCHAR (MAX) NULL,
    CONSTRAINT [PK_ReferralNotes] PRIMARY KEY CLUSTERED ([ReferralInternalMessageID] ASC),
    CONSTRAINT [FK_ReferralInternalMessages_Referrals] FOREIGN KEY ([ReferralID]) REFERENCES [dbo].[Referrals] ([ReferralID]),
    CONSTRAINT [FK_ReferralNotes_Employees] FOREIGN KEY ([Assignee]) REFERENCES [dbo].[Employees] ([EmployeeID])
);


GO
CREATE NONCLUSTERED INDEX [missing_index_13_12_ReferralInternalMessages]
    ON [dbo].[ReferralInternalMessages]([IsResolved] ASC, [CreatedBy] ASC, [IsDeleted] ASC)
    INCLUDE([ResolveDate]);


GO
CREATE NONCLUSTERED INDEX [missing_index_11_10_ReferralInternalMessages]
    ON [dbo].[ReferralInternalMessages]([Assignee] ASC, [IsResolved] ASC, [IsDeleted] ASC)
    INCLUDE([CreatedDate]);

