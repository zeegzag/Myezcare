-- EXEC GetOverviewFileList01 @BatchID = '91'
CREATE PROCEDURE [dbo].[GetOverviewFileList01]           
@BatchID varchar(500)=0          
AS                        
BEGIN           
	
	DECLARE @IsSent Bit
	SELECT @IsSent= IsSent FROM Batches WHERE BatchID=@BatchID

	
	IF(@IsSent=0)
	BEGIN

		Select  R.ReferralID,BN.NoteID,BN.BatchID,ClientName=R.LastName+', '+R.FirstName,R.FirstName,R.LastName,C.Address,C.City,C.State,C.ZipCode, R.Gender, ClientDob=CONVERT(VARCHAR(10), R.Dob, 101),R.AHCCCSID,R.CISNumber,            
		R.Population, R.Title,N.NoteID, N.NoteDetails, N.Assessment, N.ActionPlan, N.ServiceCode,N.PosID,N.POSDetail ,N.ServiceDate, N.BillingProviderName, N.BillingProviderNPI,N.BillingProviderEIN,N.BillingProviderAddress,N.BillingProviderCity,            
		N.BillingProviderState, N.BillingProviderZipcode ,N.RenderingProviderName, N.RenderingProviderNPI,N.RenderingProviderEIN,N.RenderingProviderAddress,N.RenderingProviderCity,            
		N.RenderingProviderState, N.RenderingProviderZipcode,    
		 CONVERT(VARCHAR(10),N.ServiceDate, 101)  as ServiceDate,    
		 CONVERT(VARCHAR(10), R.ClosureDate, 101)  as ClosureDate,    
		B.BatchID,BatchStartDate=B.StartDate, BatchEndDate=B.EndDate, CONVERT(VARCHAR(10), B.SentDate, 101) as SentDate,SentBy=SB.UserName,GatherDate=        
		CONVERT(VARCHAR(10),B.CreatedDate, 101),BN.CLM_UNIT CalculatedUnit,        
		GatheredBy=CB.UserName, BatchPayorName=BP.PayorName, BatchPayorShortName=BP.ShortName,BT.BatchTypeName,            
		(SELECT  STUFF((SELECT top 12 ',' + F.DXCodeWithoutDot    
		   FROM NoteDXCodeMappings F where F.NoteID=N.NoteID ORDER BY F.Precedence ASC  
		   FOR XML PATH('')),1,1,'')) AS ContinuedDX,

		   CONVERT(DECIMAL(10,2),BN.CLM_BilledAmount) as CalculatedAmount,ModifierName = M.ModifierCode
		FROM BatchNotes BN 
		INNER JOIN Batches B ON B.BatchID=BN.BatchID AND BN.IsUseInBilling=1 AND B.BatchID in (SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@BatchID)) 
		INNER JOIN Notes N ON BN.NoteID=N.NoteID AND ((BN.IsFirstTimeClaimInBatch IS NULL OR BN.IsFirstTimeClaimInBatch=1))           
		INNER JOIN BatchTypes BT on BT.BatchTypeID=B.BatchTypeID            
		INNER JOIN Payors BP on BP.PayorID=B.PayorID
		INNER JOIN Employees CB on CB.EmployeeID=B.CreatedBy            
		INNER JOIN Referrals R ON N.ReferralID=R.ReferralID  
		LEFT JOIN  ContactMappings CM ON CM.ReferralID=R.ReferralID AND (CM.IsPrimaryPlacementLegalGuardian=1 OR CM.IsDCSLegalGuardian=1)            
		LEFT JOIN  Contacts C ON C.ContactID=CM.ContactID 
		LEFT JOIN Employees SB on SB.EmployeeID=B.IsSentBy       
		LEFT JOIN Modifiers M on M.ModifierID=N.ModifierID    
		ORDER BY  R.LastName  ASC , R.FirstName ASC      
	END
	ELSE 
	BEGIN
		Select  R.ReferralID,BN.NoteID,BN.BatchID,ClientName=R.LastName+', '+R.FirstName,R.FirstName,R.LastName,C.Address,C.City,C.State,C.ZipCode, R.Gender, ClientDob=CONVERT(VARCHAR(10), R.Dob, 101),R.AHCCCSID,R.CISNumber,            
		R.Population, R.Title,N.NoteID, N.NoteDetails, N.Assessment, N.ActionPlan, N.ServiceCode,N.PosID,N.POSDetail ,N.ServiceDate, N.BillingProviderName, N.BillingProviderNPI,N.BillingProviderEIN,N.BillingProviderAddress,N.BillingProviderCity,            
		N.BillingProviderState, N.BillingProviderZipcode ,N.RenderingProviderName, N.RenderingProviderNPI,N.RenderingProviderEIN,N.RenderingProviderAddress,N.RenderingProviderCity,            
		N.RenderingProviderState, N.RenderingProviderZipcode,    
		 CONVERT(VARCHAR(10),N.ServiceDate, 101)  as ServiceDate,    
		 CONVERT(VARCHAR(10), R.ClosureDate, 101)  as ClosureDate,    
		B.BatchID,BatchStartDate=B.StartDate, BatchEndDate=B.EndDate, CONVERT(VARCHAR(10), B.SentDate, 101) as SentDate,SentBy=SB.UserName,GatherDate=        
		CONVERT(VARCHAR(10),B.CreatedDate, 101),BN.CLM_UNIT CalculatedUnit,        
		GatheredBy=CB.UserName, BatchPayorName=BP.PayorName, BatchPayorShortName=BP.ShortName,BT.BatchTypeName,
		(SELECT  STUFF((SELECT top 12 ',' + F.DXCodeWithoutDot    
		   FROM NoteDXCodeMappings F where F.NoteID=N.NoteID ORDER BY F.Precedence ASC  
		   FOR XML PATH('')),1,1,'')) AS ContinuedDX,
		CONVERT(DECIMAL(10,2),BN.CLM_BilledAmount) as CalculatedAmount,ModifierName = M.ModifierCode
		FROM BatchNotes BN 
		INNER JOIN Batches B ON B.BatchID=BN.BatchID AND BN.IsUseInBilling=1 AND B.BatchID in (SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@BatchID))          
		INNER JOIN BatchNoteDetails N ON N.BatchNoteID=BN.BatchNoteID AND ((BN.IsFirstTimeClaimInBatch IS NULL OR BN.IsFirstTimeClaimInBatch=1))
		INNER JOIN BatchTypes BT on BT.BatchTypeID=B.BatchTypeID            
		INNER JOIN Payors BP on BP.PayorID=B.PayorID
		INNER JOIN Employees CB on CB.EmployeeID=B.CreatedBy            
		INNER JOIN Referrals R ON N.ReferralID=R.ReferralID  
		LEFT JOIN  ContactMappings CM ON CM.ReferralID=R.ReferralID AND (CM.IsPrimaryPlacementLegalGuardian=1 OR CM.IsDCSLegalGuardian=1)            
		LEFT JOIN  Contacts C ON C.ContactID=CM.ContactID 
		LEFT JOIN Employees SB on SB.EmployeeID=B.IsSentBy 
		LEFT JOIN Modifiers M on M.ModifierID=N.ModifierID          
		ORDER BY R.LastName  ASC , R.FirstName ASC    
	END

END
