﻿CREATE TABLE [dbo].[JO_BatchNotes] (
    [JO_BatchNoteID]                                          BIGINT        IDENTITY (1, 1) NOT NULL,
    [BatchNoteID]                                             BIGINT        NOT NULL,
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
    [Action]                                                  CHAR (1)      NULL,
    [ActionDate]                                              DATETIME      NULL
);

