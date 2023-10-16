CREATE TABLE [dbo].[JO_ContactTypes] (
    [JO_ContactTypeID] BIGINT        NOT NULL,
    [ContactTypeID]    BIGINT        NOT NULL,
    [ContactTypeName]  VARCHAR (100) NOT NULL,
    [Action]           CHAR (1)      NOT NULL,
    [ActionDate]       DATETIME      NOT NULL,
    CONSTRAINT [PK_JO_ContactTypes] PRIMARY KEY CLUSTERED ([JO_ContactTypeID] ASC)
);

