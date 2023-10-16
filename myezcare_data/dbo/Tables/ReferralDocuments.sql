CREATE TABLE [dbo].[ReferralDocuments] (
    [ReferralDocumentID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [FileName]           VARCHAR (100) NOT NULL,
    [FilePath]           VARCHAR (200) NOT NULL,
    [KindOfDocument]     VARCHAR (50)  NOT NULL,
    [DocumentTypeID]     INT           NOT NULL,
    [CreatedDate]        DATETIME      NOT NULL,
    [CreatedBy]          BIGINT        NOT NULL,
    [UpdatedDate]        DATETIME      NOT NULL,
    [UpdatedBy]          BIGINT        NOT NULL,
    [SystemID]           VARCHAR (100) NOT NULL,
    [UserID]             BIGINT        NOT NULL,
    [UserType]           INT           CONSTRAINT [DF_ReferralDocuments_UserType] DEFAULT ((0)) NOT NULL,
    [ComplianceID]       BIGINT        NULL,
    [ExpirationDate]     DATE          NULL,
    [ReferralID]         BIGINT        NULL,
    CONSTRAINT [PK_Documents] PRIMARY KEY CLUSTERED ([ReferralDocumentID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [missing_index_20_19_ReferralDocuments]
    ON [dbo].[ReferralDocuments]([DocumentTypeID] ASC, [UserID] ASC);


GO

CREATE TRIGGER [dbo].[tr_ReferralDocuments_Updated] ON dbo.ReferralDocuments
FOR UPDATE AS 

INSERT INTO JO_ReferralDocuments( 
ReferralDocumentID,
FileName,
FilePath,
KindOfDocument,
DocumentTypeID,
ReferralID,
CreatedDate,
CreatedBy,
UpdatedDate,
UpdatedBy,
SystemID,
Action,ActionDate
)

SELECT  
ReferralDocumentID,
FileName,
FilePath,
KindOfDocument,
DocumentTypeID,
ReferralID,
CreatedDate,
CreatedBy,
UpdatedDate,
UpdatedBy,
SystemID,
'U',GETUTCDATE() FROM deleted

GO
DISABLE TRIGGER [dbo].[tr_ReferralDocuments_Updated]
    ON [dbo].[ReferralDocuments];


GO

CREATE TRIGGER [dbo].[tr_ReferralDocuments_Deleted] ON dbo.ReferralDocuments
FOR Delete AS 

INSERT INTO JO_ReferralDocuments( 
ReferralDocumentID,
FileName,
FilePath,
KindOfDocument,
DocumentTypeID,
ReferralID,
CreatedDate,
CreatedBy,
UpdatedDate,
UpdatedBy,
SystemID,
Action,ActionDate
)

SELECT  
ReferralDocumentID,
FileName,
FilePath,
KindOfDocument,
DocumentTypeID,
ReferralID,
CreatedDate,
CreatedBy,
UpdatedDate,
UpdatedBy,
SystemID,
'D',GETUTCDATE() FROM deleted

GO
DISABLE TRIGGER [dbo].[tr_ReferralDocuments_Deleted]
    ON [dbo].[ReferralDocuments];

