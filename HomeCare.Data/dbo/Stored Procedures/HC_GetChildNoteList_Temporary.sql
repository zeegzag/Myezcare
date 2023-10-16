                      
-- EXEC HC_GetChildNoteList_Temporary @BatchID = '105', @BatchTypeID = '1', @PayorId = '13', @StartDate = '', @EndDate = '', @ServiceCodeIDs = '', @ReferralID = '74'                      
-- EXEC HC_GetChildNoteList_Temporary @BatchID = '0', @BatchTypeID = '3', @PayorId = '12', @StartDate = '2021/10/19', @EndDate = '2021/10/26', @ServiceCodeIDs = '', @ReferralID = '41', @LoggedInID = '47'                            
                                    
CREATE PROCEDURE [dbo].[HC_GetChildNoteList_Temporary]                                                                    
@BatchID bigint=0,                                                                                                                 
@BatchTypeID bigint=1,                                                                                                                 
@PayorId BIGINT=18,                                                                                        
@ServiceCodeIDs VARCHAR(MAX)=null,                                                                                            
@StartDate Date=null,                                                                              
@EndDate Date=null,                                                                       
@ReferralID bigint  ,                                        
@LoggedInID BIGINT = 0   ,            
@NoteIDs NVARCHAR(MAX)            
              
AS                                                                                                          
BEGIN                                                                               
                                                                  
if(@BatchID = 0)                                                                  
begin                                             
                                  
 if(@BatchTypeID = 1) -- 1: Initial Submission                                                                            
begin                                                             
 select N.NoteID,dbo.GetGeneralNameFormat(N.SupervisingProvidername2420DLoop_NM104_NameFirst,N.SupervisingProvidername2420DLoop_NM103_NameLastOrOrganizationName) as EmployeeName,N.ServiceDate,N.Rate,                                                        
 ROUND(CN.CalculatedAmount,2) as CalculatedAmount,CN.CalculatedUnit,N.CalculatedServiceTime,                                                  
 --DM.Title as CareType,                                                                    
 ServiceCode = N.ServiceCode +                                                                      
 case                                                                      
 WHEN (SC.ModifierID IS NULL OR SC.ModifierID ='')                                                                  
 then ''                                                                      
 else                                                                     
 ' -'+                                                                      
  STUFF(                                                                          
  (SELECT ', ' + convert(varchar(100),M.ModifierCode, 120)                                                                          
  FROM Modifiers M                                                                          
  where M.ModifierID IN (SELECT val FROM GetCSVTable(SC.ModifierID))                                                                          
  FOR XML PATH (''))                                                                          
  , 1, 1, '')                                                                      
 end                                                                        
 from Notes_Temporary N                                                        
 INNER JOIN ChildNotes_Temporary CN ON CN.NoteID=N.NoteID                                                        
 inner join Payors P  On P.PayorID = N.PayorID                                                                   
 inner join Referrals R  On R.ReferralID = N.ReferralID                         
                             
 Inner JOIN ReferralPayorMappings RPM ON RPM.ReferralID = R.ReferralID and RPM.isDeleted=0                                  
 and ( N.ServiceDate between cast(rpm.PayorEffectiveDate as date) and rpm.PayorEffectiveEndDate)                        
                            
 inner join ServiceCodes SC on SC.ServiceCodeID = N.ServiceCodeID                                                                     
 LEFT JOIN Modifiers M ON M.ModifierID=SC.ModifierID                                                                        
 LEFT JOIN Notes NE ON NE.EmployeeVisitID = N.EmployeeVisitID                                          
 --left join DDMaster DM on DM.DDMasterID =  SC.CareType                                                                      
 where                                                  
 N.CreatedBy=@LoggedInID  AND NE.NoteID  IS NULL  AND                                           
 N.IsBillable=1 AND N.IsDeleted=0 AND N.MarkAsComplete=1  AND N.GroupID IS NOT NULL                                                          
 AND (((@StartDate is null OR N.ServiceDate>= @StartDate) AND (@EndDate is null OR N.ServiceDate <= @EndDate)))                                                                           
 AND N.PayorID=@PayorID                                                                       
 AND (@ServiceCodeIDs is null or @ServiceCodeIDs = '' or (N.ServiceCodeID in (SELECT val FROM GetCSVTable(@ServiceCodeIDs)) ))                                                                       
 AND N.ReferralID=@ReferralID                  
 AND  N.NoteID IN (SELECT DISTINCT VAL FROM DBO.GetCSVTable(@NoteIDs))            
 --   AND                                                                       
 --N.NoteID NOT IN (                                                  
 --    SELECT DISTINCT NoteID FROM (                                                                            
--    SELECT BatchNoteID,BN.NoteID,CLP02_ClaimStatusCode, ROW_NUMBER() OVER ( PARTITION BY BN.NoteID ORDER BY BN.BatchNoteID DESC) AS RowNumber                                                             
 --    FROM BatchNotes BN                                                                            
 --    ) AS A WHERE RowNumber=1  AND ( CLP02_ClaimStatusCode IS NULL OR ( CLP02_ClaimStatusCode IN (1,2,3,4) ) )                                                                 
 --    -- SELECT NoteID FROM BatchNotes WHERE NoteID NOT IN ( SELECT NoteID FROM BatchNotes  WHERE CLP02_ClaimStatusCode IN (22) AND Submitted_ClaimAdjustmentTypeID IN ('Void','Replacement') )                                                              
   
   
       
        
         
             
             
 --   )                                                                             
 order by N.ServiceDate desc                                                                        
