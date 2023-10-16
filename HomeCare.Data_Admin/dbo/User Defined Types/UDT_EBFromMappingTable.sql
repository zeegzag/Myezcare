CREATE TYPE [dbo].[UDT_EBFromMappingTable] AS TABLE (
    [EbriggsFormMppingID] BIGINT         NOT NULL,
    [EBriggsFormID]       NVARCHAR (MAX) NULL,
    [OriginalEBFormID]    NVARCHAR (MAX) NULL,
    [FormId]              NVARCHAR (MAX) NULL,
    [ReferralID]          BIGINT         NOT NULL,
    [PatientName]         NVARCHAR (MAX) NULL,
    [EmployeeID]          BIGINT         NOT NULL,
    [EmployeeName]        NVARCHAR (MAX) NULL,
    [CreatedBy]           NVARCHAR (MAX) NULL,
    [CreatedDate]         DATETIME       NULL,
    [UpdatedBy]           NVARCHAR (MAX) NULL,
    [UpdatedDate]         DATETIME       NULL);

