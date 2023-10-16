CREATE TABLE [dbo].[DDMaster] (
    [DDMasterID]  BIGINT          IDENTITY (1, 1) NOT NULL,
    [ItemType]    INT             NULL,
    [Title]       NVARCHAR (1000) NOT NULL,
    [Value]       NVARCHAR (1000) NULL,
    [ParentID]    BIGINT          NOT NULL,
    [SortOrder]   INT             NULL,
    [Remark]      NVARCHAR (1000) NULL,
    [IsDeleted]   BIT             CONSTRAINT [DF_DDMaster_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedDate] DATETIME        NULL,
    [CreatedBy]   BIGINT          NULL,
    [UpdatedDate] DATETIME        NULL,
    [UpdatedBy]   BIGINT          NULL,
    [SystemID]    VARCHAR (100)   NULL,
    CONSTRAINT [PK_DDMaster] PRIMARY KEY CLUSTERED ([DDMasterID] ASC)
);




GO
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20211117-104543]
    ON [dbo].[DDMaster]([DDMasterID] ASC, [ItemType] ASC, [Title] ASC, [Value] ASC);