end                                                                          
                                                                                                 
 if(@BatchTypeID = 2) -- 2: Denial Re-Submission                                                                            
begin                                                                          
                                                                      
 select N.NoteID,dbo.GetGeneralNameFormat(N.RenderingProviderFirstName ,N.RenderingProviderName) as EmployeeName,N.ServiceDate,N.Rate,ROUND(N.CalculatedAmount,2) as CalculatedAmount,N.CalculatedUnit,N.CalculatedServiceTime,                                
  
    
      
        
          
            
              
                
                 
                   
 --DM.Title as CareType,                                                             
 ServiceCode = N.ServiceCode +                                                                   
 case                                                                      
 when (SC.ModifierID IS NULL OR SC.ModifierID ='')                                                            
 then ''        
 else                                                                     
 ' -'+                                                                      
  STUFF(                                            
  (SELECT ', ' + convert(varchar(100),M.ModifierCode, 120)                                                                          
  FROM Modifiers M                                                                        
  where M.ModifierID IN (SELECT val FROM GetCSVTable(SC.ModifierID))                                
  FOR XML PATH (''))                                                                          
  , 1, 1, '')                                                                      
 end                                                           
 from Notes N                                                                       
 INNER JOIN (                            
  SELECT t.NoteID                                                                      
  FROM                                                                            
  (SELECT DISTINCT ROW_NUMBER() OVER                                                                             
  -- ( PARTITION BY BN.BatchID, BN.NoteID ORDER BY BN.BatchNoteID DESC) AS RowNumber,                                                 
  ( PARTITION BY BN.NoteID Order BY BN.BatchID DESC, MarkAsLatest DESC,  BN.BatchNoteID Desc)  AS RowNumber,                                                                             
  NoteID,CLP02_ClaimStatusCode,CLP01_ClaimSubmitterIdentifier,                                                                            
  CLP07_PayerClaimControlNumber, ClaimAdjustmentTypeID,ClaimAdjustmentReason,CLP04_TotalClaimPaymentAmount FROM BatchNotes BN                                                                             
  ) AS t --WHERE RowNumber=1 AND CLP02_ClaimStatusCode IS NOT NULL AND CLP02_ClaimStatusCode IN (4,22) AND ClaimAdjustmentTypeID IS NULL                                                                            
  LEFT JOIN  BatchNotes BN1 ON   t.CLP07_PayerClaimControlNumber=BN1.Original_PayerClaimControlNumber                                                                            
  WHERE RowNumber=1 AND                                                                             
  (t.ClaimAdjustmentTypeID IS NULL OR t.ClaimAdjustmentTypeID NOT IN ('Replacement','Void','Write-Off','Denial') ) AND                                                                            
  ( CONVERT(DECIMAL(18,2),ISNULL(t.CLP04_TotalClaimPaymentAmount,0))= 0 AND t.CLP02_ClaimStatusCode IS NOT NULL) AND BN1.Original_PayerClaimControlNumber IS NULL                                                                            
 --t.CLP02_ClaimStatusCode IS NOT NULL AND t.CLP02_ClaimStatusCode IN (4,22) AND t.ClaimAdjustmentTypeID IS NULL AND BN1.Original_PayerClaimControlNumber IS NULL                                                     
                                                                            
 ) BND ON BND.NoteID=N.NoteID                                                                          
 inner join Payors P  On P.PayorID = N.PayorID                                                                          
 inner join Referrals R  On R.ReferralID = N.ReferralID                                      
 inner join ServiceCodes SC on SC.ServiceCodeID = N.ServiceCodeID                                                                    
 LEFT JOIN Modifiers M ON M.ModifierID=SC.ModifierID                                                                            
 --left join DDMaster DM on DM.DDMasterID =  SC.CareType           
 where                                                                
 N.IsBillable=1 AND N.IsDeleted=0 AND N.MarkAsComplete=1  AND N.GroupID IS NOT NULL                                                                      
 AND (((@StartDate is null OR ServiceDate>= @StartDate) AND (@EndDate is null OR ServiceDate <= @EndDate)))                  
 AND N.PayorID=@PayorID                                                                       
 AND (@ServiceCodeIDs is null or @ServiceCodeIDs = '' or (N.ServiceCodeID in (SELECT val FROM GetCSVTable(@ServiceCodeIDs)) ))                                                                      
 AND N.ReferralID=@ReferralID             
 AND  N.NoteID IN (SELECT DISTINCT VAL FROM DBO.GetCSVTable(@NoteIDs))            
    order by N.ServiceDate desc                       
