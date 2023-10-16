CREATE TABLE [dbo].[FacilityApprovedPayors] (
    [FacilityApprovedPayorID] BIGINT IDENTITY (1, 1) NOT NULL,
    [PayorID]                 BIGINT NULL,
    [FacilityID]              BIGINT NULL,
    CONSTRAINT [PK_FacilityApprovedPayors] PRIMARY KEY CLUSTERED ([FacilityApprovedPayorID] ASC),
    CONSTRAINT [FK_FacilityApprovedPayors_Payors] FOREIGN KEY ([PayorID]) REFERENCES [dbo].[Payors] ([PayorID])
);


GO
CREATE TRIGGER [dbo].[tr_FacilityApprovedPayors_Updated] ON [dbo].[FacilityApprovedPayors]
FOR UPDATE AS 

INSERT INTO JO_FacilityApprovedPayors( 
FacilityApprovedPayorID,
PayorID,
FacilityID,
Action,ActionDate
)

SELECT  
FacilityApprovedPayorID,
PayorID,
FacilityID,
'U',GETUTCDATE() FROM deleted

GO
DISABLE TRIGGER [dbo].[tr_FacilityApprovedPayors_Updated]
    ON [dbo].[FacilityApprovedPayors];


GO
CREATE TRIGGER [dbo].[tr_FacilityApprovedPayors_Deleted] ON [dbo].[FacilityApprovedPayors]
FOR DELETE AS 

INSERT INTO JO_FacilityApprovedPayors( 
FacilityApprovedPayorID,
PayorID,
FacilityID,
Action,ActionDate
)

SELECT  
FacilityApprovedPayorID,
PayorID,
FacilityID,
'D',GETUTCDATE() FROM deleted

GO
DISABLE TRIGGER [dbo].[tr_FacilityApprovedPayors_Deleted]
    ON [dbo].[FacilityApprovedPayors];

