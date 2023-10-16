CREATE TYPE [dbo].[GroupTask] AS TABLE (
    [VisitTaskID] BIGINT         NULL,
    [IsDetail]    BIT            NULL,
    [ServiceTime] INT            NULL,
    [Remarks]     NVARCHAR (MAX) NULL,
    [TaskOption]  NVARCHAR (MAX) NULL);


