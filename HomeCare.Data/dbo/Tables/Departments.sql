CREATE TABLE [dbo].[Departments] (
    [DepartmentID]   BIGINT        IDENTITY (1, 1) NOT NULL,
    [DepartmentName] VARCHAR (50)  NOT NULL,
    [Manager]        VARCHAR (50)  NULL,
    [Location]       VARCHAR (50)  NOT NULL,
    [Address]        VARCHAR (100) NULL,
    [City]           VARCHAR (50)  NULL,
    [StateCode]      VARCHAR (10)  NULL,
    [ZipCode]        VARCHAR (15)  NULL,
    [CreatedDate]    DATETIME      NOT NULL,
    [CreatedBy]      BIGINT        NOT NULL,
    [UpdatedDate]    DATETIME      NOT NULL,
    [UpdatedBy]      BIGINT        NOT NULL,
    [SystemID]       VARCHAR (100) NOT NULL,
    [IsDeleted]      BIT           NOT NULL,
    CONSTRAINT [PK_Departments] PRIMARY KEY CLUSTERED ([DepartmentID] ASC)
);


GO
CREATE TRIGGER [dbo].[tr_Departments_Updated] ON [dbo].[Departments]
FOR UPDATE AS 

INSERT INTO JO_Departments( 
DepartmentID,
DepartmentName,
Manager,
Location,
Address,
City,
StateCode,
ZipCode,
CreatedDate,
CreatedBy,
UpdatedDate,
UpdatedBy,
SystemID,
IsDeleted,
Action,ActionDate
)

SELECT  
DepartmentID,
DepartmentName,
Manager,
Location,
Address,
City,
StateCode,
ZipCode,
CreatedDate,
CreatedBy,
UpdatedDate,
UpdatedBy,
SystemID,
IsDeleted,
'U',GETUTCDATE() FROM deleted