end                                                                             
                                                                          
 if(@BatchTypeID = 3) -- 3: Adjustment(Void/Replace) Submission                                                                           
begin                                                                          
                     
 Select N.NoteID,dbo.GetGeneralNameFormat(N.SupervisingProvidername2420DLoop_NM104_NameFirst,N.SupervisingProvidername2420DLoop_NM103_NameLastOrOrganizationName) as EmployeeName,N.ServiceDate,N.Rate,                                                       
 
 ROUND(CN.CalculatedAmount,2) as CalculatedAmount,CN.CalculatedUnit,N.CalculatedServiceTime,                                                  
 --DM.Title as CareType,                                                                    
 ServiceCode = N.ServiceCode +                                                                      
 case                                                 
 WHEN (SC.ModifierID IS NULL OR SC.ModifierID ='')                                                                  
 then ''                                                                      
 else                                          
 ' -'+                                                                      
  STUFF(                                                                          
  (SELECT ', ' + convert(varchar(100),M.ModifierCode, 120)                                                                          
  FROM Modifiers M                                                                          
  where M.ModifierID IN (SELECT val FROM GetCSVTable(SC.ModifierID))                                                                          
  FOR XML PATH (''))                                                                          
  , 1, 1, '')                                                                      
 end                                                                        
 from Notes N                                                        
 INNER JOIN ChildNotes CN ON CN.NoteID=N.NoteID                                                        
 inner join Payors P  On P.PayorID = N.PayorID                                                                          
 inner join Referrals R  On R.ReferralID = N.ReferralID                                                                       
                             
 Inner JOIN ReferralPayorMappings RPM ON RPM.ReferralID = R.ReferralID and RPM.isDeleted=0                                  
 and ( N.ServiceDate between cast(rpm.PayorEffectiveDate as date) and rpm.PayorEffectiveEndDate)                                
                            
 inner join ServiceCodes SC on SC.ServiceCodeID = N.ServiceCodeID                                                                     
 LEFT JOIN Modifiers M ON M.ModifierID=SC.ModifierID                                                                        
 --LEFT JOIN Notes NE ON NE.EmployeeVisitID = N.EmployeeVisitID                                          
 --left join DDMaster DM on DM.DDMasterID =  SC.CareType                      
                     
 INNER JOIN (                              
                                   
     SELECT t.NoteID,t.CLP01_ClaimSubmitterIdentifier AS Original_ClaimSubmitterIdentifier, t.CLP07_PayerClaimControlNumber AS Original_PayerClaimControlNumber,t.ClaimAdjustmentTypeID AS Submitted_ClaimAdjustmentTypeID ,t.ClaimAdjustmentReason            
  
   
      
        
          
            
              
                
                  
 FROM                              
       (SELECT DISTINCT ROW_NUMBER() OVER                               
       ( PARTITION BY BN.NoteID Order BY BN.BatchID DESC,MarkAsLatest DESC,  BN.BatchNoteID Desc)  AS RowNumber,                               
       -- ( PARTITION BY BN.BatchID, BN.NoteID ORDER BY BN.BatchNoteID DESC) AS RowNumber,                               
       NoteID,CLP02_ClaimStatusCode,CLP01_ClaimSubmitterIdentifier,                               
        CLP07_PayerClaimControlNumber, ClaimAdjustmentTypeID , ClaimAdjustmentReason FROM BatchNotes BN                               
       ) AS t -- WHERE RowNumber=1 AND ClaimAdjustmentTypeID IS NOT NULL                              
       LEFT JOIN  BatchNotes BN1 ON   t.CLP07_PayerClaimControlNumber=BN1.Original_PayerClaimControlNumber AND t.NoteID = BN1.NoteID                    
       WHERE RowNumber=1 AND (t.ClaimAdjustmentTypeID IS NOT NULL AND                               
       t.ClaimAdjustmentTypeID NOT IN ('Write-Off','Denial','Resend','Data-Validation','Payor-Change','Other')                              
       )                               
       AND BN1.Original_PayerClaimControlNumber IS NULL                              
                              
    ) BND ON BND.NoteID=N.NoteID                      
                    
 where                                                  
  --NE.NoteID  IS NULL  AND                                           
 N.IsBillable=1 AND N.IsDeleted=0 AND N.MarkAsComplete=1  AND N.GroupID IS NOT NULL                                                          
 AND (((@StartDate is null OR N.ServiceDate>= @StartDate) AND (@EndDate is null OR N.ServiceDate <= @EndDate)))                                                                           
 AND N.PayorID=@PayorID                                                   
 AND (@ServiceCodeIDs is null or @ServiceCodeIDs = '' or (N.ServiceCodeID in (SELECT val FROM GetCSVTable(@ServiceCodeIDs)) ))                                                                       
 AND N.ReferralID=@ReferralID              
 AND  N.NoteID IN (SELECT DISTINCT VAL FROM DBO.GetCSVTable(@NoteIDs))            
 --   AND                                                      
 --N.NoteID NOT IN (                                                  
 --    SELECT DISTINCT NoteID FROM (                                                                            
--    SELECT BatchNoteID,BN.NoteID,CLP02_ClaimStatusCode, ROW_NUMBER() OVER ( PARTITION BY BN.NoteID ORDER BY BN.BatchNoteID DESC) AS RowNumber                                                             
 --    FROM BatchNotes BN                                                                            
 --    ) AS A WHERE RowNumber=1  AND ( CLP02_ClaimStatusCode IS NULL OR ( CLP02_ClaimStatusCode IN (1,2,3,4) ) )                                                                 
 --    -- SELECT NoteID FROM BatchNotes WHERE NoteID NOT IN ( SELECT NoteID FROM BatchNotes  WHERE CLP02_ClaimStatusCode IN (22) AND Submitted_ClaimAdjustmentTypeID IN ('Void','Replacement') )                                                               
  
    
      
        
          
            
             
 --   )                                                                             
 order by N.ServiceDate desc                     
                    
                    
