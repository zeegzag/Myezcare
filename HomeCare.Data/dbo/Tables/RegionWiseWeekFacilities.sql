CREATE TABLE [dbo].[RegionWiseWeekFacilities] (
    [RegionWiseWeekFacilityID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [WeekMasterID]             BIGINT        NOT NULL,
    [RegionID]                 BIGINT        NOT NULL,
    [Facilities]               VARCHAR (500) NULL,
    CONSTRAINT [PK_RegionWiseWeekFacilities] PRIMARY KEY CLUSTERED ([RegionWiseWeekFacilityID] ASC),
    CONSTRAINT [FK__RegionWis__Regio__53B8409C] FOREIGN KEY ([RegionID]) REFERENCES [dbo].[Regions] ([RegionID]),
    CONSTRAINT [FK__RegionWis__WeekM__52C41C63] FOREIGN KEY ([WeekMasterID]) REFERENCES [dbo].[WeekMasters] ([WeekMasterID])
);

