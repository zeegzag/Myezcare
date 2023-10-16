﻿CREATE TABLE [dbo].[ReferralSparForms] (
    [ReferralSparFormID]                BIGINT         IDENTITY (1, 1) NOT NULL,
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
    [IsSparFormOffline]                 BIT            NULL,
    [CASIIScore]                        VARCHAR (100)  NULL,
    CONSTRAINT [PK_SparForms] PRIMARY KEY CLUSTERED ([ReferralSparFormID] ASC),
    CONSTRAINT [FK_ReferralSparForms_Referrals] FOREIGN KEY ([ReferralID]) REFERENCES [dbo].[Referrals] ([ReferralID])
);


GO
CREATE TRIGGER [dbo].[tr_ReferralSparForms_Deleted] ON dbo.ReferralSparForms
FOR DELETE AS 

INSERT INTO JO_ReferralSparForms( 
ReferralSparFormID,
ReviewDate,
AdmissionDate	,
AssessmentDate	,
AssessmentCompletedAndSignedByBHP,
IdentifyDTSDTOBehavior	,
DemographicDate	,
IsROI	,
IsSNCD	,
DTSDTOUpdateText	,
AdditionInformation	,
ServicePlanCompleted,
ServicePlanSignedDatedByBHP	,
ServicePlanIdentified	,
ServicePlanHasFrequency,
ServicePlanAdditionalInfo,
IsSparFormCompleted	,
SparFormCompletedBy	,
SparFormCompletedDate,
BHPReviewSignature	,
Date	,
ReferralID	,
CreatedDate	,
CreatedBy	,
UpdatedDate	,
UpdatedBy	,
SystemID	,
Action,ActionDate
)

SELECT  
ReferralSparFormID,
ReviewDate,
AdmissionDate	,
AssessmentDate	,
AssessmentCompletedAndSignedByBHP,
IdentifyDTSDTOBehavior	,
DemographicDate	,
IsROI	,
IsSNCD	,
DTSDTOUpdateText	,
AdditionInformation	,
ServicePlanCompleted,
ServicePlanSignedDatedByBHP	,
ServicePlanIdentified	,
ServicePlanHasFrequency,
ServicePlanAdditionalInfo,
IsSparFormCompleted	,
SparFormCompletedBy	,
SparFormCompletedDate,
BHPReviewSignature	,
Date	,
ReferralID	,
CreatedDate	,
CreatedBy	,
UpdatedDate	,
UpdatedBy	,
SystemID	,
'D',GETUTCDATE() FROM deleted

GO
CREATE TRIGGER [dbo].[tr_ReferralSparForms_Updated] ON dbo.ReferralSparForms
FOR UPDATE AS 

INSERT INTO JO_ReferralSparForms( 
ReferralSparFormID,
ReviewDate,
AdmissionDate	,
AssessmentDate	,
AssessmentCompletedAndSignedByBHP,
IdentifyDTSDTOBehavior	,
DemographicDate	,
IsROI	,
IsSNCD	,
DTSDTOUpdateText	,
AdditionInformation	,
ServicePlanCompleted,
ServicePlanSignedDatedByBHP	,
ServicePlanIdentified	,
ServicePlanHasFrequency,
ServicePlanAdditionalInfo,
IsSparFormCompleted	,
SparFormCompletedBy	,
SparFormCompletedDate,
BHPReviewSignature	,
Date	,
ReferralID	,
CreatedDate	,
CreatedBy	,
UpdatedDate	,
UpdatedBy	,
SystemID	,
Action,ActionDate
)

SELECT  
ReferralSparFormID,
ReviewDate,
AdmissionDate	,
AssessmentDate	,
AssessmentCompletedAndSignedByBHP,
IdentifyDTSDTOBehavior	,
DemographicDate	,
IsROI	,
IsSNCD	,
DTSDTOUpdateText	,
AdditionInformation	,
ServicePlanCompleted,
ServicePlanSignedDatedByBHP	,
ServicePlanIdentified	,
ServicePlanHasFrequency,
ServicePlanAdditionalInfo,
IsSparFormCompleted	,
SparFormCompletedBy	,
SparFormCompletedDate,
BHPReviewSignature	,
Date	,
ReferralID	,
CreatedDate	,
CreatedBy	,
UpdatedDate	,
UpdatedBy	,
SystemID	,
'U',GETUTCDATE() FROM deleted
