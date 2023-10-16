CREATE TABLE [dbo].[BatchUploadedClaimErrors] (
    [BatchUpClaimErrorID]  BIGINT         IDENTITY (1, 1) NOT NULL,
    [BatchUploadedClaimID] BIGINT         NULL,
    [Field]                NVARCHAR (100) NULL,
    [MsgID]                NVARCHAR (100) NULL,
    [Message]              NVARCHAR (MAX) NULL,
    [Status]               NVARCHAR (50)  NULL,
    CONSTRAINT [PK_BatchUploadedClaimErrors] PRIMARY KEY CLUSTERED ([BatchUpClaimErrorID] ASC)
);

