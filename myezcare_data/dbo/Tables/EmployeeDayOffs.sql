CREATE TABLE [dbo].[EmployeeDayOffs] (
    [EmployeeDayOffID] BIGINT          IDENTITY (1, 1) NOT NULL,
    [EmployeeID]       BIGINT          NOT NULL,
    [StartTime]        DATETIME        NOT NULL,
    [EndTime]          DATETIME        NOT NULL,
    [DayOffStatus]     VARCHAR (50)    CONSTRAINT [DF_EmployeeDayOffs_IsApproved] DEFAULT ((0)) NULL,
    [ActionTakenBy]    BIGINT          NULL,
    [ActionTakenDate]  DATETIME        NULL,
    [EmployeeComment]  NVARCHAR (1000) NULL,
    [ApproverComment]  NVARCHAR (1000) NULL,
    [IsDeleted]        BIT             CONSTRAINT [DF_EmployeeDayOffs_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedDate]      DATETIME        NULL,
    [CreatedBy]        BIGINT          NULL,
    [UpdatedDate]      DATETIME        NULL,
    [UpdatedBy]        BIGINT          NULL,
    [SystemID]         VARCHAR (100)   NULL,
    [DayOffTypeID]     BIGINT          NULL,
    CONSTRAINT [PK_EmployeeDayOffs] PRIMARY KEY CLUSTERED ([EmployeeDayOffID] ASC),
    CONSTRAINT [FK_EmployeeDayOffs_Employees] FOREIGN KEY ([EmployeeID]) REFERENCES [dbo].[Employees] ([EmployeeID]),
    CONSTRAINT [FK_EmployeeDayOffs_Employees1] FOREIGN KEY ([EmployeeID]) REFERENCES [dbo].[Employees] ([EmployeeID])
);

