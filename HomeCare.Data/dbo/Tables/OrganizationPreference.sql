CREATE TABLE [dbo].[OrganizationPreference] (
    [OrganizationID]    BIGINT         NOT NULL,
    [DateFormat]        VARCHAR (50)   NULL,
    [Currency]          VARCHAR (50)   NULL,
    [Language]          VARCHAR (50)   NULL,
    [NameDisplayFormat] VARCHAR (50)   NULL,
    [CssFilePath]       VARCHAR (MAX)  NULL,
    [Region]            VARCHAR (100)  NULL,
    [CreatedDate]       DATETIME       NULL,
    [CreatedBy]         BIGINT         NULL,
    [UpdatedDate]       DATETIME       NULL,
    [UpdatedBy]         BIGINT         NULL,
    [SystemID]          NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_OrganizationPreference] PRIMARY KEY CLUSTERED ([OrganizationID] ASC)
);

