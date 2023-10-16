CREATE TABLE [dbo].[AgencyTypes] (
    [AgencyTypeID]   BIGINT       NOT NULL,
    [AgencyTypeName] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_AgencyTypes] PRIMARY KEY CLUSTERED ([AgencyTypeID] ASC)
);

