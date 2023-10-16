CREATE TABLE [dbo].[SaveDeviationNote] (
    [DeviationNoteID] INT            IDENTITY (1, 1) NOT NULL,
    [DeviationID]     INT            NULL,
    [EmployeeID]      INT            NULL,
    [DeviationNotes]  NVARCHAR (500) NULL,
    [DeviationType]   NVARCHAR (50)  NULL,
    [IsDeleted]       BIT            NULL,
    [CreatedBy]       BIGINT         NULL,
    [CreatedDate]     DATETIME       NULL,
    [UpdatedBy]       BIGINT         NULL,
    [UpdatedDate]     DATETIME       NULL,
    [EmployeeVisitID] BIGINT         NULL,
    [DeviationTime]   BIGINT         NULL,
    CONSTRAINT [PK_SaveDeviationNote] PRIMARY KEY CLUSTERED ([DeviationNoteID] ASC)
);

