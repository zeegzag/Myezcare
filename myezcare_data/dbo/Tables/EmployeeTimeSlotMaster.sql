CREATE TABLE [dbo].[EmployeeTimeSlotMaster] (
    [EmployeeTimeSlotMasterID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [EmployeeID]               BIGINT        NOT NULL,
    [StartDate]                DATE          NOT NULL,
    [EndDate]                  DATE          NULL,
    [IsDeleted]                BIT           CONSTRAINT [DF_EmployeeTimeSlotMaster_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedDate]              DATETIME      NULL,
    [CreatedBy]                BIGINT        NULL,
    [UpdatedDate]              DATETIME      NULL,
    [UpdatedBy]                BIGINT        NULL,
    [SystemID]                 VARCHAR (100) NULL,
    [IsEndDateAvailable]       BIT           DEFAULT ((0)) NULL,
    CONSTRAINT [PK_EmployeeTimeSlotMaster] PRIMARY KEY CLUSTERED ([EmployeeTimeSlotMasterID] ASC),
    CONSTRAINT [FK_EmployeeTimeSlotMaster_EmployeeTimeSlotMaster] FOREIGN KEY ([EmployeeID]) REFERENCES [dbo].[Employees] ([EmployeeID])
);

