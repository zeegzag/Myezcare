CREATE TABLE [dbo].[ReferralCareGivers] (
    [ReferralCareGiverID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [ReferralID]          BIGINT        NOT NULL,
    [AgencyID]            BIGINT        NOT NULL,
    [StartDate]           DATETIME      NULL,
    [EndDate]             DATETIME      NULL,
    [CreatedDate]         DATETIME      NULL,
    [CreatedBy]           BIGINT        NULL,
    [UpdatedDate]         DATETIME      NULL,
    [UpdatedBy]           BIGINT        NULL,
    [SystemID]            VARCHAR (100) NULL,
    [IsDeleted]           BIT           CONSTRAINT [DF_ReferralCareGivers_IsDeleted] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_ReferralCareGivers] PRIMARY KEY CLUSTERED ([ReferralCareGiverID] ASC)
);

