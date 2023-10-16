﻿CREATE TABLE [dbo].[TempEmployees] (
    [EmployeeUniqueID]        VARCHAR (100)  NULL,
    [FirstName]               NVARCHAR (255) NULL,
    [MiddleName]              NVARCHAR (255) NULL,
    [LastName]                NVARCHAR (255) NULL,
    [Email]                   NVARCHAR (255) NULL,
    [UserName]                NVARCHAR (255) NULL,
    [PhoneWork]               NVARCHAR (255) NULL,
    [PhoneHome]               FLOAT (53)     NULL,
    [Role]                    NVARCHAR (255) NULL,
    [MobileNumber / IVR Code] NVARCHAR (255) NULL,
    [IVRPin]                  NVARCHAR (255) NULL,
    [Address 1]               NVARCHAR (255) NULL,
    [Address 2]               NVARCHAR (255) NULL,
    [City]                    NVARCHAR (255) NULL,
    [StateCode]               NVARCHAR (255) NULL,
    [ZipCode]                 FLOAT (53)     NULL,
    [Designation]             BIGINT         NULL,
    [RoleID]                  BIGINT         NULL,
    [CareTypeIds]             VARCHAR (MAX)  NULL,
    [SyncStatus]              BIT            DEFAULT ('0') NULL,
    [SyncedON]                DATETIME       DEFAULT (getdate()) NULL,
    [SyncError]               VARCHAR (MAX)  NULL,
    [AssociatedWith]          VARCHAR (MAX)  NULL,
    [Latitude]                FLOAT (53)     NULL,
    [Longitude]               FLOAT (53)     NULL,
    [HHANPI]                  VARCHAR (50)   NULL
);



