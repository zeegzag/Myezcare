CREATE TABLE [dbo].[lu_DDMasterTypes] (
    [DDMasterTypeID] BIGINT        NOT NULL,
    [Name]           VARCHAR (100) NOT NULL,
    [SortOrder]      INT           NOT NULL,
    [IsDisplayValue] BIT           CONSTRAINT [DF__lu_DDMast__IsDis__129EAA56] DEFAULT ((0)) NOT NULL,
    [ParentID]       BIGINT        CONSTRAINT [DF__lu_DDMast__Paren__5654B625] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_lu_DDMasterTypes] PRIMARY KEY CLUSTERED ([DDMasterTypeID] ASC)
);

