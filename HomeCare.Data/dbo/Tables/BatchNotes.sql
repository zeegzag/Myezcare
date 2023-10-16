CREATE TABLE [dbo].[BatchNotes] (
    [BatchNoteID]                                             BIGINT        IDENTITY (1, 1) NOT NULL,
    [BatchID]                                                 BIGINT        NOT NULL,
    [NoteID]                                                  BIGINT        NOT NULL,
    [CLM_BilledAmount]                                        VARCHAR (MAX) NULL,
    [BatchNoteStatusID]                                       BIGINT        NULL,
    [ParentBatchNoteID]                                       BIGINT        NULL,
    [N102_PayorName]                                          VARCHAR (MAX) NULL,
    [PER02_PayorBusinessContactName]                          VARCHAR (MAX) NULL,
    [PER04_PayorBusinessContact]                              VARCHAR (MAX) NULL,
    [PER02_PayorTechnicalContactName]                         VARCHAR (MAX) NULL,
    [PER04_PayorTechnicalContact]                             VARCHAR (MAX) NULL,
    [PER06_PayorTechnicalEmail]                               VARCHAR (MAX) NULL,
    [PER04_PayorTechnicalContactUrl]                          VARCHAR (MAX) NULL,
    [N102_PayeeName]                                          VARCHAR (MAX) NULL,
    [N103_PayeeIdentificationQualifier]                       VARCHAR (MAX) NULL,
    [N104_PayeeIdentification]                                VARCHAR (MAX) NULL,
    [LX01_ClaimSequenceNumber]                                VARCHAR (MAX) NULL,
    [CLP02_ClaimStatusCode]                                   BIGINT        NULL,
    [CLP01_ClaimSubmitterIdentifier]                          VARCHAR (MAX) NULL,
    [CLP03_TotalClaimChargeAmount]                            VARCHAR (MAX) NULL,
    [CLP04_TotalClaimPaymentAmount]                           VARCHAR (MAX) NULL,
    [CLP05_PatientResponsibilityAmount]                       VARCHAR (MAX) NULL,
    [CLP07_PayerClaimControlNumber]                           VARCHAR (MAX) NULL,
    [CLP08_PlaceOfService]                                    VARCHAR (MAX) NULL,
    [NM103_PatientLastName]                                   VARCHAR (MAX) NULL,
    [NM104_PatientFirstName]                                  VARCHAR (MAX) NULL,
    [NM109_PatientIdentifier]                                 VARCHAR (MAX) NULL,
    [NM103_ServiceProviderName]                               VARCHAR (MAX) NULL,
    [NM109_ServiceProviderNpi]                                VARCHAR (MAX) NULL,
    [SVC01_01_ServiceCodeQualifier]                           VARCHAR (MAX) NULL,
    [SVC01_02_ServiceCode]                                    VARCHAR (MAX) NULL,
    [SVC02_SubmittedLineItemServiceChargeAmount_BilledAmount] VARCHAR (MAX) NULL,
    [SVC03_LineItemProviderPaymentAmoun_PaidAmount]           VARCHAR (MAX) NULL,
    [SVC05_ServiceCodeUnit]                                   VARCHAR (MAX) NULL,
    [DTM02_ServiceStartDate]                                  VARCHAR (MAX) NULL,
    [DTM02_ServiceEndDate]                                    VARCHAR (MAX) NULL,
    [CAS01_ClaimAdjustmentGroupCode]                          VARCHAR (10)  NULL,
    [CAS02_ClaimAdjustmentReasonCode]                         VARCHAR (10)  NULL,
    [REF02_LineItem_ReferenceIdentification]                  VARCHAR (MAX) NULL,
    [AMT01_ServiceLineAllowedAmount_AllowedAmount]            VARCHAR (MAX) NULL,
    [CheckDate]                                               VARCHAR (MAX) NULL,
    [CheckAmount]                                             VARCHAR (MAX) NULL,
    [CheckNumber]                                             VARCHAR (MAX) NULL,
    [PolicyNumber]                                            VARCHAR (MAX) NULL,
    [AccountNumber]                                           VARCHAR (MAX) NULL,
    [ICN]                                                     VARCHAR (MAX) NULL,
    [Deductible]                                              VARCHAR (MAX) NULL,
    [Coins]                                                   VARCHAR (MAX) NULL,
    [Upload835FileID]                                         BIGINT        NULL,
    [ProcessedDate]                                           DATETIME      NULL,
    [ReceivedDate]                                            DATETIME      NULL,
    [LoadDate]                                                DATETIME      NULL,
    [DXCode]                                                  VARCHAR (200) NULL,
    [CAS03_ClaimAdjustmentAmount]                             VARCHAR (10)  NULL,
    [ClaimAdjustmentTypeID]                                   VARCHAR (50)  NULL,
    [IsFirstTimeClaimInBatch]                                 BIT           DEFAULT ((0)) NOT NULL,
    [Submitted_ClaimSubmitterIdentifier]                      VARCHAR (MAX) NULL,
    [Submitted_ClaimAdjustmentTypeID]                         VARCHAR (MAX) NULL,
    [Original_ClaimSubmitterIdentifier]                       VARCHAR (MAX) NULL,
    [Original_PayerClaimControlNumber]                        VARCHAR (MAX) NULL,
    [ClaimAdjustmentReason]                                   VARCHAR (MAX) NULL,
    [MarkAsLatest]                                            BIT           DEFAULT ((0)) NOT NULL,
    [CLM_UNIT]                                                VARCHAR (MAX) NULL,
    [S277]                                                    VARCHAR (MAX) NULL,
    [S277CA]                                                  VARCHAR (MAX) NULL,
    [IsUseInBilling]                                          BIT           DEFAULT ((1)) NOT NULL,
    [IsNewProcess]                                            BIT           DEFAULT ((0)) NOT NULL,
    [Original_Amount]                                         FLOAT (53)    NULL,
    [Original_Unit]                                           FLOAT (53)    NULL,
    [IsShowOnParentReconcile]                                 BIT           DEFAULT ((1)) NOT NULL,
    [MYEZCARE_ClaimStatus]                                    VARCHAR (MAX) NULL,

    [MPP_AdjustmentGroupCodeID]                               NVARCHAR (MAX) NULL,
    [MPP_AdjustmentGroupCodeName]                             NVARCHAR (MAX) NULL,
    [MPP_AdjustmentAmount]                                    NVARCHAR (MAX) NULL,
    [MPP_AdjustmentComment]                                   NVARCHAR (MAX) NULL,


    CONSTRAINT [PK_BatchNotes] PRIMARY KEY CLUSTERED ([BatchNoteID] ASC),
    CONSTRAINT [fk_BatchNote_ClaimAdjustmentTypes_ID] FOREIGN KEY ([ClaimAdjustmentTypeID]) REFERENCES [dbo].[ClaimAdjustmentTypes] ([ClaimAdjustmentTypeID]),
    CONSTRAINT [FK_BatchNotes_Batches] FOREIGN KEY ([BatchID]) REFERENCES [dbo].[Batches] ([BatchID]),
    CONSTRAINT [FK_BatchNotes_BatchNoteStatus] FOREIGN KEY ([BatchNoteStatusID]) REFERENCES [dbo].[BatchNoteStatus] ([BatchNoteStatusID]),
    CONSTRAINT [FK_BatchNotes_Notes] FOREIGN KEY ([NoteID]) REFERENCES [dbo].[Notes] ([NoteID])
);




