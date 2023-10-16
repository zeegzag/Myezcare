CREATE TABLE [dbo].[ReferralTimeSlotMaster] (
    [ReferralTimeSlotMasterID]       BIGINT        IDENTITY (1, 1) NOT NULL,
    [ReferralID]                     BIGINT        NOT NULL,
    [StartDate]                      DATE          NOT NULL,
    [EndDate]                        DATE          NULL,
    [IsDeleted]                      BIT           CONSTRAINT [DF_ReferralTimeSlotMaster_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedDate]                    DATETIME      NULL,
    [CreatedBy]                      BIGINT        NULL,
    [UpdatedDate]                    DATETIME      NULL,
    [UpdatedBy]                      BIGINT        NULL,
    [SystemID]                       VARCHAR (100) NULL,
    [IsEndDateAvailable]             BIT           DEFAULT ((0)) NULL,
    [ReferralBillingAuthorizationID] BIGINT        NULL,
    CONSTRAINT [PK_ReferralTimeSlotMaster] PRIMARY KEY CLUSTERED ([ReferralTimeSlotMasterID] ASC),
    CONSTRAINT [FK_ReferralTimeSlotMaster_Referrals] FOREIGN KEY ([ReferralID]) REFERENCES [dbo].[Referrals] ([ReferralID])
);

