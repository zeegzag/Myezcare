CREATE TABLE [dbo].[JO_TransportLocations] (
    [JO_TransportLocationID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [TransportLocationID]    BIGINT         NOT NULL,
    [Location]               VARCHAR (255)  NOT NULL,
    [LocationCode]           VARCHAR (255)  NOT NULL,
    [MapImage]               VARCHAR (MAX)  NULL,
    [Address]                NVARCHAR (255) NULL,
    [City]                   NVARCHAR (50)  NOT NULL,
    [State]                  NVARCHAR (50)  NOT NULL,
    [Zip]                    NVARCHAR (50)  NOT NULL,
    [Phone]                  NVARCHAR (50)  NOT NULL,
    [CreatedBy]              BIGINT         NOT NULL,
    [CreatedDate]            DATETIME       NOT NULL,
    [UpdatedDate]            DATETIME       NOT NULL,
    [UpdatedBy]              BIGINT         NOT NULL,
    [SystemID]               VARCHAR (50)   NOT NULL,
    [IsDeleted]              BIT            NOT NULL,
    [Action]                 CHAR (1)       NOT NULL,
    [ActionDate]             DATETIME       NOT NULL,
    CONSTRAINT [PK_JO_TransportLocations] PRIMARY KEY CLUSTERED ([JO_TransportLocationID] ASC)
);

