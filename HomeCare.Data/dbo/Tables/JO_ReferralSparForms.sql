﻿CREATE TABLE [dbo].[JO_ReferralSparForms] (
    [JO_ReferralSparFormID]             BIGINT         IDENTITY (1, 1) NOT NULL,
    [ReferralSparFormID]                BIGINT         NOT NULL,
    [ReviewDate]                        DATETIME       NULL,
    [AdmissionDate]                     DATETIME       NULL,
    [AssessmentDate]                    VARCHAR (100)  NULL,
    [AssessmentCompletedAndSignedByBHP] BIT            NULL,
    [IdentifyDTSDTOBehavior]            BIT            NULL,
    [DemographicDate]                   DATETIME       NULL,
    [IsROI]                             BIT            NULL,
    [IsSNCD]                            BIT            NULL,
    [DTSDTOUpdateText]                  VARCHAR (500)  NULL,
    [AdditionInformation]               VARCHAR (500)  NULL,
    [ServicePlanCompleted]              BIT            NULL,
    [ServicePlanSignedDatedByBHP]       BIT            NULL,
    [ServicePlanIdentified]             BIT            NULL,
    [ServicePlanHasFrequency]           BIT            NULL,
    [ServicePlanAdditionalInfo]         VARCHAR (500)  NULL,
    [IsSparFormCompleted]               BIT            NULL,
    [SparFormCompletedBy]               BIGINT         NULL,
    [SparFormCompletedDate]             DATETIME       NULL,
    [BHPReviewSignature]                NVARCHAR (MAX) NULL,
    [Date]                              DATETIME       NOT NULL,
    [ReferralID]                        BIGINT         NULL,
    [CreatedDate]                       DATETIME       NOT NULL,
    [CreatedBy]                         BIGINT         NOT NULL,
    [UpdatedDate]                       DATETIME       NOT NULL,
    [UpdatedBy]                         BIGINT         NOT NULL,
    [SystemID]                          VARCHAR (100)  NOT NULL,
    [Action]                            CHAR (10)      NOT NULL,
    [ActionDate]                        DATETIME       NOT NULL,
    [IsSparFormOffline]                 BIT            NULL,
    CONSTRAINT [PK_JO_ReferralSparForms] PRIMARY KEY CLUSTERED ([JO_ReferralSparFormID] ASC)
);

