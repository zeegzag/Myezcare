CREATE TABLE [dbo].[ReferralStatuses] (
    [ReferralStatusID]  BIGINT        NOT NULL,
    [Status]            VARCHAR (100) NOT NULL,
    [UsedInRespiteCare] BIT           DEFAULT ((0)) NOT NULL,
    [UsedInHomeCare]    BIT           DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ReferralStatuses] PRIMARY KEY CLUSTERED ([ReferralStatusID] ASC)
);

