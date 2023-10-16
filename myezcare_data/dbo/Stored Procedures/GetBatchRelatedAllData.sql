-- EXEC GetBatchRelatedAllData @BatchID = '63'
CREATE Procedure [dbo].[GetBatchRelatedAllData]
@BatchID as bigint
AS
BEGIN

DECLARE @PayorID bigint;
SELECT @PayorID=PayorID from Batches where BatchID=@BatchID;
Select * from PayorEdi837Settings Where PayorID=@PayorID

DECLARE @PayorShortName varchar(100);
Select @PayorShortName=ShortName from Payors Where PayorID=@PayorID


IF EXISTS (SELECT 1 FROM Batches WHERE BatchID=@BatchID AND IsSent=0)
BEGIN

		Select  R.ReferralID,R.FirstName, R.LastName,Dob=CONVERT(VARCHAR(10), R.Dob, 112), R.Gender ,R.AHCCCSID,R.CISNumber, MedicalRecordNumber=R.AHCCCSID, R.PolicyNumber,
		--SubscriberID=R.AHCCCSID, 
		CASE WHEN @PayorShortName='UHC' THEN R.CISNumber ELSE R.AHCCCSID END AS SubscriberID,
		ClaimSubmitterIdentifier=CONVERT(VARCHAR,BN.NoteID)+ 'ZRPB'+CONVERT(VARCHAR,BN.BatchID) +'BN'+CONVERT(VARCHAR,BN.BatchNoteID),
		PatientAccountNumber=CONVERT(VARCHAR,BN.NoteID)+ 'ZRPB'+CONVERT(VARCHAR,BN.BatchID) +'BN'+CONVERT(VARCHAR,BN.BatchNoteID),
		C.Address,C.City,C.State,C.ZipCode,ServiceDateSpan=CONVERT(VARCHAR(10), N.ServiceDate, 112)+'-'+CONVERT(VARCHAR(10), N.ServiceDate, 112),
		P.PosName, ModifierName = M.ModifierCode

		,B.*,BN.*,

		 N.NoteID,N.ServiceDate,N.ServiceCodeID,N.ServiceCode,N.ServiceName,N.Description,N.MaxUnit,N.DailyUnitLimit,N.UnitType,N.PerUnitQuantity,N.ServiceCodeType,N.ServiceCodeStartDate
		,N.ServiceCodeEndDate,N.CheckRespiteHours,N.ModifierID,N.PosID,N.Rate,N.POSStartDate,N.POSEndDate,N.ZarephathService,N.StartMile,N.EndMile,N.StartTime,N.EndTime,N.CalculatedUnit
		,N.NoteDetails,N.Assessment,N.ActionPlan,N.SpokeTo,N.Relation,N.OtherNoteType,N.MarkAsComplete,N.SignatureDate,N.CreatedBy,N.CreatedDate,N.UpdatedDate,N.UpdatedBy,N.SystemID
		,N.IssueID,N.IssueAssignID,N.POSDetail,N.IsBillable,N.HasGroupOption,N.PayorServiceCodeMappingID,N.PayorID,N.IsDeleted,N.NoOfStops,N.Source,N.RenderingProviderID,N.BillingProviderID
		,N.BillingProviderName,N.BillingProviderAddress,N.BillingProviderCity,N.BillingProviderState,N.BillingProviderZipcode,N.BillingProviderEIN,N.BillingProviderNPI,N.BillingProviderGSA
		,N.BillingProviderAHCCCSID,N.RenderingProviderName,N.RenderingProviderAddress,N.RenderingProviderCity,N.RenderingProviderState,N.RenderingProviderZipcode,N.RenderingProviderEIN
		,N.RenderingProviderNPI,N.RenderingProviderGSA,N.RenderingProviderAHCCCSID,N.PayorName,N.PayorShortName,N.PayorAddress,N.PayorIdentificationNumber,N.PayorCity,N.PayorState,N.PayorZipcode
		,N.CalculatedAmount,N.AttachmentURL,N.RandomGroupID,N.DriverID,N.VehicleNumber,N.VehicleType,N.PickUpAddress,N.DropOffAddress,N.RoundTrip,N.OneWay,N.MultiStops,N.EscortName
		,N.Relationship,N.DTRIsOnline,N.GroupIDForMileServices,N.NoteComments,N.NoteAssignee,N.NoteAssignedBy,N.NoteAssignedDate,N.MonthlySummaryIds,PSM.BillingUnitLimit

		,
		(SELECT  STUFF((SELECT TOP 12 ',' +  F.DxCodeType+':'+F.DXCodeWithoutDot --+'|'+ 
		 FROM NoteDXCodeMappings F where F.NoteID=N.NoteID Order BY Precedence ASC    
		 FOR XML PATH('')),1,1,'')) AS ContinuedDX

		From Batches B 
		INNER JOIN BatchNotes BN ON BN.BatchID=B.BatchID
		INNER JOIN Notes N ON N.NoteID=BN.NoteID
		--INNER JOIN ServiceCodes S ON S.ServiceCodeID=N.ServiceCodeID
		INNER JOIN PayorServiceCodeMapping PSM ON PSM.PayorServiceCodeMappingID=N.PayorServiceCodeMappingID                         
		INNER JOIN Referrals R on R.ReferralID=N.ReferralID
		LEFT JOIN ContactMappings CM on CM.ReferralID=R.ReferralID AND (CM.IsDCSLegalGuardian=1 OR CM.IsPrimaryPlacementLegalGuardian=1)
		LEFT JOIN Contacts C on C.ContactID=CM.ContactID
		LEFT JOIN Modifiers M on M.ModifierID=N.ModifierID
		INNER JOIN PlaceOfServices P on P.PosID=N.PosID
		WHERE B.BatchID=@BatchID AND  (BN.IsFirstTimeClaimInBatch IS NULL OR BN.IsFirstTimeClaimInBatch=1)
