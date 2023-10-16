CREATE TABLE [dbo].[AdminTempEmployee] (
    [AdminTempEmployeeID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [EmployeeID]          VARCHAR (100)  NULL,
    [UserName]            VARCHAR (50)   NOT NULL,
    [FirstName]           VARCHAR (50)   NOT NULL,
    [LastName]            VARCHAR (50)   NOT NULL,
    [Email]               VARCHAR (50)   NULL,
    [Role]                VARCHAR (100)  NOT NULL,
    [Address]             VARCHAR (100)  NULL,
    [City]                VARCHAR (50)   NULL,
    [State]               VARCHAR (10)   NULL,
    [ZipCode]             VARCHAR (15)   NULL,
    [ErrorMessage]        NVARCHAR (MAX) NULL,
    [CreatedDate]         DATETIME       NOT NULL,
    [CreatedBy]           BIGINT         NOT NULL
);