GO
create TRIGGER [dbo].[tr_BatchNotes_Updated] ON [dbo].[BatchNotes]
FOR DELETE AS 

INSERT INTO JO_BatchNotes( 
BatchNoteID,
BatchID,
NoteID,
CLM_BilledAmount,
BatchNoteStatusID,
ParentBatchNoteID,
N102_PayorName,
PER02_PayorBusinessContactName,
PER04_PayorBusinessContact,
PER02_PayorTechnicalContactName,
PER04_PayorTechnicalContact,
PER06_PayorTechnicalEmail,
PER04_PayorTechnicalContactUrl,
N102_PayeeName,
N103_PayeeIdentificationQualifier,
N104_PayeeIdentification,
LX01_ClaimSequenceNumber,
CLP02_ClaimStatusCode,
CLP01_ClaimSubmitterIdentifier,
CLP03_TotalClaimChargeAmount,
CLP04_TotalClaimPaymentAmount,
CLP05_PatientResponsibilityAmount,
CLP07_PayerClaimControlNumber,
CLP08_PlaceOfService,
NM103_PatientLastName,
NM104_PatientFirstName,
NM109_PatientIdentifier,
NM103_ServiceProviderName,
NM109_ServiceProviderNpi,
SVC01_01_ServiceCodeQualifier,
SVC01_02_ServiceCode,
SVC02_SubmittedLineItemServiceChargeAmount_BilledAmount,
SVC03_LineItemProviderPaymentAmoun_PaidAmount,
SVC05_ServiceCodeUnit,
DTM02_ServiceStartDate,
DTM02_ServiceEndDate,
CAS01_ClaimAdjustmentGroupCode,
CAS02_ClaimAdjustmentReasonCode,
REF02_LineItem_ReferenceIdentification,
AMT01_ServiceLineAllowedAmount_AllowedAmount,
CheckDate,
CheckAmount,
CheckNumber,
PolicyNumber,
AccountNumber,
ICN,
Deductible,
Coins,
Upload835FileID,
ProcessedDate,
ReceivedDate,
LoadDate,
DXCode,
Action,
ActionDate
)
SELECT  
BatchNoteID,
BatchID,
NoteID,
CLM_BilledAmount,
BatchNoteStatusID,
ParentBatchNoteID,
N102_PayorName,
PER02_PayorBusinessContactName,
PER04_PayorBusinessContact,
PER02_PayorTechnicalContactName,
PER04_PayorTechnicalContact,
PER06_PayorTechnicalEmail,
PER04_PayorTechnicalContactUrl,
N102_PayeeName,
N103_PayeeIdentificationQualifier,
N104_PayeeIdentification,
LX01_ClaimSequenceNumber,
CLP02_ClaimStatusCode,
CLP01_ClaimSubmitterIdentifier,
CLP03_TotalClaimChargeAmount,
CLP04_TotalClaimPaymentAmount,
CLP05_PatientResponsibilityAmount,
CLP07_PayerClaimControlNumber,
CLP08_PlaceOfService,
NM103_PatientLastName,
NM104_PatientFirstName,
NM109_PatientIdentifier,
NM103_ServiceProviderName,
NM109_ServiceProviderNpi,
SVC01_01_ServiceCodeQualifier,
SVC01_02_ServiceCode,
SVC02_SubmittedLineItemServiceChargeAmount_BilledAmount,
SVC03_LineItemProviderPaymentAmoun_PaidAmount,
SVC05_ServiceCodeUnit,
DTM02_ServiceStartDate,
DTM02_ServiceEndDate,
CAS01_ClaimAdjustmentGroupCode,
CAS02_ClaimAdjustmentReasonCode,
REF02_LineItem_ReferenceIdentification,
AMT01_ServiceLineAllowedAmount_AllowedAmount,
CheckDate,
CheckAmount,
CheckNumber,
PolicyNumber,
AccountNumber,
ICN,
Deductible,
Coins,
Upload835FileID,
ProcessedDate,
ReceivedDate,
LoadDate,
DXCode,
'U',GETUTCDATE() FROM deleted
GO
CREATE NONCLUSTERED INDEX [missing_index_223768_223767_BatchNotes]
    ON [dbo].[BatchNotes]([NoteID] ASC)
    INCLUDE([BatchID], [SVC02_SubmittedLineItemServiceChargeAmount_BilledAmount], [SVC03_LineItemProviderPaymentAmoun_PaidAmount], [Original_Amount]);


