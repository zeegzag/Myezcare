CREATE TABLE [dbo].[DocumentTypes] (
    [DocumentTypeID]   INT           NOT NULL,
    [DocumentTypeName] VARCHAR (100) NOT NULL,
    [KindOfDocument]   VARCHAR (50)  NULL,
    [IsDeleted]        BIT           DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_DocumentTypes] PRIMARY KEY CLUSTERED ([DocumentTypeID] ASC)
);


GO
CREATE TRIGGER [dbo].[tr_DocumentTypes_Updated] ON [dbo].[DocumentTypes]
FOR UPDATE AS 

INSERT INTO JO_DocumentTypes( 
DocumentTypeID,
DocumentTypeName,
KindOfDocument,
Action,ActionDate
)

SELECT  
DocumentTypeID,
DocumentTypeName,
KindOfDocument,
'U',GETUTCDATE() FROM deleted