CREATE TABLE [dbo].[JO_Regions] (
    [JO_RegionID]       BIGINT        IDENTITY (1, 1) NOT NULL,
    [RegionID]          BIGINT        NOT NULL,
    [RegionName]        VARCHAR (50)  NOT NULL,
    [RegionDescription] VARCHAR (100) NULL,
    [RegionCode]        VARCHAR (2)   NULL,
    [Coordinator]       VARCHAR (50)  NULL,
    [CoordinatorPhone]  VARCHAR (15)  NULL,
    [CommunityLiason]   VARCHAR (50)  NULL,
    [PhCommunityLiason] VARCHAR (15)  NULL,
    [Fax]               VARCHAR (15)  NULL,
    [Ph24Hour]          VARCHAR (15)  NULL,
    [Email]             VARCHAR (50)  NULL,
    [Action]            CHAR (1)      NOT NULL,
    [ActionDate]        DATETIME      NOT NULL,
    CONSTRAINT [PK_JO_Regions] PRIMARY KEY CLUSTERED ([JO_RegionID] ASC)
);

