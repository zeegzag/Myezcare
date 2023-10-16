CREATE TABLE [dbo].[OrganizationStats] (
    [OrganizationStatID]     BIGINT         IDENTITY (1, 1) NOT NULL,
    [OrganizationID]         BIGINT         NOT NULL,
    [OrganizationName]       NVARCHAR (100) NOT NULL,
    [Month]                  INT            NOT NULL,
    [Year]                   INT            NOT NULL,
    [ActivePatientCount]     BIGINT         NOT NULL,
    [DischargedPatientCount] BIGINT         NOT NULL,
    [EmployeeID]             BIGINT         NOT NULL,
    [ClockInTimeCount]       BIGINT         NOT NULL,
    [ClockOutTimeCount]      BIGINT         NOT NULL,
    [PCACompleteCount]       BIGINT         NOT NULL,
    [IVRClockInCount]        BIGINT         NOT NULL,
    [IVRClockOutCount]       BIGINT         NOT NULL,
    CONSTRAINT [PK_OrganizationStats] PRIMARY KEY CLUSTERED ([OrganizationStatID] ASC)
);

