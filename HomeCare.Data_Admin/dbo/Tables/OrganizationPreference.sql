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
    [WeekStartDay]      VARCHAR (50)   NULL,
    CONSTRAINT [PK_OrganizationPreference] PRIMARY KEY CLUSTERED ([OrganizationID] ASC)
);




GO
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20211117-104414]
    ON [dbo].[OrganizationPreference]([OrganizationID] ASC, [Currency] ASC);


GO
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20211117-104341]
    ON [dbo].[OrganizationPreference]([OrganizationID] ASC, [DateFormat] ASC);

