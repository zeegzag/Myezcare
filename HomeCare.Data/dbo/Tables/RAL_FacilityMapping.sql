CREATE TABLE [dbo].[RAL_FacilityMapping] (
    [FacilityMappingID] BIGINT   IDENTITY (1, 1) NOT NULL,
    [FacilityID]        BIGINT   NOT NULL,
    [EmployeeID]        BIGINT   NOT NULL,
    [CreatedDate]       DATETIME NULL,
    [CreatedBy]         INT      NULL,
    [UpdatedDate]       DATETIME NULL,
    [UpdatedBy]         BIGINT   NULL,
    [IsDeleted]         BIT      CONSTRAINT [DF_FacilityMappingRAL_IsDeleted] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_FacilityMappingRAL] PRIMARY KEY CLUSTERED ([FacilityMappingID] ASC),
    CONSTRAINT [FK_RAL_FacilityMapping_Employees] FOREIGN KEY ([EmployeeID]) REFERENCES [dbo].[Employees] ([EmployeeID]),
    CONSTRAINT [FK_RAL_FacilityMapping_Facilities] FOREIGN KEY ([FacilityID]) REFERENCES [dbo].[Facilities] ([FacilityID])
);

