CREATE PROCEDURE [dbo].[HC_GenerateBatchNotesAfterSent]     
@BatchIDs VARCHAR(MAX)      
AS      
BEGIN      
      
  DECLARE @id VARCHAR(50) -- database name       
  DECLARE db_cursor CURSOR FOR  SELECT val  FROM GetCSVTable(@BatchIDs)      
  OPEN db_cursor         
  FETCH NEXT FROM db_cursor INTO @id         
      
  WHILE @@FETCH_STATUS = 0         
  BEGIN         
          
 INSERT INTO BatchNoteDetails (BatchID,BatchNoteID,    
 NoteID,ReferralID,AHCCCSID,CISNumber,ContinuedDX,ServiceDate,ServiceCodeID,ServiceCode,ServiceName,Description,    
 MaxUnit,DailyUnitLimit,UnitType,PerUnitQuantity,ServiceCodeType,ServiceCodeStartDate,ServiceCodeEndDate,CheckRespiteHours,    
 ModifierID,PosID,Rate,POSStartDate,POSEndDate,ZarephathService,StartMile,EndMile,StartTime,EndTime,
 CalculatedUnit,NoteDetails,Assessment,ActionPlan,SpokeTo,Relation,OtherNoteType,MarkAsComplete,SignatureDate,CreatedBy,
 CreatedDate,UpdatedDate,UpdatedBy,SystemID,IssueID,IssueAssignID,POSDetail,IsBillable,HasGroupOption,PayorServiceCodeMappingID,
 PayorID,IsDeleted,NoOfStops,Source,RenderingProviderID,BillingProviderID,BillingProviderName,BillingProviderAddress,
 BillingProviderCity,BillingProviderState,BillingProviderZipcode,BillingProviderEIN,BillingProviderNPI,BillingProviderGSA,BillingProviderAHCCCSID,
 RenderingProviderName,RenderingProviderAddress,RenderingProviderCity,RenderingProviderState,RenderingProviderZipcode,RenderingProviderEIN,
 RenderingProviderNPI,RenderingProviderGSA,RenderingProviderAHCCCSID,PayorName,PayorShortName,PayorAddress,PayorIdentificationNumber,
 PayorCity,PayorState,PayorZipcode,CalculatedAmount,AttachmentURL,RandomGroupID,DriverID,VehicleNumber,VehicleType,PickUpAddress,
 DropOffAddress,RoundTrip,OneWay,MultiStops,EscortName,Relationship,DTRIsOnline,GroupIDForMileServices,NoteComments,NoteAssignee,
 NoteAssignedBy,NoteAssignedDate,MonthlySummaryIds,GroupID,ParentID,BilledUnit,
 BilledAmount,RenderingProviderFirstName,BillingProviderFirstName,CalculatedServiceTime)
 SELECT     
 BatchID,BatchNoteID,    
 NT.NoteID,NT.ReferralID,NT.AHCCCSID,NT.CISNumber,NT.ContinuedDX,NT.ServiceDate,NT.ServiceCodeID,NT.ServiceCode,NT.ServiceName,NT.Description,    
 NT.MaxUnit,NT.DailyUnitLimit,NT.UnitType,NT.PerUnitQuantity,NT.ServiceCodeType,NT.ServiceCodeStartDate,NT.ServiceCodeEndDate,NT.CheckRespiteHours,    
 NT.ModifierID,NT.PosID,NT.Rate,NT.POSStartDate,NT.POSEndDate,NT.ZarephathService,NT.StartMile,NT.EndMile,NT.StartTime,NT.EndTime,    
 NT.CalculatedUnit,NT.NoteDetails,NT.Assessment,NT.ActionPlan,NT.SpokeTo,NT.Relation,NT.OtherNoteType,NT.MarkAsComplete,NT.SignatureDate,NT.CreatedBy,    
 NT.CreatedDate,NT.UpdatedDate,NT.UpdatedBy,NT.SystemID,NT.IssueID,NT.IssueAssignID,NT.POSDetail,NT.IsBillable,NT.HasGroupOption,NT.PayorServiceCodeMappingID,    
 NT.PayorID,NT.IsDeleted,NT.NoOfStops,NT.Source,NT.RenderingProviderID,NT.BillingProviderID,NT.BillingProviderName,NT.BillingProviderAddress,    
 NT.BillingProviderCity,NT.BillingProviderState,NT.BillingProviderZipcode,NT.BillingProviderEIN,NT.BillingProviderNPI,NT.BillingProviderGSA,NT.BillingProviderAHCCCSID,    
 NT.RenderingProviderName,NT.RenderingProviderAddress,NT.RenderingProviderCity,NT.RenderingProviderState,NT.RenderingProviderZipcode,NT.RenderingProviderEIN,    
 NT.RenderingProviderNPI,NT.RenderingProviderGSA,NT.RenderingProviderAHCCCSID,NT.PayorName,NT.PayorShortName,NT.PayorAddress,NT.PayorIdentificationNumber,    
 NT.PayorCity,NT.PayorState,NT.PayorZipcode,NT.CalculatedAmount,NT.AttachmentURL,NT.RandomGroupID,NT.DriverID,NT.VehicleNumber,NT.VehicleType,NT.PickUpAddress,    
 NT.DropOffAddress,NT.RoundTrip,NT.OneWay,NT.MultiStops,NT.EscortName,NT.Relationship,NT.DTRIsOnline,NT.GroupIDForMileServices,NT.NoteComments,NT.NoteAssignee,    
 NT.NoteAssignedBy,NT.NoteAssignedDate,NT.MonthlySummaryIds,NT.GroupID,NT.ParentID,TempCalculatedUnit, TempCalculatedAmount,  NT.RenderingProviderFirstName,NT.BillingProviderFirstName,  
 NT.CalculatedServiceTime    
 FROM Notes NT      
    INNER JOIN (      
    SELECT  BN.BatchID,BN.BatchNoteID, N.NoteID,      
    TempCalculatedUnit= CASE WHEN (PSM.BillingUnitLimit IS NOT NULL AND SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))> PSM.BillingUnitLimit)       
        THEN PSM.BillingUnitLimit ELSE SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit)) END ,       
    TempCalculatedAmount= CASE WHEN (PSM.BillingUnitLimit IS NOT NULL AND SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))> PSM.BillingUnitLimit)       
        THEN CONVERT( DECIMAL(10,2), (SUM(ISNULL(CN.CalculatedAmount,N.CalculatedAmount)) / SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))) * PSM.BillingUnitLimit ) ELSE CONVERT(DECIMAL(10,2), SUM(ISNULL(CN.CalculatedAmount,N.CalculatedAmount))) END     
 
          
        FROM BatchNotes BN      
        INNER JOIN NOTES N ON N.NoteID = BN.NoteID      
        LEFT  JOIN ChildNotes CN ON CN.ParentNoteID = N.NoteID      
        INNER JOIN PayorServiceCodeMapping PSM ON PSM.PayorServiceCodeMappingID=N.PayorServiceCodeMappingID      
        LEFT JOIN BatchNoteDetails BND ON BND.BatchID=BN.BatchID AND BND.NoteID = BN.NoteID      
        WHERE (BN.IsFirstTimeClaimInBatch IS NULL OR BN.IsFirstTimeClaimInBatch=1) AND BND.BatchNoteDetailID IS NULL AND BN.BatchID=CONVERT(BIGINT,@id)      
        GROUP BY BN.BatchID,BN.BatchNoteID,N.NoteID, CN.ParentNoteID, PSM.BillingUnitLimit      
        --ORDER BY  BN.BatchID ASC,BN.BatchNoteID ASC      
      
    ) AS T ON T.NoteID=NT.NoteID      
      
    ORDER BY  BatchID ASC, BatchNoteID ASC      
      
    INSERT INTO BatchChildNotes      
    SELECT CN.ChildNoteID,CN.ParentNoteID,CN.NoteID,CN.CalculatedUnit,CN.CalculatedAmount      
    FROM BatchNotes BN      
    INNER JOIN ChildNotes CN ON CN.ParentNoteID = BN.NoteID      
    LEFT JOIN BatchChildNotes BCN ON BCN.ChildNoteID=CN.ChildNoteID      
    WHERE (BN.IsFirstTimeClaimInBatch IS NULL OR BN.IsFirstTimeClaimInBatch=1) AND BCN.ParentNoteID IS NULL AND BN.BatchID=CONVERT(BIGINT,@id)      
      
    FETCH NEXT FROM db_cursor INTO @id         
  END         
      
  CLOSE db_cursor         
  DEALLOCATE db_cursor      
      
END
