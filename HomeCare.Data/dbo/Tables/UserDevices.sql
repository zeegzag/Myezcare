CREATE TABLE [dbo].[UserDevices] (
    [UserDeviceId]    BIGINT         IDENTITY (1, 1) NOT NULL,
    [EmployeeID]      BIGINT         NOT NULL,
    [DeviceUDID]      NVARCHAR (MAX) NULL,
    [DeviceType]      NVARCHAR (50)  NOT NULL,
    [CreatedDate]     DATETIME       CONSTRAINT [DF_UserDevices_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [DeviceOSVersion] NVARCHAR (500) NULL,
    CONSTRAINT [PK_UserDevices] PRIMARY KEY CLUSTERED ([UserDeviceId] ASC)
);

