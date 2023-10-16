CREATE TABLE [dbo].[ScheduleDataEventProcessLogs]
(
    [ScheduleDataEventProcessLogID] BIGINT		   NOT NULL IDENTITY (1, 1),
	[OrganizationID]				BIGINT         NOT NULL,
	[TransactionID]					NVARCHAR (MAX) NOT NULL,
	[ScheduleID]					BIGINT         NOT NULL,
	[Aggregator]					NVARCHAR (MAX) NOT NULL,
	[FileName]						NVARCHAR (MAX) NOT NULL,
	[CreatedDate]                   DATETIME       NOT NULL,
	[UpdatedDate]                   DATETIME       NULL,
	[IsSuccess]						BIT			   NULL,
    [Messages]						NVARCHAR (MAX) NULL,
    [IsWaitingForResponse]			BIT			   NULL,
	[UploadedFileName]				NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_ScheduleDataEventProcessLogs] PRIMARY KEY CLUSTERED ([ScheduleDataEventProcessLogID] ASC)
)