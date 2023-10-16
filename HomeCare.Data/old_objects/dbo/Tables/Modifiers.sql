CREATE TABLE [dbo].[Modifiers] (
    [ModifierID]   BIGINT        IDENTITY (1, 1) NOT NULL,
    [ModifierCode] VARCHAR (10)  NULL,
    [ModifierName] VARCHAR (100) NOT NULL,
    [IsDeleted]    BIT           NOT NULL,
    [CreatedDate]  DATETIME      NULL,
    [CreatedBy]    BIGINT        NULL,
    [UpdatedDate]  DATETIME      NULL,
    [UpdatedBy]    BIGINT        NULL,
    [SystemID]     VARCHAR (MAX) NULL,
    CONSTRAINT [PK_Modifiers] PRIMARY KEY CLUSTERED ([ModifierID] ASC)
);

