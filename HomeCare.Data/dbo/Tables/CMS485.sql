CREATE TABLE [dbo].[CMS485] (
    [Cms485ID]    BIGINT         IDENTITY (1, 1) NOT NULL,
    [JsonData]    NVARCHAR (MAX) NULL,
    [EmployeeID]  BIGINT         NULL,
    [ReferralID]  BIGINT         NULL,
    [IsDeleted]   BIT            NULL,
    [CreatedDate] DATETIME       NULL,
    [CreatedBy]   BIGINT         NULL,
    [UpdatedDate] DATETIME       NULL,
    [UpdatedBy]   BIGINT         NULL,
    CONSTRAINT [PK_CMS485] PRIMARY KEY CLUSTERED ([Cms485ID] ASC)
);

