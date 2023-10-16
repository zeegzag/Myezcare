CREATE TABLE [dbo].[CaseManagers] (
    [CaseManagerID]    BIGINT        IDENTITY (1, 1) NOT NULL,
    [FirstName]        VARCHAR (50)  NOT NULL,
    [LastName]         VARCHAR (50)  NOT NULL,
    [Phone]            VARCHAR (15)  NULL,
    [Extension]        VARCHAR (10)  NULL,
    [Fax]              VARCHAR (15)  NULL,
    [Cell]             VARCHAR (15)  NULL,
    [Email]            VARCHAR (100) NULL,
    [Notes]            VARCHAR (500) NULL,
    [AgencyID]         BIGINT        NULL,
    [AgencyLocationID] BIGINT        NULL,
    [IsDeleted]        BIT           CONSTRAINT [DF_CaseManagers_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedDate]      DATETIME      NULL,
    [CreatedBy]        BIGINT        NULL,
    [UpdatedDate]      DATETIME      NULL,
    [UpdatedBy]        BIGINT        NULL,
    [SystemID]         VARCHAR (100) NULL,
    [Agency]           VARCHAR (200) NULL,
    CONSTRAINT [PK_CaseManagers] PRIMARY KEY CLUSTERED ([CaseManagerID] ASC)
);


GO
CREATE TRIGGER [dbo].[tr_CaseManagers_Updated] ON [dbo].[CaseManagers]
FOR UPDATE AS 

INSERT INTO JO_CaseManagers( 
CaseManagerID,
FirstName,
LastName,
Phone,
Extension,
Fax,
Cell,
Email,
Notes,
AgencyID,
AgencyLocationID,
IsDeleted,
CreatedDate,
CreatedBy,
UpdatedDate,
UpdatedBy,
SystemID,
Action,ActionDate
)

SELECT  
CaseManagerID,
FirstName,
LastName,
Phone,
Extension,
Fax,
Cell,
Email,
Notes,
AgencyID,
AgencyLocationID,
IsDeleted,
CreatedDate,
CreatedBy,
UpdatedDate,
UpdatedBy,
SystemID,
'U',GETUTCDATE() FROM deleted

GO
DISABLE TRIGGER [dbo].[tr_CaseManagers_Updated]
    ON [dbo].[CaseManagers];


GO
CREATE TRIGGER [dbo].[tr_CaseManagers_Deleted] ON [dbo].[CaseManagers]
FOR DELETE AS 

INSERT INTO JO_CaseManagers( 
CaseManagerID,
FirstName,
LastName,
Phone,
Extension,
Fax,
Cell,
Email,
Notes,
AgencyID,
AgencyLocationID,
IsDeleted,
CreatedDate,
CreatedBy,
UpdatedDate,
UpdatedBy,
SystemID,
Action,ActionDate
)

SELECT  
CaseManagerID,
FirstName,
LastName,
Phone,
Extension,
Fax,
Cell,
Email,
Notes,
AgencyID,
AgencyLocationID,
IsDeleted,
CreatedDate,
CreatedBy,
UpdatedDate,
UpdatedBy,
SystemID,
'D',GETUTCDATE() FROM deleted

GO
DISABLE TRIGGER [dbo].[tr_CaseManagers_Deleted]
    ON [dbo].[CaseManagers];

