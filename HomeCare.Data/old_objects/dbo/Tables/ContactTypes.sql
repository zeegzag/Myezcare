CREATE TABLE [dbo].[ContactTypes] (
    [ContactTypeID]   BIGINT        NOT NULL,
    [ContactTypeName] VARCHAR (100) NOT NULL,
    [OrderNumber]     INT           NULL,
    [IsDeleted]       BIT           DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ContactTypes] PRIMARY KEY CLUSTERED ([ContactTypeID] ASC)
);


GO
CREATE TRIGGER [dbo].[tr_ContactTypes_Updated] ON [dbo].[ContactTypes]
FOR UPDATE AS 

INSERT INTO JO_ContactTypes( 
ContactTypeID,
ContactTypeName,
Action,ActionDate
)

SELECT  
ContactTypeID,
ContactTypeName,
'U',GETUTCDATE() FROM deleted

GO
DISABLE TRIGGER [dbo].[tr_ContactTypes_Updated]
    ON [dbo].[ContactTypes];


GO
CREATE TRIGGER [dbo].[tr_ContactTypes_Deleted] ON [dbo].[ContactTypes]
FOR DELETE AS 

INSERT INTO JO_ContactTypes( 
ContactTypeID,
ContactTypeName,
Action,ActionDate
)

SELECT  
ContactTypeID,
ContactTypeName,
'D',GETUTCDATE() FROM deleted

GO
DISABLE TRIGGER [dbo].[tr_ContactTypes_Deleted]
    ON [dbo].[ContactTypes];

