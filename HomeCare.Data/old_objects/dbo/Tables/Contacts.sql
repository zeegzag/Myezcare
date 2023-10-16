CREATE TABLE [dbo].[Contacts] (
    [ContactID]     BIGINT        IDENTITY (1, 1) NOT NULL,
    [FirstName]     VARCHAR (50)  NULL,
    [LastName]      VARCHAR (50)  NULL,
    [Email]         VARCHAR (100) NULL,
    [Address]       VARCHAR (100) NULL,
    [City]          VARCHAR (50)  NULL,
    [State]         VARCHAR (10)  NULL,
    [ZipCode]       VARCHAR (15)  NULL,
    [Phone1]        VARCHAR (50)  NULL,
    [Phone2]        VARCHAR (50)  NULL,
    [OtherPhone]    VARCHAR (MAX) NULL,
    [LanguageID]    BIGINT        NOT NULL,
    [CreatedDate]   DATETIME      NOT NULL,
    [CreatedBy]     BIGINT        NOT NULL,
    [UpdatedDate]   DATETIME      NOT NULL,
    [UpdatedBy]     BIGINT        NOT NULL,
    [SystemID]      VARCHAR (100) NOT NULL,
    [IsDeleted]     BIT           DEFAULT ((0)) NOT NULL,
    [Latitude]      FLOAT (53)    NULL,
    [Longitude]     FLOAT (53)    NULL,
    [TempPatientID] BIGINT        NULL,
    [OldLatitude]   FLOAT (53)    NULL,
    [OldLongitude]  FLOAT (53)    NULL,
    [ApartmentNo]   NVARCHAR (50) NULL,
	[ReferenceMasterID] [bigint] NULL,
    CONSTRAINT [PK_Contacts] PRIMARY KEY CLUSTERED ([ContactID] ASC)
);


GO
CREATE TRIGGER [dbo].[tr_Contacts_Updated] ON [dbo].[Contacts]
FOR UPDATE AS 

INSERT INTO JO_Contacts( 
ContactID,
FirstName,
LastName,
Email,
Address,
City,
State,
ZipCode,
Phone1,
Phone2,
OtherPhone,
LanguageID,
CreatedDate,
CreatedBy,
UpdatedDate,
UpdatedBy,
SystemID,
Action,ActionDate
)

SELECT  
ContactID,
FirstName,
LastName,
Email,
Address,
City,
State,
ZipCode,
Phone1,
Phone2,
OtherPhone,
LanguageID,
CreatedDate,
CreatedBy,
UpdatedDate,
UpdatedBy,
SystemID,
'U',GETUTCDATE() FROM deleted

GO
DISABLE TRIGGER [dbo].[tr_Contacts_Updated]
    ON [dbo].[Contacts];


GO
CREATE TRIGGER [dbo].[tr_Contacts_Deleted] ON [dbo].[Contacts]
FOR DELETE AS 

INSERT INTO JO_Contacts( 
ContactID,
FirstName,
LastName,
Email,
Address,
City,
State,
ZipCode,
Phone1,
Phone2,
OtherPhone,
LanguageID,
CreatedDate,
CreatedBy,
UpdatedDate,
UpdatedBy,
SystemID,
Action,ActionDate
)

SELECT  
ContactID,
FirstName,
LastName,
Email,
Address,
City,
State,
ZipCode,
Phone1,
Phone2,
OtherPhone,
LanguageID,
CreatedDate,
CreatedBy,
UpdatedDate,
UpdatedBy,
SystemID,
'D',GETUTCDATE() FROM deleted

GO
DISABLE TRIGGER [dbo].[tr_Contacts_Deleted]
    ON [dbo].[Contacts];

