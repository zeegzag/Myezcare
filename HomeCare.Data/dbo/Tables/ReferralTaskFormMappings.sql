CREATE TABLE [dbo].[ReferralTaskFormMappings] (
    [ReferralTaskFormMappingID] BIGINT IDENTITY (1, 1) NOT NULL,
    [ReferralTaskMappingID]     BIGINT NOT NULL,
    [TaskFormMappingID]         BIGINT NOT NULL,
    [ReferralDocumentID]        BIGINT NOT NULL,
    [EmployeeVisitID]           BIGINT NULL,
    CONSTRAINT [PK_ReferralTaskFormMappings] PRIMARY KEY CLUSTERED ([ReferralTaskFormMappingID] ASC)
);

