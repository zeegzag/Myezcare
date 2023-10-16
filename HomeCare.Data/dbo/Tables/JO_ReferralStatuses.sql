CREATE TABLE [dbo].[JO_ReferralStatuses] (
    [JO_ReferralStatusID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [ReferralStatusID]    BIGINT        NOT NULL,
    [Status]              VARCHAR (100) NOT NULL,
    [Action]              CHAR (1)      NOT NULL,
    [ActionDate]          DATETIME      NOT NULL,
    CONSTRAINT [PK_JO_ReferralStatuses] PRIMARY KEY CLUSTERED ([JO_ReferralStatusID] ASC)
);

