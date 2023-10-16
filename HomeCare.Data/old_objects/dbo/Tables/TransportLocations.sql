CREATE TABLE [dbo].[TransportLocations] (
    [TransportLocationID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [Location]            VARCHAR (255) NOT NULL,
    [LocationCode]        VARCHAR (255) NOT NULL,
    [MapImage]            VARCHAR (MAX) NULL,
    [Address]             VARCHAR (100) NULL,
    [City]                VARCHAR (50)  NULL,
    [State]               VARCHAR (10)  NULL,
    [Zip]                 VARCHAR (15)  NULL,
    [Phone]               VARCHAR (15)  NULL,
    [MondayPickUp]        VARCHAR (50)  NULL,
    [TuesdayPickUp]       VARCHAR (50)  NULL,
    [WednesdayPickUp]     VARCHAR (50)  NULL,
    [ThursdayPickUp]      VARCHAR (50)  NULL,
    [FridayPickUp]        VARCHAR (50)  NULL,
    [SaturdayPickUp]      VARCHAR (50)  NULL,
    [SundayPickUp]        VARCHAR (50)  NULL,
    [MondayDropOff]       VARCHAR (50)  NULL,
    [TuesdayDropOff]      VARCHAR (50)  NULL,
    [WednesdayDropOff]    VARCHAR (50)  NULL,
    [ThursdayDropOff]     VARCHAR (50)  NULL,
    [FridayDropOff]       VARCHAR (50)  NULL,
    [SaturdayDropOff]     VARCHAR (50)  NULL,
    [SundayDropOff]       VARCHAR (50)  NULL,
    [CreatedBy]           BIGINT        NOT NULL,
    [CreatedDate]         DATETIME      NOT NULL,
    [UpdatedDate]         DATETIME      NOT NULL,
    [UpdatedBy]           BIGINT        NOT NULL,
    [SystemID]            VARCHAR (100) NULL,
    [IsDeleted]           BIT           NOT NULL,
    [RegionID]            BIGINT        NULL,
    CONSTRAINT [PK_TransportLocations] PRIMARY KEY CLUSTERED ([TransportLocationID] ASC),
    CONSTRAINT [FK__Transport__Regio__19CACAD2] FOREIGN KEY ([RegionID]) REFERENCES [dbo].[Regions] ([RegionID])
);


GO
CREATE TRIGGER [dbo].[tr_TransportLocations_Updated] ON [dbo].[TransportLocations]
FOR UPDATE AS 

INSERT INTO JO_TransportLocations( 
TransportLocationID,
Location		   ,
LocationCode	   ,
MapImage		   ,
Address			   ,
City			   ,
State			   ,
Zip				   ,
Phone			   ,
CreatedBy		   ,
CreatedDate		   ,
UpdatedDate		   ,
UpdatedBy		   ,
SystemID		   ,
IsDeleted		   ,
Action,ActionDate
)

SELECT  
TransportLocationID,
Location		   ,
LocationCode	   ,
MapImage		   ,
Address			   ,
City			   ,
State			   ,
Zip				   ,
Phone			   ,
CreatedBy		   ,
CreatedDate		   ,
UpdatedDate		   ,
UpdatedBy		   ,
SystemID		   ,
IsDeleted		   ,
'U',GETUTCDATE() FROM deleted

GO
DISABLE TRIGGER [dbo].[tr_TransportLocations_Updated]
    ON [dbo].[TransportLocations];


GO
CREATE TRIGGER [dbo].[tr_TransportLocations_Deleted] ON [dbo].[TransportLocations]
FOR DELETE AS 

INSERT INTO JO_TransportLocations( 
TransportLocationID,
Location		   ,
LocationCode	   ,
MapImage		   ,
Address			   ,
City			   ,
State			   ,
Zip				   ,
Phone			   ,
CreatedBy		   ,
CreatedDate		   ,
UpdatedDate		   ,
UpdatedBy		   ,
SystemID		   ,
IsDeleted		   ,
Action,ActionDate
)

SELECT  
TransportLocationID,
Location		   ,
LocationCode	   ,
MapImage		   ,
Address			   ,
City			   ,
State			   ,
Zip				   ,
Phone			   ,
CreatedBy		   ,
CreatedDate		   ,
UpdatedDate		   ,
UpdatedBy		   ,
SystemID		   ,
IsDeleted		   ,
'D',GETUTCDATE() FROM deleted

GO
DISABLE TRIGGER [dbo].[tr_TransportLocations_Deleted]
    ON [dbo].[TransportLocations];