end                     
                    
                    
if(@BatchTypeID = 4) -- 4: Resend / Data-Validation          
begin                                                                          
                     
 Select N.NoteID,dbo.GetGeneralNameFormat(N.SupervisingProvidername2420DLoop_NM104_NameFirst,N.SupervisingProvidername2420DLoop_NM103_NameLastOrOrganizationName) as EmployeeName,N.ServiceDate,N.Rate,                                                       
 
 ROUND(CN.CalculatedAmount,2) as CalculatedAmount,CN.CalculatedUnit,N.CalculatedServiceTime,                                                  
 --DM.Title as CareType,                                                                    
 ServiceCode = N.ServiceCode +                                                                      
 case                                 
 WHEN (SC.ModifierID IS NULL OR SC.ModifierID ='')                                                                  
 then ''                                                                      
 else                                                                     
 ' -'+                                                                      
  STUFF(                   
  (SELECT ', ' + convert(varchar(100),M.ModifierCode, 120)                                                                          
  FROM Modifiers M                                                                          
  where M.ModifierID IN (SELECT val FROM GetCSVTable(SC.ModifierID))                                                                          
  FOR XML PATH (''))                                        
  , 1, 1, '')                                                                      
 end                                                                        
 from Notes N                                                        
 INNER JOIN ChildNotes CN ON CN.NoteID=N.NoteID                                                        
 inner join Payors P  On P.PayorID = N.PayorID                                                 
 inner join Referrals R  On R.ReferralID = N.ReferralID                                                                       
                             
 Inner JOIN ReferralPayorMappings RPM ON RPM.ReferralID = R.ReferralID and RPM.isDeleted=0                                  
 and ( N.ServiceDate between cast(rpm.PayorEffectiveDate as date) and rpm.PayorEffectiveEndDate)                                
                            
 inner join ServiceCodes SC on SC.ServiceCodeID = N.ServiceCodeID                                                                     
 LEFT JOIN Modifiers M ON M.ModifierID=SC.ModifierID                                                                        
 --LEFT JOIN Notes NE ON NE.EmployeeVisitID = N.EmployeeVisitID                                          
 --left join DDMaster DM on DM.DDMasterID =  SC.CareType                      
                     
 INNER JOIN (                              
                         
     SELECT t.NoteID,t.CLP01_ClaimSubmitterIdentifier AS Original_ClaimSubmitterIdentifier, t.CLP07_PayerClaimControlNumber AS Original_PayerClaimControlNumber,t.ClaimAdjustmentTypeID AS Submitted_ClaimAdjustmentTypeID ,t.ClaimAdjustmentReason            
  
   
      
        
          
             
             
                
                  
 FROM                              
       (SELECT DISTINCT ROW_NUMBER() OVER                               
       ( PARTITION BY BN.NoteID Order BY BN.BatchID DESC,MarkAsLatest DESC,  BN.BatchNoteID Desc)  AS RowNumber,                               
       -- ( PARTITION BY BN.BatchID, BN.NoteID ORDER BY BN.BatchNoteID DESC) AS RowNumber,                               
       NoteID,CLP02_ClaimStatusCode,CLP01_ClaimSubmitterIdentifier,                               
        CLP07_PayerClaimControlNumber, ClaimAdjustmentTypeID , ClaimAdjustmentReason FROM BatchNotes BN                               
       ) AS t -- WHERE RowNumber=1 AND ClaimAdjustmentTypeID IS NOT NULL                              
       LEFT JOIN  BatchNotes BN1 ON   t.CLP07_PayerClaimControlNumber=BN1.Original_PayerClaimControlNumber AND t.NoteID = BN1.NoteID                    
       WHERE RowNumber=1 AND (t.ClaimAdjustmentTypeID IS NOT NULL AND                               
       t.ClaimAdjustmentTypeID IN ('Resend','Data-Validation')                              
       )                               
       AND BN1.Original_PayerClaimControlNumber IS NULL                              
                              
    ) BND ON BND.NoteID=N.NoteID                      
                    
 where                                            
  --NE.NoteID  IS NULL  AND                                           
 N.IsBillable=1 AND N.IsDeleted=0 AND N.MarkAsComplete=1  AND N.GroupID IS NOT NULL                                                          
 AND (((@StartDate is null OR N.ServiceDate>= @StartDate) AND (@EndDate is null OR N.ServiceDate <= @EndDate)))                                                                           
 AND N.PayorID=@PayorID                                                                       
 AND (@ServiceCodeIDs is null or @ServiceCodeIDs = '' or (N.ServiceCodeID in (SELECT val FROM GetCSVTable(@ServiceCodeIDs)) ))                                                                       
 AND N.ReferralID=@ReferralID             
 AND  N.NoteID IN (SELECT DISTINCT VAL FROM DBO.GetCSVTable(@NoteIDs))            
 --   AND                                                                       
 --N.NoteID NOT IN (                                                  
 --    SELECT DISTINCT NoteID FROM (                                                                            
--    SELECT BatchNoteID,BN.NoteID,CLP02_ClaimStatusCode, ROW_NUMBER() OVER ( PARTITION BY BN.NoteID ORDER BY BN.BatchNoteID DESC) AS RowNumber                                                             
 --    FROM BatchNotes BN                                                                   
 --    ) AS A WHERE RowNumber=1  AND ( CLP02_ClaimStatusCode IS NULL OR ( CLP02_ClaimStatusCode IN (1,2,3,4) ) )                                                                 
 --    -- SELECT NoteID FROM BatchNotes WHERE NoteID NOT IN ( SELECT NoteID FROM BatchNotes  WHERE CLP02_ClaimStatusCode IN (22) AND Submitted_ClaimAdjustmentTypeID IN ('Void','Replacement') )                        
             
 --   )                                                                             
 order by N.ServiceDate desc                     
                    
                    