GO
CREATE NONCLUSTERED INDEX [missing_index_210803_210802_BatchNotes]
    ON [dbo].[BatchNotes]([NoteID] ASC)
    INCLUDE([BatchID], [CLM_BilledAmount], [CLP02_ClaimStatusCode], [CLP01_ClaimSubmitterIdentifier], [CLP04_TotalClaimPaymentAmount], [CLP07_PayerClaimControlNumber], [CAS01_ClaimAdjustmentGroupCode], [CAS02_ClaimAdjustmentReasonCode], [AMT01_ServiceLineAllowedAmount_AllowedAmount], [Upload835FileID], [ProcessedDate], [ReceivedDate], [LoadDate], [ClaimAdjustmentTypeID], [Submitted_ClaimAdjustmentTypeID], [Original_PayerClaimControlNumber], [ClaimAdjustmentReason], [MarkAsLatest], [CLM_UNIT], [S277], [S277CA], [IsShowOnParentReconcile]);


GO
CREATE NONCLUSTERED INDEX [missing_index_210727_210726_BatchNotes]
    ON [dbo].[BatchNotes]([BatchID] ASC, [IsFirstTimeClaimInBatch] ASC);


GO
CREATE NONCLUSTERED INDEX [missing_index_210723_210722_BatchNotes]
    ON [dbo].[BatchNotes]([IsFirstTimeClaimInBatch] ASC)
    INCLUDE([BatchID], [NoteID]);


GO
CREATE NONCLUSTERED INDEX [missing_index_210721_210720_BatchNotes]
    ON [dbo].[BatchNotes]([BatchID] ASC, [IsFirstTimeClaimInBatch] ASC)
    INCLUDE([NoteID]);


GO
CREATE NONCLUSTERED INDEX [missing_index_210702_210701_BatchNotes]
    ON [dbo].[BatchNotes]([BatchID] ASC);


GO
CREATE NONCLUSTERED INDEX [missing_index_210693_210692_BatchNotes]
    ON [dbo].[BatchNotes]([NoteID] ASC);


GO
CREATE NONCLUSTERED INDEX [missing_index_210690_210689_BatchNotes]
    ON [dbo].[BatchNotes]([BatchID] ASC, [NoteID] ASC);


GO
CREATE NONCLUSTERED INDEX [missing_index_210686_210685_BatchNotes]
    ON [dbo].[BatchNotes]([BatchID] ASC)
    INCLUDE([NoteID], [CLM_BilledAmount], [SVC03_LineItemProviderPaymentAmoun_PaidAmount], [AMT01_ServiceLineAllowedAmount_AllowedAmount], [MarkAsLatest], [CLM_UNIT], [IsUseInBilling], [Original_Amount], [Original_Unit]);

