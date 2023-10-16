CREATE TABLE [dbo].[JO_Contacts] (
    [JO_ContactID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [ContactID]    BIGINT        NOT NULL,
    [FirstName]    VARCHAR (50)  NOT NULL,
    [LastName]     VARCHAR (50)  NOT NULL,
    [Email]        VARCHAR (50)  NULL,
    [Address]      VARCHAR (100) NOT NULL,
    [City]         VARCHAR (50)  NOT NULL,
    [State]        VARCHAR (10)  NOT NULL,
    [ZipCode]      VARCHAR (15)  NOT NULL,
    [Phone1]       VARCHAR (15)  NOT NULL,
    [Phone2]       VARCHAR (15)  NULL,
    [OtherPhone]   VARCHAR (15)  NULL,
    [LanguageID]   BIGINT        NOT NULL,
    [CreatedDate]  DATETIME      NOT NULL,
    [CreatedBy]    BIGINT        NOT NULL,
    [UpdatedDate]  DATETIME      NOT NULL,
    [UpdatedBy]    BIGINT        NOT NULL,
    [SystemID]     VARCHAR (100) NOT NULL,
    [Action]       CHAR (1)      NOT NULL,
    [ActionDate]   DATETIME      NOT NULL,
    CONSTRAINT [PK_JO_Contacts] PRIMARY KEY CLUSTERED ([JO_ContactID] ASC)
);

