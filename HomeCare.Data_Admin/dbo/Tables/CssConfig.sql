CREATE TABLE [dbo].[CssConfig] (
    [CssID]          BIGINT        IDENTITY (1, 1) NOT NULL,
    [CssDisplayName] VARCHAR (500) NOT NULL,
    [CssFilePath]    VARCHAR (MAX) NOT NULL,
    [CssRegion]      VARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_CssConfig] PRIMARY KEY CLUSTERED ([CssID] ASC)
);

