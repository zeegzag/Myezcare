CREATE TYPE [dbo].[UT_ServicePlanModule] AS TABLE (
    [ModuleID]             INT            NOT NULL,
    [ModuleName]           NVARCHAR (100) NULL,
    [MaximumAllowedNumber] INT            NULL,
    [ModuleDisplayName]    NVARCHAR (MAX) NULL,
    [ModuleHelpText]       NVARCHAR (MAX) NULL,
    [ModuleRequiredText]   NVARCHAR (100) NULL);

