CREATE TABLE [dbo].[tmpddmasters] (
    [DDMasterID]  BIGINT          IDENTITY (1, 1) NOT NULL,
    [ItemType]    INT             NULL,
    [Title]       NVARCHAR (1000) NOT NULL,
    [Value]       NVARCHAR (1000) NULL,
    [ParentID]    BIGINT          NOT NULL,
    [SortOrder]   INT             NULL,
    [Remark]      NVARCHAR (1000) NULL,
    [IsDeleted]   BIT             NOT NULL,
    [CreatedDate] DATETIME        NULL,
    [CreatedBy]   BIGINT          NULL,
    [UpdatedDate] DATETIME        NULL,
    [UpdatedBy]   BIGINT          NULL,
    [SystemID]    VARCHAR (100)   NULL
);

