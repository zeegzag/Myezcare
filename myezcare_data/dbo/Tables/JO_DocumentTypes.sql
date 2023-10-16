CREATE TABLE [dbo].[JO_DocumentTypes] (
    [JO_DocumentTypeID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [DocumentTypeID]    BIGINT        NOT NULL,
    [DocumentTypeName]  VARCHAR (100) NOT NULL,
    [KindOfDocument]    VARCHAR (50)  NULL,
    [Action]            CHAR (1)      NOT NULL,
    [ActionDate]        DATETIME      NOT NULL,
    CONSTRAINT [PK_JO_DocumentTypes] PRIMARY KEY CLUSTERED ([JO_DocumentTypeID] ASC)
);

