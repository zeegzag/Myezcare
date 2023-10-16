CREATE TABLE [dbo].[Clients] (
    [ClientID]      BIGINT        IDENTITY (1, 1) NOT NULL,
    [FirstName]     VARCHAR (50)  NOT NULL,
    [MiddleName]    VARCHAR (50)  NULL,
    [LastName]      VARCHAR (50)  NOT NULL,
    [Dob]           DATE          NOT NULL,
    [Gender]        CHAR (1)      NOT NULL,
    [ClientNumber]  VARCHAR (15)  NULL,
    [AHCCCSID]      VARCHAR (10)  NOT NULL,
    [CISNumber]     VARCHAR (15)  NULL,
    [CreatedDate]   DATETIME      NOT NULL,
    [CreatedBy]     BIGINT        NOT NULL,
    [UpdatedDate]   DATETIME      NOT NULL,
    [UpdatedBy]     BIGINT        NOT NULL,
    [SystemID]      VARCHAR (100) NOT NULL,
    [IsDeleted]     BIT           CONSTRAINT [DF_Clients_IsDeleted] DEFAULT ((0)) NOT NULL,
    [TempPatientID] BIGINT        NULL,
    CONSTRAINT [PK_Clients] PRIMARY KEY CLUSTERED ([ClientID] ASC)
);


GO
CREATE TRIGGER [dbo].[tr_Clients_Updated] ON [dbo].[Clients]
FOR UPDATE AS 

INSERT INTO JO_Clients( 
ClientID	,
FirstName	,
MiddleName	,
LastName	,
Dob			,
Gender		,
ClientNumber,
AHCCCSID	,
CISNumber	,
CreatedDate	,
CreatedBy	,
UpdatedDate	,
UpdatedBy	,
SystemID	,
IsDeleted	,
Action,ActionDate
)

SELECT  
ClientID	,
FirstName	,
MiddleName	,
LastName	,
Dob			,
Gender		,
ClientNumber,
AHCCCSID	,
CISNumber	,
CreatedDate	,
CreatedBy	,
UpdatedDate	,
UpdatedBy	,
SystemID	,
IsDeleted	,
'U',GETUTCDATE() FROM deleted