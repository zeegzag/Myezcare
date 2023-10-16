CREATE TABLE [dbo].[ReleaseNotes] (
    [ReleaseNoteID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [Title]         NVARCHAR (200) NULL,
    [Description]   NVARCHAR (MAX) NULL,
    [StartDate]     DATETIME       NULL,
    [EndDate]       DATETIME       NULL,
    [CreatedBy]     BIGINT         NULL,
    [CreatedDate]   DATETIME       NULL,
    [UpdatedBy]     BIGINT         NULL,
    [UpdatedDate]   DATETIME       NULL,
    [SystemID]      NVARCHAR (100) NULL,
    [IsDeleted]     BIT            NULL,
    [IsActive]      BIT            NULL,
    CONSTRAINT [PK_ReleaseNotes] PRIMARY KEY CLUSTERED ([ReleaseNoteID] ASC)
);

