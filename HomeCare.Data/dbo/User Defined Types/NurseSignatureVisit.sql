CREATE TYPE [dbo].[NurseSignatureVisit] AS TABLE (
	[EmployeeVisitID] BIGINT NOT NULL
	,[SignNote] NVARCHAR(MAX) NULL
);