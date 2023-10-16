CREATE TABLE [dbo].[ServicePlanPermissions] (
    [ServicePlanPermissionID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [ServicePlanID]           BIGINT        NOT NULL,
    [PermissionID]            BIGINT        NOT NULL,
    [CreatedBy]               BIGINT        NOT NULL,
    [CreatedDate]             DATETIME      NOT NULL,
    [UpdatedDate]             DATETIME      NOT NULL,
    [UpdatedBy]               BIGINT        NOT NULL,
    [SystemID]                VARCHAR (100) NOT NULL,
    [IsDeleted]               BIT           NOT NULL,
    CONSTRAINT [PK_ServicePlanPermissions] PRIMARY KEY CLUSTERED ([ServicePlanPermissionID] ASC),
    CONSTRAINT [FK_ServicePlanPermissions_Permissions] FOREIGN KEY ([PermissionID]) REFERENCES [dbo].[Permissions] ([PermissionID]),
    CONSTRAINT [FK_ServicePlanPermissions_ServicePlans] FOREIGN KEY ([ServicePlanID]) REFERENCES [dbo].[ServicePlans] ([ServicePlanID])
);

