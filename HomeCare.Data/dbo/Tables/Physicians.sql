CREATE TABLE [dbo].[Physicians] (
    [PhysicianID]       BIGINT         IDENTITY (1, 1) NOT NULL,
    [FirstName]         NVARCHAR (50)  NOT NULL,
    [MiddleName]        NVARCHAR (50)  NULL,
    [LastName]          NVARCHAR (50)  NOT NULL,
    [Email]             NVARCHAR (50)  NULL,
    [Phone]             NVARCHAR (20)  NULL,
    [Mobile]            NVARCHAR (20)  NULL,
    [Address]           NVARCHAR (100) NULL,
    [City]              NVARCHAR (50)  NULL,
    [StateCode]         NVARCHAR (10)  NULL,
    [ZipCode]           NVARCHAR (15)  NULL,
    [IsDeleted]         BIT            CONSTRAINT [DF_Physicians_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedDate]       DATETIME       NULL,
    [CreatedBy]         BIGINT         NULL,
    [UpdatedDate]       DATETIME       NULL,
    [UpdatedBy]         BIGINT         NULL,
    [SystemID]          VARCHAR (100)  NULL,
    [NPINumber]         NVARCHAR (20)  NULL,
    [PhysicianTypeID]   NVARCHAR (20)  NULL,
    [PhysicianTypeName] NVARCHAR (255) NULL,
    CONSTRAINT [PK_Physicians] PRIMARY KEY CLUSTERED ([PhysicianID] ASC)
);

