CREATE TABLE [dbo].[JO_Contacts] (
    [JO_ContactID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [ContactID]    BIGINT        NOT NULL,
    [FirstName]    VARCHAR (50)  NULL,
    [LastName]     VARCHAR (50)  NULL,
    [Email]        VARCHAR (50)  NULL,
    [Address]      VARCHAR (100) NULL,
    [City]         VARCHAR (50)  NULL,
    [State]        VARCHAR (10)  NULL,
    [ZipCode]      VARCHAR (15)  NULL,
    [Phone1]       VARCHAR (15)  NULL,
    [Phone2]       VARCHAR (15)  NULL,
    [OtherPhone]   VARCHAR (15)  NULL,
    [LanguageID]   BIGINT        NULL,
    [CreatedDate]  DATETIME      NULL,
    [CreatedBy]    BIGINT        NULL,
    [UpdatedDate]  DATETIME      NULL,
    [UpdatedBy]    BIGINT        NULL,
    [SystemID]     VARCHAR (100) NULL,
    [Action]       CHAR (1)      NULL,
    [ActionDate]   DATETIME      NULL,
    CONSTRAINT [PK_JO_Contacts] PRIMARY KEY CLUSTERED ([JO_ContactID] ASC)
);