end                     
                    
end                                                                  
                    
else                                                                  
begin                                                                
                                
                                
SELECT                                 
                                
 NoteID,EmployeeName, ServiceDate,Rate, CalculatedUnit,CalculatedServiceTime,ServiceCode,RowNumber,CalculatedAmount,                                
 ClaimBilledAmount= CLM_BilledAmount,                                      
 ClaimAllowedAmount= AMT01_ServiceLineAllowedAmount_AllowedAmount,                                
 ClaimPaidAmount= SVC03_LineItemProviderPaymentAmoun_PaidAmount,                                
 --ClaimStatus              
 ClaimStatus = CASE               
               
 WHEN ClaimStatus = 'Paid' AND CLM_BilledAmount > SVC03_LineItemProviderPaymentAmoun_PaidAmount THEN 'Partial Paid'              
 ELSE  ClaimStatus END              
               
 ,BatchNoteID ,PayorClaimNumber,                      
  BatchID,ClaimAdjustmentTypeID,ClaimAdjustmentReason,              
              
CSC.ClaimStatusCodeID,CSC.ClaimStatusName,CSC.ClaimStatusCodeDescription,              
CAGC.ClaimAdjustmentGroupCodeID,CAGC.ClaimAdjustmentGroupCodeName,CAGC.ClaimAdjustmentGroupCodeDescription,              
CARC.ClaimAdjustmentReasonCodeID,CARC.ClaimAdjustmentReasonDescription,AdjustmentAmount=CAS03_ClaimAdjustmentAmount        ,      
MPP_AdjustmentGroupCodeID ,MPP_AdjustmentGroupCodeName , MPP_AdjustmentAmount  , MPP_AdjustmentComment     
                
              
              
                        
                                
