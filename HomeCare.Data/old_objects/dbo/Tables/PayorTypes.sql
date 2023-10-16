CREATE TABLE [dbo].[PayorTypes] (
    [PayorTypeID]   BIGINT       NOT NULL,
    [PayorTypeName] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_PayorTypes] PRIMARY KEY CLUSTERED ([PayorTypeID] ASC)
);


GO
CREATE TRIGGER [dbo].[tr_PayorTypes_Deleted] ON [dbo].[PayorTypes]
FOR DELETE AS 

INSERT INTO JO_PayorTypes( 
PayorTypeID,
PayorTypeName,
Action,ActionDate
)

SELECT  
PayorTypeID,
PayorTypeName,
'D',GETUTCDATE() FROM deleted

GO
DISABLE TRIGGER [dbo].[tr_PayorTypes_Deleted]
    ON [dbo].[PayorTypes];


GO
CREATE TRIGGER [dbo].[tr_PayorTypes_Updated] ON [dbo].[PayorTypes]
FOR UPDATE AS 

INSERT INTO JO_PayorTypes( 
PayorTypeID,
PayorTypeName,
Action,ActionDate
)

SELECT  
PayorTypeID,
PayorTypeName,
'U',GETUTCDATE() FROM deleted

GO
DISABLE TRIGGER [dbo].[tr_PayorTypes_Updated]
    ON [dbo].[PayorTypes];

