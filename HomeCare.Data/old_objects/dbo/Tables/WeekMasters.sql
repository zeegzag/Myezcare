CREATE TABLE [dbo].[WeekMasters] (
    [WeekMasterID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [Name]         VARCHAR (50)  NULL,
    [StartDate]    DATE          NOT NULL,
    [EndDate]      DATE          NOT NULL,
    [CreatedBy]    BIGINT        NULL,
    [CreatedDate]  DATETIME      NULL,
    [UpdatedBy]    BIGINT        NULL,
    [UpdatedDate]  DATETIME      NULL,
    [SystemID]     VARCHAR (100) NULL,
    CONSTRAINT [PK_WeekendMasters] PRIMARY KEY CLUSTERED ([WeekMasterID] ASC)
);