END
ELSE IF EXISTS (SELECT 1 FROM Batches WHERE BatchID=@BatchID AND IsSent=1)
BEGIN
		Select  R.ReferralID,R.FirstName, R.LastName,Dob=CONVERT(VARCHAR(10), R.Dob, 112), R.Gender ,R.AHCCCSID,R.CISNumber, MedicalRecordNumber=R.AHCCCSID, R.PolicyNumber,
		--SubscriberID=R.AHCCCSID, 
		CASE WHEN @PayorShortName='UHC' THEN R.CISNumber ELSE R.AHCCCSID END AS SubscriberID,
		ClaimSubmitterIdentifier=CONVERT(VARCHAR,BN.NoteID)+ 'ZRPB'+CONVERT(VARCHAR,BN.BatchID) +'BN'+CONVERT(VARCHAR,BN.BatchNoteID),
		PatientAccountNumber=CONVERT(VARCHAR,BN.NoteID)+ 'ZRPB'+CONVERT(VARCHAR,BN.BatchID) +'BN'+CONVERT(VARCHAR,BN.BatchNoteID),
		C.Address,C.City,C.State,C.ZipCode,ServiceDateSpan=CONVERT(VARCHAR(10), N.ServiceDate, 112)+'-'+CONVERT(VARCHAR(10), N.ServiceDate, 112),
		P.PosName,ModifierName = M.ModifierCode

		,B.*,BN.*,

		 N.NoteID,N.ServiceDate,N.ServiceCodeID,N.ServiceCode,N.ServiceName,N.Description,N.MaxUnit,N.DailyUnitLimit,N.UnitType,N.PerUnitQuantity,N.ServiceCodeType,N.ServiceCodeStartDate
		,N.ServiceCodeEndDate,N.CheckRespiteHours,N.ModifierID,N.PosID,N.Rate,N.POSStartDate,N.POSEndDate,N.ZarephathService,N.StartMile,N.EndMile,N.StartTime,N.EndTime,N.CalculatedUnit
		,N.NoteDetails,N.Assessment,N.ActionPlan,N.SpokeTo,N.Relation,N.OtherNoteType,N.MarkAsComplete,N.SignatureDate,N.CreatedBy,N.CreatedDate,N.UpdatedDate,N.UpdatedBy,N.SystemID
		,N.IssueID,N.IssueAssignID,N.POSDetail,N.IsBillable,N.HasGroupOption,N.PayorServiceCodeMappingID,N.PayorID,N.IsDeleted,N.NoOfStops,N.Source,N.RenderingProviderID,N.BillingProviderID
		,N.BillingProviderName,N.BillingProviderAddress,N.BillingProviderCity,N.BillingProviderState,N.BillingProviderZipcode,N.BillingProviderEIN,N.BillingProviderNPI,N.BillingProviderGSA
		,N.BillingProviderAHCCCSID,N.RenderingProviderName,N.RenderingProviderAddress,N.RenderingProviderCity,N.RenderingProviderState,N.RenderingProviderZipcode,N.RenderingProviderEIN
		,N.RenderingProviderNPI,N.RenderingProviderGSA,N.RenderingProviderAHCCCSID,N.PayorName,N.PayorShortName,N.PayorAddress,N.PayorIdentificationNumber,N.PayorCity,N.PayorState,N.PayorZipcode
		,N.CalculatedAmount,N.AttachmentURL,N.RandomGroupID,N.DriverID,N.VehicleNumber,N.VehicleType,N.PickUpAddress,N.DropOffAddress,N.RoundTrip,N.OneWay,N.MultiStops,N.EscortName
		,N.Relationship,N.DTRIsOnline,N.GroupIDForMileServices,N.NoteComments,N.NoteAssignee,N.NoteAssignedBy,N.NoteAssignedDate,N.MonthlySummaryIds,PSM.BillingUnitLimit

		,
		(SELECT  STUFF((SELECT TOP 12 ',' +  F.DxCodeType+':'+F.DXCodeWithoutDot --+'|'+ 
		 FROM NoteDXCodeMappings F where F.NoteID=N.NoteID Order BY Precedence ASC    
		 FOR XML PATH('')),1,1,'')) AS ContinuedDX

		From Batches B 
		INNER JOIN BatchNotes BN ON BN.BatchID=B.BatchID
		INNER JOIN BatchNoteDetails N ON N.NoteID=BN.NoteID AND N.BatchID=BN.BatchID
		--INNER JOIN ServiceCodes S ON S.ServiceCodeID=N.ServiceCodeID
		INNER JOIN PayorServiceCodeMapping PSM ON PSM.PayorServiceCodeMappingID=N.PayorServiceCodeMappingID                         
		INNER JOIN Referrals R on R.ReferralID=N.ReferralID
		LEFT JOIN ContactMappings CM on CM.ReferralID=R.ReferralID AND (CM.IsDCSLegalGuardian=1 OR CM.IsPrimaryPlacementLegalGuardian=1)
		LEFT JOIN Contacts C on C.ContactID=CM.ContactID
		LEFT JOIN Modifiers M on M.ModifierID=N.ModifierID
		INNER JOIN PlaceOfServices P on P.PosID=N.PosID
		WHERE B.BatchID=@BatchID AND  (BN.IsFirstTimeClaimInBatch IS NULL OR BN.IsFirstTimeClaimInBatch=1)



END

END




