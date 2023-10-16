CREATE TABLE [dbo].[Regions] (
    [RegionID]          BIGINT        IDENTITY (1, 1) NOT NULL,
    [RegionName]        VARCHAR (50)  NOT NULL,
    [RegionDescription] VARCHAR (100) NULL,
    [RegionCode]        VARCHAR (10)  NULL,
    [Coordinator]       VARCHAR (50)  NULL,
    [CoordinatorPhone]  VARCHAR (15)  NULL,
    [CommunityLiason]   VARCHAR (50)  NULL,
    [PhCommunityLiason] VARCHAR (15)  NULL,
    [Fax]               VARCHAR (15)  NULL,
    [Ph24Hour]          VARCHAR (15)  NULL,
    [Email]             VARCHAR (50)  NULL,
    CONSTRAINT [PK_Regions] PRIMARY KEY CLUSTERED ([RegionID] ASC)
);


GO
CREATE TRIGGER [dbo].[tr_Regions_Updated] ON dbo.Regions
FOR UPDATE AS 

INSERT INTO JO_Regions( 
RegionID	,
RegionName	,
RegionDescription	,
RegionCode	,
Coordinator	,
CoordinatorPhone,
CommunityLiason	,
PhCommunityLiason,
Fax	,
Ph24Hour,
Email	,
Action,ActionDate
)

SELECT  
RegionID	,
RegionName	,
RegionDescription	,
RegionCode	,
Coordinator	,
CoordinatorPhone,
CommunityLiason	,
PhCommunityLiason,
Fax	,
Ph24Hour,
Email	,
'U',GETUTCDATE() FROM deleted

GO
DISABLE TRIGGER [dbo].[tr_Regions_Updated]
    ON [dbo].[Regions];


GO
CREATE TRIGGER [dbo].[tr_Regions_Deleted] ON dbo.Regions
FOR DELETE AS 

INSERT INTO JO_Regions( 
RegionID	,
RegionName	,
RegionDescription	,
RegionCode	,
Coordinator	,
CoordinatorPhone,
CommunityLiason	,
PhCommunityLiason,
Fax	,
Ph24Hour,
Email	,
Action,ActionDate
)

SELECT  
RegionID	,
RegionName	,
RegionDescription	,
RegionCode	,
Coordinator	,
CoordinatorPhone,
CommunityLiason	,
PhCommunityLiason,
Fax	,
Ph24Hour,
Email	,
'D',GETUTCDATE() FROM deleted

GO
DISABLE TRIGGER [dbo].[tr_Regions_Deleted]
    ON [dbo].[Regions];

