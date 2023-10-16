CREATE TABLE [dbo].[ReportPermissionMapping] (
    [ReportPermissionMappingID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [RoleID]                    BIGINT        NOT NULL,
    [ReportID]                  BIGINT        NOT NULL,
    [CreatedDate]               DATETIME      CONSTRAINT [DF_ReportPermissionMapping_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]                 BIGINT        NOT NULL,
    [UpdatedDate]               DATETIME      NOT NULL,
    [UpdatedBy]                 BIGINT        NOT NULL,
    [SystemID]                  VARCHAR (100) NOT NULL,
    [IsDeleted]                 BIT           CONSTRAINT [DF_ReportPermissionMapping_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ReportPermissionMapping] PRIMARY KEY CLUSTERED ([ReportPermissionMappingID] ASC),
    CONSTRAINT [FK_ReportPermissionMapping_Roles] FOREIGN KEY ([RoleID]) REFERENCES [dbo].[Roles] ([RoleID])
);

