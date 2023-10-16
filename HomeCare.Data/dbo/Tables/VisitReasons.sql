CREATE TABLE [dbo].[VisitReasons] (
    [VisitReasonID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [Type]          VARCHAR (6)    NULL,
    [ReasonCode]    NVARCHAR (MAX) NULL,
    [ActionCode]    NVARCHAR (MAX) NULL,
    [ScheduleID]    BIGINT         NULL,
    [CompanyName]   VARCHAR (11)   NOT NULL,
    [IsDeleted]     BIGINT         NULL,
    [CreatedDate]   DATETIME       NULL,
    [CreatedBy]     BIGINT         NULL,
    [UpdatedDate]   DATETIME       NULL,
    [UpdatedBy]     BIGINT         NULL
);

