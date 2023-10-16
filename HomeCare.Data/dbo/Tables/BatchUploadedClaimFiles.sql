CREATE TABLE [dbo].[BatchUploadedClaimFiles] (
    [BatchUpClaimFileID]   BIGINT         IDENTITY (1, 1) NOT NULL,
    [ClaimMD_FileID]       NVARCHAR (100) NULL,
    [FileName]             NVARCHAR (100) NULL,
    [Claims]               INT            NULL,
    [Amount]               NVARCHAR (50)  NULL,
    [Date]                 DATETIME       NULL,
    [BatchUploadedClaimID] BIGINT         NULL,
    [ClaimMD_ID]           NVARCHAR (100) NULL,
    CONSTRAINT [PK_BatchUploadedClaimFiles] PRIMARY KEY CLUSTERED ([BatchUpClaimFileID] ASC)
);

