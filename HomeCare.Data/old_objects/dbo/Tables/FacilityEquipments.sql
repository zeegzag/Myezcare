CREATE TABLE [dbo].[FacilityEquipments] (
    [FacilityEquipmentID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [FacilityID]          BIGINT        NOT NULL,
    [DDMasterID]          BIGINT        NOT NULL,
    [CreatedBy]           BIGINT        NOT NULL,
    [CreatedDate]         DATETIME      NOT NULL,
    [UpdatedDate]         DATETIME      NOT NULL,
    [UpdatedBy]           BIGINT        NOT NULL,
    [SystemID]            VARCHAR (100) NOT NULL,
    [IsDeleted]           BIT           NOT NULL,
    CONSTRAINT [PK_FacilityEquipments] PRIMARY KEY CLUSTERED ([FacilityEquipmentID] ASC),
    CONSTRAINT [FK_FacilityEquipments_DDMaster] FOREIGN KEY ([DDMasterID]) REFERENCES [dbo].[DDMaster] ([DDMasterID])
);

