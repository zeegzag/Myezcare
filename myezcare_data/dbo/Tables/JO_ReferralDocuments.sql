CREATE TABLE [dbo].[JO_ReferralDocuments] (
    [JO_ReferralDocumentID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [ReferralDocumentID]    BIGINT        NOT NULL,
    [FileName]              VARCHAR (100) NOT NULL,
    [FilePath]              VARCHAR (200) NOT NULL,
    [KindOfDocument]        VARCHAR (50)  NOT NULL,
    [DocumentTypeID]        INT           NOT NULL,
    [ReferralID]            BIGINT        NULL,
    [CreatedDate]           DATETIME      NOT NULL,
    [CreatedBy]             BIGINT        NOT NULL,
    [UpdatedDate]           DATETIME      NOT NULL,
    [UpdatedBy]             BIGINT        NOT NULL,
    [SystemID]              VARCHAR (100) NOT NULL,
    [Action]                CHAR (1)      NOT NULL,
    [ActionDate]            DATETIME      NOT NULL,
    CONSTRAINT [PK_JO_ReferralDocuments] PRIMARY KEY CLUSTERED ([JO_ReferralDocumentID] ASC)
);

