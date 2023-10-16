CREATE TABLE [dbo].[DMAS99] (
    [Dmas99ID]    BIGINT         IDENTITY (1, 1) NOT NULL,
    [JsonData]    NVARCHAR (MAX) NULL,
    [EmployeeID]  BIGINT         CONSTRAINT [DF_DMAS99_EmployeeID] DEFAULT ((0)) NULL,
    [ReferralID]  BIGINT         CONSTRAINT [DF_DMAS99_ReferralID] DEFAULT ((0)) NULL,
    [IsDeleted]   BIT            CONSTRAINT [DF_DMAS99_IsDeleted] DEFAULT ((0)) NULL,
    [CreatedDate] DATETIME       NULL,
    [CreatedBy]   BIGINT         NULL,
    [UpdatedDate] DATETIME       NULL,
    [UpdatedBy]   BIGINT         NULL,
    CONSTRAINT [PK_DMAS99] PRIMARY KEY CLUSTERED ([Dmas99ID] ASC)
);

