CREATE TABLE [dbo].[InsuranceTypes] (
    [InsuranceTypeID]   BIGINT       NOT NULL,
    [InsuranceTypeName] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_InsuranceTypes] PRIMARY KEY CLUSTERED ([InsuranceTypeID] ASC)
);


GO
create TRIGGER [dbo].[tr_InsuranceTypes_Deleted] ON [dbo].[InsuranceTypes]
FOR DELETE AS 

INSERT INTO JO_InsuranceTypes( 
InsuranceTypeID,
InsuranceTypeName,
Action,
ActionDate)

SELECT  
InsuranceTypeID,
InsuranceTypeName,
'D',GETUTCDATE() FROM deleted

GO
DISABLE TRIGGER [dbo].[tr_InsuranceTypes_Deleted]
    ON [dbo].[InsuranceTypes];


GO
create TRIGGER [dbo].[tr_InsuranceTypes_Updated] ON [dbo].[InsuranceTypes]
FOR DELETE AS 

INSERT INTO JO_InsuranceTypes( 
InsuranceTypeID,
InsuranceTypeName,
Action,
ActionDate)

SELECT  
InsuranceTypeID,
InsuranceTypeName,
'U',GETUTCDATE() FROM deleted

GO
DISABLE TRIGGER [dbo].[tr_InsuranceTypes_Updated]
    ON [dbo].[InsuranceTypes];

