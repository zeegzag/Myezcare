CREATE TABLE [dbo].[JO_Departments] (
    [JO_DepartmentID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [DepartmentID]    BIGINT        NOT NULL,
    [DepartmentName]  VARCHAR (50)  NOT NULL,
    [Manager]         VARCHAR (50)  NULL,
    [Location]        VARCHAR (50)  NOT NULL,
    [Address]         VARCHAR (100) NULL,
    [City]            VARCHAR (50)  NULL,
    [StateCode]       VARCHAR (10)  NULL,
    [ZipCode]         VARCHAR (15)  NULL,
    [CreatedDate]     DATETIME      NOT NULL,
    [CreatedBy]       BIGINT        NOT NULL,
    [UpdatedDate]     DATETIME      NOT NULL,
    [UpdatedBy]       BIGINT        NOT NULL,
    [SystemID]        VARCHAR (100) NOT NULL,
    [IsDeleted]       BIT           NOT NULL,
    [Action]          CHAR (1)      NOT NULL,
    [ActionDate]      DATETIME      NOT NULL,
    CONSTRAINT [PK_JO_Departments] PRIMARY KEY CLUSTERED ([JO_DepartmentID] ASC)
);

