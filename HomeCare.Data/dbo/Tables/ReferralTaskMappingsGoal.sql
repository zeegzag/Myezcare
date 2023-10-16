CREATE TABLE [dbo].[ReferralTaskMappingsGoal](
	[GoalID] [BIGINT] IDENTITY(1,1),
	[ReferralID] [bigint] NULL,
	[Goal] [nvarchar](max) NULL, 
    [IsActive] BIT NULL DEFAULT 0, 
    [IsDeleted] BIT NULL DEFAULT 0, 
    [CreatedDate] DATETIME NULL, 
    [CreatedBy] BIGINT NULL, 
    [UpdatedDate] DATETIME NULL, 
    [UpdatedBy] BIGINT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]