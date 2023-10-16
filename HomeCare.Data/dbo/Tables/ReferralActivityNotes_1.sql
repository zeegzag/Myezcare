CREATE TABLE [dbo].[ReferralActivityNotes] (
    [ReferralActivityNoteId]   BIGINT         IDENTITY (1, 1) NOT NULL,
    [ReferralActivityMasterId] INT            NULL,
    [Date]                     DATETIME       NULL,
    [Description]              NVARCHAR (MAX) NULL,
    [Initials]                 NVARCHAR (500) NULL,
    [CreatedBy]                INT            NULL
);

