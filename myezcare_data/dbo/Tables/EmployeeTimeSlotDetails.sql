CREATE TABLE [dbo].[EmployeeTimeSlotDetails] (
    [EmployeeTimeSlotDetailID] BIGINT          IDENTITY (1, 1) NOT NULL,
    [EmployeeTimeSlotMasterID] BIGINT          NOT NULL,
    [Day]                      INT             NOT NULL,
    [StartTime]                TIME (7)        NOT NULL,
    [EndTime]                  TIME (7)        NOT NULL,
    [IsDeleted]                BIT             CONSTRAINT [DF_EmployeeTimeSlotDetails_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedDate]              DATETIME        NULL,
    [CreatedBy]                BIGINT          NULL,
    [UpdatedDate]              DATETIME        NULL,
    [UpdatedBy]                BIGINT          NULL,
    [SystemID]                 VARCHAR (100)   NULL,
    [Notes]                    NVARCHAR (1000) NULL,
    CONSTRAINT [PK_EmployeeTimeSlotDetails] PRIMARY KEY CLUSTERED ([EmployeeTimeSlotDetailID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_EmployeeTimeSlotDetails_EmployeeTimeSlotMasterID_IsDeleted_D735B]
    ON [dbo].[EmployeeTimeSlotDetails]([EmployeeTimeSlotMasterID] ASC, [IsDeleted] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_EmployeeTimeSlotDetails_StartTime_EndTime_IsDeleted_0DF04]
    ON [dbo].[EmployeeTimeSlotDetails]([StartTime] ASC, [EndTime] ASC, [IsDeleted] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_EmployeeTimeSlotDetails_Day_IsDeleted_StartTime_EndTime_E2319]
    ON [dbo].[EmployeeTimeSlotDetails]([Day] ASC, [IsDeleted] ASC, [StartTime] ASC, [EndTime] ASC)
    INCLUDE([EmployeeTimeSlotMasterID]);


GO
CREATE NONCLUSTERED INDEX [IX_EmployeeTimeSlotDetails_IsDeleted_4A48B]
    ON [dbo].[EmployeeTimeSlotDetails]([IsDeleted] ASC)
    INCLUDE([EmployeeTimeSlotMasterID]);


GO
CREATE NONCLUSTERED INDEX [IX_EmployeeTimeSlotDetails_IsDeleted_FE176]
    ON [dbo].[EmployeeTimeSlotDetails]([IsDeleted] ASC)
    INCLUDE([EmployeeTimeSlotMasterID], [Day], [StartTime], [EndTime]);


GO
CREATE NONCLUSTERED INDEX [IX_EmployeeTimeSlotDetails_EmployeeTimeSlotMasterID_Day_IsDeleted_2DFFB]
    ON [dbo].[EmployeeTimeSlotDetails]([EmployeeTimeSlotMasterID] ASC, [Day] ASC, [IsDeleted] ASC)
    INCLUDE([StartTime], [EndTime]);


GO
CREATE NONCLUSTERED INDEX [IX_EmployeeTimeSlotDetails_IsDeleted_EmployeeTimeSlotDetailID_52C93]
    ON [dbo].[EmployeeTimeSlotDetails]([IsDeleted] ASC, [EmployeeTimeSlotDetailID] ASC)
    INCLUDE([EmployeeTimeSlotMasterID], [Day], [StartTime], [EndTime]);


GO
CREATE NONCLUSTERED INDEX [IX_EmployeeTimeSlotDetails_EmployeeTimeSlotMasterID_IsDeleted_1EECC]
    ON [dbo].[EmployeeTimeSlotDetails]([EmployeeTimeSlotMasterID] ASC, [IsDeleted] ASC)
    INCLUDE([Day], [StartTime], [EndTime]);


GO
CREATE NONCLUSTERED INDEX [IX_EmployeeTimeSlotDetails_Day_IsDeleted_BE8F9]
    ON [dbo].[EmployeeTimeSlotDetails]([Day] ASC, [IsDeleted] ASC)
    INCLUDE([EmployeeTimeSlotMasterID], [StartTime], [EndTime]);


GO
CREATE NONCLUSTERED INDEX [IX_EmployeeTimeSlotDetails_Day_StartTime_IsDeleted_A3E27]
    ON [dbo].[EmployeeTimeSlotDetails]([Day] ASC, [StartTime] ASC, [IsDeleted] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_EmployeeTimeSlotDetails_IsDeleted_F2062]
    ON [dbo].[EmployeeTimeSlotDetails]([IsDeleted] ASC)
    INCLUDE([Day], [StartTime]);


GO
CREATE NONCLUSTERED INDEX [IX_EmployeeTimeSlotDetails_Day_IsDeleted_EmployeeTimeSlotDetailID_40A83]
    ON [dbo].[EmployeeTimeSlotDetails]([Day] ASC, [IsDeleted] ASC, [EmployeeTimeSlotDetailID] ASC)
    INCLUDE([EmployeeTimeSlotMasterID], [StartTime], [EndTime]);

