CREATE TABLE [dbo].[FacilityTransportLocationMappings] (
    [FacilityTransportationMappingID] BIGINT       IDENTITY (1, 1) NOT NULL,
    [FacilityID]                      BIGINT       NOT NULL,
    [TransportLocationID]             BIGINT       NOT NULL,
    [MondayPickUp]                    VARCHAR (50) NULL,
    [TuesdayPickUp]                   VARCHAR (50) NULL,
    [WednesdayPickUp]                 VARCHAR (50) NULL,
    [ThursdayPickUp]                  VARCHAR (50) NULL,
    [FridayPickUp]                    VARCHAR (50) NULL,
    [SaturdayPickUp]                  VARCHAR (50) NULL,
    [SundayPickUp]                    VARCHAR (50) NULL,
    [MondayDropOff]                   VARCHAR (50) NULL,
    [TuesdayDropOff]                  VARCHAR (50) NULL,
    [WednesdayDropOff]                VARCHAR (50) NULL,
    [ThursdayDropOff]                 VARCHAR (50) NULL,
    [FridayDropOff]                   VARCHAR (50) NULL,
    [SaturdayDropOff]                 VARCHAR (50) NULL,
    [SundayDropOff]                   VARCHAR (50) NULL,
    CONSTRAINT [PK_FacilityTransportLocationMappings] PRIMARY KEY CLUSTERED ([FacilityTransportationMappingID] ASC),
    CONSTRAINT [FK__FacilityT__Facil__5788D180] FOREIGN KEY ([FacilityID]) REFERENCES [dbo].[Facilities] ([FacilityID]),
    CONSTRAINT [FK__FacilityT__Trans__5694AD47] FOREIGN KEY ([TransportLocationID]) REFERENCES [dbo].[TransportLocations] ([TransportLocationID])
);

