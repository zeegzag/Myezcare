CREATE TABLE [dbo].[CareTypeTimeSlots] (
    [CareTypeTimeSlotID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [ReferralID]         BIGINT        NOT NULL,
    [CareTypeID]         BIGINT        NOT NULL,
    [Count]              INT           NULL,
    [Frequency]          INT           NULL,
    [StartDate]          DATE          NULL,
    [EndDate]            DATE          NULL,
    [IsDeleted]          BIT           CONSTRAINT [DF_CareTypeTimeSlots_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedDate]        DATETIME      NULL,
    [CreatedBy]          BIGINT        NULL,
    [UpdatedDate]        DATETIME      NULL,
    [UpdatedBy]          BIGINT        NULL,
    [SystemID]           VARCHAR (100) NULL,
    CONSTRAINT [PK_CareTypeTimeSlots] PRIMARY KEY CLUSTERED ([CareTypeTimeSlotID] ASC)
);

