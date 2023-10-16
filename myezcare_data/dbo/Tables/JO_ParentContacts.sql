CREATE TABLE [dbo].[JO_ParentContacts] (
    [JO_ParentContactID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [ParentContactID]    BIGINT        NOT NULL,
    [Name]               VARCHAR (50)  NOT NULL,
    [Email]              VARCHAR (50)  NULL,
    [Address]            VARCHAR (100) NOT NULL,
    [City]               VARCHAR (50)  NOT NULL,
    [State]              VARCHAR (10)  NOT NULL,
    [ZipCode]            VARCHAR (15)  NOT NULL,
    [Phone]              VARCHAR (15)  NOT NULL,
    [Phone2]             VARCHAR (15)  NULL,
    [OtherPhone]         VARCHAR (15)  NULL,
    [ContactType]        CHAR (2)      NOT NULL,
    [LanguagePreference] VARCHAR (50)  NOT NULL,
    [CreatedDate]        DATETIME      NOT NULL,
    [CreatedBy]          BIGINT        NOT NULL,
    [UpdatedDate]        DATETIME      NOT NULL,
    [UpdatedBy]          BIGINT        NOT NULL,
    [SystemID]           VARCHAR (100) NOT NULL,
    [Action]             CHAR (1)      NOT NULL,
    [ActionDate]         DATETIME      NOT NULL,
    CONSTRAINT [PK_JO_ParentContacts] PRIMARY KEY CLUSTERED ([JO_ParentContactID] ASC)
);

