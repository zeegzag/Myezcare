CREATE TABLE [dbo].[Preferences] (
    [PreferenceID]   BIGINT          IDENTITY (1, 1) NOT NULL,
    [PreferenceName] NVARCHAR (1000) NOT NULL,
    [KeyType]        VARCHAR (100)   NULL,
    [IsDeleted]      BIT             DEFAULT ((0)) NOT NULL,
    [CreatedDate]    DATETIME        NULL,
    [CreatedBy]      BIGINT          NULL,
    [UpdatedDate]    DATETIME        NULL,
    [UpdatedBy]      BIGINT          NULL,
    [SystemID]       VARCHAR (100)   NULL,
    CONSTRAINT [PK_Preferences] PRIMARY KEY CLUSTERED ([PreferenceID] ASC)
);

