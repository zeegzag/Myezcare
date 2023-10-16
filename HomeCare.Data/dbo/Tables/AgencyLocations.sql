CREATE TABLE [dbo].[AgencyLocations] (
    [AgencyLocationID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [LocationName]     VARCHAR (100) NOT NULL,
    [StreetAddress1]   VARCHAR (100) NULL,
    [StreetAddress2]   VARCHAR (100) NULL,
    [City]             VARCHAR (50)  NULL,
    [StateCode]        VARCHAR (10)  NULL,
    [ZipCode]          VARCHAR (15)  NULL,
    [AgencyID]         BIGINT        NOT NULL,
    [IsDeleted]        BIT           CONSTRAINT [DF_AgencyLocations_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_AgencyLocations] PRIMARY KEY CLUSTERED ([AgencyLocationID] ASC),
    CONSTRAINT [FK_AgencyLocations_Agencies] FOREIGN KEY ([AgencyID]) REFERENCES [dbo].[Agencies] ([AgencyID])
);


GO
CREATE TRIGGER [dbo].[tr_AgencyLocations_Updated] ON [dbo].[AgencyLocations]
FOR UPDATE AS 

INSERT INTO JO_AgencyLocations( 
AgencyLocationID,
LocationName,
StreetAddress1,
StreetAddress2,
City,
StateCode,
ZipCode,
AgencyID,
Action,ActionDate,IsDeleted
)

SELECT  
AgencyLocationID,
LocationName,
StreetAddress1,
StreetAddress2,
City,
StateCode,
ZipCode,
AgencyID,
'U',GETUTCDATE(),IsDeleted FROM deleted