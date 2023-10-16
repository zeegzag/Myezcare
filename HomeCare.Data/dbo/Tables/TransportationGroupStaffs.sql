CREATE TABLE [dbo].[TransportationGroupStaffs] (
    [TransportationGroupStaffID] BIGINT IDENTITY (1, 1) NOT NULL,
    [TransportationGroupID]      BIGINT NULL,
    [StaffID]                    BIGINT NULL,
    CONSTRAINT [PK_TransportationGroupStaffs] PRIMARY KEY CLUSTERED ([TransportationGroupStaffID] ASC),
    CONSTRAINT [FK_TransportationGroupStaffs_Employees] FOREIGN KEY ([StaffID]) REFERENCES [dbo].[Employees] ([EmployeeID]),
    CONSTRAINT [FK_TransportationGroupStaffs_TransportationGroups] FOREIGN KEY ([TransportationGroupID]) REFERENCES [dbo].[TransportationGroups] ([TransportationGroupID])
);

