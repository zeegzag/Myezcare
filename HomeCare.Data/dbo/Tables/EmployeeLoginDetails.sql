CREATE TABLE [dbo].[EmployeeLoginDetails] (
    [EmployeeLoginDetailID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [EmployeeID]            BIGINT        NOT NULL,
    [LoginTime]             DATETIME      NOT NULL,
    [CreatedDate]           DATETIME      NOT NULL,
    [CreatedBy]             BIGINT        NOT NULL,
    [UpdatedDate]           DATETIME      NOT NULL,
    [UpdatedBy]             BIGINT        NOT NULL,
    [SystemID]              VARCHAR (100) NULL,
    [ActionPlatform]        VARCHAR (100) NULL,
    [ActionType]            VARCHAR (100) NULL,
    CONSTRAINT [PK_EmployeeLoginDetails] PRIMARY KEY CLUSTERED ([EmployeeLoginDetailID] ASC),
    CONSTRAINT [FK__EmployeeL__Emplo__6BE40491] FOREIGN KEY ([EmployeeID]) REFERENCES [dbo].[Employees] ([EmployeeID])
);


GO
CREATE TRIGGER [dbo].[tr_EmployeeLoginDetails_Updated] ON [dbo].[EmployeeLoginDetails]
FOR UPDATE AS 

INSERT INTO JO_EmployeeLoginDetails( 
EmployeeLoginDetailID,
EmployeeID,
LoginTime,
CreatedDate,
CreatedBy,
UpdatedDate,
UpdatedBy,
SystemID,
Action,ActionDate
)

SELECT  
EmployeeLoginDetailID,
EmployeeID,
LoginTime,
CreatedDate,
CreatedBy,
UpdatedDate,
UpdatedBy,
SystemID,
'U',GETUTCDATE() FROM deleted