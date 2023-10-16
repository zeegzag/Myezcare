CREATE TABLE [dbo].[JO_Regions] (
    [JO_RegionID]       BIGINT        IDENTITY (1, 1) NOT NULL,
    [RegionID]          BIGINT        NULL,
    [RegionName]        VARCHAR (MAX) NULL,
    [RegionDescription] VARCHAR (MAX) NULL,
    [RegionCode]        VARCHAR (MAX) NULL,
    [Coordinator]       VARCHAR (50)  NULL,
    [CoordinatorPhone]  VARCHAR (15)  NULL,
    [CommunityLiason]   VARCHAR (50)  NULL,
    [PhCommunityLiason] VARCHAR (15)  NULL,
    [Fax]               VARCHAR (15)  NULL,
    [Ph24Hour]          VARCHAR (15)  NULL,
    [Email]             VARCHAR (50)  NULL,
    [Action]            CHAR (1)      NULL,
    [ActionDate]        DATETIME      NULL,
    CONSTRAINT [PK_JO_Regions] PRIMARY KEY CLUSTERED ([JO_RegionID] ASC)
);

