CREATE TABLE [dbo].[ReferralPhysicians] (
    [ReferralPhysicianID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [ReferralID]          BIGINT        NOT NULL,
    [PhysicianID]         BIGINT        NOT NULL,
    [CreatedDate]         DATETIME      NOT NULL,
    [CreatedBy]           BIGINT        NOT NULL,
    [UpdatedDate]         DATETIME      NULL,
    [UpdatedBy]           BIGINT        NULL,
    [SystemID]            VARCHAR (100) NOT NULL,
    [IsDeleted]           BIT           NOT NULL,
    CONSTRAINT [PK_ReferralPhysicians] PRIMARY KEY CLUSTERED ([ReferralPhysicianID] ASC)
);

