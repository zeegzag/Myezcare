CREATE TABLE [dbo].[ReferralDocumentUploadStatuses] (
    [ReferralDocumentUploadStatusID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [ReferralID]                     BIGINT        NULL,
    [AHCCCSID]                       VARCHAR (100) NOT NULL,
    [UploadStatus]                   BIT           NOT NULL,
    CONSTRAINT [PK_ReferralDocumentUploadStatuses] PRIMARY KEY CLUSTERED ([ReferralDocumentUploadStatusID] ASC),
    CONSTRAINT [FK_ReferralDocumentUploadStatuses_Referrals] FOREIGN KEY ([ReferralID]) REFERENCES [dbo].[Referrals] ([ReferralID])
);

