CREATE TABLE [dbo].[JO_FacilityApprovedPayors] (
    [JO_FacilityApprovedPayorID] BIGINT    IDENTITY (1, 1) NOT NULL,
    [FacilityApprovedPayorID]    BIGINT    NOT NULL,
    [PayorID]                    BIGINT    NULL,
    [FacilityID]                 BIGINT    NULL,
    [Action]                     CHAR (10) NOT NULL,
    [ActionDate]                 DATETIME  NOT NULL,
    CONSTRAINT [PK_JO_FacilityApprovedPayors] PRIMARY KEY CLUSTERED ([JO_FacilityApprovedPayorID] ASC)
);

