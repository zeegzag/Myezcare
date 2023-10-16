CREATE TABLE [dbo].[DMAS97AB] (
    [Dmas97ID]    BIGINT         IDENTITY (1, 1) NOT NULL,
    [JsonData]    NVARCHAR (MAX) NULL,
    [EmployeeID]  BIGINT         CONSTRAINT [DF_DMAS97AB_EmployeeID] DEFAULT ((0)) NULL,
    [ReferralID]  BIGINT         CONSTRAINT [DF_DMAS97AB_ReferralID] DEFAULT ((0)) NULL,
    [IsDeleted]   BIT            CONSTRAINT [DF_DMAS97AB_IsDeleted] DEFAULT ((0)) NULL,
    [CreatedDate] DATETIME       NULL,
    [CreatedBy]   BIGINT         NULL,
    [UpdatedDate] DATETIME       NULL,
    [UpdatedBy]   BIGINT         NULL,
    CONSTRAINT [PK_DMAS97AB] PRIMARY KEY CLUSTERED ([Dmas97ID] ASC)
);