FROM                                 
                                
(                                                                  
 SELECT N.NoteID,dbo.GetGeneralNameFormat(N.SupervisingProvidername2420DLoop_NM104_NameFirst,N.SupervisingProvidername2420DLoop_NM103_NameLastOrOrganizationName) as EmployeeName,                                
 N.ServiceDate,N.Rate, N.CalculatedUnit,  N.CalculatedServiceTime,                                                                                
 ServiceCode = N.ServiceCode +                                                                      
 case                                                             
 when (SC.ModifierID IS NULL OR SC.ModifierID ='')                           
 then ''                                                                      
 else                           
 ' -'+                                                                      
 STUFF(                                                                          
 (SELECT ', ' + convert(varchar(100),M.ModifierCode, 120)                                                                          
 FROM Modifiers M                                                                          
 where M.ModifierID IN (SELECT val FROM GetCSVTable(SC.ModifierID))                                                                   
 FOR XML PATH (''))                                                                          
 , 1, 1, '')                                                     
 end         ,                                
                                 
 BN.BatchNoteID,                                
        
  ClaimStatus= CASE WHEN (Submitted_ClaimAdjustmentTypeID IS NULL OR Submitted_ClaimAdjustmentTypeID NOT IN ('Void'))         
   AND CONVERT(DECIMAL(18,2),ISNULL(BN.CLP04_TotalClaimPaymentAmount,0))= 0 AND CSC.ClaimStatusName IS NOT NULL         
   AND BN.CLP07_PayerClaimControlNumber IS NOT NULL THEN 'Denied'         
           
   WHEN Submitted_ClaimAdjustmentTypeID = 'Void'         
   AND Original_PayerClaimControlNumber IS NOT NULL AND CONVERT(DECIMAL(18,2),ISNULL(BN.CLP04_TotalClaimPaymentAmount,0))= 0         
   AND CSC.ClaimStatusName='Reversal of Previous Payment'         
   AND BN.CLP07_PayerClaimControlNumber IS NOT NULL THEN 'Paid'                       
          
   WHEN Submitted_ClaimAdjustmentTypeID = 'Void'         
   AND Original_PayerClaimControlNumber IS NOT NULL AND CONVERT(DECIMAL(18,2),ISNULL(BN.CLP04_TotalClaimPaymentAmount,0))= 0         
   AND CSC.ClaimStatusName!='Reversal of Previous Payment'         
   AND BN.CLP07_PayerClaimControlNumber IS NOT NULL THEN 'Denied'          
           
   WHEN CSC.ClaimStatusName IS NULL  THEN NULL         
   WHEN  BN.CLP07_PayerClaimControlNumber IS NOT NULL THEN 'Paid'         
   ELSE  NULL END,           
                                
                                
                                 
-- Inital Note Amount                                
CalculatedAmount =  CONVERT(DECIMAL(10,2), N.CalculatedAmount),                                
                                
-- Capture Amount in Batch - Should same as Initial Note Amount                                
CLM_BilledAmount =  CONVERT(DECIMAL(10,2),    CASE WHEN BN.CLM_BilledAmount IS NULL OR LEN(BN.CLM_BilledAmount)=0 THEN '0' ELSE BN.CLM_BilledAmount END ) ,                                 
                                
-- Payor Allowed Amount Per Service Line Item                                
AMT01_ServiceLineAllowedAmount_AllowedAmount =  CONVERT(DECIMAL(10,2), CASE WHEN BN.AMT01_ServiceLineAllowedAmount_AllowedAmount IS NULL OR LEN(BN.AMT01_ServiceLineAllowedAmount_AllowedAmount)=0                                 
THEN '0' ELSE BN.AMT01_ServiceLineAllowedAmount_AllowedAmount END ),                                
                                
-- Payor Paid Amount Per Service Line Item                                
SVC03_LineItemProviderPaymentAmoun_PaidAmount = CONVERT(DECIMAL(10,2),    CASE WHEN BN.SVC03_LineItemProviderPaymentAmoun_PaidAmount IS NULL OR LEN(BN.SVC03_LineItemProviderPaymentAmoun_PaidAmount)=0                                 
THEN '0' ELSE BN.SVC03_LineItemProviderPaymentAmoun_PaidAmount END),                                
                                
                                 
 RowNumber = ROW_NUMBER() OVER ( PARTITION BY BN.NoteID Order BY BatchID DESC, BN.MarkAsLatest DESC, BN.BatchNoteID DESC) ,                    
                         
 PayorClaimNumber=BN.CLP07_PayerClaimControlNumber, BN.BatchID, BN.ClaimAdjustmentTypeID, BN.ClaimAdjustmentReason       ,              
               
 CLP02_ClaimStatusCode,CAS01_ClaimAdjustmentGroupCode,CAS02_ClaimAdjustmentReasonCode,BN.CAS03_ClaimAdjustmentAmount        ,      
       
 BN.MPP_AdjustmentGroupCodeID ,BN.MPP_AdjustmentGroupCodeName , BN.MPP_AdjustmentComment ,  
 MPP_AdjustmentAmount = CONVERT(DECIMAL(10,2), CASE WHEN BN.MPP_AdjustmentAmount IS NULL OR LEN(BN.MPP_AdjustmentAmount)=0 THEN '0' ELSE BN.MPP_AdjustmentAmount END)  
  
                                
 from BatchNotes BN                                                                  
 inner join Notes N on N.NoteID = BN.NoteID AND BN.BatchID=@BatchID                                                                  
 inner join Referrals R on R.referralid =N.referralid and N.ReferralID=@ReferralID                                                                   
 inner join ServiceCodes SC on SC.ServiceCodeID = N.ServiceCodeID                                                                     
 LEFT JOIN Modifiers M ON M.ModifierID=SC.ModifierID                                   
 LEFT JOIN ClaimStatusCodes CSC  ON CSC.ClaimStatusCodeID=BN.CLP02_ClaimStatusCode                                                
 --WHERE BN.NoteID IN (SELECT DISTINCT NoteID FROM BatchNotes WHERE BatchID=@BatchID)                                            
 WHERE BN.NoteID IN (SELECT DISTINCT VAL FROM DBO.GetCSVTable(@NoteIDs))            
             
                                 
 ) AS TEMP                
               
LEFT JOIN ClaimStatusCodes CSC  ON CSC.ClaimStatusCodeID=TEMP.CLP02_ClaimStatusCode                            
LEFT JOIN ClaimAdjustmentGroupCodes CAGC  ON CAGC.ClaimAdjustmentGroupCodeID=TEMP.CAS01_ClaimAdjustmentGroupCode                            
LEFT JOIN ClaimAdjustmentReasonCodes CARC  ON CARC.ClaimAdjustmentReasonCodeID=TEMP.CAS02_ClaimAdjustmentReasonCode                    
               
               
 WHERE RowNumber = 1                                
 ORDER BY ServiceDate DESC                                
                                
                                
end                                                        
                                                                      
END 