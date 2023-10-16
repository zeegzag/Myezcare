-- EXEC GetOverviewFileList @BatchID = '63'  
CREATE PROCEDURE [dbo].[GetOverviewFileList]             
@BatchID varchar(500)=0            
AS                          
BEGIN      
  DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat() 
  
DECLARE @SENTBatches VARCHAR(MAX);  
DECLARE @NonSENTBatches VARCHAR(MAX);  
  
(SELECT @SENTBatches= STUFF((SELECT ',' +  CONVERT(VARCHAR(MAX),BatchID)  
   FROM Batches F where BatchID IN (SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@BatchID)) AND ISSent=1  
   FOR XML PATH('')),1,1,''))  
     
(SELECT @NonSENTBatches= STUFF((SELECT  ',' +   CONVERT(VARCHAR(MAX),BatchID)  
   FROM Batches F where BatchID IN (SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@BatchID)) AND ISSent=0  
   FOR XML PATH('')),1,1,''))      
  
PRINT 'SENT'+ @SENTBatches  
PRINT 'Non SENT'+ @NonSENTBatches  
  
  
     
     
  SELECT   
  R.ReferralID,BN.NoteID,BN.BatchID,ClientName=dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat),R.FirstName,R.LastName,C.Address,C.City,C.State,C.ZipCode, R.Gender, ClientDob=CONVERT(VARCHAR(10), R.Dob, 101),R.AHCCCSID,R.CISNumber,              
  R.Population, R.Title,N.NoteID, N.NoteDetails, N.Assessment, N.ActionPlan, N.ServiceCode,N.PosID,N.POSDetail ,N.ServiceDate, N.BillingProviderName, N.BillingProviderNPI,N.BillingProviderEIN,N.BillingProviderAddress,N.BillingProviderCity,              
  N.BillingProviderState, N.BillingProviderZipcode ,N.RenderingProviderName, N.RenderingProviderNPI,N.RenderingProviderEIN,N.RenderingProviderAddress,N.RenderingProviderCity,              
  N.RenderingProviderState, N.RenderingProviderZipcode,      
   CONVERT(VARCHAR(10),N.ServiceDate, 101)  as ServiceDate,      
   CONVERT(VARCHAR(10), R.ClosureDate, 101)  as ClosureDate,      
  B.BatchID,BatchStartDate=B.StartDate, BatchEndDate=B.EndDate, CONVERT(VARCHAR(10), B.SentDate, 101) as SentDate,SentBy=SB.UserName,GatherDate=          
  CONVERT(VARCHAR(10),B.CreatedDate, 101),N.CalculatedUnit,          
  GatheredBy=CB.UserName, BatchPayorName=BP.PayorName, BatchPayorShortName=BP.ShortName,BT.BatchTypeName,              
    
  (SELECT  STUFF((SELECT top 12 ',' + F.DXCodeWithoutDot      
     FROM NoteDXCodeMappings F where F.NoteID=N.NoteID ORDER BY F.Precedence ASC    
     FOR XML PATH('')),1,1,'')) AS ContinuedDX,  
     
     --CONVERT(DECIMAL(10,2),N.CalculatedAmount) as CalculatedAmount  
     (CASE WHEN N.ServiceDate >'2017-10-01' AND PSM.BillingUnitLimit IS NOT NULL AND N.CalculatedUnit> PSM.BillingUnitLimit THEN CONVERT(DECIMAL(10,2),   
     (N.CalculatedAmount / N.CalculatedUnit) * PSM.BillingUnitLimit ) ELSE CONVERT(DECIMAL(10,2),N.CalculatedAmount) END ) as CalculatedAmount,ModifierName = M.ModifierCode  
              
  from Referrals R              
  LEFT JOIN  ContactMappings CM ON CM.ReferralID=R.ReferralID AND (CM.IsPrimaryPlacementLegalGuardian=1 OR CM.IsDCSLegalGuardian=1)              
  LEFT JOIN  Contacts C ON C.ContactID=CM.ContactID              
  INNER JOIN Notes N on N.ReferralID=R.ReferralID               
  INNER JOIN PayorServiceCodeMapping PSM ON PSM.PayorServiceCodeMappingID=N.PayorServiceCodeMappingID                           
  --INNER JOIN PlaceOfServices P on P.PosID=N.PosID  
  INNER JOIN BatchNotes BN on BN.NoteID=N.NoteID AND ((BN.IsFirstTimeClaimInBatch IS NULL OR BN.IsFirstTimeClaimInBatch=1))             
  INNER JOIN Batches B on B.BatchID=BN.BatchID AND B.BatchID in (SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@NonSENTBatches))   --AND B.IsSent=1              
  INNER JOIN Employees CB on CB.EmployeeID=B.CreatedBy              
  LEFT JOIN Employees SB on SB.EmployeeID=B.IsSentBy              
  INNER JOIN BatchTypes BT on BT.BatchTypeID=B.BatchTypeID   
  LEFT JOIN Modifiers M on M.ModifierID=N.ModifierID                
  INNER JOIN Payors BP on BP.PayorID=B.PayorID Where 1=1              
  --ORDER BY R.LastName ASC    
    
    
  UNION    
  
      
  SELECT   
  R.ReferralID,BN.NoteID,BN.BatchID,ClientName=dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat),R.FirstName,R.LastName,C.Address,C.City,C.State,C.ZipCode, R.Gender, ClientDob=CONVERT(VARCHAR(10), R.Dob, 101),R.AHCCCSID,R.CISNumber,     
  R.Population, R.Title,N.NoteID, N.NoteDetails, N.Assessment, N.ActionPlan, N.ServiceCode,N.PosID,N.POSDetail ,N.ServiceDate, N.BillingProviderName, N.BillingProviderNPI,N.BillingProviderEIN,N.BillingProviderAddress,N.BillingProviderCity,              
  N.BillingProviderState, N.BillingProviderZipcode ,N.RenderingProviderName, N.RenderingProviderNPI,N.RenderingProviderEIN,N.RenderingProviderAddress,N.RenderingProviderCity,              
  N.RenderingProviderState, N.RenderingProviderZipcode,      
   CONVERT(VARCHAR(10),N.ServiceDate, 101)  as ServiceDate,      
   CONVERT(VARCHAR(10), R.ClosureDate, 101)  as ClosureDate,      
  B.BatchID,BatchStartDate=B.StartDate, BatchEndDate=B.EndDate, CONVERT(VARCHAR(10), B.SentDate, 101) as SentDate,SentBy=SB.UserName,GatherDate=          
  CONVERT(VARCHAR(10),B.CreatedDate, 101),N.CalculatedUnit,          
  GatheredBy=CB.UserName, BatchPayorName=BP.PayorName, BatchPayorShortName=BP.ShortName,BT.BatchTypeName,              
    
  (SELECT  STUFF((SELECT top 12 ',' + F.DXCodeWithoutDot      
     FROM NoteDXCodeMappings F where F.NoteID=N.NoteID ORDER BY F.Precedence ASC    
     FOR XML PATH('')),1,1,'')) AS ContinuedDX,  
     
     --CONVERT(DECIMAL(10,2),N.CalculatedAmount) as CalculatedAmount  
     (CASE WHEN N.ServiceDate >'2017-10-01' AND PSM.BillingUnitLimit IS NOT NULL AND N.CalculatedUnit> PSM.BillingUnitLimit THEN CONVERT(DECIMAL(10,2),  
      (N.CalculatedAmount / N.CalculatedUnit) * PSM.BillingUnitLimit ) ELSE CONVERT(DECIMAL(10,2),N.CalculatedAmount) END ) as CalculatedAmount,ModifierName = M.ModifierCode  
              
  from Referrals R              
  LEFT JOIN  ContactMappings CM ON CM.ReferralID=R.ReferralID AND (CM.IsPrimaryPlacementLegalGuardian=1 OR CM.IsDCSLegalGuardian=1)              
  LEFT JOIN  Contacts C ON C.ContactID=CM.ContactID              
  INNER JOIN BatchNoteDetails N on N.ReferralID=R.ReferralID               
  INNER JOIN PayorServiceCodeMapping PSM ON PSM.PayorServiceCodeMappingID=N.PayorServiceCodeMappingID                           
  --INNER JOIN PlaceOfServices P on P.PosID=N.PosID  
  INNER JOIN BatchNotes BN on BN.NoteID=N.NoteID AND ((BN.IsFirstTimeClaimInBatch IS NULL OR BN.IsFirstTimeClaimInBatch=1))             
  INNER JOIN Batches B on B.BatchID=BN.BatchID AND B.BatchID in(SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@SENTBatches))   --AND B.IsSent=1              
  INNER JOIN Employees CB on CB.EmployeeID=B.CreatedBy              
  LEFT JOIN Employees SB on SB.EmployeeID=B.IsSentBy              
  INNER JOIN BatchTypes BT on BT.BatchTypeID=B.BatchTypeID    
  LEFT JOIN Modifiers M on M.ModifierID=N.ModifierID               
  INNER JOIN Payors BP on BP.PayorID=B.PayorID Where 1=1              
  --ORDER BY R.LastName ASC   
  
    
  
  
END  