CREATE TABLE [dbo].[ScheduleBatchServices] (
    [ScheduleBatchServiceID]     BIGINT         IDENTITY (1, 1) NOT NULL,
    [ScheduleBatchServiceName]   VARCHAR (100)  NULL,
    [ScheduleBatchServiceType]   INT            NULL,
    [ScheduleIDs]                NVARCHAR (MAX) NULL,
    [ScheduleBatchServiceStatus] VARCHAR (50)   NULL,
    [ServiceStatusDescription]   VARCHAR (200)  NULL,
    [CreatedDate]                DATETIME       NULL,
    [CreatedBy]                  BIGINT         NULL,
    [UpdatedDate]                DATETIME       NULL,
    [UpdatedBy]                  BIGINT         NULL,
    [SystemID]                   VARCHAR (100)  NULL,
    [IsDeleted]                  BIT            NULL,
    [FilePath]                   VARCHAR (100)  NULL,
    CONSTRAINT [PK_ScheduleBatchServices] PRIMARY KEY CLUSTERED ([ScheduleBatchServiceID] ASC)
);

