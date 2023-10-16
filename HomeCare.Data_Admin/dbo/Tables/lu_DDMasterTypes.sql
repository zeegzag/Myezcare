CREATE TABLE [dbo].[lu_DDMasterTypes] (
    [DDMasterTypeID] BIGINT        NOT NULL,
    [Name]           VARCHAR (100) NOT NULL,
    [SortOrder]      INT           NOT NULL,
    [IsDisplayValue] BIT           CONSTRAINT [DF__lu_DDMast__IsDis__0C50D423] DEFAULT ((0)) NOT NULL,
    [ParentID]       BIGINT        CONSTRAINT [DF__lu_DDMast__Paren__0D44F85C] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_lu_DDMasterTypes] PRIMARY KEY CLUSTERED ([DDMasterTypeID] ASC)
);

